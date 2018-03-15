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
    /// Interaction logic for DisplayTableWindow.xaml
    /// </summary>
    public partial class DisplayTableWindow : Window
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        SqlDataAdapter data_adapter;
        SqlCommandBuilder command_builder;
        DataTable datatable;
       
        public string tableToShow;
        public string temporaryView = "";

        public DisplayTableWindow(SqlConnection conn, string tableName)
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

        private void buttonFilterIt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                if (temporaryView.Equals(""))
                {
                    temporaryView = "Select * FROM [dbo].[" + tableToShow + "]" + " WHERE "
                                    + textBoxColumnName.Text.ToString() + textBoxSign.Text.ToString()
                                    + "'" + textBoxValue.Text.ToString() + "'";
                }
                else
                {
                    temporaryView += " " + textBoxOperation.Text.ToString() + " " + textBoxColumnName.Text.ToString() + textBoxSign.Text.ToString()
                                     + "'" + textBoxValue.Text.ToString() + "'";
                }

                command = new SqlCommand(temporaryView, connection);
                command.ExecuteNonQuery();
                data_adapter = new SqlDataAdapter(command);
                datatable = new DataTable(tableToShow);
                data_adapter.Fill(datatable);
                dataGrid.ItemsSource = datatable.DefaultView;
                data_adapter.Update(datatable);
                connection.Close();
            }
            catch (Exception exp)
            {
                temporaryView = "";
                connection.Close();
                MessageBox.Show("Error \n" + exp.Message);

            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            temporaryView = "";

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                command = new SqlCommand("Select * FROM [dbo].[" + tableToShow + "]", connection);
                command.ExecuteNonQuery();
                data_adapter = new SqlDataAdapter(command);
                datatable = new DataTable(tableToShow);
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
    }
}
