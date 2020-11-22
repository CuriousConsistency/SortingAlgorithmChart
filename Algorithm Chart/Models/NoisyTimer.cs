using System;
using System.Timers;
using System.ComponentModel;

namespace Algorithm_Chart.Models
{
    public class NoisyTimer : INotifyPropertyChanged
    {
        private readonly float _Interval;
        private readonly Timer _Timer;
        private float _Time;

        public float Time
        {
            get
            {
                return this._Time;
            }
            set
            {
                this._Time = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.FormattedTime)));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TimeAvailable)));
            }
        }

        public bool TimeAvailable
        {
            get
            {
                return this._Time != 0;
            }
        }

        public string FormattedTime
        {
            get
            {
                return $"{this._Time.ToString("n2")} seconds";
            }
        }

        public NoisyTimer(int interval)
        {
            this._Interval = (float)interval/1000;
            this._Timer = new Timer(interval);
            this._Timer.Elapsed += this.OnTimeElapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnTimeElapsed(object sender, EventArgs e)
        {
            this.Time += this._Interval;
        }

        public void ResetTimer()
        {
            this.Time = 0;
        }

        public void StartTimer()
        {
            this.Time = 0;
            this._Timer.Enabled = true;
            this._Timer.AutoReset = true;
        }

        public void EndTimer()
        {
            this._Timer.Stop();
        }

        public void Close()
        {
            this._Timer.Close();
        }
    }
}
