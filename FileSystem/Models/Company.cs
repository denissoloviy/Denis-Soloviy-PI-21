using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsSystem.Models
{
    class Company
    {
        public string Name { get; set; }
        public Dictionary<int, User> Users { get; set; }
        public Dictionary<int, Group>  Groups { get; set; }
        public Dictionary<int, Role> Roles { get; set; }
        public Dictionary<int, Position> Positions { get; set; }
        public Repository Repos { get; set; }

        public Company(string Name)
        {
            this.Name = Name;
            this.Users = new Dictionary<int, User>();
            this.Groups = new Dictionary<int, Group>();
            this.Roles = new Dictionary<int, Role>();
            this.Positions = new Dictionary<int, Position>();
        }
        public void CreateRepository(string name)
        {
            this.Repos = new Repository(name);
        }
        public void AddUser(User user)
        {
            this.Users.Add(user.UserId, user);
        }
        public void AddRoleToUser(Role role, User user)
        {
            this.Users[user.UserId].Roles.Add(role.RoleId, role);
        }
        public void AddRole(Role role)
        {
            this.Roles.Add(role.RoleId, role);
        }
        public void AddGroup(Group group)
        {
            this.Groups.Add(group.GroupId, group);
        }
        public void AddUserToGroup(User user, Group group)
        {
            this.Groups[group.GroupId].Users.Add(user.UserId, user);
            this.Users[user.UserId].Groups.Add(group.GroupId, group);
        }
        public void AddGroupToGroup(Group group1, Group group2)
        {
            this.Groups[group2.GroupId].SubGroups.Add(group1.GroupId, group1);
        }
        public void AddRoleToGroup(Role role, Group group)
        {
            this.Groups[group.GroupId].Roles.Add(role.RoleId, role);
        }
        public void AddPosition(Position position)
        {
            this.Positions.Add(position.PositionId, position);
        }
        public void AddPositionToUser(Position position, User user)
        {
            this.Users[user.UserId].Position = position;
        }
        public class Repository
        {
            public string Name { get; set; }
            public Dictionary<int, Document> Documents { get; set; }
            User CurrentUser { get; set; }
            bool IsOpened { get; set; }
            public Repository(string name)
            {
                this.Name = name;
                this.Documents = new Dictionary<int, Document>();
            }
            public void OpenRepository(User user)
            {
                this.CurrentUser = user;
                this.IsOpened = true;
            }
            public bool CreateDocument(int id, string name)
            {
                if(IsOpened)
                {
                    var role = (from c in this.CurrentUser.Roles where c.Value.Accesses.CanCreate select c).FirstOrDefault().Value;
                    var group = (from c in CurrentUser.Groups select (from d in c.Value.Roles where d.Value.Accesses.CanCreate select d)).FirstOrDefault();
                    if (role != null || group != null)
                    {
                        var doc = new Document(id, name);
                        Documents.Add(doc.DocumentId, doc);
                        return true;
                    }
                }
                return false;
            }
            public bool DeleteDocument(int key)
            {
                if (IsOpened)
                {
                    var role = (from c in this.CurrentUser.Roles where c.Value.Accesses.CanRemove select c).FirstOrDefault().Value;
                    var group = (from c in CurrentUser.Groups select (from d in c.Value.Roles where d.Value.Accesses.CanRemove select d)).FirstOrDefault();
                    if (role != null || group != null)
                        return Documents.Remove(key);
                }
                return false;
            }
            public void CloseRepository()
            {
                this.CurrentUser = null;
                this.IsOpened = false;
            }
            public void Rename(string name)
            {
                if (this.IsOpened)
                {
                    var role = (from c in this.CurrentUser.Roles where c.Value.Accesses.CanChange select c).FirstOrDefault().Value;
                    var group = (from c in CurrentUser.Groups select (from d in c.Value.Roles where d.Value.Accesses.CanChange select d)).FirstOrDefault();
                    if (role != null || group != null)
                    {
                        this.Name = name;
                    }
                }
            }
        }
    }
}
