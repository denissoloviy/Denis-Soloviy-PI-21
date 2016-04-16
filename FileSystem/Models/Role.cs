using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsSystem.Models
{
    class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public Access Accesses { get; set; }
        public Role(string name, int id, Access accesses)
        {
            this.RoleId = id;
            this.Name = name;
            this.Accesses = accesses;
        }
    }
}
