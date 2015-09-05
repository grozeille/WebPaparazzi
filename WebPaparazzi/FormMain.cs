using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebPaparazzi.Model;

namespace WebPaparazzi
{
    public partial class FormMain : Form
    {
        private ImageStreamingServer _server;

        public FormMain()
        {
            InitializeComponent();
        }

        #region events
        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            AddPage(null, 30*1000);
        }

        private void toolStripButtonRemove_Click(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedIndex >= 0)
            {
                this.tabControl.TabPages.RemoveAt(this.tabControl.SelectedIndex);
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadSettings();

            _server = new ImageStreamingServer(ImageStream());
        }

        private void toolStripButtonStart_Click(object sender, EventArgs e)
        {
            this.toolStripButtonStop.Enabled = true;
            this.toolStripButtonStart.Enabled = false;
            this.toolStripTextBoxPort.Enabled = false;

            if (!_server.IsRunning)
            {
                _server.Start(Int32.Parse(this.toolStripTextBoxPort.Text), PaparazziResolutionConverter.FromString(this.toolStripComboBoxResolution.Text));
            }
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            this.toolStripButtonStop.Enabled = false;
            this.toolStripButtonStart.Enabled = true;
            this.toolStripTextBoxPort.Enabled = true;

            if (_server.IsRunning)
            {
                _server.Stop();
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
        #endregion

        private IEnumerable<ImageWithTime> ImageStream()
        {
            int cptTab = 0;
            while (true)
            {
                if (cptTab >= this.tabControl.TabPages.Count)
                {
                    cptTab = 0;
                    //this.tabControl.InvokeOnUiThreadIfRequired(() => this.tabControl.SelectedIndex = cptTab);
                }

                var webPage = (UserControlWebPage)this.tabControl.TabPages[cptTab].Controls[0];
                var image = webPage.Screenshot(true, 3 * 1000);
                yield return new ImageWithTime { Image = image, TimeInMilliseconds = webPage.Frequency * 1000 };

                cptTab++;
                if (cptTab >= this.tabControl.TabPages.Count)
                {
                    cptTab = 0;
                }
                //this.tabControl.InvokeOnUiThreadIfRequired(() => this.tabControl.SelectedIndex = cptTab);
            }
        }

        private void SaveSettings()
        {
            IList<PaparazziTabSetting> settings = new List<PaparazziTabSetting>();
            foreach (TabPage tab in this.tabControl.TabPages)
            {
                var webPageControl = ((UserControlWebPage)tab.Controls[0]);
                PaparazziTabSetting setting = new PaparazziTabSetting();
                setting.RereshTimeInMillisec = webPageControl.Frequency * 1000;
                setting.Url = webPageControl.Url;

                settings.Add(setting);
            }
            PaparazziSettings settingsObject = new PaparazziSettings();
            settingsObject.Port = Int32.Parse(this.toolStripTextBoxPort.Text);
            settingsObject.Resolution = PaparazziResolutionConverter.FromString(this.toolStripComboBoxResolution.Text);
            settingsObject.Settings = settings.ToArray();
            String json = Newtonsoft.Json.JsonConvert.SerializeObject(settingsObject);

            var parentFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WebPaparazzi");
            if (!Directory.Exists(parentFolder))
            {
                Directory.CreateDirectory(parentFolder);
            }

            var fileName = Path.Combine(parentFolder, "settings.json");
            File.WriteAllText(fileName, json);
        }

        private void AddPage(String url, Int32? frequencyMillis)
        {
            TabPage page = new TabPage("");
            UserControlWebPage webPage = new UserControlWebPage(url, frequencyMillis);
            webPage.BrowserSize = PaparazziResolutionConverter.ToSize(PaparazziResolutionConverter.FromString(this.toolStripComboBoxResolution.Text));
            page.Controls.Add(webPage);
            webPage.Dock = DockStyle.Fill;
            webPage.OnPageLoad += (e, a) =>
            {
                page.InvokeOnUiThreadIfRequired(() => page.Text = a.Title.Substring(0, (a.Title.Length > 20 ? 20 : a.Title.Length)) + (a.Title.Length > 20 ? "..." : ""));
            };
            this.tabControl.TabPages.Add(page);
        }
        
        private void LoadSettings()
        {
            var parentFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WebPaparazzi");
            if (Directory.Exists(parentFolder))
            {
                var fileName = Path.Combine(parentFolder, "settings.json");
                if (File.Exists(fileName))
                {
                    var json = File.ReadAllText(fileName);
                    var settingsObject = Newtonsoft.Json.JsonConvert.DeserializeObject<PaparazziSettings>(json);

                    foreach (var item in settingsObject.Settings)
                    {
                        AddPage(item.Url, item.RereshTimeInMillisec);
                    }

                    this.toolStripTextBoxPort.Text = settingsObject.Port.ToString();
                    this.toolStripComboBoxResolution.Text = PaparazziResolutionConverter.ToString(settingsObject.Resolution);
                }
            }
        }

        private void toolStripComboBoxResolution_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBoxResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (TabPage tab in this.tabControl.TabPages)
            {
                var webPageControl = ((UserControlWebPage)tab.Controls[0]);
                webPageControl.BrowserSize = PaparazziResolutionConverter.ToSize(PaparazziResolutionConverter.FromString(this.toolStripComboBoxResolution.Text));
            }
        }

    }
}
