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
using System.Windows.Shapes;

namespace PLC
{
    /// <summary>
    /// ChooseBoard.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseBoard : Window
    {
        public int Board_type = 0;  //0:MSP430 1:LPC2119+RTOS 2:STM32 3:Arduino

        public ChooseBoard()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        public void Board_Clicked(object sender, RoutedEventArgs e)
        {
            Button bbtn = (Button)sender;
            string boardname = (bbtn).Name;
            if (boardname == "MSP430")
            { this.Board_type = 0; }
            if (boardname == "LPC2119")
            { this.Board_type = 1; }
            if (boardname == "STM32")
            { this.Board_type = 2; }
            if (boardname == "Arduino")
            { this.Board_type = 3; }
            this.Close();
        }

    }
}
