using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WindowsFormsApplication3
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Splashscreen());
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\securewebbrowser");

                // Checks if default settings are saved in registry, if not then load SelectDatabase(else)
                if (key != null)
                {
                    string instance, userID, password;
                    instance = key.GetValue("instance").ToString();
                    userID = key.GetValue("userID").ToString();
                    password = key.GetValue("password").ToString();
                    //Checks if default database is selected or not, if not then load AddDatabase(else)
                    if (key.GetValue("databaseName") != null)
                    {
                        string databaseName = key.GetValue("databaseName").ToString();
                        //Call form 1
                        Application.Run(new LoginForm(instance, userID, password, databaseName));
                    }
                    else
                    {
                        Application.Run(new AddDatabase(instance, userID, password));
                    }
                }
                else
                {
                    Application.Run(new SelectDatabase());
                }
            }
            catch
            {
                Application.Run(new SelectDatabase());
            }


        }

        
    }
}
