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

namespace PLC
{
    public partial class MainWindow : Window
    {
        public void Generate_Config()
        {
            MessageBox.Show("请去配置元件I/O端口");
            foreach (BlockButton bbtn in this.btn)
            {
                if (bbtn.type > 0)
                { Create_IO(bbtn); }
            }
            Button confirmbtn = new Button();
            confirmbtn.Content = "我已完成配置";
            confirmbtn.Click += Config_to_C;
            this.IO_Tab.Children.Add(confirmbtn);
        }

        public void Port_Clicked(object sender, RoutedEventArgs e)
        {
            int clickrow=((BlockTextBox) sender).row;
            int clickcol = ((BlockTextBox)sender).column;
            int port = Convert.ToInt32(((BlockTextBox)sender).Text);
            this.btn[clickrow, clickcol].Port_Number = port;
        }

        public void Create_IO(BlockButton bbtn)
        {
            TextBlock bName = new TextBlock();
            bName.Text = bbtn.Block_Name;
            bName.HorizontalAlignment = HorizontalAlignment.Center;
            this.IO_Tab.Children.Add(bName);

            TextBlock IOtype = new TextBlock();
            if (bbtn.type > 0 && bbtn.type != 5)
            {
                IOtype.Text = "Input";
                this.btn[bbtn.row, bbtn.column].Port_State = 0;
            }
            else
            {
                IOtype.Text = "Output";
                this.btn[bbtn.row, bbtn.column].Port_State = 1;
            }
            IOtype.HorizontalAlignment = HorizontalAlignment.Center;
            this.IO_Tab.Children.Add(IOtype);
            
            BlockComboBox Port_choose = new BlockComboBox();
            Port_choose.row = bbtn.row;
            Port_choose.column = bbtn.column;
            foreach (string portname in this.Input_Ports)
            {
                ComboBoxItem pnum = new ComboBoxItem();
                pnum.Content = portname;
                pnum.Background = Brushes.LightGreen;
                Port_choose.Items.Add(pnum);
            }
            foreach (string portname in this.Output_Ports)
            {
                ComboBoxItem pnum = new ComboBoxItem();
                pnum.Content = portname;
                pnum.Background = Brushes.LightBlue;
                Port_choose.Items.Add(pnum);
            }
            this.IO_Tab.Children.Add(Port_choose);
            this.Port_Maps.Add(Port_choose);
        }
    }
}
