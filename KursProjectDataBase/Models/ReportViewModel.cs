using DataBaseModel.Entity;

namespace KursProjectDataBase.Models
{
    public class ReportViewModel
    {
        public int CountContracts { get; set; } = 0;
        public int SumContracts { get; set; } = 0;

        public string FirstDate { get; set; }
        public string LastDate { get; set; }

        public IQueryable<Contract> Contracts { get; set; }
    }
}
