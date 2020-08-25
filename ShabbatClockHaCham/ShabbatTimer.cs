using System;
using System.Windows.Threading;

namespace ShabbatClockHaCham
{
    public class ShabbatTimer
    {
        public double PostShabbatDelay { get; set; }
        public DateTime ShabbatTime { get; set; }
        private DispatcherTimer Timer { get; set; }
        public bool Enabled { get; internal set; }

        public event EventHandler<ShabbatTimerEventArgs> ShabbatTick;

        public ShabbatTimer(DateTime sunsetDateTime, double postSunsetDelay)
        {
            ShabbatTime = sunsetDateTime;
            Timer = new DispatcherTimer();
            PostShabbatDelay = postSunsetDelay;
            Timer.Tick += new EventHandler(TimerTick);
            Timer.Interval = TimeSpan.FromMilliseconds(500);
        }

        // This is the method to run when the timer is raised.
        private void TimerTick(object sender, EventArgs elapsedEventArgs)
        {
            var signalTime = DateTime.Now;
            if (signalTime.CompareTo(ShabbatTime.AddMinutes(PostShabbatDelay)) >= 0)
            {
                TimerElapsed();
                return;

            }
            
            ShabbatTick?.Invoke(this, new ShabbatTimerEventArgs(ShabbatTickType.Tick, CalculateRemainingSeconds(signalTime)));
        }

        private double CalculateRemainingSeconds(DateTime signalTime)
        {
            return ShabbatTime.AddMinutes(PostShabbatDelay).Subtract(signalTime).TotalSeconds;
        }

        public void Start()
        {
            Timer.Start();
            this.Enabled = true;
            // emit event that the clock started for the UI
            ShabbatTick?.Invoke(this, new ShabbatTimerEventArgs(ShabbatTickType.Started, CalculateRemainingSeconds(DateTime.Now)));
        }
        public void Stop()
        {
            Timer.Stop();
            this.Enabled = false;
            // emit event that the clock started for the UI
            ShabbatTick?.Invoke(this, new ShabbatTimerEventArgs(ShabbatTickType.Stopped, CalculateRemainingSeconds(DateTime.Now)));
        }
        private void TimerElapsed()
        {
            Timer.Stop();
            this.Enabled = false;
            EventHandler<ShabbatTimerEventArgs> handler = ShabbatTick;
            handler?.Invoke(this, new ShabbatTimerEventArgs(ShabbatTickType.Elapsed, 0));
        }
    }

    public class ShabbatTimerEventArgs : EventArgs
    {
        public ShabbatTickType ShabbatTickType { get; private set; }
        public double SecondsRemaing { get; private set; }
        public ShabbatTimerEventArgs(ShabbatTickType shabbatTickType, double secondsRemaining): base()
        {
            SecondsRemaing = secondsRemaining;
            ShabbatTickType = shabbatTickType;
        }
    }
    public enum ShabbatTickType {
        Started = 1,
        Tick = 2,
        Stopped = 3,
        Elapsed = 4
    }
}
