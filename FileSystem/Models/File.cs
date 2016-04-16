using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsSystem.Models
{
    class File
    {
        public int FileId { get; set; }
        public string Name { get; set; }
        public object Body { get; set; }
        User CurrentUser { get; set; }
        bool IsOpened { get; set; }
        public void OpenFile(User user)
        {
            this.CurrentUser = user;
            this.IsOpened = true;
        }
        public void CloseFile()
        {
            this.CurrentUser = null;
            this.IsOpened = false;
        }
        public File(int id, string name, object body)
        {
            this.FileId = id;
            this.Name = name;
            this.Body = body;
        }
        public void Edit(object body)
        {
            this.Body = body;
        }
        public bool Rename(string newName)
        {
            if (this.IsOpened)
            {
                var role = (from c in this.CurrentUser.Roles where c.Value.Accesses.CanChange select c).FirstOrDefault().Value;
                var group = (from c in CurrentUser.Groups select (from d in c.Value.Roles where d.Value.Accesses.CanChange select d)).FirstOrDefault();
                if (role != null || group != null)
                {
                    this.Name = newName;
                    return true;
                }
            }
            return false;
        }
    }
}
