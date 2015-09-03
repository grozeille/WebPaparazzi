using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPaparazzi
{
    public static class ApiConstants
    {
        public const int SRCCOPY = 13369376;

        public const int
            WM_PRINTCLIENT = 0x0318,
            WM_PRINT = 0x317, 
            PRF_CLIENT = 4,
            PRF_CHILDREN = 0x10, 
            PRF_NON_CLIENT = 2,
            PRF_OWNED = 0x20,
            COMBINED_PRINTFLAGS = PRF_CLIENT | PRF_CHILDREN | PRF_NON_CLIENT | PRF_OWNED;
    }
}
