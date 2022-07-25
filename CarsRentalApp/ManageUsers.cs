using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarsRentalApp
{
    public partial class frmManageUsers : Form
    {
        private readonly CarRentalEntities2 _db;
        
        public frmManageUsers()
        {
            InitializeComponent();
            _db = new CarRentalEntities2();
            //dgvUserList.DataSource = _db.Users.ToList();
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("frmAddUser"))
            {
                var addUser = new frmAddUser(this);
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }
            
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)dgvUserList.SelectedRows[0].Cells["Id"].Value;

                // query database for record
                var user = _db.Users.FirstOrDefault(q => q.id == id);
                var hashed_password = Utils.DefaultHashedPassword();
                user.password = hashed_password;
                _db.SaveChanges();

                MessageBox.Show($"{user.username}'s Password has been reset");
            }
            catch
            {
                MessageBox.Show("Unable to Edit car");
            }
        }

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)dgvUserList.SelectedRows[0].Cells["Id"].Value;

                // query database for record
                var user = _db.Users.FirstOrDefault(q => q.id == id);
                
                // Activate/Deactivate user
                        //if (user.isActive == true)
                        //    user.isActive = false;
                        //else
                        //    user.isActive = true;
                user.isActive = user.isActive == true ? false : true;
                _db.SaveChanges();

                MessageBox.Show($"{user.username}'s active status has changed!");
                PopulateGrid();
            }
            catch
            {
                MessageBox.Show("Unable to Edit car");
            }
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            var users = _db.Users.Select(q => new
            {
                q.id,
                q.username,
                q.UserRoles.FirstOrDefault().Role.name,
                q.isActive
            }).ToList();

            dgvUserList.DataSource = users;
            dgvUserList.Columns["username"].HeaderText = "UserName";
            dgvUserList.Columns["name"].HeaderText = "Role Name";
            dgvUserList.Columns["isActive"].HeaderText = "Active";

            dgvUserList.Columns["id"].Visible = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}
