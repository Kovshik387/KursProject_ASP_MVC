using DataBaseModel.Entity;

namespace KursProjectDataBase.Models
{
    public class IndexViewModel
    {
        public enum SortState
        {
            Asc = 1,
            Desc
        }

        public IEnumerable<DataBaseModel.Entity.Contract>? Contracts { get; set; }

        public SortState sort { get; set; } = SortState.Asc;

        public string? Filter { get; set; }

        public PageViewModel? PageViewModel { get; set; }
    }
}
