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

namespace Wpf_Tecaj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    
    public partial class MainWindow : Window
    {
      
        public MainWindow()
        {
            InitializeComponent();

           
        }

        public class HNBResponse
        {
            public string valuta { get; set; }
            public decimal kupovni_tecaj { get; set; }
            public decimal srednji_tecaj { get; set; }
            public decimal prodajni_tecaj { get; set; }
            public DateTime datum { get; set; }
            public string color { get; set; }
        }

      
        private  async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

            var currencies =   await GetCurrencies();
            HnbGrid.ItemsSource = currencies; //ovo maknit


        }


        private async Task<List<HNBResponse>> GetCurrencies()
        {
            List<HNBResponse> result = new List<HNBResponse>();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://www.hnb.hr");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("/hnb-tecajna-lista-portlet/rest/tecajn");
            response.EnsureSuccessStatusCode(); // Throw on error code. 
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var jsonSetting = new JsonSerializerSettings();
            jsonSetting.Culture = new CultureInfo("hr-HR");
            return await JsonConvert.DeserializeObjectAsync<List<HNBResponse>>(jsonResponse, jsonSetting);
        }


        private void HnbGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private async void HnbGrid_Initialized(object sender, EventArgs e)
        {
            var currencies = await GetCurrencies();
        }





    }
}
