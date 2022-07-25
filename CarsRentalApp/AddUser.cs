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
    public partial class frmAddUser : Form
    {
        private readonly CarRentalEntities2 _db;
        private frmManageUsers _manageUsers;
        public frmAddUser(frmManageUsers manageUsers)
        {
            InitializeComponent();
            _db = new CarRentalEntities2();
            _manageUsers = manageUsers;
        }

        private void frmAddUser_Load(object sender, EventArgs e)
        {
            var roles = _db.Roles.ToList();
            cboRole.DataSource = roles;
            cboRole.ValueMember = "id";
            cboRole.DisplayMember = "name";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var username = txtUserName.Text;
                var roleId = (int)cboRole.SelectedValue;
                var password = Utils.DefaultHashedPassword();
                var user = new User
                {
                    username = username,
                    password = password,
                    isActive = true,
                };
                _db.Users.Add(user);
                _db.SaveChanges();

                var userid = user.id;

                var userRole = new UserRole
                {
                    roleid = roleId,
                    userid = userid
                };

                _db.UserRoles.Add(userRole);
                _db.SaveChanges();

                MessageBox.Show("New User added successfully");
                _manageUsers.PopulateGrid();
                Close();
            }
            catch (Exception)
            {

                MessageBox.Show("An Error has occurred");
            }
            
        }
    }
}
