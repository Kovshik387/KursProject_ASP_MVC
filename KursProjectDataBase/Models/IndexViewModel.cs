using DataBaseModel.Entity;

namespace KursProjectDataBase.Models
{
    public class IndexViewModel
    {
        public enum SortState
        {
            None =0,
            Asc,
            Desc
        }

        public IEnumerable<DataBaseModel.Entity.Contract>? Contracts { get; set; }

        public SortState sort { get; set; }

        public bool None { get; set; }

        public int HasType { get; set; }

        public string? Filter { get; set; }

        public PageViewModel? PageViewModel { get; set; }
    }
}
