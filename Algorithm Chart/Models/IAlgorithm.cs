using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Algorithm_Chart.Models.Commands;
using LiveCharts;

namespace Algorithm_Chart.Models
{
    public interface IAlgorithm
    {
        string Title { get; }
        void Algorithm(SeriesCollection dataSet, int sortingDelay);
    }
}
