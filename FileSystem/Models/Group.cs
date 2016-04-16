using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsSystem.Models
{
    class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public Dictionary<int, Group> SubGroups { get; set; }
        public Dictionary<int, Role> Roles { get; set; }
        public Dictionary<int, User> Users { get; set; }
        public Group(string name, int id)
        {
            this.Name = name;
            this.GroupId = id;
            this.SubGroups = new Dictionary<int, Group>();
            this.Roles = new Dictionary<int, Role>();
            this.Users = new Dictionary<int, User>();
        }
    }
}
