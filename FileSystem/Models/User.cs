using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsSystem.Models
{
    class User : Person
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public Position Position { get; set; }
        public Dictionary<int, Role> Roles { get; set; }
        public Dictionary<int, Group> Groups { get; set; }
        public User(string firstName, string secondName, string login, int Id)
            :base(firstName,secondName)
        {
            this.Login = login;
            this.UserId = Id;
            this.Roles = new Dictionary<int, Role>();
            this.Groups = new Dictionary<int, Group>();
            this.Position = new Position();
        }
    }
}