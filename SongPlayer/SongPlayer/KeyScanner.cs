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
    public static class KeyScanner
    {
        public static Color Black = Color.FromArgb(255, Color.Black);
        public static Color White = Color.FromArgb(255, Color.White);
        public static Color Red = Color.FromArgb(255, Color.Red);

        private static int q, e, w, a, s, d, space;

        private static bool qDown = false;
        private static bool wDown = false;
        private static bool aDown = false;
        private static bool sDown = false;
        private static bool dDown = false;
        private static bool eDown = false;
        private static bool spaceDown = false;

        private static MinigamePlayer mgp = null;

        public static void ScanFrame(Bitmap bitmap, InstrumentType it, TextBox output)
        {
            
            if (it == InstrumentType.Minigame)
            {
                Color gray = bitmap.GetPixel(0, 7);
                bool minigameDetected = true;
                for (int i = 0; i < 401; i++)
                {
                    Color c2 = bitmap.GetPixel(i, 1);
                    if (c2 != Black && c2 != Red)
                    {
                        minigameDetected = false;
                        break;
                    }

                    Color c1 = bitmap.GetPixel(i, 7);
                    if (c1 != White && c1 != Red && c1 != gray)
                    {
                        minigameDetected = false;
                        break;
                    }
                }

                if (minigameDetected)
                {
                    if (mgp == null)
                    {
                        Console.WriteLine("Minigame Detected!");
                        mgp = new MinigamePlayer(bitmap);
                    }

                    mgp.Update(bitmap);
                }

                if (mgp != null && mgp.Complete)
                    mgp = null;
            }
            else
            {
                double ep = 15;
                int x = 4;

                loadOffsets(bitmap.Size, it);

                if (q != -1 && bitmap.GetPixel(x, q).IsCloseTo(Black, ep))
                {
                    //Send Q
                    if (!qDown)
                    {
                        SendKeys.SendWait("q");
                        qDown = true;
                        write(output, "Q");
                    }
                }
                else
                {
                    qDown = false;
                }

                if (w != -1 && bitmap.GetPixel(x, w).IsCloseTo(Black, ep))
                {
                    //Send W
                    if (!wDown)
                    {
                        SendKeys.SendWait("w");
                        wDown = true;
                        write(output, "W");
                    }
                }
                else
                {
                    wDown = false;
                }

                if (d != -1 && bitmap.GetPixel(x, d).IsCloseTo(Black, ep))
                {
                    //Send D
                    if (!dDown)
                    {
                        SendKeys.SendWait("d");
                        dDown = true;
                        write(output, "D");
                    }
                }
                else
                {
                    dDown = false;
                }

                if (s != -1 && bitmap.GetPixel(x, s).IsCloseTo(Black, ep))
                {
                    //Send S
                    if (!sDown)
                    {
                        SendKeys.SendWait("s");
                        sDown = true;
                        write(output, "S");
                    }
                }
                else
                {
                    sDown = false;
                }

                if (a != -1 && bitmap.GetPixel(x, a).IsCloseTo(Black, ep))
                {
                    //Send A
                    if (!aDown)
                    {
                        SendKeys.SendWait("a");
                        aDown = true;
                        write(output, "A");
                    }
                }
                else
                {
                    aDown = false;
                }

                if (e != -1 && bitmap.GetPixel(x, e).IsCloseTo(Black, ep))
                {
                    //Send E
                    if (!eDown)
                    {
                        SendKeys.SendWait("e");
                        eDown = true;
                        write(output, "E");
                    }
                }
                else
                {
                    eDown = false;
                }

                if (space != -1 && bitmap.GetPixel(x, space).IsCloseTo(Black, ep))
                {
                    //Send Space
                    if (!spaceDown)
                    {
                        SendKeys.SendWait(" ");
                        spaceDown = true;
                        write(output, "{SPACE}");
                    }
                }
                else
                {
                    spaceDown = false;
                }
            }
        }
        
        private static void write(TextBox tb, string message)
        {
            tb.Invoke((MethodInvoker)(() =>
            {
                tb.AppendText(message + Environment.NewLine);
            }));
        }

        private static void loadOffsets(SizeF size, InstrumentType it)
        {
            if (it == InstrumentType.Accordion) 
            {
                q = 20;
                e = 137;
                w = 82;
                a = 200;
                s = 259; 
                d = 320; 
                space = -1;
            }
            else if (it == InstrumentType.Drums)
            {
                q = -1;
                e = -1;
                w = 138;
                a = 81;
                s = -1;
                d = 202;
                space = 22;
            }
        }

        private static bool IsCloseTo(this Color c, Color color, double ep)
        {
            double e1 = c.B + c.G + c.R;
            double e2 = color.B + color.G + color.R;

            if(Math.Abs(e2 - e1) <= ep)
            {
                return true;
            }

            return false;
        }
    }
}
