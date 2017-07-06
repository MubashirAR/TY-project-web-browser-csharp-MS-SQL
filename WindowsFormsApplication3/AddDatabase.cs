using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Win32;

namespace WindowsFormsApplication3
{
    public partial class AddDatabase : Form
    {
        string instance, userID, password,databaseName;
        SqlConnection connection;
        public AddDatabase()
        {
            InitializeComponent();
        }

        public AddDatabase(string instance,string userID, string password)
        {
            InitializeComponent();
            this.instance = instance;
            this.userID = userID;
            this.password = password;
            ListDatabases();
            textBox1.Text = "testing";
            this.CenterToScreen();
            //NewDatabase();
        }

        public void NewDatabase()
        {
            string connect = "Data Source="+instance+";User ID="+userID+";Password="+password;
            string time = "\"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\"";
            connection = new SqlConnection(connect);
            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "Create database "+textBox1.Text;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                connect = "Data Source=" + instance + ";Initial Catalog="+textBox1.Text+"; User ID=" + userID + ";Password=" + password;
                connection = new SqlConnection(connect);
                command = connection.CreateCommand();
                
                command.CommandText = "Create table publichistory(url varchar(255),Title varchar(255),TOA datetime)";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                
                command.CommandText = "Create table publicbookmarks(url varchar(255),Title varchar(255))";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                command.CommandText = "Create table usercredentials(userid varchar(255)not null unique,userpassword varchar(255)not null)";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                
                MessageBox.Show("Database & public use tables created !");
                listBox1.Items.Clear();
                ListDatabases();


                var signup = new SignUp(instance, userID, password, textBox1.Text);
                signup.Show();
                this.Close();
                
            }
            catch (SqlException se)
            {
                MessageBox.Show("Error :"+se);
            }
        }

        public void ListDatabases()
        {
            connection = new SqlConnection("Data Source=" + instance + ";User ID=" + userID + ";Password=" + password);
            //connection.Open();
            
            
                SqlDataReader reader= null;
                SqlCommand command = new SqlCommand("SELECT name FROM sys.databases",connection);
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    listBox1.Items.Add(reader["name"].ToString());
                }
                connection.Close();
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            label1.Visible = true;
            textBox1.Visible = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want us to set this database as the default database?", "Save settings", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\securewebbrowser",true);
                    key.SetValue("databaseName", listBox1.SelectedItem.ToString());
                    
                }
                catch(Exception re)
                {
                    MessageBox.Show("Sorry, we are not allowed to write settings to the registry. \nHint: Did you forget to select 'remember me' while connecting to SQL Server ?","Oops!");
                }
                try
                {
                    connection = new SqlConnection("Data Source=" + instance + ";Initial Catalog=" + (databaseName = listBox1.SelectedItem.ToString()) + "User ID=" + userID + ";Password=" + password);
                    MessageBox.Show("connected");
                    var loginform = new LoginForm(instance, userID, password, databaseName);
                    loginform.Show();
                    this.Hide();
                }
                catch (SqlException se)
                {
                    MessageBox.Show(se.ToString());
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                try
                {
                    connection = new SqlConnection("Data Source=" + instance + ";Initial Catalog=" + (databaseName = listBox1.SelectedItem.ToString()) + "User ID=" + userID + ";Password=" + password);
                    MessageBox.Show("connected");
                    var loginform = new LoginForm(instance, userID, password, databaseName);
                    loginform.Show();
                    this.Hide();
                }
                catch (SqlException se)
                {
                    MessageBox.Show(se.ToString());
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length>2)
            {

                NewDatabase();
            }
            else
            {
                MessageBox.Show("Error : Length of Name of the new database must be 3 characters or more", "Invalid entry !");
            }
            
        }

        private void AddDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void AddDatabase_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        

        
    }
}
