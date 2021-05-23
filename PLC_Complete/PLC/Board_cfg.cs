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
        public void Set_INOUT()
        {
            //MSP430G2553
            if (this.Board_Type == 0)
            {
                this.Input_Ports.Add("P1.0");
                this.Input_Ports.Add("P1.1");
                this.Input_Ports.Add("P1.2");
                this.Input_Ports.Add("P1.3");
                this.Output_Ports.Add("P1.4");
                this.Output_Ports.Add("P1.5");
                this.Output_Ports.Add("P1.6");
                this.Output_Ports.Add("P1.7");
            }
            //LPC2119 + RTOS
            if (this.Board_Type == 1)
            {
                this.Input_Ports.Add("P1.0");
                this.Input_Ports.Add("P1.1");
                this.Input_Ports.Add("P1.2");
                this.Input_Ports.Add("P1.3");
                this.Output_Ports.Add("P1.4");
                this.Output_Ports.Add("P1.5");
                this.Output_Ports.Add("P1.6");
                this.Output_Ports.Add("P1.7");
            }
            //STM32
            if (this.Board_Type == 2)
            {
                this.Input_Ports.Add("P1.0");
                this.Input_Ports.Add("P1.1");
                this.Input_Ports.Add("P1.2");
                this.Input_Ports.Add("P1.3");
                this.Output_Ports.Add("P1.4");
                this.Output_Ports.Add("P1.5");
                this.Output_Ports.Add("P1.6");
                this.Output_Ports.Add("P1.7");
            }
            //Arduino Nano
            if (this.Board_Type == 3)
            {
                this.Input_Ports.Add("P1.0");
                this.Input_Ports.Add("P1.1");
                this.Input_Ports.Add("P1.2");
                this.Input_Ports.Add("P1.3");
                this.Output_Ports.Add("P1.4");
                this.Output_Ports.Add("P1.5");
                this.Output_Ports.Add("P1.6");
                this.Output_Ports.Add("P1.7");
            }
        }

        public void IO_Mapping(int boardtype)
        {
            foreach (BlockComboBox bcx in this.Port_Maps)
            {
                string portname = bcx.SelectionBoxItem.ToString();
                //MessageBox.Show(portname);
                switch (boardtype)
                {
                    case 0:
                        {
                            this.btn[bcx.row, bcx.column].Port_Number = Get_MSP430_num(portname);
                        }break;
                    case 1:
                        {
                            this.btn[bcx.row, bcx.column].Port_Number = Get_LPC_num(portname);
                        }
                        break;
                    case 2:
                        {
                            this.btn[bcx.row, bcx.column].Port_Number = Get_STM32_num(portname);
                        }
                        break;
                    case 3:
                        {
                            this.btn[bcx.row, bcx.column].Port_Number = Get_Arduino_num(portname);
                        }
                        break;
                }
            }
        }

        public int Get_MSP430_num(string portname)
        {
            int port_num = 0;
            switch (portname)
            {
                case "P1.0": port_num = 0; break;
                case "P1.1": port_num = 1; break;
                case "P1.2": port_num = 2; break;
                case "P1.3": port_num = 3; break;
                case "P1.4": port_num = 0; break;
                case "P1.5": port_num = 1; break;
                case "P1.6": port_num = 2; break;
                case "P1.7": port_num = 3; break;
            }
            return port_num;
        }

        public int Get_LPC_num(string portname)
        {
            int port_num = 0;
            switch (portname)
            {
                case "P1.0": port_num = 0; break;
                case "P1.1": port_num = 1; break;
                case "P1.2": port_num = 2; break;
                case "P1.3": port_num = 3; break;
                case "P1.4": port_num = 0; break;
                case "P1.5": port_num = 1; break;
                case "P1.6": port_num = 2; break;
                case "P1.7": port_num = 3; break;
            }
            return port_num;
        }

        public int Get_STM32_num(string portname)
        {
            int port_num = 0;
            switch (portname)
            {
                case "P1.0": port_num = 0; break;
                case "P1.1": port_num = 1; break;
                case "P1.2": port_num = 2; break;
                case "P1.3": port_num = 3; break;
                case "P1.4": port_num = 0; break;
                case "P1.5": port_num = 1; break;
                case "P1.6": port_num = 2; break;
                case "P1.7": port_num = 3; break;
            }
            return port_num;
        }

        public int Get_Arduino_num(string portname)
        {
            int port_num = 0;
            switch (portname)
            {
                case "P1.0": port_num = 0; break;
                case "P1.1": port_num = 1; break;
                case "P1.2": port_num = 2; break;
                case "P1.3": port_num = 3; break;
                case "P1.4": port_num = 0; break;
                case "P1.5": port_num = 1; break;
                case "P1.6": port_num = 2; break;
                case "P1.7": port_num = 3; break;
            }
            return port_num;
        }
    }
}
