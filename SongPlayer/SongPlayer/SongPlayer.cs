using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongPlayer
{
    public enum InstrumentType
    {
        Accordion,
        Drums,
        Minigame
    }

    public partial class SongPlayer : Form
    {
        private bool running = false;
        private Rectangle music = new Rectangle(339, 185, 10, 382);
        private Rectangle minigame = new Rectangle(760, 886, 401, 10);

        public SongPlayer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs ea)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            if(button1.Text == "Start")
            {
                button1.Text = "Stop";
                running = true;

                new Thread(() =>
                {
                    List<double> fpsAverages = new List<double>();

                    while (running)
                    {
                        double fpsTime = 1000.0 / (double)timer.ElapsedMilliseconds;
                        fpsAverages.Add(fpsTime);
                        if (fpsAverages.Count > 10)
                            fpsAverages.RemoveAt(0);

                        double averageTime = fpsAverages.Average();

                        timer.Restart();

                        this.Invoke((MethodInvoker)(() =>
                        {
                            Text = "FPS: " + averageTime.ToString("N2");
                        }));

                        if (getActiveWindowTitle() == "ATLAS") 
                        {
                            if (radioButton3.Checked)
                            {
                                using (Bitmap frame = ScreenCapture.Get(minigame))
                                {
                                    KeyScanner.ScanFrame(frame, InstrumentType.Minigame, textBox1);
                                }
                            }
                            else
                            {
                                using (Bitmap frame = ScreenCapture.Get(music))
                                {
                                    KeyScanner.ScanFrame(frame, radioButton1.Checked ? InstrumentType.Accordion : InstrumentType.Drums, textBox1);
                                }
                            }
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                    }
                }).Start();
            }
            else
            {
                button1.Text = "Start";
                running = false;
                Thread.Sleep(100);
            }
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string getActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        private void SongPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            running = false;
        }
    }
}
