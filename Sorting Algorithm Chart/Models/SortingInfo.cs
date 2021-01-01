namespace Sorting_Algorithm_Chart.Models
{
    public class SortingInfo
    {
        public bool Sorted { get; set; }

        public int SortingDelay { get; set; }

        public SortingInfo(bool sorted, int sortingDelay)
        {
            this.Sorted = sorted;
            this.SortingDelay = sortingDelay;
        }
    }
}
