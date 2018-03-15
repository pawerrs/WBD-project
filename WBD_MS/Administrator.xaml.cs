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
    /// Interaction logic for Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
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

        public Administrator(int ID, SqlConnection conn, string password, MainWindow main_window)
        {
            this.ID = ID;
            this.password = password;
            this.main_window = main_window;
            connection = new SqlConnection();
            connection = conn;
            InitializeComponent();
            comboBox.Items.Add("Konta");
            comboBox.Items.Add("Placówki");
            comboBox.Items.Add("Sprzęt");
            comboBox.Items.Add("Pracownicy");
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
                command = new SqlCommand("Select * FROM [dbo].[Pracownicy] Where ID_Pracownika =" + IDtoString, connection);
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
                connection.Close();
                MessageBox.Show(error.Message);
            }

        }

        private void buttonLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            main_window.Show();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            string tableName = comboBox.SelectedItem.ToString();
            if (!tableName.Equals(""))
            {
                if (tableName.Equals("Placówki"))
                {
                    tableName = "Placówki_sprzedaży";
                }
                else if (tableName.Equals("Konta"))
                {
                    tableName = "Konta";
                }
                else if (tableName.Equals("Pracownicy"))
                {
                    tableName = "Pracownicy";
                }
                else
                {
                    tableName = "Sprzęty";
                }

                AddingWindow addingdisplay = new AddingWindow(connection, tableName);
                addingdisplay.Show();
            }
            else
            {
                MessageBox.Show("Please enter name of the table");
            }
            }catch(Exception err)
            {
                MessageBox.Show("Please enter name of the table");
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            try { 
            string tableName = comboBox.SelectedItem.ToString();
            if (!tableName.Equals(""))
            {
                if (tableName.Equals("Placówki"))
                {
                    tableName = "Placówki_sprzedaży";
                }
                else if (tableName.Equals("Konta"))
                {
                    tableName = "Konta";
                }
                else if (tableName.Equals("Pracownicy"))
                {
                    tableName = "Pracownicy";
                }
                else
                {
                    tableName = "Sprzęty";
                }

                DeleteWindow delete = new DeleteWindow(connection, tableName);
                delete.Show();
            }
            else
            {
                MessageBox.Show("Please enter name of the table");
            }
            }
            catch(Exception err)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void buttonChange1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            string tableName = comboBox.SelectedItem.ToString();
            if (!tableName.Equals(""))
            {
                if (tableName.Equals("Placówki"))
                {
                    tableName = "Placówki_sprzedaży";
                }
                else if (tableName.Equals("Konta"))
                {
                    tableName = "Konta";
                }
                else if (tableName.Equals("Pracownicy"))
                {
                    tableName = "Pracownicy";
                }
                else
                {
                    tableName = "Sprzęty";
                }

                UpdateWindow addingdisplay = new UpdateWindow(connection, tableName);
                addingdisplay.Show();
            }
            else
            {
                MessageBox.Show("Please enter name of the table");
            }

            }
            catch(Exception rre)
            {
                MessageBox.Show("Please enter name of the table");
            }
        }

        private void buttonShow_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
            string tableName = comboBox.SelectedItem.ToString();
            if (!tableName.Equals(""))
            {
                if (tableName.Equals("Placówki"))
                {
                    tableName = "Placówki_sprzedaży";
                }
                else if (tableName.Equals("Konta"))
                {
                    tableName = "Konta";
                }
                else if (tableName.Equals("Pracownicy"))
                {
                    tableName = "Pracownicy";
                }
                else
                {
                    tableName = "Sprzęty";
                }
                DisplayTableWindow display = new DisplayTableWindow(connection, tableName);
                display.Show();
            }
            else
            {
                MessageBox.Show("Please enter name of the table");
            }
            }
            catch(Exception er)
            {
                MessageBox.Show("Please enter name of the table");
            }
        }

        private void buttonChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PersonalData personaldata_window = new PersonalData(ID, connection, this);
                connection.Close();
                this.Hide();
                personaldata_window.Show();
            }
            catch (Exception err)
            {

                MessageBox.Show("Błąd \n" + err.Message);
            }
        }
    }
}
