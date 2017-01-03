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

namespace Wpf_Tecaj.Model
{
    class GetDataModel
    {


        public class HNBResponse
        {
            public string valuta { get; set; }
            public decimal kupovni_tecaj { get; set; }
            public decimal srednji_tecaj { get; set; }
            public decimal prodajni_tecaj { get; set; }
            public DateTime datum { get; set; }
            public Color HNBcolor { get; set; }
        }

        public class PBZResponse
        {
            public string name { get; set; }
            public decimal meanRate { get; set; }
            public Color PBZcolor { get; set; }
        }





        public async Task<List<HNBResponse>> GetCurrencies()
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



        public async Task<List<PBZResponse>> PbzCurrencies()
        {
            HttpClient pbzClient = new HttpClient();
            var xml = await pbzClient.GetStringAsync("https://www.pbz.hr/Tecajnice/Danasnja/pbzteclist.xml");

            var doc = XElement.Parse(xml);

            var result = doc.Element("ExchRate").Elements("Currency").Select(x => new PBZResponse
            {
                name = x.Element("Name").Value,
                meanRate = decimal.Parse(x.Element("MeanRate").Value.Replace(',', '.'), CultureInfo.InvariantCulture)
            });



            return result.ToList();
        }



        public async void loadData()
        {
            if (Application.Current.Resources["IsUserAuthenticated"] == null)
                return;

            var currencies = await GetCurrencies();
            var pbzValuta = await PbzCurrencies();

            using (var context = new CurrenciesContext())
            {
                foreach (var hnb in currencies)
                {

                    var tecajTable = context.Tecaj.SingleOrDefault(m => m.Ime == hnb.valuta);

                    if (tecajTable == null)
                    {
                        tecajTable = new Tecaj();
                        tecajTable.Srednji_Tecaj = hnb.srednji_tecaj;
                        tecajTable.Ime = hnb.valuta;
                        context.Tecaj.Add(tecajTable);
                    }
                    else
                    {
                        tecajTable.Srednji_Tecaj = hnb.srednji_tecaj;
                    }

                    var pbz = pbzValuta.SingleOrDefault(m => m.name.ToLower() == hnb.valuta.ToLower());
                }

                context.SaveChanges();
            }
        }




    }
}
