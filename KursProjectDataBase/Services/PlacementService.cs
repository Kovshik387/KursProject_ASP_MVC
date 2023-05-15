using DataBaseModel;
using DataBaseModel.Entity;
using DataBaseModel.ViewEntity;
using Microsoft.EntityFrameworkCore;
using KursProjectDataBase.Helpers;
using System.Security.Claims;

namespace KursProjectDataBase.Services
{
    public class PlacementService
    {
        private readonly KursProjectDataBaseContext _dataBaseModelContext;
        private readonly HashHelper _hashHelper;
        public PlacementService(KursProjectDataBaseContext dataBaseModelContext)
        {
            _dataBaseModelContext = dataBaseModelContext;
            _hashHelper = new HashHelper();
        }

        public IQueryable<Contract> ShowPlacement(string _id)
        {
            var id_renter = _dataBaseModelContext!.Renters!.FirstOrDefault(r => r.IdU == int.Parse(_id));

            IQueryable<Contract> result = _dataBaseModelContext.Contracts.
                Include(t => t.IdPNavigation).
                    ThenInclude(type => type.IdTypeNavigation).
                Include(s => s.IdSNavigation).
                Where(p => p.IdSNavigation.IdR == id_renter!.IdR);

            return result;
        }

        public IQueryable<Contract> TenantPlacements()
        {
            return _dataBaseModelContext.Contracts.
                Include(t => t.IdPNavigation).
                    ThenInclude(type => type.IdTypeNavigation).
                Include(s => s.IdSNavigation).
                    ThenInclude(r => r.IdRNavigation).
                    ThenInclude(u => u.IdUNavigation).
                Include(s => s.IdSNavigation).Where(item => item.IdSNavigation.IdT == null);
        }
        
        public PlacementView GetPlacementView(string _id)
        {
            int position = default(int);

            foreach (var item in _dataBaseModelContext.Contracts)
                if (_hashHelper.HashString(item.IdC).Equals(_id))
                    position = item.IdP;

            var placement = _dataBaseModelContext.Contracts.
                Include(t => t.IdPNavigation).
                    ThenInclude(type => type.IdTypeNavigation).
                Include(s => s.IdSNavigation).
                Where(p => p.IdP == position).First();

            return new PlacementView()
            {
                IdP = placement.IdP,
                Floor = placement.IdPNavigation.Floor,
                Area = placement.IdPNavigation.Area,
                Type = placement.IdPNavigation.IdTypeNavigation.Type,
                IdType = placement.IdPNavigation.IdType,
                Number = placement.IdPNavigation.Number,
                Room = placement.IdPNavigation.Room,
                Square = placement.IdPNavigation.Square,
                Street = placement.IdPNavigation.Street,
                Size = placement.Paymentsize,
                IdSolution = placement.IdSNavigation.IdS,
                Description = placement.IdSNavigation.Description != null ? placement.IdSNavigation.Description : "",
            };
        }

        public Placement GetPlacement(string _id) => 
            _dataBaseModelContext.Placements.Include(t => t.IdTypeNavigation).Where(p => p.IdP == int.Parse(_id)).First();

        public void Update(PlacementView view)
        {
            var solution = this._dataBaseModelContext.Contracts.Include(c => c.IdSNavigation).Include(p => p.IdPNavigation).Where(p => p.IdPNavigation.IdP == view.IdP).First();

            solution.IdPNavigation.Street = view.Street;
            solution.IdPNavigation.Square = view.Square;
            solution.IdPNavigation.Number = view.Number;
            solution.IdPNavigation.Floor = view.Floor;
            solution.IdPNavigation.Room = view.Room;
            solution.IdPNavigation.Area = view.Area;
            solution.IdPNavigation.IdP = view.IdP;

            solution.IdSNavigation.Description = view.Description;
            
            solution.Paymentsize = view.Size;

            _dataBaseModelContext.SaveChanges();
        }

        public void SetDeal(PlacementView view,int id)
        {
            _dataBaseModelContext.Contracts.Include(c => c.IdSNavigation).Where(s => s.IdSNavigation.IdS == view.IdSolution).ExecuteUpdate(prop => prop.
                SetProperty(item => item.IdPay, item => view.IdPay)
                
            );

            var date = DateOnly.Parse(DateTime.Now.ToString().Substring(0, 10));

            _dataBaseModelContext.Solutions.Where(id => id.IdS == view.IdSolution).ExecuteUpdate(prop => prop.
                SetProperty(item => item.IdT, item => _dataBaseModelContext.Tenants.Where(item => item.IdU == id).First().IdT).
                SetProperty(item => item.Datesolution, item => date)
            );

            var user = this._dataBaseModelContext.Contracts.
                Include(s => s.IdSNavigation).
                    ThenInclude(r => r.IdTNavigation).
                    ThenInclude(u => u.IdUNavigation).
                Include(p => p.IdPNavigation).
                    ThenInclude(p => p.IdRNavigation).
                    ThenInclude(u => u.IdUNavigation).
                Where(id_s => id_s.IdS == view.IdSolution).First();

            EmailService email = new();

            email.SentRenter(user);
            email.SentTenant(user);
        }

        public void Delete(int id)
        {
            var context = _dataBaseModelContext.Contracts.Include(p => p.IdPNavigation).Include(s => s.IdSNavigation).Where(c => c.IdC == id).First();

            _dataBaseModelContext.Contracts.Remove(context);
            _dataBaseModelContext.Placements.Remove(context.IdPNavigation);
            _dataBaseModelContext.Solutions.Remove(context.IdSNavigation);

            _dataBaseModelContext.SaveChanges();
        }

        public void Create(PlacementView view, string _id)
        {
            int id_r = _dataBaseModelContext!.Renters!.FirstOrDefault(r => r.IdU == int.Parse(_id))!.IdR;

            var contract = new Contract()
            {
                Paymentsize = view.Size,
            };

            _dataBaseModelContext.Contracts.Add(contract);

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
                
                Description = view.Description,
                IdR = id_r,
            };

            
            _dataBaseModelContext.Placements.Add(placement);

            _dataBaseModelContext.Solutions.Add(solution);

            contract.IdPNavigation = placement;
            contract.IdSNavigation = solution;

            _dataBaseModelContext.SaveChanges();
        }
    }
}
