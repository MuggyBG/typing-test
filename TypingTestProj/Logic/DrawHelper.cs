using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TypingTest_Project.Logic
{
    public static class DrawHelper
    {
        // Windows message constant to turn redrawing on/off
        private const int WM_SETREDRAW = 11;

        // Import the user32.dll method
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);

        /// <summary>
        /// Suspends the visual redrawing of a control to prevent flickering.
        /// </summary>
        public static void SuspendDrawing(Control control)
        {
            SendMessage(control.Handle, WM_SETREDRAW, false, 0);
        }

        /// <summary>
        /// Resumes the visual redrawing of a control and forces a refresh.
        /// </summary>
        public static void ResumeDrawing(Control control)
        {
            SendMessage(control.Handle, WM_SETREDRAW, true, 0);
            control.Refresh();
        }
    }
}