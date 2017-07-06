using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WindowsFormsApplication3;
using System.Data.SqlClient;

namespace WindowsFormsApplication3
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            

        }
        public string user, instance, userID, password, databaseName;

        private void Form4_Load(object sender, EventArgs e)
        {
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            int i = 0;
            string sql = null;
            connetionString = "Data Source=" + instance + ";Initial Catalog=" + databaseName + ";User ID=" + userID + ";Password=" + password;
            sql = "select * from "+user+"bookmarks";
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                adapter.Dispose();
                command.Dispose();
                connection.Close();
                listBox1.DataSource = ds.Tables[0];
                listBox1.ValueMember = "url";
                listBox1.DisplayMember = "title";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot open connection ! ","Error !");
            }
        }

        public Form4(string username, string instance, string userID, string password, string databaseName)
        {
            this.instance = instance;
            this.userID = userID;
            this.password = password;
            this.databaseName = databaseName;
            InitializeComponent();
            MessageBox.Show("User " + username);
            user = username;
           
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
