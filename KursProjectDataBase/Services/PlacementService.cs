using DataBaseModel;
using DataBaseModel.Entity;
using DataBaseModel.ViewEntity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KursProjectDataBase.Services
{
    public class PlacementService
    {
        private readonly KursProjectDataBaseContext _dataBaseModelContext;

        public PlacementService(KursProjectDataBaseContext dataBaseModelContext)
        {
            _dataBaseModelContext = dataBaseModelContext;
        }

        public Tuple<IQueryable<Placement>, List<Contract>> ShowPlacement(string _id)
        {
            var id_renter = _dataBaseModelContext!.Renters!.FirstOrDefault(r => r.IdU == int.Parse(_id));

            Tuple<IQueryable<Placement>, List<Contract>> result = new (_dataBaseModelContext.Placements.Include(t => t.IdTypeNavigation).Where(p => p.IdR == id_renter!.IdR),
                _dataBaseModelContext.Contracts.Include(s => s.IdSNavigation).Where(s => s.IdSNavigation.IdR== id_renter!.IdR).ToList());

            return result;
        }

        public void Create(PlacementView view, string _id)
        {
            int id_r = _dataBaseModelContext!.Renters!.FirstOrDefault(r => r.IdU == int.Parse(_id))!.IdR;
            
            var placement = new Placement()
            {
                Floor = view.Floor,
                Square = view.Square,
                Room = view.Room,
                Area = view.Area,
                Street = view.Street,
                Number = view.Number,
                IdType = view.IdType,
                IdR = id_r,
            };


            var solution = new Solution()
            {
                Datesolution = DateOnly.Parse(DateTime.Now.ToString().Substring(0, 10)),
                Description = view.Description,
                IdR = id_r,
            };

            _dataBaseModelContext.Placements.Add(placement);
            _dataBaseModelContext.Solutions.Add(solution);

            var contract = new Contract()
            {
                Paymentsize = view.Size,
                IdS = _dataBaseModelContext.Solutions.OrderBy(id => id.IdS).Last().IdS,
                IdP = _dataBaseModelContext.Placements.OrderBy(id => id.IdP).Last().IdP,
            };

            _dataBaseModelContext.Contracts.Add(contract);

            _dataBaseModelContext.SaveChanges();
        }
    }
}
