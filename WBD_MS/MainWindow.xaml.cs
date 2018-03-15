using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;


namespace WBD_MS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection conn;
        private DataTable dt;
        private SqlDataReader reader;
        
        private string string_id;
        //public MainWindow main_window;
        public MainWindow()
        {
            InitializeComponent();
            //main_window = new MainWindow();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            dt = new DataTable();
            txtPassword.PasswordChar = '*';
            string role = String.Empty;
            string profession = String.Empty;
            int id;
            
            conn = new SqlConnection(ConfigurationManager.AppSettings["constring"]);
            
            try
            {
                
                conn.Open();
                SqlCommand command = new SqlCommand("Select * FROM[dbo].[Konta] Where username='"  + txtUserName.Text +  "' and hasło = '" + txtPassword.Password.ToString() + "'" , conn);
                reader = command.ExecuteReader();
              
                dt.Load(reader);
                if (dt.Rows.Count > 0)
                {
                    role = dt.Rows[0].ItemArray[2].ToString();

                    string_id = dt.Rows[0].ItemArray[3].ToString();
                }
                

                reader.Close();
                conn.Close();


                if (dt.Rows.Count == 1)
                {
                    if (role.Contains("prezes"))
                    {

                    }
                    else if (role.Contains("klient"))
                    {

                    }
                    else if (role.Contains("administrator"))
                    {
                        id = Int32.Parse(string_id);
                        Administrator admin = new Administrator(id, conn, txtPassword.Password.ToString(), this);
                        txtUserName.Text = String.Empty;
                        txtPassword.Clear();
                        
                        this.Hide();
                        admin.Show();
                    }
                    else if(role.Contains("kierownik"))
                    {
                        id = Int32.Parse(string_id);
                        Pracownik pracownik = new Pracownik(id, conn, txtPassword.Password.ToString(), this);
                        txtUserName.Text = String.Empty;
                        txtPassword.Clear();
                       
                        this.Hide();
                        pracownik.Show();
                    }
                }
                else
                {
                    MessageBox.Show("There is no user with these credentials in database. Write your username and login once again.");
                    txtUserName.Text = String.Empty;
                    txtPassword.Clear();
                    conn.Close();
                }

            }

            catch(Exception error)
            {
                MessageBox.Show(error.Message);
                txtUserName.Text = String.Empty;
                txtPassword.Clear();
                conn.Close();
            }

}
    }
}
