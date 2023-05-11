using DataBaseModel.Entity;

namespace KursProjectDataBase.Models
{
    public class IndexViewModel
    {
        public IEnumerable<DataBaseModel.Entity.Contract> Contracts { get; set; }
        public PageViewModel? PageViewModel { get; set; }
    }
}
