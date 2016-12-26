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
            public Color color { get; set; }
        }

        public class PBZResponse
        {
            public string valuta { get; set; }
            public decimal kupovni_tecaj { get; set; }
            public decimal srednji_tecaj { get; set; }
            public decimal prodajni_tecaj { get; set; }
            public DateTime datum { get; set; }
            public Color color { get; set; }
        }


        private  async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            List<HNBResponse> boja = new List<HNBResponse>();

            var currencies = await GetCurrencies();
            var pbzValuta = await PbzCurrencies();

            foreach (var hnb in currencies)
            {
               

                var pbz = pbzValuta.SingleOrDefault(m => m.valuta.ToLower() == hnb.valuta.ToLower());

                if (pbz.kupovni_tecaj > hnb.srednji_tecaj)
                {
                    pbz.color = (Color)ColorConverter.ConvertFromString("blue");
                    hnb.color = (Color)ColorConverter.ConvertFromString("red");
                }
                else
                {
                    pbz.color = (Color)ColorConverter.ConvertFromString("red");
                    hnb.color = (Color)ColorConverter.ConvertFromString("blue");
                }
            }



            HnbGrid.ItemsSource = currencies;
            PbzGrid.ItemsSource = pbzValuta;

            
        }


        private async Task<List<HNBResponse>> GetCurrencies()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://www.hnb.hr");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("/hnb-tecajna-lista-portlet/rest/tecajn");
            response.EnsureSuccessStatusCode(); // Throw on error code. 
            var jsonResponse = await response.Content.ReadAsStringAsync();


            var jsonSetting = new JsonSerializerSettings();
            jsonSetting.Culture = new CultureInfo("hr-HR");
            var result = await JsonConvert.DeserializeObjectAsync<List<HNBResponse>>(jsonResponse, jsonSetting);

                   
            return result;


        }

        private async Task<List<PBZResponse>> PbzCurrencies()
        {
            List<PBZResponse> PbzResult = new List<PBZResponse>();

            HttpClient pbzClient = new HttpClient();
            pbzClient.BaseAddress = new Uri("http://www.hnb.hr");
            pbzClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage pbzResponse = await pbzClient.GetAsync("/hnb-tecajna-lista-portlet/rest/tecajn");
            pbzResponse.EnsureSuccessStatusCode(); // Throw on error code. 
            var jsonPbzResponse = await pbzResponse.Content.ReadAsStringAsync();
            var jsonPbzSetting = new JsonSerializerSettings();
            jsonPbzSetting.Culture = new CultureInfo("hr-HR");
            var result = await JsonConvert.DeserializeObjectAsync<List<PBZResponse>>(jsonPbzResponse, jsonPbzSetting);

           
            return result;
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
