using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsSystem.Models
{
    class Position
    {
        public int PositionId { get; set; }
        public string Name { get; set; }
        public Position(string name, int id)
        {
            this.Name = name;
            this.PositionId = id;
        }
        public Position()
        {

        }
    }
}
