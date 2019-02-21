using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MultitaskTimer
{
    public partial class Form1 : Form
    {
        float duration = 1200f;
        DateTime start_time = DateTime.Now;
        bool allow_dragging = false;

        public Form1()
        {
            InitializeComponent();

            close_button.MouseClick += (s, e) =>
            {
                Environment.Exit(0);
            };

            time_label.MouseDoubleClick += (s, e) =>
            {
                start_time = DateTime.Now;
            };

            MouseDown += (s, e) =>
            {
                allow_dragging = true;
            };

            MouseUp += (s, e) =>
            {
                allow_dragging = false;
            };

            MouseMove += (s, e) =>
            {
                if (allow_dragging)
                {
                    this.Location = new Point(Cursor.Position.X - Width / 2, Cursor.Position.Y - Height / 2);
                }
            };

            time_label.MouseDown += (s, e) =>
            {
                allow_dragging = true;
            };

            time_label.MouseUp += (s, e) =>
            {
                allow_dragging = false;
            };

            time_label.MouseMove += (s, e) =>
            {
                if (allow_dragging)
                {
                    this.Location = new Point(Cursor.Position.X - Width / 2, Cursor.Position.Y - Height / 2);
                }
            };

            try
            {
                if (File.Exists(Application.StartupPath + "/duration.txt"))
                {
                    var data = File.ReadAllText(Application.StartupPath + "/duration.txt").Trim();
                    duration = float.Parse(data);
                }
            }
            catch { }

            Thread t = new Thread(() =>
            {
                while (true)
                {
                    var time_left = (duration - DateTime.Now.Subtract(start_time).TotalSeconds);
                    int time_hour = (int)Math.Floor(time_left / 60f);
                    int time_seconds = (int)(time_left % 60);
                    var time_text = time_hour.ToString("00") + ":" + time_seconds.ToString("00");

                    if (time_label.InvokeRequired)
                    {
                        time_label.Invoke(new Action(() =>
                        {
                            time_label.Text = time_text;
                            Refresh();
                        }));
                    }
                    else
                    {
                        time_label.Text = time_text;
                        Refresh();
                    }
                    Thread.Sleep(100);
                }
            });
            t.Start();
        }
    }
}
