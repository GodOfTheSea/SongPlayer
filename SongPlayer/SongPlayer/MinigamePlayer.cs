using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongPlayer
{
    public class MinigamePlayer
    {
        public bool Complete { get; private set; } = false;

        private int left = -1;
        private int right = -1;
        private int red = -1;

        public MinigamePlayer(Bitmap frame)
        {
            detectBounds(frame, out left, out right);
            red = scanForRed(frame) + 3;

            tryExecuteClick();
        }

        public void Update(Bitmap frame)
        {
            red = scanForRed(frame) + 3;
            detectBounds(frame, out left, out right);

            tryExecuteClick();
        }

        private void tryExecuteClick()
        {
            if (red != -1 && left != -1 && right != -1)
            {
                if (red >= left && red <= right)
                {
                    mouseClick();
                    Thread.Sleep(300);
                    Complete = true;
                }
            }
        }

        private int scanForRed(Bitmap frame)
        {
            for (int i = 0; i < 401; i++)
            {
                Color c = frame.GetPixel(i, 5);
                if (c == KeyScanner.Red)
                {
                    return i;
                }
            }

            return -1;
        }

        private void detectBounds(Bitmap bitmap, out int left, out int right)
        {
            left = -1;
            right = -1;
            for (int i = 0; i < 401; i++)
            {
                Color c = bitmap.GetPixel(i, 5);
                if (c == KeyScanner.White)
                {
                    left = i;
                    break;
                }
            }

            for (int i = 400; i >= 0; i--)
            {
                Color c = bitmap.GetPixel(i, 5);
                if (c == KeyScanner.White)
                {
                    right = i;
                    break;
                }
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private static void mouseClick()
        {
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
    }
}
