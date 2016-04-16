using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsSystem.Models
{
    class Access
    {
        public string Name { get; set; }
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanChange { get; set; }
        public bool CanRemove { get; set; }
        public Access(string name, bool canCreate, bool canRead, bool canChange, bool canRemove)
        {
            this.Name = name;
            this.CanCreate = canCreate;
            this.CanRead = canRead;
            this.CanChange = canChange;
            this.CanRemove = canRemove;
        }
    }
}
