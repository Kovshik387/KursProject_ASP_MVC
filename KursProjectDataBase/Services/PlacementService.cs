using DataBaseModel;
using DataBaseModel.Entity;
using DataBaseModel.ViewEntity;
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
            _dataBaseModelContext.SaveChanges();
        }
    }
}
