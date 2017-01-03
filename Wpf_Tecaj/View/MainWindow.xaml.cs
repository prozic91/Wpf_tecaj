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
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Globalization;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Xml.Linq;

using Wpf_Tecaj.Model;
using Wpf_Tecaj.ViewModel;

namespace Wpf_Tecaj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    
    public partial class MainWindow : Window
    {
        MainWindowViewModel viewService = new MainWindowViewModel();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            viewService.BindData();
        }

        private void HnbGrid_Initialized(object sender, EventArgs e)
        {

        }
        private void PbzGrid_Initialized(object sender, EventArgs e)
        {

        }

    }
}
