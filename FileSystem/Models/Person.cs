using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsSystem.Models
{
    class Person
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public Person(string firstName, string secondName)
        {
            this.FirstName = firstName;
            this.SecondName = secondName;
        }
    }
}
