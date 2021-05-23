using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PLC
{
    public partial class MainWindow : Window
    {
        public void New_File(object sender, RoutedEventArgs e)
        {
            TabItem ntab = new TabItem();
            ntab.Header = "New file";
            //TextBox ntext = new TextBox();
            Grid ngrid = new Grid();

            ntab.Content = ngrid;
            this.Tab_Total.Items.Add(ntab);
        }

        public void New_LD(object sender, RoutedEventArgs e)
        { }

        public void New_C(object sender, RoutedEventArgs e)
        {

        }

        public void Close_File(object sender, RoutedEventArgs e)
        { }

        public void Save_File(object sender, RoutedEventArgs e)
        { }

        public void Add_File(object sender, RoutedEventArgs e)
        { }

        public void Delete_File(object sender, RoutedEventArgs e)
        { }
    }
}
