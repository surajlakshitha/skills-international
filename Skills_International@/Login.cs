using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Skills_International_
{
    public partial class Login : Form
    {
        SqlConnection con = new SqlConnection(@"Server=DESKTOP-O1G7G3L;Database=skills_International;Trusted_Connection=True;MultipleActiveResultSets=true");
        string Username, Password;
        public Login()
        {
            InitializeComponent();
        }


        private void Login_(object sender, EventArgs e)
        {
            string enteredUsername = txtUsername.Text;
            string enteredPassword = txtPassword.Text;

            // Validate if both username and password are provided
            if (string.IsNullOrWhiteSpace(enteredUsername) || string.IsNullOrWhiteSpace(enteredPassword))
            {
                MessageBox.Show("Please enter both username and password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection con = new SqlConnection(@"Server=DESKTOP-O1G7G3L;Database=skills_International;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                con.Open();

                string selectQuery = "SELECT COUNT(*) FROM dbo.Login WHERE Username = @Username AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Username", enteredUsername);
                    cmd.Parameters.AddWithValue("@Password", enteredPassword);

                    int count = (int)cmd.ExecuteScalar();

                    // If count is greater than 0, the username and password are correct
                    if (count > 0)
                    {
                        // Login successful, navigate to the main form or perform further actions
                        MessageBox.Show("Login successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // For example, navigate to the main form:
                        Registration R = new Registration();
                        this.Hide();
                        R.Show();
                    }
                    else
                    {
                        // Login failed, display error message
                        MessageBox.Show("Invalid credentials. Please check your username and password and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void Clear(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
        }

        private void Exit(object sender, EventArgs e)
        {
            this.Close();
        }
    }


}
