using System.ComponentModel;

namespace KursProjectDataBase.Models
{
    public class UserModelView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Contact { get; set; } = null;
        public int? License { get; set; } = 0;
        public int? Rating { get; set; } = 0;
        public string Type { get; set; }

        public string IdHash { get; set; }
    }

}
