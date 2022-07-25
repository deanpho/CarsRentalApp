using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarsRentalApp
{
    public partial class frmLogin : Form
    {
        private readonly CarRentalEntities2 _db;
        public frmLogin()
        {
            InitializeComponent();
            _db = new CarRentalEntities2();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                SHA256 sha = SHA256.Create();

                var username = txtUsername.Text.Trim();
                var password = txtPassword.Text;

                var hashed_password = Utils.HashPassword(password);

                // Check for matching username, password and active flag
                var user = _db.Users.FirstOrDefault(q => q.username == username && q.password == hashed_password 
                && q.isActive == true);
                
                // if user is not matched
                if(user == null)
                {
                    MessageBox.Show("Please provide valid credentials");
                }
                else
                {
                    //var role = user.UserRoles.FirstOrDefault();
                    //var roleShortName = role.Role.shortname;
                    //var mainWindow = new frmMainWindow(this, roleShortName);
                    var mainWindow = new frmMainWindow(this, user);
                    mainWindow.Show();
                    Hide();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Something went wrong. Please try again");
            }
        }
    }
}
