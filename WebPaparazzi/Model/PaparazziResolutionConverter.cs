using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WebPaparazzi.Model
{
    public static class PaparazziResolutionConverter
    {
        public static PaparazziResolution FromString(String s)
        {
            if(s.Equals("720p"))
            {
                return PaparazziResolution.R_720p;
            }
            else if(s.Equals("1024x768"))
            {
                return PaparazziResolution.R_1024x768;
            }

            return PaparazziResolution.R_1080p;
        }

        public static String ToString(PaparazziResolution r)
        {
            if(r.Equals(PaparazziResolution.R_720p))
            {
                return "720p";
            }
            else if(r.Equals(PaparazziResolution.R_1024x768))
            {
                return "1024x768";
            }

            return "1080p";
        }

        public static Size ToSize(PaparazziResolution r)
        {
            if(r.Equals(PaparazziResolution.R_720p))
            {
                return new Size(1280, 720);
            }
            else if(r.Equals(PaparazziResolution.R_1024x768))
            {
                return new Size(1024, 768);
            }

            return new Size(1920, 1200);
        }

        public static Size DefaultSize { get { return ToSize(PaparazziResolution.R_1080p); } }

    }
}
