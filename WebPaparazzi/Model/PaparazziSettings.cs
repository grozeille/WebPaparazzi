using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPaparazzi.Model
{
    public class PaparazziSettings
    {
        public int Port { get; set; }

        public PaparazziResolution Resolution { get; set; }

        public PaparazziTabSetting[] Settings { get; set; }
    }
}
