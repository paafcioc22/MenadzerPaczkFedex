using MenadżerPaczek.Model;
using MenadżerPaczek.Serwis;
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

namespace MenadżerPaczek
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQL sQL;
        public MainWindow()
        {
            InitializeComponent();
            sQL = new SQL();

            //var rowList = sQL.GetListaZlecen<ListaZlecenView>();
            //dataGrid.ItemsSource = rowList;
        }

        private void btn_tworzZlecenie_Click(object sender, RoutedEventArgs e)
        {
            
           
            ListaPaczekPage listaPaczek = new ListaPaczekPage(sQL.GetLastGidNUmer());
            listaPaczek.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Ustawienia ustawienia = new Ustawienia();
            ustawienia.Show();
        }

        protected override void OnActivated(EventArgs e)
        {
            //dataGrid.Items.Clear();
            var rowList = sQL.GetListaZlecen<ListaZlecenView>();

            //if (rowList.Count > 0)
            //{
            //    foreach (object[] row in rowList)
            //    {
            //        string[] orderDetails = new string[row.Length];
            //        int columnIndex = 0;

            //        foreach (object column in row)
            //        {
            //            orderDetails[columnIndex++] = Convert.ToString(column);
            //        }


            //    }
            //}
            dataGrid.ItemsSource = rowList;


            base.OnActivated(e);
        }
    }
}
