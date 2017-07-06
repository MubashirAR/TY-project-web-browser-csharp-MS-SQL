using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApplication3
{
    public partial class SignUp : Form
    {
        string instance,userID,password,databaseName;
        public SignUp()
        {
            
            InitializeComponent();
            this.CenterToScreen();
            
        }

      


        public SignUp(string instance,string userID,string password,string databaseName)
        {
            InitializeComponent();
            this.instance = instance;
            this.userID = userID;
            this.password = password;
            this.databaseName = databaseName;
            this.CenterToScreen();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        SqlConnection dataconn;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length>2&&textBox2.Text.Length>5)
            {
                createuser();
               
            }
            else
            {
                MessageBox.Show("Please enter a username with atleast 3 characters and password with atleast 6 characters");
            }
        }

        public void createuser()
        {

            if ((textBox1.Text != null) && (textBox2.Text != null) && (textBox3.Text != null) && textBox2.Text.Length > 5)
            {
                if (textBox2.Text == textBox3.Text)
                {

                    string dconn = "Data Source="+instance+";Initial Catalog=" + databaseName + ";User ID="+userID+";Password="+password;
                    dataconn = new SqlConnection(dconn);
                    try
                    {
                        SqlCommand dcom = dataconn.CreateCommand();
                        dcom.CommandText = "Insert into usercredentials values(@user,@password)";
                        dcom.Parameters.Add("@user", textBox1.Text);
                        dcom.Parameters.Add("@password", textBox2.Text);
                        dataconn.Open();
                        dcom.ExecuteNonQuery();
                        dataconn.Close();
                        dcom.CommandText = "Create table " + textBox1.Text + "history(url varchar(255),Title varchar(255),TOA datetime)";
                        dataconn.Open();
                        dcom.ExecuteNonQuery();
                        dataconn.Close();

                        dcom.CommandText = "Create table " + textBox1.Text + "bookmarks(url varchar(255),Title varchar(255))";
                        dataconn.Open();
                        dcom.ExecuteNonQuery();
                        dataconn.Close();

                        dcom.CommandText = "Create table " + textBox1.Text + "_session_history(session_time nchar(10),time time,date date)";
                        dataconn.Open();
                        dcom.ExecuteNonQuery();
                        dataconn.Close();
                        MessageBox.Show("User created", "Welcome to SecureWebBrowser");

                    }
                    catch (SqlException se)
                    {
                        //MessageBox.Show("Error :" + se);

                        MessageBox.Show("Sorry, The username entered is already taken !", "Username exists");
                    }
                }
                else
                {
                    MessageBox.Show("The passwords entered do not seem to match, please try again !", "Password mismatch!");

                }
            }
            else
            {
                MessageBox.Show("Entered password should be atleast 6 characters long", "Invalid Password !");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var login = new LoginForm();
            login.Show();
            this.Close();
        }

        
    }
}
