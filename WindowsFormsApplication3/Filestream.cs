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


namespace WindowsFormsApplication3
{
    public partial class Filestream : Form
    {
        
        //Form2 f2 = new Form2();
        
        string bpath = @"c:\Users\MohammedMubashir\Documents\temp\BookmarkFile.txt";
        String b1;
        string path = @"c:\Users\MohammedMubashir\Documents\temp\HistoryFile.txt";
        String line;
        /// <summary>
        /// Functions to be performed when the main browser is launched
        /// </summary>
        public Filestream()
        {
           
            InitializeComponent();
           
            addtab();
            this.tabControl1.Dock = DockStyle.Fill;

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            try
            {
                path = fbd.SelectedPath+"HistoryFile.txt";
                bpath = fbd.SelectedPath + "BookmarkFile.txt";
                
            }
            catch (Exception fe)
            {
                MessageBox.Show("Invalid Folder", "Error !");
                this.Close();
            }
            this.CenterToScreen();
            
        }
        public void SplashStart()
        {
            //Application.Run(new Form7());
        }

        public Filestream(string username)
        {

            InitializeComponent();

            addtab();
            this.tabControl1.Dock = DockStyle.Fill;
            this.CenterToScreen();
        }
        // Create an obj



        public string user,instance,userID,password,databaseName;
        public Filestream(string username, string instance, string userID, string password, string databaseName)
        {
            this.instance = instance;
            this.userID = userID;
            this.password = password;
            this.databaseName = databaseName;
            InitializeComponent();
            addtab();
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
            this.CenterToScreen(); 
        
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

            Historywriter();
            enablebuttons();
            
            

        }


       
        public void enablebuttons()
        {
            comboBox1.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            menuStrip1.Enabled = true;
        }


        public void Historywriter()
        {
            //if (File.Exists("history"))
            //{
                
            //}
            FileStream F = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(F);
            sw.WriteLine(browser.Document.Title + " " + comboBox1.Text);
            sw.Close();

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
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            menuStrip1.Enabled = false;
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
                button6.Enabled = true;
            }
            else
            {
                browser.Navigate("http://google.com/search?q=" + comboBox1.Text);
                tabControl1.Enabled = true;
                button6.Enabled = true;
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
            
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).GoForward();


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
           
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Refresh();

        }
        /// <summary>
        /// Code to load google homepage when button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            sec = 0;
            ms = 0;
            timer1.Start();
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Navigate("http://www.google.com");

        }
        /// <summary>
        /// Code to add a new tab to tabcontrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            addtab();
            this.tabControl1.Dock = DockStyle.Fill;

        }





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
            tabControl1.SelectTab(i);
            tabControl1.SelectedTab.Controls.Add(browser);
            i += 1;
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
          
            FileStream F = new FileStream(bpath, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(F);
            
            sw.WriteLine(browser.Document.Title+" "+comboBox1.Text);
            sw.Close();

        
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
                if (comboBox1.SelectionLength > 0)
                {
                    comboBox1.SelectedText = "";
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
                
            }    
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Navigate(comboBox1.Text, tabControl1.SelectedIndex);
            SetValueForText1 = comboBox1.Text;




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

            cancelbrowsing();
            
        }
        public void cancelbrowsing()
        {
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Stop();
            enablebuttons();
        }

        private void sessionTimeHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var myForm = new Form6(user,instance, userID, password, databaseName);
            myForm.Show();
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
            DialogResult dialogResult = MessageBox.Show("Do You Want To Save Your Data", "Caution !", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                SaveMyFile();
               
            }
            else if (dialogResult == DialogResult.No)
            {
                
            }

            DialogResult result = MessageBox.Show("Are you sure you want to exit the application?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                
                Application.ExitThread();




            }
        }

        private void bookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var myForm = new Form4(user, instance, userID, password, databaseName);
            myForm.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }


        
            public void SaveMyFile()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\securewebbrowser", true);
            string history = key.GetValue("History Location").ToString();
            FileStream fs = new FileStream(history+ "\\Variables.txt", FileMode.OpenOrCreate, FileAccess.Read);            
            fs.Close();
            StreamWriter sw = new StreamWriter(history+ "\\Variables.txt", true, Encoding.ASCII);
           
            string NextLine = (comboBox1.Text);
            sw.Write(NextLine); 
            sw.Close();
        }

        public void LoadMyFile()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\securewebbrowser", true);
                string history = key.GetValue("History Location").ToString();
                if (File.Exists(history + "\\Variables.txt"))
                {
                    

                    StreamReader sr = new StreamReader(history + "\\Variables.txt");

                    string content = File.ReadAllText(@"" + history + "\\Variables.txt");
                    comboBox1.Text = content;

                    sr.Close();
                    File.WriteAllText(history + "\\Variables.txt", string.Empty);


                    

                }

            }
            
            catch(Exception re)
            {
                
                
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.Description = "Please select History and Bookmarks Location";
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
        
       

        private void button10_Click_1(object sender, EventArgs e)
        {

        }
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


