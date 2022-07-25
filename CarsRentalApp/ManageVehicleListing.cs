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
    public partial class frmManageVehicleListing : Form
    {

        private readonly CarRentalEntities2 _db;
        public frmManageVehicleListing()
        {
            InitializeComponent();
            _db = new CarRentalEntities2();
        }

        private void frmManageVehicleListing_Load(object sender, EventArgs e)
        {
            //Select * From TypeOfCars
            //Var cars = _db.TypesOfCars.ToList();

            //Select id as CarId, name as CarName from TypeOfCars
            //var cars = _db.TypesOfCars
            //    .Select(q => new { CarId = q.Id, CarName = q.Make})
            //    .ToList();

            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)dgvVehicleList.SelectedRows[0].Cells["Id"].Value;

                // query database for record
                var car = _db.TypesOfCars
                    .FirstOrDefault(q => q.Id == id);

                // launch AddEditVehicle window with data
                var addEditVehicle = new frmAddEditVehicle(car, this);
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();
            }
            catch
            {
                MessageBox.Show("Unable to Edit car");
            }

        }
        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)dgvVehicleList.SelectedRows[0].Cells["Id"].Value;

                // query database for record
                var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);

                DialogResult dr = MessageBox.Show("Are you sure to delete this record?",
                    "Delete", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                // if user click yes, remove record
                if (dr == DialogResult.Yes)
                {
                    // delete vehicle from table
                    _db.TypesOfCars.Remove(car);
                    _db.SaveChanges();
                }
                PopulateGrid();
            }
            catch
            {

                MessageBox.Show("Delete Operation failed");
            }


            //     get Id of selected row
            //var id = (int)dgvVehicleList.SelectedRows[0].Cells["Id"].Value;

            //     query database for record
            //    var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);

            //      if user click yes, remove record

            //          delete vehicle from table
            //     _db.TypesOfCars.Remove(car);
            //     _db.SaveChanges();

        }

        public void PopulateGrid()
        {
            // Select a custom model collection of cars from database
            var car = _db.TypesOfCars
                .Select(q => new
                {
                    q.Id,
                    Make = q.Make,
                    Model = q.Model,
                    Year = q.Year,
                    VIN = q.VIN,
                    LicensePlateNumber = q.LicensePlateNumber,

                })
                .ToList();
            dgvVehicleList.DataSource = car;
            dgvVehicleList.Columns[5].HeaderText = "License Plate Number";
            // hide the column for ID. Changed from the hard coded column value to the name,
            // to make it more dynamic.
            dgvVehicleList.Columns["Id"].Visible = false;

        }
        private void btnAddNewCar_Click(object sender, EventArgs e)
        {
            try
            {
                var addEditVehicle = new frmAddEditVehicle(this);
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();
            }
            catch
            {

                MessageBox.Show("Unable to Add New Car");
            }

        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {

            try
            {
                PopulateGrid();
            }
            catch (Exception)
            {

                MessageBox.Show("method PopulateGrid is not working");
            }

        }
    }
}
