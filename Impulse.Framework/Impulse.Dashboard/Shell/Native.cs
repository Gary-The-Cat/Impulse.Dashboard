// <copyright file="Native.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Impulse.Dashboard.Shell
{
    public static class Native
    {
        public static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam, int minWidth, int minHeight)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            const int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                var rcWorkArea = monitorInfo.RcWork;
                var rcMonitorArea = monitorInfo.RcMonitor;
                mmi.PtMaxPosition.X = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
                mmi.PtMaxPosition.Y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
                mmi.PtMaxSize.X = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
                mmi.PtMaxSize.Y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);
                mmi.PtMinTrackSize.X = minWidth;
                mmi.PtMinTrackSize.Y = minHeight;
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("user32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;

            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT PtReserved;
            public POINT PtMaxSize;
            public POINT PtMaxPosition;
            public POINT PtMinTrackSize;
            public POINT PtMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public static readonly RECT Empty = default(RECT);
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }

            public RECT(RECT rcSrc)
            {
                this.Left = rcSrc.Left;
                this.Top = rcSrc.Top;
                this.Right = rcSrc.Right;
                this.Bottom = rcSrc.Bottom;
            }

            public int Width
            {
                get { return Math.Abs(Right - Left); }
            }

            public int Height
            {
                get { return Bottom - Top; }
            }

            public bool IsEmpty
            {
                get
                {
                    // BUGBUG : On Bidi OS (hebrew arabic) left > right
                    return Left >= Right || Top >= Bottom;
                }
            }

            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return rect1.Left == rect2.Left && rect1.Top == rect2.Top && rect1.Right == rect2.Right && rect1.Bottom == rect2.Bottom;
            }

            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }

            public override string ToString()
            {
                if (this == RECT.Empty)
                {
                    return "RECT {Empty}";
                }

                return "RECT { left : " + Left + " / top : " + Top + " / right : " + Right + " / bottom : " + Bottom + " }";
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Rect))
                {
                    return false;
                }

                return this == (RECT)obj;
            }

            public override int GetHashCode()
            {
                return Left.GetHashCode() + Top.GetHashCode() + Right.GetHashCode() + Bottom.GetHashCode();
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int CbSize { get; set; } = Marshal.SizeOf(typeof(MONITORINFO));

            public RECT RcMonitor { get; set; } = default(RECT);

            public RECT RcWork { get; set; } = default(RECT);

            public int DwFlags { get; set; } = 0;
        }
    }
}
