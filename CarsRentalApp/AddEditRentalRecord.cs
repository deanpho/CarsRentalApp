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
    public partial class frmAddEditRentalRecord : Form
    {
        private bool isEditMode;
        private readonly CarRentalEntities2 _db;

        public frmAddEditRentalRecord()
        {
            InitializeComponent();
            lblTitle.Text = "Add New Rental Record";
            this.Text = "Add New Rental Record";
            isEditMode = false;
            _db = new CarRentalEntities2();
        }

        public frmAddEditRentalRecord(CarRentalRecord recordToEdit)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Rental Record";
            this.Text = "Edit Rental Record";

            if (recordToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities2();
                PopulateFields(recordToEdit);
            }
        }

        private void PopulateFields(CarRentalRecord recordToEdit)
        {
            txtCustomerName.Text = recordToEdit.CustomerName;
            dtpRented.Value = Convert.ToDateTime(recordToEdit.DateRented);
            dtpRented.Value = (DateTime)recordToEdit.DateReturned;
            txtCost.Text = Convert.ToString(recordToEdit.Cost);
            txtCustomerName.Text = recordToEdit.CustomerName;
            lblRecordId.Text = recordToEdit.id.ToString();

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = txtCustomerName.Text;
                //DateTime dateOut = Convert.ToDateTime(dtpRented.Value.ToString()); // same condition as -var dateOut = dtpRented.Value;-
                //DateTime dateIn = Convert.ToDateTime(dtpReturned.Value.ToString());
                var dateOut = dtpRented.Value;
                var dateIn = dtpReturned.Value;
                double cost = Convert.ToDouble(txtCost.Text);

                var carType = cboTypeofCar.Text;
                bool isValid = true;
                var errorMessage = "";

                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isValid = false;
                    errorMessage += "Error: Please provide missing data.\n\r";
                }

                if (dateOut > dateIn)
                {
                    isValid = false;
                    errorMessage += "Error: Illegal Date Selection.\n\r";
                }

                //if (isValid == true)
                if (isValid)
                {
                    // Declare an object of the record to be added
                    var rentalRecord = new CarRentalRecord();
                    if (isEditMode)
                    {
                        // if in edit mode, then get the ID and retrieve the record from the _db and place 
                        // the result in the record object
                        var id = int.Parse(lblRecordId.Text);
                        rentalRecord = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);
                    }

                    // Populate record object with values from the form
                    rentalRecord.CustomerName = customerName;
                    rentalRecord.DateRented = dateOut;
                    rentalRecord.DateReturned = dateIn;
                    rentalRecord.Cost = (decimal)cost;
                    rentalRecord.TypeOfCarID = (int)cboTypeofCar.SelectedValue;

                    // If not in edit mode, then add the record object to the database
                    if (!isEditMode)
                    {
                        _db.CarRentalRecords.Add(rentalRecord);
                    }

                    // Save Changes made to the entity
                    _db.SaveChanges();

                    MessageBox.Show($"Customer Name: {customerName}\n\r"
                        + $"Date Rented: {dateOut}\n\r"
                        + $"Date Returned: {dateIn}\n\r"
                        + $"Cost: {cost}\n\r"
                        + $"Car Type: {carType}\n\r"
                        + "Thank you for your business!");

                    Close();
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                //if (txtCustomerName.Text == string.Empty)
                //    MessageBox.Show("Please enter your name") ;
                //if (txtCost.Text == string.Empty)
                //    MessageBox.Show("Please enter the cost for vehiclen");
                //if (cboTypeofCar.SelectedItem == null)
                //    MessageBox.Show("Please select a car brand");
                //MessageBox.Show(ex.Message);
                //throw;
                MessageBox.Show(ex.Message);

            }

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //select * from TypesOfCars
            //var cars = carRentalEntities1.TypesOfCars.ToList();

            var cars = _db.TypesOfCars
                .Select (q => new {Id = q.Id, Name = q.Make + " " + q.Model})
                .ToList();
            cboTypeofCar.DisplayMember = "Name";
            cboTypeofCar.ValueMember = "Id";
            cboTypeofCar.DataSource = cars;
        }

        
    }
}
