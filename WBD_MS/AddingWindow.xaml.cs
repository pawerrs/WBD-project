using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WBD_MS
{
    /// <summary>
    /// Interaction logic for AddingWindow.xaml
    /// </summary>
    public partial class AddingWindow : Window
    {
        public bool isB2pressed;
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        SqlDataAdapter data_adapter;
        SqlCommandBuilder command_builder;
        DataTable datatable;
        
        int ID;
        public string tableToShow;
        public string temporaryView = "";
        private readonly ManualResetEvent mre = new ManualResetEvent(false);


        public AddingWindow(SqlConnection conn, string tableName)
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
                buttonAdd.Content = "Press to START";
            }
            catch (Exception exp)
            {
                connection.Close();
                MessageBox.Show("Error \n" + exp.Message);

            }
        }






        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {

              
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                int text = 0;
                List<string> listacolumnas = new List<string>();

                var temporaryView = "SELECT count(*) FROM information_schema.columns WHERE table_name = '" + tableToShow +
                                    "'";
                command = new SqlCommand(temporaryView, connection);
                SqlDataReader reader2 = command.ExecuteReader();
                while (reader2.Read())
                {
                    text = reader2.GetInt32(0);
                }

                reader2.Close();

                command.CommandText =
                    "select c.name from sys.columns c inner join sys.tables t on t.object_id = c.object_id and t.name = '" +
                    tableToShow + "' and t.type = 'U'";
                using (SqlDataReader reader1 = command.ExecuteReader())
                {
                    while (reader1.Read())
                    {
                        listacolumnas.Add(reader1.GetString(0));
                    }

                    reader1.Close();
                }

                //INSERT INTO table_name VALUES(value1, value2, value3, ...);
                string temporaryCommand = "INSERT INTO " + tableToShow + " VALUES(";


                for (int i = 0; i < text; i++)
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() => textBoxColumnName.Text = listacolumnas[i]));


                    isB2pressed = false;
                    while (isB2pressed == false)
                    {

                    }
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() => temporaryCommand += "'" + textBoxValue.Text.ToString() + "', "));
                }/*
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() => temporaryCommand += "'" + textBoxValue.Text +"')"));*/
                 /*Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                     new Action(() => temporaryCommand += "'" + textBoxValue.Text.ToString() + "', "));
                 for (int z = 0; z < 200000; z++)
                 {

                 }*/
                isB2pressed = false;
                while (isB2pressed == false)
                {

                }
                var myString = temporaryCommand.Substring(0, temporaryCommand.Length - 2);
                temporaryCommand = myString + ")";
                command = new SqlCommand(temporaryCommand, connection);
                command.ExecuteNonQuery();
                command = new SqlCommand("Select * FROM [dbo].[" + tableToShow + "]", connection);
                command.ExecuteNonQuery();

                data_adapter = new SqlDataAdapter(command);
                datatable = new DataTable(tableToShow);
                data_adapter.Fill(datatable);
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() =>
                        dataGrid.ItemsSource = datatable.DefaultView));
                data_adapter.Update(datatable);
                connection.Close();

                }
                catch(Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            });

        }

        public void buttonAdd2_Click(object sender, RoutedEventArgs e)
        {
            isB2pressed = true;
        }
    }
}
