using System;
using System.Diagnostics;
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
using WindowsFormsApplication3;
using Microsoft.Win32;

namespace WindowsFormsApplication3
{

   
    public partial class LoginForm : Form
    {
        string instance, userID, password, databaseName;
        public LoginForm()
        {
            
            InitializeComponent();
            
            textBox2.PasswordChar = '*';
            textBox1.Text = "mubashir";
            textBox2.Text = "hello";
            this.CenterToScreen();
        }

       

        public LoginForm(string instance,string userID,string password,string databaseName)
        {
            this.instance = instance;
            this.userID = userID;
            this.password = password;
            this.databaseName = databaseName;
            InitializeComponent();
            textBox2.PasswordChar = '*';
            
                try
                {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\securewebbrowser");
                    if (key.GetValue("user")!=null)
                    {
                        textBox1.Text = key.GetValue("user").ToString();
                        textBox2.Text = key.GetValue("pass").ToString();
                        if (key.GetValue("remember").ToString()=="yes")
                        {
                            checkBox1.Checked = true;
                        }
                        else
                        {
                            checkBox1.Checked = false;
                        }
                    }
                    
                    key.Close();

                }
                catch (Exception re)
                {
                    MessageBox.Show(re.ToString());
                    //MessageBox.Show("Sorry, we are not allowed to read settings from the registry.", "Oops!");
                }
                this.CenterToScreen();
            //textBox1.Text = "mubashir";
            //textBox2.Text = "hello";
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text=="" || textBox2.Text==""))
            {
                MessageBox.Show("Username or password field cannot be empty", "Blank fields detected");
                
            }
            else if (textBox2.Text.Length<8)
            {
                MessageBox.Show("Password cannot be less than 8 characters","Invalid password");
            }
            else
            {
                login();
            }
                
            
        }
        SqlConnection historycon;
        string ID;
        private void login()
        {
            string hcon = "Data Source="+instance+";Initial Catalog="+databaseName+";User ID="+userID+";Password="+password;

            historycon = new SqlConnection(hcon);
            try
            {
                SqlDataReader historyreader = null;
                SqlCommand historycom = new SqlCommand("select * from usercredentials", historycon);

                

                historycon.Open();
                historyreader = historycom.ExecuteReader();
                while (historyreader.Read())
                {
                    string username;
                    if (textBox1.Text == (username=historyreader["UserID"].ToString()))
                    {
                        
                        if (textBox2.Text == (ID = historyreader["userpassword"].ToString()))
                        {

                            ID=username;
                            if (checkBox1.Checked == true)
                            {

                                
                                try
                                {
                                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\securewebbrowser");

                                    key.SetValue("user", textBox1.Text);
                                    key.SetValue("pass", textBox2.Text);
                                    key.SetValue("remember", "yes");


                                }
                                catch (Exception re)
                                {
                                    MessageBox.Show("Sorry, we are not allowed to write settings to the registry.", "Oops!");
                                }
                            }

                            else
                            {


                                try
                                {
                                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\securewebbrowser");

                                    key.SetValue("user", "");
                                    key.SetValue("pass", "");
                                    key.SetValue("remember", "no");
                                }
                                catch (Exception re)
                                {
                                    //MessageBox.Show("Sorry, we are not allowed to write settings to the registry.", "Oops!");
                                }
                            }
                            
                            var f1 = new Browser(username,instance,userID,password,databaseName);
                  
                            f1.Show();
                            this.Hide();
                          
                           



                            break;
                        }
                        else
                        {
                            MessageBox.Show("Please check caps lock, verify password and try again !", "Invalid password for user " + username);

                        }
                    }
                    

                }
                if (ID==null)
                {
                    MessageBox.Show("User does not exist!", "Invalid user");
                }
                var f3 = new LoginForm();
                f3.Hide();
                historycon.Close();

            }
            catch (SqlException se)
            {
                MessageBox.Show("Error :"+se);
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var f1 = new Browser("public",instance,userID,password,databaseName);
            f1.Show();
           
            this.Hide();
            
            f1.Closed += (s, args) => this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var f5 = new SignUp(instance,userID,password,databaseName);
            
            f5.Show();
            //this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i].Name != "Menu")
                    Application.OpenForms[i].Close();
            }



        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            cleardefaults();
            try
            {
                //run the program again and close this one
                Process.Start(Application.StartupPath + "\\windowsformsapplication3.exe");
                //or you can use Application.ExecutablePath

                //close this one
                Process.GetCurrentProcess().Kill();
            }
            catch
            { }
        }


        public void cleardefaults()
        {
            try
            {
                Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\securewebbrowser");

                MessageBox.Show("Please start the application again !", "Cleared defaults");
                Application.Exit();
            }
            catch (Exception re)
            {
                MessageBox.Show("Sorry, we are not allowed to write settings to the registry.", "Oops!");
            }
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            
        }
        
    }
}