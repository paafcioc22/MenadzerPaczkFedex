using MenadżerPaczek.Serwis;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logika interakcji dla klasy ListaPaczekPage.xaml
    /// </summary>
    public partial class ListaPaczekPage : Window
    {

        SQL sQL;
        private int gidNumer;

        public ListaPaczekPage()
        {
            InitializeComponent();
            sQL = new SQL();

        }

        public ListaPaczekPage(int v)
        {
            InitializeComponent();
            sQL = new SQL();
            this.gidNumer = v;
        }

        private void btn_addPaczka_Click(object sender, RoutedEventArgs e)
        {
            DodajMMdoPaczki addMMtoPack = new DodajMMdoPaczki(gidNumer);
            addMMtoPack.Show();
        }

        protected override void OnActivated(EventArgs e)
        {
            if (sQL.GetListaPaczek(gidNumer).Count > 0)
            {
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = sQL.GetListaPaczek(gidNumer);
            }
             
            base.OnActivated(e);

        }
    }
}
