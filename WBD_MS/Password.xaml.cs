using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WBD_MS
{


    
    public partial class Password : Window
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        Pracownik pracownik_window;
        string password;
        int ID;
        public Password(int ID, SqlConnection conn,string password, Pracownik pracownik_window)
        {
            this.ID = ID;
            this.pracownik_window = pracownik_window;
            connection = new SqlConnection();
            connection = conn;
            this.password = password;
            InitializeComponent();
           
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
           if(txtCurrentPassword.Password.ToString() == password)
            {
                if(txtPassword.Password.ToString() == txtPasswordConfirmation.Password.ToString())
                {
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        string commandstring = "Update Konta set Hasło ='" + txtPassword.Password.ToString() + "' where ID_pracownika = " + ID;
                        command = new SqlCommand(commandstring, connection);
                        command.ExecuteNonQuery();
                        this.Close();
                       
                    }
                    catch (Exception err)
                    {
                        connection.Close();
                        MessageBox.Show(err.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Confirmation password must be the same as New Passowrd");
                    txtCurrentPassword.Clear();
                    txtPassword.Clear();
                    txtPasswordConfirmation.Clear();
                }


            }
            else
            {
                MessageBox.Show("Please write correctly your current password");
                txtCurrentPassword.Clear();
                txtPassword.Clear();
                txtPasswordConfirmation.Clear();

            }
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
            pracownik_window.Show();
        }
    }
}
