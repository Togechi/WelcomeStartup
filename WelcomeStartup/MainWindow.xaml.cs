using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;
using System.Threading;
using Microsoft.Win32;

namespace GoodMorningStartup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string time;
        public bool startWaitToKill = false;
        int iterationsForKill = 0;
        private MediaPlayer mediaPlayer = new MediaPlayer();

        public static void AddApplicationToStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("My Program", "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"");
            }
        }

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.K))
            {
                AddApplicationToStartup();
            }

            if (startWaitToKill == true)
            {
                iterationsForKill++;
                if (iterationsForKill >= 100)
                {
                    Application.Current.Shutdown();
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            time = DateTime.Now.ToString("HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            DoYourStuff();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Start();
        }

        private void DoYourStuff()
        {
            string timeHour = new string(time.Take(2).ToArray());
            textBox.Text = timeHour;

            int intTime;
            Int32.TryParse(timeHour, out intTime);

            if (intTime >= 0 && intTime <= 11)
            {
                GoodMorning();
            } 
            else if (intTime >= 12 && intTime <= 16)
            {
                GoodAfternoon();
            }
            else if (intTime >= 17 && intTime <= 23)
            {
                GoodEvening();
            }
        }

        private void GoodEvening()
        {
            programWindow.Title = "Good Evening";
            textBox.Text = "Good evening";
            // Play sound
            mediaPlayer.Open(new Uri("Sound/GoodEvening.mp3", UriKind.Relative));
            mediaPlayer.Play();

            Kill();
        }

        private void GoodAfternoon()
        {
            programWindow.Title = "Good Afternoon";
            textBox.Text = "Good afternoon";

            mediaPlayer.Open(new Uri("Sound/GoodAfternoon.mp3", UriKind.Relative));
            mediaPlayer.Play();

            Kill();
        }

        private void Kill()
        {
            startWaitToKill = true;
        }

        private void GoodMorning()
        {
            programWindow.Title = "Good Morning";
            textBox.Text = "Good morning";

            mediaPlayer.Open(new Uri("Sound/GoodMorning.mp3", UriKind.Relative));
            mediaPlayer.Play();
            Kill();
        }
    }
}
