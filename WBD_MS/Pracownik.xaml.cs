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
    /// Interaction logic for Pracownik.xaml
    /// </summary>
    public partial class Pracownik : Window
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        SqlDataAdapter data_adapter;
        SqlCommandBuilder command_builder;
        DataTable datatable;
        string password;
        MainWindow main_window;
        int ID;

        public Pracownik(int ID, SqlConnection conn, string password, MainWindow main_window)
        {
            this.ID = ID;
            this.password = password;
            this.main_window = main_window;
            connection = new SqlConnection();
            connection = conn;
            InitializeComponent();
            LoadParameters();

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
                command = new SqlCommand("Select * FROM [dbo].[Pracownicy] Where ID_Pracownika =" + IDtoString , connection);
                reader = command.ExecuteReader();

                datatable.Load(reader);
                connection.Close();
                if (datatable.Rows.Count == 1)
                {
                    textBox_name.Text = datatable.Rows[0].ItemArray[1].ToString() + " " + datatable.Rows[0].ItemArray[2].ToString();
                    textBox_name.IsReadOnly = true;
                    textBoxPESEL.Text = datatable.Rows[0].ItemArray[3].ToString();
                    textBoxPESEL.IsReadOnly = true;
                    textBoxBirth.Text = datatable.Rows[0].ItemArray[4].ToString();
                    textBoxBirth.IsReadOnly = true;
                    textBoxEmployment.Text = datatable.Rows[0].ItemArray[5].ToString();
                    textBoxEmployment.IsReadOnly = true;
                    textBoxAccount.Text = datatable.Rows[0].ItemArray[7].ToString();
                    textBoxAccount.IsReadOnly = true;
                    textBoxprofession.Text = datatable.Rows[0].ItemArray[6].ToString();
                    textBoxprofession.IsReadOnly = true;
                    textBoxphone.Text = datatable.Rows[0].ItemArray[8].ToString();
                    textBoxphone.IsReadOnly = true;
                    textBoxEmail.Text = datatable.Rows[0].ItemArray[9].ToString();
                    textBoxEmail.IsReadOnly = true;
                }
            }
            catch (Exception error)
            {
               
                MessageBox.Show(error.Message);
            }

        }

        /// <summary>
        /// wyswietlenie listy klientów po nacisnięciu przycisku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            datatable = new DataTable();
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                if(textBoxclients.Text == string.Empty)
                {
                    command = new SqlCommand("Select ID_klienta, Imie, Nazwisko, PESEL, Data_urodzenia, Nr_Dokumentu_Tożsamości, Data_ważności_Dokumentu, Numer_konta_bankowego, Numer_telefonu, Adres_email FROM [dbo].[Klienci]", connection);
                    command.ExecuteNonQuery();
                }
                else
                {
                    int id = Int32.Parse(textBoxclients.Text.ToString());
                    command = new SqlCommand("Select ID_klienta, Imie, Nazwisko, PESEL, Data_urodzenia, Nr_Dokumentu_Tożsamości, Data_ważności_Dokumentu, Numer_konta_bankowego, Numer_telefonu, Adres_email FROM [dbo].[Klienci] where ID_klienta = " + id, connection);
                    command.ExecuteNonQuery();
                }
                data_adapter = new SqlDataAdapter(command);
                datatable = new DataTable("Klienci");
                data_adapter.Fill(datatable);
                dataGrid.ItemsSource = datatable.DefaultView;
                data_adapter.Update(datatable);

            }
            catch (Exception exp)
            {
                
                MessageBox.Show("Błąd \n" + exp.Message);

            }
        }


        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PersonalData personaldata_window = new PersonalData(ID, connection, this);
                connection.Close();
                this.Hide();
                personaldata_window.Show();
            }
            catch(Exception err)
            {
              
                MessageBox.Show("Błąd \n" + err.Message);
            }

        }
        
        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                command_builder = new SqlCommandBuilder(data_adapter);
                data_adapter.Update(datatable);
            }
            catch(Exception err)
            {
                
                MessageBox.Show("Błąd \n" + err.Message);
            }
           
        }




        private void buttonLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            main_window.Show();
        }

        private void buttonPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Password password_window = new Password(ID, connection, password,   this);
                connection.Close();
                
                password_window.Show();
            }
            catch (Exception err)
            {
                connection.Close();
                MessageBox.Show("Błąd \n" + err.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
            main_window.Show();
        }

    }
}
