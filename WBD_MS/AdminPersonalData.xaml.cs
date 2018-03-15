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
    /// <summary>
    /// Interaction logic for AdminPersonalData.xaml
    /// </summary>
    public partial class AdminPersonalData : Window
    {
        public AdminPersonalData()
        {
            InitializeComponent();
        }

        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        Administrator administrator_window;
        int ID;

        public AdminPersonalData(int ID, SqlConnection conn, Administrator administrator_window)
        {
            this.ID = ID;
            this.administrator_window = administrator_window;
            connection = new SqlConnection();
            connection = conn;
            InitializeComponent();
            LoadParameters();
            InitializeComponent();
        }

        public void LoadParameters()
        {
            try
            {
                DataTable datatable = new DataTable();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string IDtoString = ID.ToString();
                command = new SqlCommand("Select * FROM [dbo].[Pracownicy] Where ID_Pracownika =" + IDtoString, connection);
                reader = command.ExecuteReader();

                datatable.Load(reader);

                if (datatable.Rows.Count == 1)
                {
                    txtFirstName.Text = datatable.Rows[0].ItemArray[1].ToString();
                    txtLastName.Text = datatable.Rows[0].ItemArray[2].ToString();

                    txtPESEL.Text = datatable.Rows[0].ItemArray[3].ToString();

                    txtBirthDate.Text = datatable.Rows[0].ItemArray[4].ToString();

                    txtEmployment.Text = datatable.Rows[0].ItemArray[5].ToString();

                    txtAccount.Text = datatable.Rows[0].ItemArray[7].ToString();

                    txtPosition.Text = datatable.Rows[0].ItemArray[6].ToString();

                    txtTelephone.Text = datatable.Rows[0].ItemArray[8].ToString();

                    txtEmail.Text = datatable.Rows[0].ItemArray[9].ToString();

                }
            }
            catch (Exception error)
            {

                MessageBox.Show(error.Message);
            }

        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable datatable = new DataTable();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string commandstring = "Update Pracownicy set Imię ='" + txtFirstName.Text + "', Nazwisko ='" + txtLastName.Text + "', PESEL = '" + txtPESEL.Text + "', Data_urodzenia = '" + txtBirthDate.Text.ToString() + "', data_zatrudnienia = '" + txtEmployment.Text.ToString() + "', stanowisko = '" + txtPosition.Text + "', Nume_konta_bankowego = '" + txtAccount.Text + "', numer_telefonu = '" + txtTelephone.Text + "', adres_email = ' " + txtEmail.Text + "'" + " where ID_pracownika = " + ID;
                command = new SqlCommand(commandstring, connection);
                command.ExecuteNonQuery();
                this.Close();
                administrator_window.Show();
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
            administrator_window.Show();
        }


    }
}
