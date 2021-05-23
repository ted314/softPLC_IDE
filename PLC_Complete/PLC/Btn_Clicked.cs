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
        public void Block_Clicked(object sender, RoutedEventArgs e)
        {
            int clickrow = ((BlockButton)sender).row;
            int clickcol = ((BlockButton)sender).column;
            if (clickcol == 0 && Pointer_State!=0)
            { Rung_Error(); }
            if (clickrow == 0 && Pointer_State != 0)
            { Row_Error(); }
            else
            {
                if (this.Pointer_State == -20)
                {
                    LineBlock_Set((BlockButton)sender, 0);
                    this.LD_Map[clickrow, clickcol] = 0;  //根据当前选中的元件设置map数组元素的值
                    this.btn[clickrow, clickcol].type = 0;  //设置按钮对象的属性值
                    this.LD_Pic[clickrow, clickcol] = 0;
                    Nears_Change(clickrow, clickcol, 0);
                }
                if (this.Pointer_State < 0 && this.Pointer_State != -20)
                {
                    LineBlock_Set((BlockButton)sender, this.Pointer_State);
                    this.LD_Map[clickrow, clickcol] = this.Pointer_State;  //根据当前选中的元件设置map数组元素的值
                    this.btn[clickrow, clickcol].type = this.Pointer_State;  //设置按钮对象的属性值
                    this.LD_Pic[clickrow, clickcol] = this.Pointer_State;
                    Nears_Change(clickrow, clickcol, this.Pointer_State);
                }
                if (this.Pointer_State > 0)
                {
                    this.LD_Map[clickrow, clickcol] = this.Pointer_State;  //根据当前选中的元件设置map数组元素的值
                    this.btn[clickrow, clickcol].type = this.Pointer_State;  //设置按钮对象的属性值
                    this.LD_Pic[clickrow, clickcol] = this.Pointer_State;
                    CellBlock_Set((BlockButton)sender, this.Pointer_State);
                    Nears_Change(clickrow, clickcol, this.Pointer_State);
                }
            }
        }

        public void LineBlock_Set(BlockButton bbtn,int btype)
        {
            this.LD_Pic[bbtn.row, bbtn.column] = btype;   //可视化数组中的值也要设置
            string PicName;
            switch (btype)
            {
                case (-1): PicName = "horizon.bmp"; break;
                case (-2): PicName = "vertical.bmp"; break;
                case (-3): PicName = "rung.bmp"; break;
                case (-4): PicName = "across.bmp"; break;
                case (-5): PicName = "adownright.bmp"; break;
                case (-6): PicName = "ahordown.bmp"; break;
                case (-7): PicName = "ahorup.bmp"; break;
                case (-8): PicName = "aleftdown.bmp"; break;
                case (-9): PicName = "aleftup.bmp"; break;
                case (-10): PicName = "aupright.bmp"; break;
                case (-11): PicName = "rung_act.bmp"; break;
                case (-12): PicName = "averleft.bmp"; break;
                case (-13): PicName = "averright.bmp"; break;
                default: PicName = "blank.bmp"; break;
            }
            Grid Bgird = new Grid();
            Image Bimage = new Image();
            Bimage.Height = 55;
            Bimage.Width = 70;
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(PicName, UriKind.Relative);
            bi3.EndInit();
            Bimage.Source = bi3;
            Bgird.Children.Add(Bimage);
            bbtn.Content = Bgird;
        }

        public void CellBlock_Set(BlockButton bbtn, int btype)
        {
            this.LD_Pic[bbtn.row, bbtn.column] = btype;  //可视化数组中的值也要设置
            string PicName;
            switch ((int)(btype))
            {
                case 1: PicName = "opentrig.bmp";break;
                case 2: PicName = "closetrig.bmp"; break;
                case 5: PicName = "coil.bmp"; break;
                default: PicName = "blank.bmp";break;
            }
            Grid Bgird = new Grid();
            //TextBox Btex = new TextBox();
            BlockTextBox Btex = new BlockTextBox();
            Btex.BorderThickness = new Thickness(0);
            Btex.FontSize = 11;
            Btex.Text = "Name";
            Btex.Height = 15;
            Btex.VerticalAlignment = VerticalAlignment.Top;
            Btex.HorizontalAlignment = HorizontalAlignment.Center;
            Btex.row = bbtn.row;
            Btex.column = bbtn.column;
            Btex.TextChanged += BlockName_Changed;
            Image Bimage = new Image();
            Bimage.Height = 55;
            Bimage.Width = 70;
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(PicName, UriKind.Relative);
            bi3.EndInit();
            Bimage.Source = bi3;
            Bgird.Children.Add(Bimage);
            Bgird.Children.Add(Btex);
            bbtn.Content = Bgird;
        }

        //要求书写的人程序之间空一行
        public void Nears_Change(int rf,int cf,int type)
        {
            if (cf == 1 && type != -2 && type != 0)  //rf、cf、type都只在设置母线单元时候使用
            {
                LineBlock_Set(this.btn[rf, cf - 1], -11);
                this.btn[rf, cf - 1].type = -11;
            }

            if (cf == 1 && this.Pointer_State==-20 && this.btn[rf, cf - 1].type==-11)  //若清除母线旁边的横线，就要重置母线单元
            {
                LineBlock_Set(this.btn[rf, cf - 1], -3);
                this.btn[rf, cf - 1].type = -3;
            }

            for (int r = 0; r < LDrow; r++)
            {
                for (int c = 1; c < LDcol; c++)
                {

                    int leftb = 0;
                    int rightb = 0;
                    int upb = 0;
                    int downb = 0;

                    if (r != 0)
                    {
                        if (this.LD_Map[r - 1, c] == -2 | (this.LD_Map[r - 1, c] == -1 && this.LD_Map[r,c] == -2 ))
                        {
                            if (this.LD_Map[r - 1, c] == -2 && this.LD_Map[r, c] == -1) ;
                            else
                            { upb = 1; }
                        }
                    }
                    if (r < LDrow-1)
                    {
                        if (this.LD_Map[r + 1, c] == -2)
                        { downb = 1; }
                    }
                    if (c != 0)
                    {
                        if (this.LD_Map[r, c - 1] != 0)
                        {
                            if (this.LD_Map[r, c] == -2 && (this.LD_Map[r, c - 1] == -3 | this.LD_Map[r, c - 1] == -11)
                                    | (this.LD_Map[r, c] == -2 && this.LD_Map[r, c - 1] == -2)) ; //无事发生
                            else
                            { leftb = 1; }
                        }
                    }
                    if (c < LDcol-1)
                    {
                        if (this.LD_Map[r, c + 1] != 0 )
                        {
                            if (this.LD_Map[r, c] == -2 && this.LD_Map[r, c + 1] == -2) ;  //无事发生
                            else
                            { rightb = 1; }
                        } 
                    }
                    
                    if (this.LD_Map[r, c] < 0)
                    {
                        if (downb == 0 && upb == 0 && leftb == 1 && rightb == 1)
                        { LineBlock_Set(this.btn[r, c], -1); }
                        if (downb == 1 && upb == 1 && leftb == 0 && rightb == 0)
                        { LineBlock_Set(this.btn[r, c], -2); }
                        if (downb == 1 && upb == 1 && leftb == 1 && rightb == 1)
                        { LineBlock_Set(this.btn[r, c], -4); }
                        if (downb == 1 && upb == 0 && leftb == 0 && rightb == 1)
                        { LineBlock_Set(this.btn[r, c], -5); }
                        if (downb == 1 && upb == 0 && leftb == 1 && rightb == 1)
                        { LineBlock_Set(this.btn[r, c], -6); }
                        if (downb == 0 && upb == 1 && leftb == 1 && rightb == 1)
                        { LineBlock_Set(this.btn[r, c], -7); }
                        if (downb == 1 && upb == 0 && leftb == 1 && rightb == 0)
                        { LineBlock_Set(this.btn[r, c], -8); }
                        if (downb == 0 && upb == 1 && leftb == 1 && rightb == 0)
                        { LineBlock_Set(this.btn[r, c], -9); }
                        if (downb == 0 && upb == 1 && leftb == 0 && rightb == 1)
                        { LineBlock_Set(this.btn[r, c], -10); }
                        if (downb == 1 && upb == 1 && leftb == 1 && rightb == 0)
                        { LineBlock_Set(this.btn[r, c], -12); }
                        if (downb == 1 && upb == 1 && leftb == 0 && rightb == 1)
                        { LineBlock_Set(this.btn[r, c], -13); }
                    }
                }
            }
        }

        public void Rung_Error()
        { MessageBox.Show("你不能设置母线单元！"); }

        public void Row_Error()
        { MessageBox.Show("你不能在第一行添加代码！"); }

        /*
        //该自动报错功能尚未实现
        public void Segment_Error()
        {
            MessageBox.Show("程序之间必须间隔一行以免引起线元错误！");
        }
        */
        public void BlockName_Changed(object sender, RoutedEventArgs e)
        {
            int textrow = ((BlockTextBox)sender).row;
            int textcol = ((BlockTextBox)sender).column;
            this.btn[textrow, textcol].Block_Name = ((BlockTextBox)sender).Text;
        }
    }
}
