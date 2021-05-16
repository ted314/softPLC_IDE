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
        public void Btn_Create(int rnum = 4, int cnum = 4)
        {
            int ID_num=0;
            for (int r = 0; r < rnum; r++)
            {
                for (int c = 0; c < cnum; c++)
                {
                    this.btn[r, c] = new BlockButton();
                    this.btn[r, c].ID = ID_num;
                    ID_num += 1;
                    this.btn[r, c].BorderThickness = new Thickness(0);
                    Grid Bgird = new Grid();
                    if (c == 0)   //母线单元
                    {
                        this.btn[r, c].type = -3;
                        Image Bimage = new Image();
                        Bimage.Height = 55;
                        Bimage.Width = 70;
                        BitmapImage bi3 = new BitmapImage();
                        bi3.BeginInit();
                        bi3.UriSource = new Uri("rung.bmp", UriKind.Relative);
                        bi3.EndInit();
                        Bimage.Source = bi3;
                        Bgird.Children.Add(Bimage);
                    }
                    else  //非母线单元，不需要带文本框
                    {
                        this.btn[r, c].type = 0;
                        /*
                        TextBox Btex = new TextBox();
                        Btex.BorderThickness = new Thickness(0);
                        Btex.FontSize = 11;
                        Btex.Text = "   ";
                        Btex.Height = 15;
                        Btex.VerticalAlignment = VerticalAlignment.Top;
                        Btex.HorizontalAlignment = HorizontalAlignment.Center;*/
                        Image Bimage = new Image();
                        Bimage.Stretch = Stretch.Fill;
                        Bimage.Height = 55;
                        Bimage.Width = 70;
                        BitmapImage bi3 = new BitmapImage();
                        bi3.BeginInit();
                        bi3.UriSource = new Uri("blank.bmp", UriKind.Relative);
                        bi3.EndInit();
                        Bimage.Source = bi3;
                        Bgird.Children.Add(Bimage);
                        //Bgird.Children.Add(Btex);
                    }
                    this.btn[r, c].row = r;
                    this.btn[r, c].column = c;
                    if (r==0)
                    { this.btn[r, c].Background=Brushes.LightGray; }
                    else
                    { this.btn[r, c].Content = Bgird; }
                    this.btn[r, c].Click += Block_Clicked;
                    //btn[r, c].Height = 55;
                    //btn[r, c].Width = 70;
                    //btn[r, c].Background = Brushes.Blue;
                    //btn[r, c].Template= {StaticResource aha};
                    //MainWindow mw = new MainWindow();
                    this.LDgraph.Children.Add(this.btn[r, c]);

                    //初始化时将Map、Pic数组中的母线单元设置为-3
                    if (c == 0)
                    {
                        this.LD_Map[r, c] = -3;
                        this.LD_Pic[r, c] = -3;
                    }
                }
            }
        }
    }
}
