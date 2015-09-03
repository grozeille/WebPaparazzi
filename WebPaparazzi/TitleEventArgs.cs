using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPaparazzi
{
    public class TitleEventArgs : EventArgs
    {
        public String Title { get; private set; }

        public TitleEventArgs(String title)
        {
            this.Title = title;
        }
    }
}
