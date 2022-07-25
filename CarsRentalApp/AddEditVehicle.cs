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
    public partial class frmAddEditVehicle : Form
    {
        private bool isEditMode;
        private frmManageVehicleListing _manageVehicleListing;
        private readonly CarRentalEntities2 _db;

        public frmAddEditVehicle(frmManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
            this.Text = "Add New Vehicle";
            isEditMode = false;
            _manageVehicleListing = manageVehicleListing;
            _db = new CarRentalEntities2();
        }

        public frmAddEditVehicle(TypesOfCar carToEdit, frmManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            this.Text = "Edit Vehicle";
            _manageVehicleListing = manageVehicleListing;

            if (carToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities2();
                PopulateFields(carToEdit);
            }
        }

        private void PopulateFields(TypesOfCar car)
        {
            lblId.Text = car.Id.ToString();
            txtMake.Text = car.Make;
            txtModel.Text = car.Model;
            txtVIN.Text = car.VIN;
            txtYear.Text = Convert.ToString(car.Year);
            txtLicenseNum.Text = car.LicensePlateNumber;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Added Validation for make and model
                if (string.IsNullOrEmpty(txtMake.Text) ||
                    string.IsNullOrEmpty(txtModel.Text))
                {
                    MessageBox.Show("Please ensure that you provide a make and a model");
                }
                else
                {
                    //if(isEditMode == true)
                    if (isEditMode)
                    {
                        // edit car info
                        var id = int.Parse(lblId.Text);
                        var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);
                        car.Model = txtModel.Text;
                        car.Make = txtMake.Text;
                        car.VIN = txtVIN.Text;
                        car.Year = Convert.ToInt32(txtYear.Text);
                        car.LicensePlateNumber = txtLicenseNum.Text;
                    }
                    else
                    {
                        // add car info
                        var newCar = new TypesOfCar
                        {
                            LicensePlateNumber = txtLicenseNum.Text,
                            Make = txtMake.Text,
                            Model = txtModel.Text,
                            VIN = txtVIN.Text,
                            Year = Convert.ToInt32(txtYear.Text),
                        };

                        _db.TypesOfCars.Add(newCar);
                    }
                    _db.SaveChanges();
                    _manageVehicleListing.PopulateGrid();
                    MessageBox.Show("Operation Completed. Refresh Grid to see changes");
                    Close();
                   
                }
                
            }
            catch (Exception)
            {

                MessageBox.Show("Unable to save");
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
