using System.ComponentModel;

namespace Sorting_Algorithm_Chart.Models
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void ResetCount()
        {
            this.Count = 0;
        }
    }
}

