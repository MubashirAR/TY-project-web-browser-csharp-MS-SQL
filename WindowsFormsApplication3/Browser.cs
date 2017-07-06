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
using System.IO;
using System.Data.SqlClient;
using System.Net;
using Microsoft.Win32;
using System.Runtime.InteropServices;


namespace WindowsFormsApplication3
{
    public partial class Browser : Form
    {
        
        //Form2 f2 = new Form2();
        public static string SetValueFortextBox1 = "";
        
        
        /// <summary>
        /// Functions to be performed when the main browser is launched
        /// </summary>
        public Browser()
        {
           
            InitializeComponent();
           
            addtab();
            this.tabControl1.Dock = DockStyle.Fill;
            this.CenterToScreen();
        }
        public void SplashStart()
        {
            //Application.Run(new Form7());
        }

        public Browser(string username)
        {

            InitializeComponent();

            addtab();
            this.tabControl1.Dock = DockStyle.Fill;
            this.CenterToScreen();
            
        }
        // Create an obj



        public string user,instance,userID,password,databaseName;
        public Browser(string username,string instance,string userID,string password,string databaseName)
        {

            
            this.instance = instance;
            this.userID = userID;
            this.password = password;
            this.databaseName = databaseName;
            InitializeComponent();
            addtab();
            newTabPage();
            this.tabControl1.Dock = DockStyle.Fill;
            user = username;
            if (user == "public")
            {
                this.Text = "Welcome";
            }
            else
            {
                this.Text = "Welcome, " + user;
                this.Update();
            }
            setTheme();
            this.CenterToScreen();
        
        }


        

        private void setTheme()
        {
            string Theme;
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\securewebbrowser", true);
                 Theme= key.GetValue("Theme").ToString();
                 if (Theme == "Lets Pop ! (Default)")
                 {
                     Back.Image = Properties.Resources.rsz_button_previous_icon;
                     Forward.Image = Properties.Resources.rsz_button_next_icon;
                     Cancel.Image = Properties.Resources.rsz_button_close_icon;
                     Refresh.Image = Properties.Resources.rsz_button_refresh_icon;
                     Home.Image = Properties.Resources.rsz_1img142;
                     button1.Image = Properties.Resources.rsz_1rsz_1button_play_icon;
                     //this.BackColor = System.Drawing.Color.DarkBlue;

                 }

                 else if (Theme == "Life is Simple")
                 {
                     Back.Image = Properties.Resources.rsz_navigation_left_button;
                     Forward.Image = Properties.Resources.rsz_navigation_right_button;
                     Cancel.Image = Properties.Resources.rsz_1delete1;
                     Refresh.Image = Properties.Resources.rsz_button_rotate_ccw;
                     Home.Image = Properties.Resources.rsz_home;
                     button1.Image = Properties.Resources.rsz_navigation_right_frame;
                     //this.BackColor = System.Drawing.Color.DarkBlue;

                 }
            

            }
            catch(Exception re)
            {
                MessageBox.Show("Unable to set theme, Default theme will be used", "Error");
            }

            
        }
        private void newTabPage()
        {
            //tabControl1.TabPages.Add("+");
        }
        // Create an object of webbrowser control
        WebBrowser browser = new WebBrowser();
        int i = 0;
        int hr = 0;
        int min = 0;
        int sec = 0;
        int ms = 0;

        
        /// <summary>
        /// Initialize web browser and tabcontrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            browser.Dock = DockStyle.Fill;
            browser.Visible = true;
            browser.DocumentCompleted += browser_DocumentCompleted;
            tabControl1.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Right & AnchorStyles.Left;
            tabControl1.TabPages.Add("New Tab");
            tabControl1.SelectTab(i);
            tabControl1.SelectedTab.Controls.Add(browser);
            i += 1;
            timer1.Stop();
            

        }

        private void browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
           
        }

        /// <summary>
        /// Code to be executed when browser finishes loading web page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           
            timer1.Stop();
            
            //textBox1.Text = +sec + " seconds".ToString();
            tabControl1.SelectedTab.Text = ((WebBrowser)tabControl1.SelectedTab.Controls[0]).DocumentTitle;
            try
            {
                comboBox1.Text = browser.Url.AbsoluteUri;
            }
            catch (NullReferenceException ne)
            {

            }
            if (!comboBox1.Items.Contains(comboBox1.Text))
            {
                comboBox1.Items.Add(comboBox1.Text);
            }
            Historywriter(tabControl1.SelectedIndex);
            enablebuttons();
            listBox2.Items.Add(comboBox1.Text);
            listBox1.Items.Add(tabControl1.SelectedTab.Text = ((WebBrowser)tabControl1.SelectedTab.Controls[0]).DocumentTitle);
            

        }


       
        public void enablebuttons()
        {
            comboBox1.Enabled = true;
            button1.Enabled = true;
            Back.Enabled = true;
            Forward.Enabled = true;
            Refresh.Enabled = true;
            Home.Enabled = true;
            
            button7.Enabled = true;
            button8.Enabled = true;
           
        }
        void browser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            
        }
        /// <summary>
        /// A method that verifies the entered url is valid url or not
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        bool CheckURL(string url)
        {

            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();

                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

        
        
        /// <summary>
        /// Check if url is vaild and load page else do a google search
        /// </summary>
        /// <param name="address"></param>
        public void Navigate(string address,int tabindex)
        {
            if (string.IsNullOrEmpty(address))
                cancelbrowsing();
            if (address.Equals("about:blank")) cancelbrowsing();
            comboBox1.Enabled = false;
            button1.Enabled = false;
            Back.Enabled = false;
            Forward.Enabled = false;
            Refresh.Enabled = false;
            Home.Enabled = false;
            
            button7.Enabled = false;
            button8.Enabled = false;
            
            //tabControl1.Enabled = false;
            

            string url = string.Empty;
            string urlProtocol = "http://";
            if (!comboBox1.Text.Trim().Contains(urlProtocol))

                url = urlProtocol + comboBox1.Text;
            else
                url = comboBox1.Text;
            if (CheckURL(url))
            {
                Uri urlResult = new Uri(url);
                
                ((WebBrowser)tabControl1.TabPages[tabindex].Controls[0]).Navigate(url.ToString());
                
                tabControl1.Enabled = true;
                
            }
            else
            {
                browser.Navigate("http://google.com/search?q=" + comboBox1.Text);
                tabControl1.Enabled = true;
                
            }

        }

        //public void navigateWithLogin()
        //{
        //    string userName = "first.last";
        //    string password = "SuperSecretPW";

        //    string hdr = "Authorization: Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + password)) + System.Environment.NewLine;

        //    browser.Navigate(String.Format("https://{0}:{1}@www.your-securesite.com/your-application/default.aspx", userName, password), null, null, hdr);
        //}

        /// <summary>
        /// Code to load the previously browsed document/web page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            sec = 0;
            ms = 0;
            timer1.Start();
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).GoBack();
           
        }
        /// <summary>
        /// code to go to page we came back from
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {


        }
        /// <summary>
        /// Code for navigating to given web page when navigate button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        /// <summary>
        /// Code to refresh the web page using refresh button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
           

        }
        /// <summary>
        /// Code to load google homepage when button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
           

        }
        /// <summary>
        /// Code to add a new tab to tabcontrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       





        public static void sessionvariables()
        {
            string loadhistory, userID;


        }


        /// <summary>
        /// Accepts "s" from form2 and pastes and loads the required website url
        /// </summary>
        /// 
        public void HistoryToBrowser(string s)
        {
            comboBox1.Text = s;
            addtab();

            Navigate(s,tabControl1.TabCount-1);
            browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(browser_DocumentCompleted);

        }
        //create an sql connection object to connect to database
        SqlConnection historycon;
        /// <summary>
        /// code to write history to the table in the database
        /// </summary>
        public void Historywriter(int tabindex)
        {

            // historytest();




            string hcon = "Data Source="+instance+";Initial Catalog="+databaseName+";User ID="+userID+";Password="+password;
            string time = "\"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\"";
            historycon = new SqlConnection(hcon);
            try
            {
                SqlCommand hcom = historycon.CreateCommand();
                hcom.CommandText = "Insert into "+user+"history(url,Title,TOA) values(@myurl,@mytitle,@myTOA)";
                hcom.Parameters.Add("@myurl", browser.Url.ToString());
                hcom.Parameters.Add("@mytitle", browser.DocumentTitle);
                hcom.Parameters.Add("@myTOA", DateTime.Now);
                historycon.Open();
                hcom.ExecuteNonQuery();
                historycon.Close();
            }
            catch (SqlException se)
            {
                //MessageBox.Show("Error :"+se);
            }




        }

        private void tab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Text = browser.Url.ToString();
        }

        /// <summary>
        /// code to add the tab when add tab button is pressed
        /// </summary>
        private void addtab()
        {
            comboBox1.Text = "";
            browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            browser.Dock = DockStyle.Fill;
            browser.Visible = true;
            browser.DocumentCompleted += browser_DocumentCompleted;
            tabControl1.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Right & AnchorStyles.Left;
            tabControl1.TabPages.Add("New Tab");
            tabControl1.SelectTab(tabControl1.TabCount-1);
            //tabControl1.SelectTab(i);
            tabControl1.SelectedTab.Controls.Add(browser);
            i += 1;
            tabControl1.Dock = DockStyle.Fill;
        }
        /// <summary>
        /// code to delete selected tab from tabcontrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count - 1 > 0)
            {
                tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
                tabControl1.SelectTab(tabControl1.TabPages.Count - 1);
                i -= 1;

            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do You Want To restore Your Data", "Secure Web Browser", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Thread t = new Thread(new ThreadStart(SplashStart));
                t.Start();
                Thread.Sleep(5000);
                LoadMyFile();
                Navigate(comboBox1.Text, tabControl1.SelectedIndex);
                SetValueForText1 = comboBox1.Text;
                t.Abort();
            }
            else if (dialogResult == DialogResult.No)
            {
                
            }

            timer1.Start();
            /*string ConnectionString = "Data Source=" + instance + ";Initial Catalog=" + databaseName + ";User ID=" + userID + ";Password=" + password;
            String SQL = "SELECT userid  FROM usercredentials";
            SqlDataAdapter Adpt = new SqlDataAdapter(SQL, ConnectionString);
            DataSet question = new DataSet();
            Adpt.Fill(question);

            foreach (DataRow dr in question.Tables[0].Rows)
            {
                label1.Text += question.Tables[0].Rows[0]["userid"].ToString();
            }
            */
        }
        public static string SetValueForText1 = "";

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //f2.HistoryWriter(browser.Document.Title,browser.Document.Url.ToString());
            // var myForm = new Form2();
            //myForm.Show();

        }
        /// <summary>
        /// open history window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //f2.HistoryWriter(browser.Document.Title,browser.Document.Url.ToString());
            var myForm = new HistoryWindow(user,instance,userID,password,databaseName);
            myForm.Show();
        }
        /// <summary>
        /// Code to add bookmark to the table in the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click_1(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// If the user presses enter on the keyboard initialize the request for the page user expects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                if (textBox1.SelectionLength > 0)
                {
                    textBox1.SelectedText = "";
                }
            }
            comboBox1.Items.Clear();
            if (comboBox1.Text.Length > 0)
            {
                comboBox1.SelectionStart = comboBox1.Text.Length;
                comboBox1.SelectionLength = 0;
            }

            
            if (e.KeyCode == Keys.Enter)
            {
                
                
                e.SuppressKeyPress = true;
                Navigate("" + comboBox1.Text,tabControl1.SelectedIndex);

                

            }
            
            else
            {
                autofillcombobox();
            }    
        }

        private void button1_Click(object sender, EventArgs e)
        {





        }

        /// <summary>
        /// The following method calculates the time required for loading the web page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text =+hr+":"+min+":" +sec+ ":" + ms + ":".ToString();
            ms++;
            if (ms > 10)
            {
                sec++;
                ms = 0;
            }
            else
            {
                ms++;
            }
            if(sec>60)
            {
                min++;
                sec = 0;

            }
            if(min>60)
            {
                hr++;
                min = 0;

            }






        }

        ///
        ///The following code is only for Testing purpose
        ///


        public void historytest(int tabindex)
        {
            string hcon = "Data Source=" + instance + ";Initial Catalog=" + databaseName + ";User ID=" + userID + ";Password=" + password;
            string time = "\"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\"";
            historycon = new SqlConnection(hcon);
            try
            {
                SqlCommand hcom = historycon.CreateCommand();
                hcom.CommandText = "Insert into publicbookmarks(url,Title) values(@myurl,@mytitle)";
                hcom.Parameters.Add("@myurl", ((WebBrowser)tabControl1.TabPages[tabindex].Controls[0]).Url.ToString());
                hcom.Parameters.Add("@mytitle", ((WebBrowser)tabControl1.TabPages[tabindex].Controls[0]).DocumentTitle.ToString());
                //hcom.Parameters.Add("@myTOA", DateTime.Now);
                historycon.Open();
                hcom.ExecuteNonQuery();
                historycon.Close();
            }
            catch (SqlException se)
            {
                MessageBox.Show("Please verify you haven't added this bookmark before." + se, "Error !");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button9_Click(object sender, EventArgs e)
        {

            
        }
        public void cancelbrowsing()
        {
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Stop();
            enablebuttons();
        }

        private void sessionTimeHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            
        }

        /* private void button10_Click(object sender, EventArgs e)
         {

             timer1.Stop();
             hr = 0;
             min = 0;
             sec = 0;
             ms = 0;

         }*/

        SqlConnection dataconn;
        int n = 1;

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
                SaveMyFile();
            
            
                timer1.Stop();
                {
                    string dconn = "Data Source=" + instance + ";Initial Catalog=" + databaseName + ";User ID=" + userID + ";Password=" + password;
                    string time = "\"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\"";
                    dataconn = new SqlConnection(dconn);
                    try
                    {
                        SqlCommand dcom = dataconn.CreateCommand();
                        dcom.CommandText = "Insert into "+user+"_session_history(session_time,time,date) values(@session_time,@time,@date)";
                        
                        dcom.Parameters.Add("@session_time", textBox1.Text);
                        //DateTime time = DateTime.Now;              // Use current time
                        //string format = "yyyy-MM-dd HH:MM:ss";
                        dcom.Parameters.Add("@time", DateTime.Now.ToString("hh:mm:ss"));
                        dcom.Parameters.Add("@date", DateTime.Now);
                       
                       
                        dataconn.Open();
                        dcom.ExecuteNonQuery();
                        dataconn.Close();
                    }
                    catch (SqlException se)
                    {
                       // MessageBox.Show("Error :" + se);


                    }
                }
                Application.ExitThread();




            
        }

        private void bookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var myForm = new Form4(user, instance, userID, password, databaseName);
            myForm.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }


        public void autofillcombobox()
        {
            if (comboBox1.Text != null && comboBox1.Text != "")
            {
                try
                {
                    string ConString = "Data Source=" + instance + ";Initial Catalog=" + databaseName + ";User ID=" + userID + ";Password=" + password;
                    using (SqlConnection con = new SqlConnection(ConString))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT url FROM publicHistory", con);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            if ((reader.GetString(0)).Contains(comboBox1.Text))

                            {
                                comboBox1.Items.Add(reader.GetString(0));
                            }

                        }
                        comboBox1.DroppedDown = true;
                        con.Close();
                    }
                }
                catch (SqlException se)
                {
                    MessageBox.Show(se.ToString());
                }

            }
        }
            public void SaveMyFile()
        {
                try
                {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\securewebbrowser", true);
                    for (int i = 0; i <=tabControl1.TabCount ; i++)
			
                    {
                        key.SetValue("Tab"+tabControl1.TabCount,comboBox1.Text);
			            
                    }
                    
                }
            
                catch(Exception re)
                {

                    VariableFileLocation();
                }
        }


        private void VariableFileLocation()
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult result = fbd.ShowDialog();
                try
                {
                    File.Create(fbd.SelectedPath + "\\Variables.txt");
                }
                catch (Exception fe)
                {
                    MessageBox.Show("Invalid Folder", "Error !");
                    this.Close();
                }
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\securewebbrowser", true);
                key.SetValue("History Location", fbd.SelectedPath);
            }
        public void LoadMyFile()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\securewebbrowser", true);
                try
                {
                    string history = key.GetValue("Tab").ToString();
                    addtab();
                    ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Navigate(history);
                }
                catch (Exception)
                {

                    MessageBox.Show("Tabs re-opened ", "Welcome Back !");
                }
                
                



            }
            
            catch(Exception re)
            {
                
                
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    DialogResult result = fbd.ShowDialog();
                try
                {
                    File.Create(fbd.SelectedPath + "\\Variables.txt");
                }
                    catch(Exception fe)
                {
                    MessageBox.Show("Invalid Folder","Error !");
                    this.Close();
                }
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\securewebbrowser", true);
                    key.SetValue("History Location",fbd.SelectedPath);
                
            }
            
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            
        }

        private void bookmarksToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            WebBrowser Browser = new WebBrowser();
            HistoryWindow frm = new HistoryWindow();
         TabPage tab = new TabPage();
           int i,j;
              i = dataGridView2.CurrentRow.Index;

              comboBox1.Text= dataGridView1.SelectedRows[i].Cells[1].Value.ToString();
             
               frm.TopLevel = false;
               Browser.Dock = DockStyle.Fill;
                  tabControl1.TabPages.Add(tab);
           // j = j + 1;
       tabControl1.SelectedTab = tab;
        tab.Controls.Add(Browser);
     button7.PerformClick();
    
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text == "+" )
            {
                
                comboBox1.Text = "";
                browser = new WebBrowser();
                browser.ScriptErrorsSuppressed = true;
                browser.Dock = DockStyle.Fill;
                browser.Visible = true;
                browser.DocumentCompleted += browser_DocumentCompleted;
                tabControl1.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Right & AnchorStyles.Left;
                tabControl1.TabPages.Insert(tabControl1.TabPages.Count - 2, "New Tab");
                tabControl1.SelectedIndex = tabControl1.TabPages.Count - 2;
                tabControl1.SelectedTab.Controls.Add(browser);
                i += 1;
                tabControl1.Dock = DockStyle.Fill;
                

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            sec = 0;
            ms = 0;
            timer1.Start();
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).GoBack();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {


            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).GoForward();
        }

        private void pictureBox1_Click_2(object sender, EventArgs e)
        {
            sec = 0;
            ms = 0;
            timer1.Start();
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Navigate("http://www.google.com");
        }

        private void pictureBox1_Click_3(object sender, EventArgs e)
        {

            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Refresh();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

            cancelbrowsing();
        }

        private void Bookmarks_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            dataGridView2.Visible = true;
            TabPage tab = new TabPage();
            tab.Text = "Bookmarks";
            dataGridView2.Visible = true;
            dataGridView2.BringToFront();
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            string str = ("Data Source=" + (instance + ("; Initial Catalog=" + (databaseName + ("; User ID=" + (userID + (";Password=" + password)))))));
            SqlConnection con = new SqlConnection(str);
            string com = ("Select * from " + (user + "bookmarks"));
            SqlDataAdapter Adpt = new SqlDataAdapter(com, con);
            DataSet ds = new DataSet();
            Adpt.Fill(ds, "url");
            dataGridView2.DataSource = ds.Tables[0];
            tab.Controls.Add(dataGridView2);
            tabControl1.TabPages.Add(tab);
        }

        private void pictureBox2_Click_2(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            TabPage tab = new TabPage();
            tab.Text = "History";
            dataGridView1.Visible = true;
            dataGridView1.BringToFront();
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            string str = ("Data Source=" + (instance + ("; Initial Catalog=" + (databaseName + ("; User ID=" + (userID + (";Password=" + password)))))));
            SqlConnection con = new SqlConnection(str);
            string com = ("Select * from " + (user + "history"));
            SqlDataAdapter Adpt = new SqlDataAdapter(com, con);
            DataSet ds = new DataSet();
            Adpt.Fill(ds, "url");
            dataGridView1.DataSource = ds.Tables[0];
            tab.Controls.Add(dataGridView1);
            tabControl1.TabPages.Add(tab);
        }

        private void pictureBox2_Click_3(object sender, EventArgs e)
        {
            string bcon = "Data Source=" + instance + ";Initial Catalog=" + databaseName + ";User ID=" + userID + ";Password=" + password;
            //string time = "\"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\"";
            historycon = new SqlConnection(bcon);
            try
            {
                SqlCommand bcom = historycon.CreateCommand();
                bcom.CommandText = "Insert into " + user + "bookmarks(url,Title) values(@myurl,@mytitle)";
                bcom.Parameters.Add("@myurl", browser.Url.ToString());
                bcom.Parameters.Add("@mytitle", browser.DocumentTitle);
                //hcom.Parameters.Add("@myTOA", DateTime.Now);
                historycon.Open();
                bcom.ExecuteNonQuery();
                historycon.Close();
            }
            catch (SqlException se)
            {
                MessageBox.Show("Please verify you haven't added this bookmark before.", "Error !");
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < this.tabControl1.TabCount; i++)
                
                    if (i != tabControl1.SelectedIndex)
                        tabControl1.TabPages.RemoveAt(i--);


            newTabPage();
        }

        private void Browser_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click_4(object sender, EventArgs e)
        {

            Navigate(comboBox1.Text, tabControl1.SelectedIndex);
            SetValueForText1 = comboBox1.Text;
        }

        private void Theme_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click_4(object sender, EventArgs e)
        {
            var th = new Themes();
            th.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (user != "public")
            {

                var myForm = new Form6(user, instance, userID, password, databaseName);
                myForm.Show();


            }
            else
            {
                MessageBox.Show("Sorry, Session history is not available for Public Users", "Please Login");
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            addtab();
        }

        private void Back_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.Visible)
            {
                listBox1.Visible = false;
            }
            else
            {
                listBox1.Visible = true;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string history;
            listBox2.SelectedIndex= listBox1.SelectedIndex;
            history = listBox2.SelectedItem.ToString();
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Navigate(history);
        }

        private void Back_DoubleClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This method is used to retrieve cookies
        /// </summary>

    }
    }
    //public class node
    //{
    //    int data;
    //    node next;

    //    public node(int data, node next)
    //    {
    //        this.data = data;
    //        this.next = next;   
    //    }
        

    //    public object Data
    //    {
    //        get { return this.data; }
            
    //        set { this.data = (Int32)value; }
        

    //    }
    
    //}


