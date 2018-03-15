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
    /// Interaction logic for DeleteWindow.xaml
    /// </summary>
    public partial class DeleteWindow : Window
    {
        DataTable data_table;
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

        public DeleteWindow(SqlConnection conn, string tableName)
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
                MessageBox.Show("Error \n" + exp.Message);

            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            try
            {

           
            temporaryView = "Delete from " + tableToShow + " where " + textBoxColumnName.Text.ToString() + " = '" +
                            textBoxValue.Text.ToString() + "'";

            command = new SqlCommand(temporaryView, connection);
            command.ExecuteNonQuery();

            command = new SqlCommand("Select * FROM [dbo].[" + tableToShow + "]", connection);
            command.ExecuteNonQuery();
            data_adapter = new SqlDataAdapter(command);
            datatable = new DataTable(tableToShow);
            data_adapter.Fill(datatable);
            dataGrid.ItemsSource = datatable.DefaultView;
            data_adapter.Update(datatable);
            connection.Close();
            }
            catch(Exception exp)
            {
                MessageBox.Show("Error \n" + exp.Message);

            }
        }
    }
}
