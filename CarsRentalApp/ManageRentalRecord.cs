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
    public partial class frmManageRentalRecord : Form
    {
        private readonly CarRentalEntities2 _db;
        public frmManageRentalRecord()
        {
            InitializeComponent();
            _db = new CarRentalEntities2();
        }

        private void btnAddNewRecord_Click(object sender, EventArgs e)
        {
            var addRentalCord = new frmAddEditRentalRecord()
            {
                MdiParent = this.MdiParent
            };
            addRentalCord.Show();
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (int)dgvRecordList.SelectedRows[0].Cells["Id"].Value;

                // query database for record
                var record = _db.CarRentalRecords
                    .FirstOrDefault(q => q.id == id);

                // launch AddEditVehicle window with data
                var addEditRentalVehicle = new frmAddEditRentalRecord(record);
                addEditRentalVehicle.MdiParent = this.MdiParent;
                addEditRentalVehicle.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)dgvRecordList.SelectedRows[0].Cells["Id"].Value;

                // query database for record
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                // delete vehicle from table
                _db.CarRentalRecords.Remove(record);
                _db.SaveChanges();

                PopulateGrid();
            }
            catch
            {

                MessageBox.Show("Unable to Delete car");
            }
        }

        private void btnRefreshRecord_Click(object sender, EventArgs e)
        {

        }

        private void frmManageRentalRecord_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateGrid()
        {
            // Select a custom model collection of cars from database
            var records = _db.CarRentalRecords
                .Select(q => new
                {
                    Customer = q.CustomerName,
                    DateOut = q.DateRented,
                    DateIn = q.DateReturned,
                    Id = q.id,
                    q.Cost,
                    car = q.TypesOfCar.Make + " " + q.TypesOfCar.Model
                })
                .ToList();
            
            dgvRecordList.DataSource = records;
            dgvRecordList.Columns["DateIn"].HeaderText = "Date In";
            dgvRecordList.Columns["DateOut"].HeaderText = "Date out";
            dgvRecordList.Columns["Id"].Visible = false;

        }
    }
}
