using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsSystem.Models
{
    class Document
    {
        public int DocumentId { get; set; }
        public string Name { get; set; }
        public Dictionary<int, File> Files { get; set; }
        User CurrentUser { get; set; }
        bool IsOpened { get; set; }
        public Document(int id, string name)
        {
            this.DocumentId = id;
            this.Name = name;
            Files = new Dictionary<int, File>();
        }
        public void OpenDocument(User user)
        {
            this.CurrentUser = user;
            this.IsOpened = true;
        }
        public IDictionary<int, File> Edit()
        {
            if (this.IsOpened)
            {
                var role = (from c in this.CurrentUser.Roles where c.Value.Accesses.CanRead select c).FirstOrDefault().Value;
                var group = (from c in CurrentUser.Groups select (from d in c.Value.Roles where d.Value.Accesses.CanRead select d)).FirstOrDefault();
                if (role != null || group != null)
                {
                    return Files;
                }
            }
            return null;
        }
        public bool CreateFile(int id, string name, object body)
        {
            if (IsOpened)
            {
                var role = (from c in this.CurrentUser.Roles where c.Value.Accesses.CanCreate select c).FirstOrDefault().Value;
                var group = (from c in CurrentUser.Groups select (from d in c.Value.Roles where d.Value.Accesses.CanCreate select d)).FirstOrDefault();
                if (role != null || group != null)
                {
                    var file = new File(id, name, body);
                    Files.Add(file.FileId, file);
                    return true;
                }
            }
            return false;
        }
        public bool DeleteFile(int key)
        {
            if(this.IsOpened)
            {
                var role = (from c in this.CurrentUser.Roles where c.Value.Accesses.CanRemove select c).FirstOrDefault().Value;
                var group = (from c in CurrentUser.Groups select (from d in c.Value.Roles where d.Value.Accesses.CanRemove select d)).FirstOrDefault();
                if (role != null || group != null)
                    if (Files.ContainsKey(key))
                        return Files.Remove(key);
            }
            return false;
        }
        public void Rename(string newName)
        {
            if (this.IsOpened)
            {
                var role = (from c in this.CurrentUser.Roles where c.Value.Accesses.CanChange select c).FirstOrDefault().Value;
                var group = (from c in CurrentUser.Groups select (from d in c.Value.Roles where d.Value.Accesses.CanChange select d)).FirstOrDefault();
                if (role != null || group != null)
                {
                    this.Name = newName;
                }
            }
        }
        public void CloseDocument()
        {
            this.CurrentUser = null;
            this.IsOpened = false;
        }
    }
}
