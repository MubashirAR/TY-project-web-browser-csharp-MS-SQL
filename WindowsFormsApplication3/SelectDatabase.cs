using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.ServiceProcess;
using Microsoft.Win32;


namespace WindowsFormsApplication3
{
    public partial class SelectDatabase : Form
    {
        string instance, userID, password;
        SqlConnection connection;
        
        public SelectDatabase()
        {
          
            InitializeComponent();
            
            
            textBox1.Text = "MUBASHIR-LAPPY";
            textBox2.Text = "sa";
            textBox3.Text = "6852";
            this.CenterToScreen();

            
        }



        bool DoesServiceExist(string serviceName, string machineName)
        {
            ServiceController[] services = ServiceController.GetServices(machineName);
            var service = services.FirstOrDefault(s => s.ServiceName == serviceName);
            return service != null;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (DoesServiceExist("MSSQLSERVER", Environment.MachineName))
            {
                if (!(checkService() == "Running"))
                {

                    MessageBox.Show("SQL server service is not running","Error");
                }
            }

            if (textBox1.Text=="" || textBox2.Text==""|| textBox3.Text=="")
            {
                MessageBox.Show("Incomplete Details, you seem to have left a field empty","Error");
            }

            else
            {
                try
                {
                Start:
                    //Check if checkbox is checked then save information to the registry
                    if (checkBox1.Checked)
                    {
                        try
                        {
                            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\securewebbrowser");
                            key.SetValue("instance", textBox1.Text);
                            key.SetValue("userID", textBox2.Text);
                            key.SetValue("password", textBox3.Text);
                            key.Close();
                        }
                        catch (Exception re)
                        {
                            MessageBox.Show("Sorry, we are not allowed to write settings to the registry.", "Oops!");
                        }
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Do you wan't to save the database details ?", "Some features will not be usable without saving database details", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            checkBox1.Checked = true;
                            goto Start;
                        }

                    }
                }
                //If any exception occurs throw an error, uncheck the checkbox and continue
                catch (Exception re)
                {
                    MessageBox.Show("Error !", "Unable to write to registry, your settings couldn't be saved");
                    checkBox1.Checked = false;
                    //goto retry;

                }

            retry: ;



                instance = textBox1.Text;
                userID = textBox2.Text;
                password = textBox3.Text;

                try
                {
                    connection = new SqlConnection("Data Source=" + instance + ";User ID=" + userID + ";Password=" + password);
                    connection.Open();
                    MessageBox.Show("Connected", "Connection Successful");
                    connection.Close();
                    var data = new AddDatabase(instance, userID, password);
                    data.Show();

                    this.Hide();

                }
                catch (SqlException se)
                {
                    MessageBox.Show("Cannot connect to database\n" + se, "Error");
                }
            }
            

        
            


           
        }

        
        public string checkService()
        {
            ServiceController sc = new ServiceController("MSSQLSERVER");

            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    return "Running";
                case ServiceControllerStatus.Stopped:
                    return "Stopped";
                case ServiceControllerStatus.Paused:
                    return "Paused";
                case ServiceControllerStatus.StopPending:
                    return "Stopping";
                case ServiceControllerStatus.StartPending:
                    return "Starting";
                default:
                    return "Status Changing";
            }
        }
    
        private void btnExit_Click(object sender, EventArgs e)
        {
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i].Name != "Menu")
                    Application.OpenForms[i].Close();
            }



        }

        private void SelectDatabase_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                    Application.OpenForms[i].Close();
            }

        }

        private void SelectDatabase_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            var simple = new Filestream();
            simple.Show();
            this.Hide();
            //Form f1 = new Form1("NO SQL");
            //f1.Show();
            
                
        }

        private void SelectDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
