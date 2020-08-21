using System;
using System.Timers;

namespace ShabbatClockHaCham
{
    public class ShabbatTimer
    {
        public double PostSunsetDelay { get; set; }
        public DateTime SunsetDateTime { get; set; }
        private Timer Timer { get; set; }
        public bool Enabled { get; internal set; }

        public event EventHandler<ShabbatTimerEventArgs> ShabbatTick;

        public ShabbatTimer(DateTime sunsetDateTime, double postSunsetDelay)
        {
            SunsetDateTime = sunsetDateTime;
            Timer = new Timer();
            PostSunsetDelay = postSunsetDelay;
            Timer.Elapsed += new ElapsedEventHandler(TimerTick);
            Timer.Interval = 1000;
        }

        // This is the method to run when the timer is raised.
        private void TimerTick(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (elapsedEventArgs.SignalTime.CompareTo(SunsetDateTime.AddMinutes(PostSunsetDelay)) >= 0)
            {
                TimerElapsed();
                return;

            }
            
            ShabbatTick?.Invoke(this, new ShabbatTimerEventArgs(ShabbatTickType.Tick, CalculateRemainingSeconds(elapsedEventArgs.SignalTime)));
        }

        private double CalculateRemainingSeconds(DateTime signalTime)
        {
            return SunsetDateTime.AddMinutes(PostSunsetDelay).Subtract(signalTime).TotalSeconds;
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
