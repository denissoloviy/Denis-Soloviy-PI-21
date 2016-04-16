using DocumentsSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSystem
{
    public partial class Form1 : Form
    {
        Company company;
        Random r;
        User currUser;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            company = new Company("SuperCompany");
            company.CreateRepository("SuperRepository");
            User AdminUser = new User("Denis", "Soloviy", "Admin", 1111);
            Access acc = new Access("AdminAccess", true, true, true, true);
            Role AdminRole = new Role("AdminRole", 1111, acc);
            company.AddUser(AdminUser);
            company.AddRole(AdminRole);
            company.AddRoleToUser(AdminRole, AdminUser);
            r = new Random();
            currUser = AdminUser;
            company.Repos.OpenRepository(currUser);
            company.Repos.CreateDocument(r.Next(0, 999999999), "Doc1");
            company.Repos.CreateDocument(r.Next(0, 999999999), "Doc2");
            int doc1 = (from c in company.Repos.Documents where c.Value.Name == "Doc1" select c.Key).FirstOrDefault();
            int doc2 = (from c in company.Repos.Documents where c.Value.Name == "Doc2" select c.Key).FirstOrDefault();
            company.Repos.Documents[doc1].OpenDocument(currUser);
            company.Repos.Documents[doc1].CreateFile(r.Next(0, 999999999), "File1", "1");
            company.Repos.Documents[doc1].CreateFile(r.Next(0, 999999999), "File2", "2");
            company.Repos.Documents[doc2].OpenDocument(currUser);
            company.Repos.Documents[doc2].CreateFile(r.Next(0, 999999999), "File3", "3");
            company.Repos.Documents[doc2].CreateFile(r.Next(0, 999999999), "File4", "4");
            User user1 = new User("", "", "+w", r.Next());
            User user2 = new User("", "", "-r", r.Next());
            User user3 = new User("", "", "rw", r.Next());
            User user4 = new User("", "", "+-", r.Next());
            User user5 = new User("", "", "group", r.Next());
            Role role1 = new Role("", r.Next(), new Access("", true, false, true, false));
            Role role2 = new Role("", r.Next(), new Access("", false, true, false, true));
            Role role3 = new Role("", r.Next(), new Access("", false, true, true, false));
            Role role4 = new Role("", r.Next(), new Access("", true, false, false, true));
            Role role5 = new Role("", r.Next(), new Access("", true, true, true, true));
            Group group = new Group("", r.Next());
            company.AddUser(user1);
            company.AddUser(user2);
            company.AddUser(user3);
            company.AddUser(user4);
            company.AddUser(user5);
            company.AddRole(role1);
            company.AddRole(role2);
            company.AddRole(role3);
            company.AddRole(role4);
            company.AddRole(role5);
            company.AddGroup(group);
            company.AddRoleToUser(role1, user1);
            company.AddRoleToUser(role2, user2);
            company.AddRoleToUser(role3, user3);
            company.AddRoleToUser(role4, user4);
            company.AddUserToGroup(user5, group);
            company.AddRoleToGroup(role5, group);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var user = (from c in company.Users where c.Value.Login == textBox3.Text select c).FirstOrDefault().Value;
                if (user == null)
                {
                    user = new User(textBox1.Text, textBox2.Text, textBox3.Text, r.Next(0, 999999999));
                    company.AddUser(user);
                    label7.Text = "Warning!!!У вас немає ніяких прав доступу!";
                }
                currUser = user;
                label4.Text = "Welcome, " + currUser.Login;
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                panel1.Visible = false;
                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;
                textBox4.Clear();
                textBox5.Clear();
                company.Repos.OpenRepository(currUser);
                var source = (from c in company.Repos.Documents select new { Id = c.Key, Name = c.Value.Name });
                dataGridView1.DataSource = source.ToArray();
                panel2.Visible = true;
            }
            catch { }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            currUser = null;
            label4.Text = "";
            panel1.Visible = true;
            panel2.Visible = false;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var a = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                company.Repos.Documents[(int)a].OpenDocument(currUser);
                var n = company.Repos.Documents[(int)a].Files;
                if (n != null)
                {
                    var source = (from c in n select new { Id = c.Key, Name = c.Value.Name });
                    dataGridView2.DataSource = source.ToArray();
                }
                else
                {
                    label7.Text = "Warning!!! Ви немаєте прав доступу на зміну об'єктів";
                }
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!company.Repos.CreateDocument(r.Next(0, 999999999), "DefaultDocument"))
                {
                    MessageBox.Show("Warning!!! Ви немаєте прав доступу на створення об'єктів");
                }

                company.Repos.OpenRepository(currUser);
                var source = (from c in company.Repos.Documents select new { Id = c.Key, Name = c.Value.Name });
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = source.ToArray();
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (!company.Repos.DeleteDocument((int)dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value))
                {
                    MessageBox.Show("Warning!!! Ви немаєте прав доступу на видалення об'єктів");
                }
                company.Repos.OpenRepository(currUser);
                var source = (from c in company.Repos.Documents select new { Id = c.Key, Name = c.Value.Name });
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = source.ToArray();
            }
            catch { }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                company.Repos.Documents[(int)dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value].OpenDocument(currUser);
                if (!company.Repos.Documents[(int)dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value].CreateFile(r.Next(0, 999999999), "DefaultFile", ""))
                {
                    MessageBox.Show("Warning!!! Ви немаєте прав доступу на створення файлів");
                }
                var a = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value;
                company.Repos.Documents[(int)a].OpenDocument(currUser);
                var n = company.Repos.Documents[(int)a].Files;//wsx
                if (n != null)
                {
                    var source = (from c in n select new { Id = c.Key, Name = c.Value.Name });
                    dataGridView2.DataSource = source.ToArray();
                }
                else
                {
                    label7.Text = "Warning!!! Ви немаєте прав доступу на зміну файлів";
                }
            }
            catch { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                company.Repos.Documents[(int)dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value].OpenDocument(currUser);
                if (!company.Repos.Documents[(int)dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value].DeleteFile((int)dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Value))
                {
                    MessageBox.Show("Warning!!! Ви немаєте прав доступу на видалення файлів");
                }
                var a = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value;
                company.Repos.Documents[(int)a].OpenDocument(currUser);
                var n = company.Repos.Documents[(int)a].Files;//wsx
                if (n != null)
                {
                    var source = (from c in n select new { Id = c.Key, Name = c.Value.Name });
                    dataGridView2.DataSource = source.ToArray();
                }
                else
                {
                    label7.Text = "Warning!!! Ви немаєте прав доступу на зміну файлів";
                }
            }
            catch { }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var temp = company.Repos.Documents[(int)dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value].Edit()[(int)dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Value];
                textBox4.Text = temp.Name;
                textBox5.Text = temp.Body.ToString();
            }
            catch { }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                company.Repos.Documents[(int)dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value].Files[(int)dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Value].OpenFile(currUser);
                if (company.Repos.Documents[(int)dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value].Files[(int)dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Value].Rename(textBox4.Text))
                {
                    company.Repos.Documents[(int)dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value].Files[(int)dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Value].Body = textBox5.Text;
                }
                else
                {
                    MessageBox.Show("Warning!!! Ви немаєте прав доступу на зміну файлів");
                }
                var a = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value;
                company.Repos.Documents[(int)a].OpenDocument(currUser);
                var n = company.Repos.Documents[(int)a].Files;//wsx
                if (n != null)
                {
                    var source = (from c in n select new { Id = c.Key, Name = c.Value.Name });
                    dataGridView2.DataSource = source.ToArray();
                }
            }
            catch { }
        }
    }
}