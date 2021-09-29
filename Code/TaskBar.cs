using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Terminal
{
    public static class TaskBar
    {
        [DllImport("user32.dll")]

        private static extern int GetWindowText(IntPtr hWind, StringBuilder text, int count);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]

        private static extern bool EnumThreadWindows(int threadId, EnumThreadProc pfnEnum, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]

        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]

        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]

        private static extern IntPtr FindWindowEx(IntPtr parentHwnd, IntPtr childAfterHwnd, IntPtr className, string windowText);

        [DllImport("user32.dll")]

        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]

        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        private const string StartMenuCaption = "Start";

        private static IntPtr StartMenuWnd = IntPtr.Zero;

        private delegate bool EnumThreadProc(IntPtr hWnd, IntPtr lParam);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;

        private static bool _isVisible = true;

        public static bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                SetVisibility(_isVisible);
            }
        }

        private static bool EnumThreadWindowsProc(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder buf = new StringBuilder(256);
            if (buf.ToString() == StartMenuCaption)
            {
                StartMenuWnd = hWnd;
                return false;
            }
            return true;
        }

        private static IntPtr GetStartMenuWnd(IntPtr taskBarWnd)
        {
            int processId;
            GetWindowThreadProcessId(taskBarWnd, out processId);
            Process p = Process.GetProcessById(processId);
            if (p != null)
                foreach (ProcessThread thread in p.Threads)
                    EnumThreadWindows(thread.Id, EnumThreadWindowsProc, IntPtr.Zero);
            return StartMenuWnd;
        }

        private static void SetVisibility(bool showTaskBar)
        {
            IntPtr taskBarWnd = FindWindow("Shell_TrayWnd", null);

            //1
            IntPtr startWindow = FindWindowEx(taskBarWnd, IntPtr.Zero, "Button", "Start");

            //2
            if (startWindow == IntPtr.Zero)
                startWindow = FindWindowEx(IntPtr.Zero, IntPtr.Zero, (IntPtr)0xC017, "Start");

            //3
            if (startWindow == IntPtr.Zero)
                startWindow = FindWindow("Button", null);

            //4
            if (startWindow == IntPtr.Zero)
                startWindow = GetStartMenuWnd(taskBarWnd);

            if (showTaskBar)
            {
                ShowWindow(taskBarWnd, SW_SHOW);
                ShowWindow(startWindow, SW_SHOW);
            }
            else
            {
                ShowWindow(taskBarWnd, SW_HIDE);
                ShowWindow(startWindow, SW_HIDE);
            }
        }
    }
}
