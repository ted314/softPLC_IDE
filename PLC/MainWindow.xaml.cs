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
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    

    //定义全局变量和主函数
    public partial class MainWindow : Window
    {
        static public int LDrow=20;
        static public int LDcol=10;

        public int Pointer_State;
        public BlockButton[,] btn = new BlockButton[LDrow, LDcol];  //按钮数组仅在添加时有用（给row、column赋值）
        public int[,] LD_Map = new int[LDrow, LDcol];  //有一说一，在block中有了type之后这个数组其实没啥用了
        public int[,] LD_Pic = new int[LDrow, LDcol];  //线元已经完成了连接，用于可视化

        public IList<Treenode> Rungnodes = new List<Treenode>();//每个子程序会有一个二叉树的根节点，全部都存储在这里
        
        public MainWindow()
        {
            InitializeComponent();
            Btn_Create(LDrow, LDcol);  //向PLC编辑区添加控件
        }
    }
}
