using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModel.ViewEntity
{
    public class UserView
    {
        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Sex { get; set; }

        public string? Contact { get; set; }

        public string Loginuser { get; set; } = null!;

        public string Passworduser { get; set; } = null!;

        public int TypeUser { get; set; }

        public override string ToString()
        {
            return $"\nИмя: {Name}\nФамилия: {Surname}\nПол: {Sex}\nКонтакты: {Contact}\nЛогин: {Loginuser}\nПароль: {Passworduser}\nТип: {TypeUser}";
        }
    }
}
