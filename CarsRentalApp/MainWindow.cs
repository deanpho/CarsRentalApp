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
    public partial class frmMainWindow : Form
    {
        private frmLogin _login;
        public string _roleName;
        public User _user;

        public frmMainWindow()
        {
            InitializeComponent();
        }

        public frmMainWindow(frmLogin login, User user)
        {
            InitializeComponent();
            _login = login;
            _user = user;
            _roleName = user.UserRoles.FirstOrDefault().Role.shortname;
        }

        private void addRentalRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("frmAddEditRentalRecord"))
            {
                var addRentalRecord = new frmAddEditRentalRecord();
                addRentalRecord.ShowDialog();
                addRentalRecord.MdiParent = this;
            }
        }

        private void manageVehicleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // launch the window as a child
                var OpenForms = Application.OpenForms.Cast<Form>();
                var isOpen = OpenForms.Any(q => q.Name == "frmManageVehicleListing");
                
                // if there is any form name "frmManageVehicleListing" then launch it, if it's not open!
                if (!isOpen)
                {
                    var vehicleListing = new frmManageVehicleListing();
                    vehicleListing.MdiParent = this;
                    vehicleListing.Show();
                }

                
            }
            catch 
            {
                MessageBox.Show("Unable to load Manage Vehicle Listing window");
            }
            
        }

        private void viewArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("frmManageRentalRecord"))
            {
                var manageRentalRecords = new frmManageRentalRecord();
                manageRentalRecords.MdiParent = this;
                manageRentalRecords.Show();
            }
        }

        private void frmMainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _login.Close();
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("frmManageUsers"))
            {
                frmManageUsers manageUsers = new frmManageUsers();
                manageUsers.MdiParent = this;
                manageUsers.Show();
            }
            
        }

        private void frmMainWindow_Load(object sender, EventArgs e)
        {
            if (_user.password == Utils.DefaultHashedPassword())
            {
                var resetPassword = new frmResetPassword(_user);
                resetPassword.ShowDialog();
            }

            var username = _user.username;
            tsiLoginText.Text = $"Logged In As: {username}";
            if (_roleName != "admin")
            {
                manageUsersToolStripMenuItem.Visible = false;
            }
        }
    }
}
