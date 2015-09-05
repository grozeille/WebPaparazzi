using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading;
using CefSharp;
using CefSharp.WinForms;
using WebPaparazzi.Model;
using System.Reflection;
using System.IO;

namespace WebPaparazzi
{
    public partial class UserControlWebPage : UserControl
    {
        private ChromiumWebBrowser _browser;

        private CefSharp.OffScreen.ChromiumWebBrowser _offScreenBrowser;

        private String _resize_script;

        public String Url { get; private set; }

        public int Frequency { get; private set; }

        public Size BrowserSize
        {
            set
            {
                this.InvokeOnUiThreadIfRequired(() => 
                    {
                        _offScreenBrowser.Size = value;
                    });                
            }
        }

        public event EventHandler<TitleEventArgs> OnPageLoad;

        public UserControlWebPage()
        {
            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "WebPaparazzi.resize_image.js";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                _resize_script = reader.ReadToEnd();
            }

            _browser = new ChromiumWebBrowser("")
            {
                Dock = DockStyle.Fill
            };
            _offScreenBrowser = new CefSharp.OffScreen.ChromiumWebBrowser("");
            _offScreenBrowser.Size = PaparazziResolutionConverter.DefaultSize;
            _offScreenBrowser.FrameLoadEnd += offScreenBrowser_FrameLoadEnd;

            this.panelBrowser.Controls.Add(_browser);
            _browser.AddressChanged += OnBrowserAddressChanged;
            _browser.FrameLoadEnd += browser_FrameLoadEnd;
            _browser.IsBrowserInitializedChanged += browser_IsBrowserInitializedChanged;
        }

        public UserControlWebPage(String url, Int32? frequencyMillis)
            : this()
        {
            this.Url = url;
            this.Frequency = frequencyMillis.HasValue ? frequencyMillis.Value / 1000 : 30;

            this.textBoxUrl.Text = this.Url != null ? this.Url : "";
            this.textBoxFrequency.Text = this.Frequency.ToString();

        }

        private void browser_IsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            this._browser.Load(this.textBoxUrl.Text);
        }

        private void offScreenBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.IsMainFrame)
            {
                _offScreenBrowser.ExecuteScriptAsync("document.body.style.overflow = 'hidden';");
                if (_offScreenBrowser.Address.EndsWith(".png") || _offScreenBrowser.Address.EndsWith(".jpg"))
                {
                    _offScreenBrowser.ExecuteScriptAsync(_resize_script);
                }
            }
        }

        private void browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.IsMainFrame)
            {
                if (OnPageLoad != null)
                {
                    OnPageLoad(this, new TitleEventArgs(_browser.Title));
                }
                if (_browser.Address.EndsWith(".png") || _browser.Address.EndsWith(".jpg"))
                {
                    _browser.ExecuteScriptAsync(_resize_script);
                }
            }
        }

        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() => {
                this.textBoxUrl.Text = e.Address;
                this.Url = e.Address;
            });
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            this._browser.Load(this.textBoxUrl.Text);
        }

        public void Screenshot(String path, Boolean refresh, int waitBeforeScreenshot)
        {
            Image img = Screenshot(refresh, waitBeforeScreenshot);
            ImageFormat format = ImageFormat.Png;
            if (path.EndsWith(".png"))
            {
                format = ImageFormat.Png;
                img.Save(path, format);
            }
            else if (path.EndsWith(".bmp"))
            {
                format = ImageFormat.Bmp;
                img.Save(path, format);
            }
            else if (path.EndsWith(".jpg"))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                // Create an Encoder object based on the GUID
                // for the Quality parameter category.
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

                // Create an EncoderParameters object.
                // An EncoderParameters object has an array of EncoderParameter
                // objects. In this case, there is only one
                // EncoderParameter object in the array.
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                img.Save(path, jpgEncoder, myEncoderParameters);
            }
            

            /*using (Bitmap bmp = new Bitmap(this.webBrowser.Width, this.webBrowser.Height))
            {
                this.webBrowser.DrawToBitmap(bmp, new Rectangle(0, 0, this.webBrowser.Width, this.webBrowser.Height));
                ImageFormat format = ImageFormat.Png;
                if (this.textBoxImgPath.Text.EndsWith(".png"))
                {
                    format = ImageFormat.Png;
                }
                else if (this.textBoxImgPath.Text.EndsWith(".bmp"))
                {
                    format = ImageFormat.Bmp;
                }
                else if (this.textBoxImgPath.Text.EndsWith(".jpg"))
                {
                    format = ImageFormat.Jpeg;
                }
                bmp.Save(this.textBoxImgPath.Text, format);
            }*/
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            Screenshot(this.textBoxImgPath.Text, true, 3 * 1000);
        }

        private void UserControlWebPage_Load(object sender, EventArgs e)
        {
            
        }

        private void textBoxFrequence_TextChanged(object sender, EventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() =>
                {
                    int frequency;
                    if (!Int32.TryParse(this.textBoxFrequency.Text, out frequency))
                    {
                        frequency = 30;
                    }
                    this.Frequency = frequency;
                });
        }

        private void textBoxUrl_TextChanged(object sender, EventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() =>
            {
                this.Url = this.textBoxUrl.Text;
            });
        }


        public Image Screenshot(Boolean refresh, int waitBeforeScreenshot)
        {
            ManualResetEvent loadComplete = new ManualResetEvent(false);
            EventHandler<FrameLoadEndEventArgs> onFrameLoadedEnd = new EventHandler<FrameLoadEndEventArgs>((o, e) =>
            {
                if (e.IsMainFrame)
                {
                    loadComplete.Set();
                }
            });

            this.InvokeOnUiThreadIfRequired(() =>
            {
                if (refresh)
                {
                    this._offScreenBrowser.FrameLoadEnd += onFrameLoadedEnd;
                    this._offScreenBrowser.Load(this._browser.Address);

                    //this._browser.FrameLoadEnd += onFrameLoadedEnd;
                    //this._browser.Reload(true);                    
                }
            });

            if(refresh)
            {
                loadComplete.WaitOne(TimeSpan.FromSeconds(30));
                //this._browser.FrameLoadEnd -= onFrameLoadedEnd;
                this._offScreenBrowser.FrameLoadEnd -= onFrameLoadedEnd;
                Thread.Sleep(waitBeforeScreenshot);
            }

            return this.InvokeOnUiThreadIfRequired<Image>(() =>
            {
                //return this._browser.DrawToImage();                
                return this._offScreenBrowser.ScreenshotOrNull();
            });
        }

    }
}
