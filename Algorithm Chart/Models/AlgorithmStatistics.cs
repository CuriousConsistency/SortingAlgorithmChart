using System.ComponentModel;

namespace Algorithm_Chart.Models
{
    public class AlgorithmStatistics : INotifyPropertyChanged
    {
        public string bestCase;
        public string averageCase;
        public string worstCase;

        public event PropertyChangedEventHandler PropertyChanged;

        public string BestCase
        {
            get
            {
                return this.bestCase;
            }
            set
            {
                this.bestCase = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.BestCase)));
            }
        }

        public string AverageCase
        {
            get
            {
                return this.averageCase;
            }
            set
            {
                this.averageCase = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.AverageCase)));
            }
        }

        public string WorstCase
        {
            get
            {
                return this.worstCase;
            }
            set
            {
                this.worstCase = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.worstCase)));
            }
        }

        public void ResetStatistics()
        {
            this.BestCase = string.Empty;
            this.AverageCase = string.Empty;
            this.WorstCase = string.Empty;
        }
    }
}
