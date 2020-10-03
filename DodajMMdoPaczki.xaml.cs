using MenadżerPaczek.Model;
using MenadżerPaczek.Serwis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace MenadżerPaczek
{
    /// <summary>
    /// Logika interakcji dla klasy DodajMMdoPaczki.xaml
    /// </summary>
    public partial class DodajMMdoPaczki : Window
    {

        ObservableCollection<MM> CheckMMToPaczkaList;
        SQL sQL;
        private int gidnr;
        public DodajMMdoPaczki(int v)
        {
            InitializeComponent();
            sQL = new SQL();
            FillDataMM();
            CheckMMToPaczkaList = new ObservableCollection<MM>();
            gidnr = v;
        }


        void FillDataMM()
        {
            

            grid_MM.ItemsSource = sQL.GetListaMM();
        }

        private void btn_addMMtoPaczka_Click(object sender, RoutedEventArgs e)
        {

            foreach (var row in grid_MM.Items)
            {


                var wpis = (MM)row;

                if (wpis.IsCheckd)
                {
                    wpis.IsCheckd = false;
                    CheckMMToPaczkaList.Add(wpis);
                }

            }

            grid_Paczka.ItemsSource = CheckMMToPaczkaList;
          
            //grid_MM.Items.OfType<MM>().ToList().ForEach(x => x.IsCheckd = false);

        }

        private void btn_delte_Click(object sender, RoutedEventArgs e)
        {
           

            if (grid_Paczka.SelectedItems != null && grid_Paczka.SelectedItems.Count > 0)
            {
                var toRemove = grid_Paczka.SelectedItems.Cast<MM>().Where(s=>s.IsCheckd==true).ToList();

 
                var items = grid_Paczka.ItemsSource as ObservableCollection<MM>;
                if (items != null)
                {
                    foreach (var order in toRemove)
                    {
                        items.Remove(order);
                    }
                }
                grid_Paczka.ItemsSource = null;
                grid_Paczka.ItemsSource = items;
            }

        }

        private void btn_addPaczkaDone_Click(object sender, RoutedEventArgs e)
        {
            if(sQL.DodajPaczke2Sql(CheckMMToPaczkaList, gidnr))
            this.Close();
        }
    }
}
