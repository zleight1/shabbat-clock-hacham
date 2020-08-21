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

namespace ShabbatClockHaCham
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ShabbatTimer ShabbatClockTimer { get; private set; }
        public ZmanimService ZmanimService { get; private set; }
        public ShutdownInteropService ShutdownInteropService { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            InitializeZmanim();
        }


        public string TopInformationText { get; set; }
        public string CountdownText { get; set; }
        public string ToggleShabbatTimerButtonContent { get; set; }

        private void InitializeZmanim()
        {
            this.ShutdownInteropService = new ShutdownInteropService();
            this.ZmanimService = new ZmanimService();
            var location = @"";
            double postShabbatDelay = 18;
            DateTime sunset = ZmanimService.ResolveSunsetDateTime(location);
            this.ShabbatClockTimer = new ShabbatTimer(sunset, postShabbatDelay);
            this.ShabbatClockTimer.ShabbatTick += ShabbatTickHandler;
            this.lblTopInformation.Text = "Your location is X";
            this.lblCountdown.Text = "00:00:00";
            this.ToggleShabbatTimerButton.Content = "Shutdown for Shabbat";
        }

        private void ShabbatTickHandler(object sender, ShabbatTimerEventArgs e)
        {
            this.lblCountdown.Text = TimeSpan.FromSeconds(e.SecondsRemaing).ToString();
            if (e.ShabbatTickType == ShabbatTickType.Elapsed)
            {
                this.ToggleShabbatTimerButton.Content = "Shutting down...";
                ShutdownInteropService.InvokeShutdown();
            }
        }

        private void ToggleShabbatTimerHandler(object sender, TouchEventArgs e)
        {
            if (this.ShabbatClockTimer.Enabled)
            {
                this.ShabbatClockTimer.Stop();
                this.ToggleShabbatTimerButton.Content = "Shutdown for Shabbat";

            } else
            {
                this.ShabbatClockTimer.Start();
                this.ToggleShabbatTimerButton.Content = "Stop shutdown for Shabbat";
            }
        }

        private void ToggleShabbatTimerHandler(object sender, MouseButtonEventArgs e)
        {
            if (this.ShabbatClockTimer.Enabled)
            {
                this.ShabbatClockTimer.Stop();
                this.ToggleShabbatTimerButton.Content = "Shutdown for Shabbat";

            }
            else
            {
                this.ShabbatClockTimer.Start();
                this.ToggleShabbatTimerButton.Content = "Stop shutdown for Shabbat";
            }
        }
    }
}
