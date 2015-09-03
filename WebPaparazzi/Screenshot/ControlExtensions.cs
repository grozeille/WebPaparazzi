using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebPaparazzi
{
    public static class ControlExtensions
    {
        public static Image DrawToImage(this Control control)
        {
            //return Utilities.CaptureWindow(control.Handle);
            //return Utilities.PrintWindow(control.Handle);
            return Utilities.Snapshot(control);
        }

        /// <summary>
        /// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
        /// </summary>
        /// <param name="control">the control for which the update is required</param>
        /// <param name="action">action to be performed on the control</param>
        public static void InvokeOnUiThreadIfRequired(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }

        public static T InvokeOnUiThreadIfRequired<T>(this Control control, Func<T> action)
        {
            if (control.InvokeRequired)
            {
                return (T)control.Invoke(action);
            }
            else
            {
                return action.Invoke();
            }
        }
    }
}
