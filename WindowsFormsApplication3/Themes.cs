using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WindowsFormsApplication3
{
    public partial class Themes : Form
    {
        public Themes()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to set 'Let's Pop !' Theme? Application will restart", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\securewebbrowser", true);
                    key.SetValue("Theme", label1.Text);
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
                catch (Exception re)
                {
                    MessageBox.Show("Sorry, we were unable to set your theme !.", "Oops!");
                }
            }
            else if (result == DialogResult.No)
            {
                //...
            }
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to set '"+label2.Text+"' Theme? Application will restart", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\securewebbrowser", true);
                    key.SetValue("Theme", label2.Text);
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
                catch (Exception re)
                {
                    MessageBox.Show("Sorry, we were unable to set your theme !.", "Oops!");
                }
            }
            else if (result == DialogResult.No)
            {
                //...
            }
        }
    }
}
