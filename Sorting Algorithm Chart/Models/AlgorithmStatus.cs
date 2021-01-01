using System.ComponentModel;
using System.Threading.Tasks;

namespace Sorting_Algorithm_Chart.Models
{
    public class AlgorithmStatus : INotifyPropertyChanged
    {
        private TaskStatus algorithmStatus;
        public event PropertyChangedEventHandler PropertyChanged;

        public AlgorithmStatus()
        {
            this.TaskStatus = TaskStatus.WaitingForActivation;
        }

        public TaskStatus TaskStatus
        {
            get
            {
                return this.algorithmStatus;
            }
            set
            {
                this.algorithmStatus = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TaskStatus)));
            }
        }
    }
}
