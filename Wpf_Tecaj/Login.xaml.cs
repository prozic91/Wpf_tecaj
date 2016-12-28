using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

using System.Net.Http.Formatting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Globalization;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Wpf_Tecaj;

namespace Login_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 

    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        Registration registration = new Registration();
        MainWindow mainWindow = new MainWindow();
        

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxUsername.Text.Length == 0)
            {
                errormessage.Text = "Enter Username.";
                textBoxUsername.Focus();
            }
           
            else
            {
                string username = textBoxUsername.Text;
                string password = passwordBox1.Password;

                CurrenciesContext loginContext = new CurrenciesContext();

                var users = new Users();
                var ime = loginContext.Users.SingleOrDefault(m => m.Name == username);
                if(ime != null)
                {
                    if (username == ime.Name && password == ime.Password)
                    {
                        mainWindow.Show();
                        Close();
                    }
                    else
                    {
                        errormessage.Text = "Username/password is incorrect!";
                    }
                }
                else
                {
                    errormessage.Text = "Username/password is incorrect!";
                }

                
            }
        }

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            registration.Show();
            Close();
        }

    }
}