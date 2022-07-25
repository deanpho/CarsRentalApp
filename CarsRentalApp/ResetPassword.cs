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
    public partial class frmResetPassword : Form
    {
        private readonly CarRentalEntities2 _db;
        private User _user; 
        public frmResetPassword(User user)
        {
            InitializeComponent();
            _db = new CarRentalEntities2();
            _user = user;
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                var password = txtEnterNewPassword.Text;
                var confirm_password = txtConfirmPassword.Text;
                var user = _db.Users.FirstOrDefault(q => q.id == _user.id);
                if (password != confirm_password)
                {
                    MessageBox.Show("Password do not match. Please try again");
                }

                user.password = Utils.HashPassword(password);
                _db.SaveChanges();

                MessageBox.Show("Password is reset successfully");
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("An error has occurred, please try again");
            }
           

        }
    }
}
