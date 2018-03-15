using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
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
        public string tableToShow;
        public string temporaryView = "";

        public UpdateWindow(SqlConnection conn, string tableName)
        {
            connection = new SqlConnection();
            connection = conn;
            InitializeComponent();
            tableToShow = tableName;
            datatable = new DataTable();
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                command = new SqlCommand("Select * FROM [dbo].[" + tableName + "]", connection);
                command.ExecuteNonQuery();
                data_adapter = new SqlDataAdapter(command);
                datatable = new DataTable(tableName);
                data_adapter.Fill(datatable);
                dataGrid.ItemsSource = datatable.DefaultView;
                data_adapter.Update(datatable);
                connection.Close();


            }
            catch (Exception exp)
            {
                connection.Close();
                MessageBox.Show("Błąd \n" + exp.Message);

            }
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();

                }
                data_adapter = new SqlDataAdapter("SELECT * FROM " + tableToShow, connection);
                //Stworzenie SqlCommandBuildera, którt wysyła odpowiednie żądania do bazy danych w momencie uruchomienia DataAdaptera (działa jako słuchacz).
                command_builder = new SqlCommandBuilder(data_adapter);
                //Powoduje zaktualizowanie tablicy w bazie danych.
                data_adapter.Update(datatable);

            }
            catch (Exception err)
            {
                connection.Close();
                MessageBox.Show("Błąd \n" + err.Message);
            }
        }
    }
}
