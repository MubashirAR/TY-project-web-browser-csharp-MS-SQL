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
using System.Data.SqlClient;
using System.Net;

namespace WindowsFormsApplication3
{
    public partial class Form6 : Form
    {
        string user, instance, userID, password, databaseName;
        public Form6()
        {
            InitializeComponent();
        }
              public Form6(string username,string instance, string userID, string password, string databaseName)

        {
            InitializeComponent();
            this.instance = instance;
            this.userID = userID;
            this.password = password;
            this.databaseName = databaseName;
            MessageBox.Show("User " + username);
            user = username;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();

            string constring = "Data Source=" + instance + ";Initial Catalog=" + databaseName + ";User ID=" + userID + ";Password=" + password;
            using (SqlConnection con = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * from " + user + "_session_history", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            dataGridView1.DataSource = dt;
                        }
                    }





                }
            }
        }


        SqlConnection connectionString;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }



    }
    }

