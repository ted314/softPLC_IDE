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

//事件函数定义在AOVtoTree
namespace PLC
{
    public partial class MainWindow : Window
    {
        public void Compile_Whole(object sender, RoutedEventArgs e)
        {
            MaptoAOV();

            String IL_Programs = "";
            foreach (Treenode rootnode in this.Rungnodes)
            {
                IList<Treenode> newTree = AOV_BTree(rootnode);
                //Tree_Visual(newTree);
                IList<Treenode> finalTree = Simplify_Tree(newTree);
                //Tree_Visual(finalTree);
                IList<Treenode> IL_List = Tree_Traversal(finalTree);
                this.IL_Programs.Add(IL_List);
                string ILtext = Get_IL(IL_List);
                IL_Programs += ILtext;
            }
            this.IL_TextBox.Text = IL_Programs;

            this.Compiling_Info += ("已将所有子程序的IL指令生成\n");
            this.Compiling_Window.Text = this.Compiling_Info;
            MessageBox.Show("IL指令已生成！");

            Generate_Config();
        }

        public void IL_Clicked(object sender, RoutedEventArgs e)
        { Generate_Config(); }

        public void Config_to_C(object sender, RoutedEventArgs e)
        {
            IO_Mapping(this.Board_Type);
            //对于每个IL程序都新建一个标签页
            int tab_num = 1;
            foreach (IList<Treenode> ILlist in this.IL_Programs)
            {
                String C_Program = "";
                TabItem ntab = new TabItem();
                ntab.Header = ("C_Program" + tab_num.ToString());
                TextBox nctext = new TextBox();
                C_Program = IL_to_C(ILlist);  //将每个梯形图转化为一个C程序
                nctext.Text = C_Program;
                ntab.Content = nctext;
                this.Tab_Total.Items.Add(ntab);
            }
            this.Compiling_Info += ("编译成功！\n");
            this.Compiling_Window.Text = this.Compiling_Info;
        }

        public string IL_to_C(IList<Treenode> IL_List)
        {
            string C_Func = "void Main_PLC()\n";
            C_Func += "{\n";
            foreach (Treenode ttn in IL_List)
            {
                C_Func += Get_Block_C(ttn);
            }
            C_Func += "}\n";
            return C_Func;
        }

        public string Get_Block_C(Treenode ttn)
        {
            string sent = "";
            switch (ttn.nodetype)
            {
                case 1: sent += "\tIL_AND();\n"; break;
                case 2: sent += "\tIL_OR();\n"; break;
                case 0:
                    {
                        if (ttn.nodedata.type == 1)
                        { sent += ("\tIL_LD( " + ttn.nodedata.Port_Number.ToString() + " );\n"); }
                        if (ttn.nodedata.type == 2)
                        { sent += ("\tIL_LDN( " + ttn.nodedata.Port_Number.ToString() + " );\n"); }
                        if (ttn.nodedata.type == 5)
                        { sent += ("\tIL_OUT( " + ttn.nodedata.Port_Number.ToString() + " );\n"); }
                    }
                    break;
            }
            return sent;
        }
    }
}
