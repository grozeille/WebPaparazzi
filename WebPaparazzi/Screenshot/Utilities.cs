using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebPaparazzi
{
    public static class Utilities
    {
        public static Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }

        public static Image CaptureWindow(IntPtr handle)
        {
            IntPtr hdcSrc = User32.GetWindowDC(handle);

            RECT windowRect = new RECT();
            User32.GetWindowRect(handle, ref windowRect);

            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;

            IntPtr hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, width, height);

            IntPtr hOld = Gdi32.SelectObject(hdcDest, hBitmap);
            Gdi32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, ApiConstants.SRCCOPY);
            Gdi32.SelectObject(hdcDest, hOld);
            Gdi32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);

            Image image = Image.FromHbitmap(hBitmap);
            Gdi32.DeleteObject(hBitmap);

            return image;
        }

        public static Bitmap PrintWindow(IntPtr hwnd)
        {
            RECT rc = new RECT();
            User32.GetWindowRect(hwnd, ref rc);

            Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();
            bool succeeded = User32.PrintWindow(hwnd, hdcBitmap, 0);
            gfxBmp.ReleaseHdc(hdcBitmap);
            if (!succeeded)
            {
                gfxBmp.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(Point.Empty, bmp.Size));
            }
            IntPtr hRgn = Gdi32.CreateRectRgn(0, 0, 0, 0);
            User32.GetWindowRgn(hwnd, hRgn);
            Region region = Region.FromHrgn(hRgn);
            if (!region.IsEmpty(gfxBmp))
            {
                gfxBmp.ExcludeClip(region);
                gfxBmp.Clear(Color.Transparent);
            }
            gfxBmp.Dispose();
            return bmp;
        }

        static List<IntPtr> GetAllChildrenWindowHandles(IntPtr hParent,  int maxCount)
        {
            List<IntPtr> result = new List<IntPtr>();
            int ct = 0;
            IntPtr prevChild = IntPtr.Zero;
            IntPtr currChild = IntPtr.Zero;
            while (true && ct < maxCount)
            {
                currChild = User32.FindWindowEx(hParent, prevChild, null, null);
                if (currChild == IntPtr.Zero) break;
                result.Add(currChild);
                prevChild = currChild;
                ++ct;
            }
            return result;
        }

        public static Bitmap Snapshot(Control c)
        {
            int width = 0, height = 0;
            IntPtr hwnd = IntPtr.Zero;
            IntPtr dc = IntPtr.Zero;
            c.Invoke(new MethodInvoker(() =>
            {
                width = c.ClientSize.Width;
                height = c.ClientSize.Height;
                hwnd = c.Handle;
                dc = User32.GetDC(hwnd);
            }));
            width = 1024;
            height = 768;
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            if (dc != IntPtr.Zero)
            {
                try
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        IntPtr bdc = g.GetHdc();
                        try
                        {
                            Gdi32.BitBlt(bdc, 0, 0, width, height, dc, 0, 0, ApiConstants.SRCCOPY);
                        }
                        finally
                        {
                            g.ReleaseHdc(bdc);
                        }
                    }
                }
                finally
                {
                    User32.ReleaseDC(hwnd, dc);
                }
            }
            return bmp;
        }

    }
}
