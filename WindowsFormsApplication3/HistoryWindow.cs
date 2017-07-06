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
    public partial class HistoryWindow : Form
    {
        //Form1 f1 = new Form1();

        

        /// <summary>
        /// This method loads the selected page from the listbox into the browser
        /// </summary>

        
        string s;
        /// <summary>
        /// This method loads the selected (Double-clicked) List item into the browser.
        /// </summary>
        public void HistoryLoader()
        {
            
            
            // -------------------------- Code for loading history from databse pending --------------------------

            if (System.Windows.Forms.Application.OpenForms["Form1"] != null)
            {
                (System.Windows.Forms.Application.OpenForms["Form1"] as Browser).HistoryToBrowser(s);
            }
            
            
            
        }


        public HistoryWindow()
        {
            InitializeComponent();
            HistoryReader();
        }
        public string user, instance, userID, password, databaseName;
        public HistoryWindow(string username,string instance,string userID,string password,string databaseName)
        {
            this.instance = instance;
            this.userID = userID;
            this.password = password;
            this.databaseName = databaseName;
            InitializeComponent();
            MessageBox.Show("User "+username);
            user = username;
            HistoryReader();
        }
        
        SqlConnection historycon;
        private void HistoryReader()
        {
            string hcon = "Data Source=" + instance + ";Initial Catalog=" + databaseName + ";User ID=" + userID + ";Password=" + password;
            string time = "\"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\"";
  historycon = new SqlConnection(hcon);
            try
            {
                SqlDataReader historyreader = null;
                SqlCommand historycom = new SqlCommand("select title from "+user+"history", historycon);
                historycon.Open();
                historyreader = historycom.ExecuteReader(); 
                while (historyreader.Read())
                {
                    History.Items.Add(historyreader["title"].ToString());
                }
                historycon.Close();

            }
            catch (SqlException se)
            {
                //MessageBox.Show("Error :"+se);
            }
             

        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string hcon = "Data Source=" + instance + ";Initial Catalog=" + databaseName + ";User ID=" + userID + ";Password=" + password;
            string time = "\"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\"";
            historycon = new SqlConnection(hcon);
            try
            {
                SqlDataReader historyreader = null;
                SqlCommand historycom = new SqlCommand("select url from "+user+"history", historycon);
                int line = 0,find=History.SelectedIndex+1;
                string url;
                historycon.Open();
                historyreader = historycom.ExecuteReader();
                for (line = 0; line <= find;line++ )
                {
                    if (line == find)
                    {
                        historyreader.Read();
                        Form f1 = Application.OpenForms["Form1"];
                        //Form1 f1 = new Form1();
                        
                        url = historyreader["url"].ToString();
                        historycon.Close();
                        f1.BringToFront();
                        //f1.HistoryToBrowser(url);
                        MessageBox.Show("Done");
                        break;
                    }
                    
                }
                
            }
            catch (SqlException se)
            {
                MessageBox.Show("Error :"+se);

            }
            
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            History.Text = Browser.SetValueFortextBox1;
            HistoryWindow f2 = new HistoryWindow();
            HistoryReader();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            History.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (History.SelectedItems.Count != 0)
            {
                while (History.SelectedIndex != -1)
                {
                    History.Items.RemoveAt(History.SelectedIndex);
                }
            }

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            HistoryLoader();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            

        }
        
    }
}



       