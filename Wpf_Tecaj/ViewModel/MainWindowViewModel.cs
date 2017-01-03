using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Wpf_Tecaj.Model;

namespace Wpf_Tecaj.ViewModel
{
    class MainWindowViewModel
    {
        public async void BindData()
        {

           var DAL = new GetDataModel();
            var hnbCurrencies = await DAL.GetCurrencies();
            var pbzCurrencies = await DAL.PbzCurrencies();

            foreach (var hnbRow in hnbCurrencies)
            {

                var hnb = hnbRow;

                var pbz = pbzCurrencies.SingleOrDefault(m => m.name.ToLower() == hnb.valuta.ToLower());


                if (pbz.meanRate > hnb.srednji_tecaj)
                {
                    pbz.PBZcolor = (Color)ColorConverter.ConvertFromString("red");
                    hnb.HNBcolor = (Color)ColorConverter.ConvertFromString("yellow");
                }
                else
                {
                    pbz.PBZcolor = (Color)ColorConverter.ConvertFromString("yellow");
                    hnb.HNBcolor = (Color)ColorConverter.ConvertFromString("red");
                }
            }

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).HnbGrid.ItemsSource = hnbCurrencies;
                    (window as MainWindow).PbzGrid.ItemsSource = pbzCurrencies;
                }
            }




}
    }
}
