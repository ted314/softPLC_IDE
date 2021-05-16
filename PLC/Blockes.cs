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
    public class BlockButton : Button
    {
        public int row;
        public int column;
        public int type;
        public string Block_Name;
        public int ID;//每个元件都有自己的标识编号

        public IList<BlockButton> leftBlocks = new List<BlockButton>();
        public IList<BlockButton> rightBlocks = new List<BlockButton>();

        public bool IsAOVPoint = false;
        public IList<BlockButton> leftAOVs = new List<BlockButton>();
        public IList<BlockButton> rightAOVs = new List<BlockButton>();
        public int left_num = 0;  //改后仅用于表征AOV节点的左右连接数
        public int right_num = 0;
        public int AccessTime = 0;//用于转二叉树时计数
    }
    

    public class BlockTextBox : TextBox
    {
        public int row;
        public int column;
    }
    
}
