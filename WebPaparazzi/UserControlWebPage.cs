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

namespace WebPaparazzi
{
    public partial class UserControlWebPage : UserControl
    {
        private ChromiumWebBrowser _browser;

        public String Url { get; private set; }

        public int Frequency { get; private set; }

        public UserControlWebPage()
        {
            InitializeComponent();

            _browser = new ChromiumWebBrowser("")
            {
                Dock = DockStyle.Fill
            };

            this.panelBrowser.Controls.Add(_browser);
            _browser.AddressChanged += OnBrowserAddressChanged;
            _browser.FrameLoadEnd += browser_FrameLoadEnd;
        }

        public UserControlWebPage(String url, Int32? frequencyMillis):this()
        {
            this.Url = url;
            this.Frequency = frequencyMillis.HasValue ? frequencyMillis.Value / 1000 : 30;

            this.textBoxUrl.Text = this.Url != null ? this.Url : "";
            this.textBoxFrequency.Text = this.Frequency.ToString();

        }

        private void browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.IsMainFrame)
            {
                
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

        private void Screenshot(String path, Boolean refresh, int waitBeforeScreenshot)
        {
            Image img = Screenshot(refresh, waitBeforeScreenshot);
            ImageFormat format = ImageFormat.Png;
            if (path.EndsWith(".png"))
            {
                format = ImageFormat.Png;
            }
            else if (path.EndsWith(".bmp"))
            {
                format = ImageFormat.Bmp;
            }
            else if (path.EndsWith(".jpg"))
            {
                format = ImageFormat.Jpeg;
            }
            img.Save(path, format);


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

        private void buttonTest_Click(object sender, EventArgs e)
        {
            Screenshot(this.textBoxImgPath.Text, true, 3 * 1000);
        }

        private void UserControlWebPage_Load(object sender, EventArgs e)
        {
            this._browser.Load(this.textBoxUrl.Text);
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
            return this.InvokeOnUiThreadIfRequired<Image>(() =>
            {

                if (refresh)
                {
                    ManualResetEvent loadComplete = new ManualResetEvent(false);
                    EventHandler<FrameLoadEndEventArgs> onFrameLoadedEnd = new EventHandler<FrameLoadEndEventArgs>((o, e) =>
                    {
                        if (e.IsMainFrame)
                        {
                            loadComplete.Set();
                        }
                    });
                    this._browser.FrameLoadEnd += onFrameLoadedEnd;
                    this._browser.Reload(true);

                    loadComplete.WaitOne(TimeSpan.FromSeconds(30));
                    this._browser.FrameLoadEnd -= onFrameLoadedEnd;
                    Thread.Sleep(waitBeforeScreenshot);
                }

                return this._browser.DrawToImage();
            });
        }

    }
}
