using System.ComponentModel;

namespace Algorithm_Chart.Models
{
    public class NoisyCounter : INotifyPropertyChanged
    {
        private int _Count;

        public int Count
        {
            get
            {
                return this._Count;
            }
            set
            {
                this._Count = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.FormattedCount)));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CountAvailable)));
            }
        }

        public bool CountAvailable
        {
            get
            {
                return this._Count != 0;
            }
        }

        public string FormattedCount
        {
            get
            {
                return $"{this._Count.ToString()} comparisons";
            }
        }

        public NoisyCounter()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ResetCount()
        {
            this.Count = 0;
        }
    }
}

