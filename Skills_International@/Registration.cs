using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skills_International_
{
    public partial class Registration : Form
    {
        //SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-LBUQURH\SQLEXPRESS;Initial Catalog=Student_@DB;Integrated Security=True");
        SqlConnection con = new SqlConnection(@"Server=DESKTOP-O1G7G3L;Database=skills_International;Trusted_Connection=True;MultipleActiveResultSets=true");
        string First_Name, Last_Name, Gender, Address, Email, Parent_Name, NIC, Reg_No, Mobile_Phone, Home_Phone, Contact_No;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login f = new Login();
            this.Hide();
            f.Show();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure, Do you really want to Exit...?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Login f = new Login();
                this.Hide();
                f.Show();
            }
        }

        DateTime dob;
        public Registration()
        {
            InitializeComponent();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            LoadElement();

            if(Reg_No == "")
            {
                MessageBox.Show("Please Add a Registration Number", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (First_Name == "")
            {
                MessageBox.Show("Please Add a First Name", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dob == DateTime.Today)
            {
                MessageBox.Show("Please Add a Valid BirthDay", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Gender == "")
            {
                MessageBox.Show("Please Add your Gender", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SqlConnection con = new SqlConnection(@"Server=DESKTOP-O1G7G3L;Database=skills_International;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                await con.OpenAsync();

                string add = "INSERT INTO dbo.Registration (Reg_No, FirstName, LastName, DOB, Sex, Address, Email, MobileNumber, HomeNumber, ParentName, NIC, ContactNumber)" +
                             "VALUES(@Reg_No, @FirstName, @LastName, @DOB, @Sex, @Address, @Email, @MobileNumber, @HomeNumber, @ParentName, @NIC, @ContactNumber)";

                using (SqlCommand cmd = new SqlCommand(add, con))
                {
                    cmd.Parameters.AddWithValue("@Reg_No", Reg_No);
                    cmd.Parameters.AddWithValue("@FirstName", First_Name);
                    cmd.Parameters.AddWithValue("@LastName", Last_Name);
                    cmd.Parameters.AddWithValue("@DOB", dob);
                    cmd.Parameters.AddWithValue("@Sex", Gender);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@MobileNumber", Mobile_Phone);
                    cmd.Parameters.AddWithValue("@HomeNumber", Home_Phone);
                    cmd.Parameters.AddWithValue("@ParentName", Parent_Name);
                    cmd.Parameters.AddWithValue("@NIC", NIC);
                    cmd.Parameters.AddWithValue("@ContactNumber", Contact_No);

                    await cmd.ExecuteNonQueryAsync();
                }

                MessageBox.Show("Record Added Successfully", "Register Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Clear();
        }


        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            LoadElement();

            if (String.IsNullOrEmpty(Reg_No))
            {
                MessageBox.Show("Please Add a Registration Number", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool recordExists = await CheckIfRecordExists(Reg_No);

            if (!recordExists)
            {
                MessageBox.Show("No record found with the specified Registration Number", "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Update the record in the database
            using (SqlConnection con = new SqlConnection(@"Server=DESKTOP-O1G7G3L;Database=skills_International;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                await con.OpenAsync();

                string updateQuery = "UPDATE dbo.Registration SET FirstName = @FirstName, LastName = @LastName, DOB = @DOB, " +
                                     "Sex = @Sex, Address = @Address, Email = @Email, MobileNumber = @MobileNumber, " +
                                     "HomeNumber = @HomeNumber, ParentName = @ParentName, NIC = @NIC, ContactNumber = @ContactNumber " +
                                     "WHERE Reg_No = @Reg_No";

                using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Reg_No", Reg_No);
                    cmd.Parameters.AddWithValue("@FirstName", First_Name);
                    cmd.Parameters.AddWithValue("@LastName", Last_Name);
                    cmd.Parameters.AddWithValue("@DOB", dob);
                    cmd.Parameters.AddWithValue("@Sex", Gender);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@MobileNumber", Mobile_Phone);
                    cmd.Parameters.AddWithValue("@HomeNumber", Home_Phone);
                    cmd.Parameters.AddWithValue("@ParentName", Parent_Name);
                    cmd.Parameters.AddWithValue("@NIC", NIC);
                    cmd.Parameters.AddWithValue("@ContactNumber", Contact_No);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record Updated Successfully", "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update record", "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async Task<bool> CheckIfRecordExists(string regNo)
        {
            bool recordExists = false;

            using (SqlConnection con = new SqlConnection(@"Server=DESKTOP-O1G7G3L;Database=skills_International;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                await con.OpenAsync();

                string selectQuery = "SELECT COUNT(*) FROM dbo.Registration WHERE Reg_No = @Reg_No";

                using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Reg_No", regNo);

                    int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                    recordExists = (count > 0);
                }
            }

            return recordExists;
        }


        private async void btnDelete_Click(object sender, EventArgs e)
        {
            LoadElement();

            if (String.IsNullOrEmpty(Reg_No))
            {
                MessageBox.Show("Please Add a Registration Number", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SqlConnection con = new SqlConnection(@"Server=DESKTOP-O1G7G3L;Database=skills_International;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                await con.OpenAsync();

                string deleteQuery = "DELETE FROM dbo.Registration WHERE Reg_No = @Reg_No";

                using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Reg_No", Reg_No);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record(s) Deleted Successfully", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No record found with the specified Registration Number", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            Clear();
        }


        private void LoadElement()
        {

            Reg_No = cmbRegNo.Text;
            First_Name = txtFirstName.Text;
            Last_Name = txtLastName.Text;
            dob = dtpDOB.Value.Date;
            if (rdbMale.Checked == true)
            {
                Gender = "male";
            }
            else if(rdbFemale.Checked == true)
            {
                Gender = "female";
            }
            else
            {
                Gender = "";
            }
            Address = txtAddress.Text;
            Email = txtEmail.Text;
            Mobile_Phone = txtMobilePhone.Text;
            Home_Phone = txtHomePhone.Text;
            Parent_Name = txtParentName.Text;
            NIC = txtNIC.Text;
            Contact_No = txtContactNo.Text;
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbRegNo.ResetText();
            txtFirstName.Clear();
            txtLastName.Clear();
            dtpDOB.ResetText();
            rdbMale.Checked = false;
            rdbFemale.Checked = false;
            txtAddress.Clear();
            txtEmail.Clear();
            txtMobilePhone.Clear();
            txtHomePhone.Clear();
            txtParentName.Clear();
            txtNIC.Clear();
            txtContactNo.Clear();
            cmbRegNo.Focus();
        }


        private void Clear()
        {
            cmbRegNo.ResetText();
            txtFirstName.Clear();
            txtLastName.Clear();
            dtpDOB.ResetText();
            rdbMale.Checked = false;
            rdbFemale.Checked = false;
            txtAddress.Clear();
            txtEmail.Clear();
            txtMobilePhone.Clear();
            txtHomePhone.Clear();
            txtParentName.Clear();
            txtNIC.Clear();
            txtContactNo.Clear();
            cmbRegNo.Focus();
        }

        private void cmbRegNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtContactNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtHomePhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtMobilePhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
