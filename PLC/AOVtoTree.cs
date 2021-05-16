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
    public class Treenode
    {
        public BlockButton nodedata;

        public Treenode leftchild;
        public Treenode rightchild;
        public Treenode parent;
        public int nodeID;
        public int nodetype; //用于标识节点的种类，0是元件，1是与节点，2是或节点

        public Treenode()  //无参数时的构造函数
        {
            this.nodeID = 0;
            this.nodetype = 0;
        }
        public Treenode(int type)  //有参数指定节点种类时的构造函数
        {
            this.nodeID = 0;
            this.nodetype = type;
        }
    }

    public partial class MainWindow : Window
    {
        public void Tree_Clicked(object sender, RoutedEventArgs e)
        {
            String Programs = "";
            foreach (Treenode rootnode in this.Rungnodes)
            {
                IList<Treenode> newTree = AOV_BTree(rootnode);
                //Tree_Visual(newTree);
                IList<Treenode> finalTree = Simplify_Tree(newTree);
                //Tree_Visual(finalTree);
                IList<Treenode> IL_List = Tree_Traversal(finalTree);
                string ILtext = Get_IL(IL_List);
                Programs += ILtext;
            }
            this.IL_TextBox.Text = Programs;
        }

        //AOV转二叉树的状态机算法
        public IList<Treenode> AOV_BTree(Treenode rungnode)
        {
            //由于没有指针，树需要用一个列表辅助存储
            IList<Treenode> TreeList = new List<Treenode>();
            TreeList.Add(rungnode);
            TreeList[0].parent = null;

            Stack<Treenode> ANDstack = new Stack<Treenode>();
            Stack<Treenode> ORstack = new Stack<Treenode>();
            Access_Reset(); //把所有AOV顶点的Accesstime清零
            BlockButton P1 = rungnode.nodedata;
            Treenode P2 = rungnode;

            //进入状态机
            int state = 3;
            bool End_Tag = false;
            int treeID = 0;
            bool OR_control = false; //当
            for (int i = 0; i < 150; i++)
            {
                int rowP1 = P1.row;
                int colP1 = P1.column;
                int P2_index = P2.nodeID;

                //MessageBox.Show(state.ToString());
                
                switch (state)
                {
                    case 3:
                        {
                            string str1 = ("TreeID=" + treeID.ToString());
                            string str2 = ("P1: " + Showblock(btn[rowP1, colP1]));
                            string str3 = ("P2: " + Show_Treenode(TreeList[P2_index]));
                            //MessageBox.Show(str1+"\n"+str2+"\n"+str3);

                            if (rowP1 == 0)
                            { End_Tag = true; }
                            else
                            {
                                btn[rowP1, colP1].AccessTime += 1;
                                if (btn[rowP1, colP1].AccessTime >= btn[rowP1, colP1].left_num)
                                { state = 4; }
                                else
                                { state = 9; }
                            }
                        } break;
                    case 4:
                        {
                            if (btn[rowP1, colP1].left_num >= 2)
                            { state = 5; }
                            else
                            { state = 6; }
                        }break;
                    case 5:
                        {
                            P2 = ANDstack.Pop();
                            //MessageBox.Show(Show_Treenode(P2));
                            OR_control = false;
                            state = 6;
                        }break;
                    case 6:
                        {
                            if (btn[rowP1, colP1].right_num >= 2)
                            { state = 8; }
                            else
                            { state = 7; }
                        }break;
                    case 7:
                        {
                            Treenode P3 = new Treenode(1); //创建一个与节点
                            treeID += 1;
                            P3.nodeID = treeID;
                            TreeList.Add(P3);
                            if (TreeList[P2_index].leftchild == null)
                            { TreeList[P2_index].leftchild = TreeList[P3.nodeID]; }
                            else
                            { TreeList[P2_index].rightchild = TreeList[P3.nodeID]; }
                            TreeList[P3.nodeID].parent = TreeList[P2_index];

                            if (colP1 != 0)
                            {
                                Treenode P0 = new Treenode(0); //创建一个元件节点，令其data为P1指向的顶点
                                treeID += 1;
                                P0.nodeID = treeID;
                                TreeList.Add(P0);
                                TreeList[P3.nodeID].leftchild = TreeList[P0.nodeID];//把元件节点设置为P3的左孩子
                                TreeList[P0.nodeID].parent = TreeList[P3.nodeID];
                                TreeList[P0.nodeID].nodedata = btn[rowP1, colP1];
                            }
                            P2 = TreeList[P3.nodeID]; //令P2指向P3的节点
                            if (this.btn[rowP1, colP1].rightAOVs.Count > 0)
                            { P1 = this.btn[rowP1, colP1].rightAOVs[0]; }
                            else
                            { P1 = this.btn[0, 0]; } //如果当前右连接集为空，就将P1指向不能编辑的第0行的按钮
                            
                            state = 3;
                        }break;
                    case 8:
                        {
                            Treenode P3 = new Treenode(1); //创建一个与节点
                            treeID += 1;
                            P3.nodeID = treeID;
                            TreeList.Add(P3);
                            Treenode P4 = new Treenode(1); //创建一个与节点
                            treeID += 1;
                            P4.nodeID = treeID;
                            TreeList.Add(P4);
                            IList<Treenode> tempAND = new List<Treenode>();//用于创建(right_num-2)个与节点
                            for (int cnt = 2; cnt < this.btn[rowP1, colP1].right_num; cnt++)
                            {
                                Treenode Pa = new Treenode(1); //创建一个与节点
                                treeID += 1;
                                Pa.nodeID = treeID;
                                TreeList.Add(Pa);
                                tempAND.Add(TreeList[Pa.nodeID]);
                            }
                            IList<Treenode> tempOR = new List<Treenode>();//用于创建(right_num-1)个或节点
                            for (int cnt = 1; cnt < this.btn[rowP1,colP1].right_num ; cnt++)
                            {
                                Treenode Pn = new Treenode(2); //创建一个或节点
                                treeID += 1;
                                Pn.nodeID = treeID;
                                TreeList.Add(Pn);
                                //Pn.nodedata = TreeList[P2_index].nodedata.rightAOVs[cnt];//加入列表时就让或节点的data指向第二个右顶点
                                Pn.nodedata = this.btn[rowP1, colP1].rightAOVs[this.btn[rowP1, colP1].right_num - cnt];//加入列表时就让或节点的data指向第二个右顶点
                                tempOR.Add(TreeList[Pn.nodeID]);
                            }
                            //把P3插入P2
                            if (TreeList[P2_index].leftchild == null)
                            { TreeList[P2_index].leftchild = TreeList[P3.nodeID]; }
                            else
                            { TreeList[P2_index].rightchild = TreeList[P3.nodeID]; }
                            TreeList[P3.nodeID].parent = TreeList[P2_index];
                            //把P4插入P3
                            TreeList[P3.nodeID].leftchild = TreeList[P4.nodeID];
                            TreeList[P4.nodeID].parent = TreeList[P3.nodeID];
                            //为P1指向的顶点申请一个元件结点，插入P4左孩子
                            if (colP1 != 0)
                            {
                                Treenode P0 = new Treenode(0); //创建一个元件节点，令其data为P1指向的顶点
                                treeID += 1;
                                P0.nodeID = treeID;
                                TreeList.Add(P0);
                                TreeList[P4.nodeID].leftchild = TreeList[P0.nodeID];//把元件节点设置为P4的左孩子
                                TreeList[P0.nodeID].parent = TreeList[P4.nodeID];
                                TreeList[P0.nodeID].nodedata = btn[rowP1, colP1];
                            }
                            //P5插入P4右孩子
                            TreeList[P4.nodeID].rightchild = TreeList[tempOR[0].nodeID];
                            TreeList[tempOR[0].nodeID].parent = TreeList[P4.nodeID];
                            for (int j = 1; j < tempOR.Count; j++)
                            {
                                TreeList[tempOR[j - 1].nodeID].leftchild = TreeList[tempAND[j - 1].nodeID];
                                TreeList[tempAND[j - 1].nodeID].parent = TreeList[tempOR[j - 1].nodeID];
                                TreeList[tempAND[j - 1].nodeID].rightchild = TreeList[tempOR[j].nodeID];
                                TreeList[tempOR[j].nodeID].parent = TreeList[tempAND[j - 1].nodeID];
                            }
                            ANDstack.Push(TreeList[P3.nodeID]);  //P3压入与堆栈
                            
                            foreach (Treenode tn in tempAND)  //所有P3、P4以外的与节点压入与堆栈，与P5以外的或节点一一对应
                            { ANDstack.Push(tn); }
                            foreach (Treenode tn in tempOR)  //所有或节点压入或堆栈
                            { ORstack.Push(tn); }
                            /*
                            for (int j = tempAND.Count - 1; j >= 0; j--)
                            {
                                ANDstack.Push(tempAND[j]);
                            }
                            for (int j = tempOR.Count - 1; j >= 0; j--)
                            {
                                ORstack.Push(tempOR[j]);
                            }*/
                            

                            /*
                            if (tempOR.Count >= 2)
                            {
                                for (int j = 0; j < tempOR.Count - 1; j++)
                                {
                                    TreeList[tempOR[j].nodeID].leftchild = TreeList[tempAND[j + 1].nodeID];
                                    TreeList[tempAND[j + 1].nodeID].parent = TreeList[tempOR[j].nodeID];
                                    TreeList[tempAND[j + 1].nodeID].rightchild = TreeList[tempOR[j + 1].nodeID];
                                    TreeList[tempOR[j + 1].nodeID].parent = TreeList[tempAND[j + 1].nodeID];
                                }
                            }
                            
                            if (TreeList[P2_index].leftchild == null)
                            { TreeList[P2_index].leftchild = TreeList[tempAND[0].nodeID]; }
                            else
                            { TreeList[P2_index].rightchild = TreeList[tempAND[0].nodeID]; }
                            TreeList[tempAND[0].nodeID].parent = TreeList[P2_index];

                            TreeList[P3.nodeID].leftchild = TreeList[P4.nodeID];
                            TreeList[P4.nodeID].parent = TreeList[P3.nodeID];

                            for (int j=0;j < tempOR.Count; j++)
                            {
                                if (j==0)  //P5插入P4是或节点插入到与节点，故是插入右孩子
                                { TreeList[P4.nodeID + j].rightchild = TreeList[P4.nodeID + j + 1]; }//P5、P6就紧接着P4被加入列表，故可以这样索引
                                else  //P6插入P5以及后面的均是或节点插入或节点，故都是左孩子
                                { TreeList[P4.nodeID + j].leftchild = TreeList[P4.nodeID + j + 1]; }
                                TreeList[P4.nodeID + j + 1].parent = TreeList[P4.nodeID + j];
                            }
                            ANDstack.Push(TreeList[P3.nodeID]);  //P3压入与堆栈
                            ANDstack.Push(TreeList[P4.nodeID]);  //P4压入与堆栈
                            
                            foreach (Treenode tn in tempAND)  //所有与节点压入与堆栈，与或节点一一对应
                            { ANDstack.Push(tn); }
                            foreach (Treenode tn in tempOR)  //所有或节点压入或堆栈
                            { ORstack.Push(tn); }
                            */

                            P1 = this.btn[rowP1, colP1].rightAOVs[0];
                            P2 = tempOR[tempOR.Count - 1];  //令P2指向最后一个或节点

                            state = 3;
                        }break;
                    case 9:
                        {
                            if (OR_control == true)  //说明该分支没有需要与的顶点，直接跳过（这个与结点是我后来加的）
                            {
                                Treenode tempa=ANDstack.Pop();
                                //MessageBox.Show(Show_Treenode(tempa));
                            }
                            Treenode P9 = ORstack.Pop();
                            //MessageBox.Show(Show_Treenode(P9));
                            OR_control = true;
                            P1 = TreeList[P9.nodeID].nodedata;
                            P2 = TreeList[P9.nodeID];
                            state = 3;
                        }break;
                }
                if (End_Tag == true)
                { break; }
            }
            return TreeList;
        }

        //用于把所有AOV顶点的Accesstime清零
        public void Access_Reset()
        {
            foreach (BlockButton bbtn in this.btn)
            {
                if (bbtn.IsAOVPoint == true)
                { bbtn.AccessTime = 0; }
            }
        }

        public bool Is_Full_Tree(IList<Treenode> TreeList)
        {
            bool full_tag = true;
            foreach (Treenode tn in TreeList)
            {
                if (TreeList[0].parent != null)
                {
                    MessageBox.Show("根节点出错！");
                }
                if (tn.nodeID != TreeList[0].nodeID && tn.nodetype != 0)
                {
                    if (tn.parent == null | tn.leftchild == null | tn.rightchild == null)
                    { full_tag = false; }
                }
            }
            return full_tag;
        }

        public IList<Treenode> Simplify_Tree(IList<Treenode> TreeList)
        {
            IList<Treenode> Removetns = new List<Treenode>(); //由于遍历过程中不能删除元素，就把要删除的元素先放在这里

            foreach (Treenode ttn in TreeList)
            {
                if (ttn.parent == null && (ttn.leftchild == null | ttn.rightchild == null))
                {
                    if (ttn.leftchild != null)
                    {
                        ttn.leftchild.parent = null;
                        Removetns.Add(ttn);
                    }
                    else
                    {
                        ttn.rightchild.parent = null;
                        Removetns.Add(ttn);
                    }
                }
                else  //如果不是根节点
                {
                    if (ttn.nodetype != 0)  //如果不是元件节点（即叶子节点）
                    {
                        if (ttn.leftchild == null)  //如果左孩子是空的
                        {
                            if (ttn.nodeID == ttn.parent.leftchild.nodeID)  //假如它是父节点的左孩子
                            {
                                ttn.parent.leftchild = ttn.rightchild;
                                ttn.rightchild.parent = ttn.parent;
                                Removetns.Add(ttn);
                            }
                            else  //假如它是父节点的右孩子
                            {
                                ttn.parent.rightchild = ttn.rightchild;
                                ttn.rightchild.parent = ttn.parent;
                                Removetns.Add(ttn);
                            }
                        }
                        if (ttn.rightchild == null)  //如果右孩子是空的
                        {
                            if (ttn.nodeID == ttn.parent.leftchild.nodeID)  //假如它是父节点的左孩子
                            {
                                ttn.parent.leftchild = ttn.leftchild;
                                ttn.leftchild.parent = ttn.parent;
                                Removetns.Add(ttn);
                            }
                            else  //假如它是父节点的右孩子
                            {
                                ttn.parent.rightchild = ttn.leftchild;
                                ttn.leftchild.parent = ttn.parent;
                                Removetns.Add(ttn);
                            }
                        }
                    }
                }
            }
            //把需要删除的元素统一在这里删除
            foreach (Treenode retn in Removetns)
            {
                TreeList.Remove(retn);
            }
            //未所有节点重新分配ID
            for (int ID = 0; ID < TreeList.Count(); ID++)
            {
                TreeList[ID].nodeID = ID;
            }

            if (Is_Full_Tree(TreeList) == false)
            { MessageBox.Show("简化完但依然不是完全二叉树！"); }
            else
            { MessageBox.Show("简化完成！"); }
            return TreeList;
        }

        //使用非递归的（右节点压入栈）先序遍历
        public IList<Treenode> Tree_Traversal(IList<Treenode> TreeList)
        {
            IList<Treenode> IL_Serial = new List<Treenode>();
            IList<Treenode> Visited = new List<Treenode>();
            int Cur_index = 0;
            for (int i = 0; i < 100; i++)
            {
                //如果左节点非空且没有访问过，就去左边
                if (TreeList[Cur_index].leftchild != null && Visited.Contains(TreeList[Cur_index].leftchild) == false)
                {
                    Cur_index = TreeList[Cur_index].leftchild.nodeID;
                    //MessageBox.Show("I go left!");
                }
                else
                {
                    //如果右节点非空且没有访问过，就去右边
                    if (TreeList[Cur_index].rightchild != null && Visited.Contains(TreeList[Cur_index].rightchild) == false)
                    {
                        Cur_index = TreeList[Cur_index].rightchild.nodeID;
                        //MessageBox.Show("I go right!");
                    }
                    else  //如果左右都走不了，就输出，并返回父节点
                    {
                        IL_Serial.Add(TreeList[Cur_index]);
                        Visited.Add(TreeList[Cur_index]);
                        //MessageBox.Show("I go back!");
                        if (TreeList[Cur_index].parent == null)
                        { break; }
                        else
                        { Cur_index = TreeList[Cur_index].parent.nodeID; }
                    }
                }
                if (IL_Serial.Count == TreeList.Count)
                { break; }
            }
            if (IL_Serial.Count != TreeList.Count)
            { MessageBox.Show("遍历少了的节点数："+(TreeList.Count- IL_Serial.Count).ToString()); }
            return IL_Serial;
        }

        public string Get_IL(IList<Treenode> IL_List)
        {
            string ILPro = "*Program\n";
            foreach (Treenode ttn in IL_List)
            {
                //MessageBox.Show(Show_Treenode(ttn));  //调试使用
                ILPro += Get_Block_IL(ttn);
            }
            return ILPro;
        }

        public string Get_Block_IL(Treenode ttn)
        {
            string sent = "";
            switch (ttn.nodetype)
            {
                case 1: sent += "AND\n";break;
                case 2: sent += "OR\n"; break;
                case 0:
                    {
                        if (ttn.nodedata.type == 1)
                        { sent += ("LD "+ ttn.nodedata.Block_Name + "\n"); }
                        if (ttn.nodedata.type == 2)
                        { sent += ("LDN " + ttn.nodedata.Block_Name + "\n"); }
                        if (ttn.nodedata.type == 5)
                        { sent += ("OUT " + ttn.nodedata.Block_Name + "\n"); }
                    }
                    break;
            }
            return sent;
        }

        public void Tree_Visual(IList<Treenode> TreeList)
        {
            foreach (Treenode tn in TreeList)
            {
                string mstr = "";
                string leftstr = "";
                string rightstr = "";
                if (tn.nodetype == 0)
                {
                    mstr += ("[" + tn.nodedata.row.ToString() + "," + tn.nodedata.column.ToString() + "]" + tn.nodeID.ToString());
                    //btn[tn.nodedata.row, tn.nodedata.column].Background = Brushes.Red;
                }
                else
                {
                    if (tn.nodetype == 1)
                    { mstr += ("AND, " + tn.nodeID.ToString()); }
                    else
                    { mstr += ("OR, " + tn.nodeID.ToString()); }
                }
                if (tn.leftchild != null)
                {
                    if (tn.leftchild.nodetype == 0)
                    { leftstr += ("[" + tn.leftchild.nodedata.row.ToString() + "," + tn.leftchild.nodedata.column.ToString() + "]----"); }
                    if (tn.leftchild.nodetype == 1)
                    { leftstr += ("AND, " + tn.leftchild.nodeID.ToString()) + "----"; }
                    if (tn.leftchild.nodetype == 2)
                    { leftstr += ("OR, " + tn.leftchild.nodeID.ToString()) + "----"; }
                }
                if (tn.rightchild != null)
                {
                    if (tn.rightchild.nodetype == 0)
                    { rightstr += ("----[" + tn.rightchild.nodedata.row.ToString() + "," + tn.rightchild.nodedata.column.ToString() + "]"); }
                    if (tn.rightchild.nodetype == 1)
                    { rightstr += ("----" + "AND, " + tn.rightchild.nodeID.ToString()); }
                    if (tn.rightchild.nodetype == 2)
                    { rightstr += ("----" + "OR, " + tn.rightchild.nodeID.ToString()); }
                }
                MessageBox.Show(leftstr + mstr + rightstr);
            }
        }

        public string Showblock(BlockButton bbtn)
        {
            string mstr = ("[" + bbtn.row.ToString() + "," + bbtn.column.ToString() + "]");
            return mstr;
        }

        public string Show_Treenode(Treenode tn)
        {
            string mstr = "";
            if (tn.nodetype == 0)
            {
                mstr += ("[" + tn.nodedata.row.ToString() + "," + tn.nodedata.column.ToString() + "]" + tn.nodeID.ToString());
                //btn[tn.nodedata.row, tn.nodedata.column].Background = Brushes.Red;
            }
            else
            {
                if (tn.nodetype == 1)
                { mstr += ("AND, " + tn.nodeID.ToString()); }
                else
                { mstr += ("OR, " + tn.nodeID.ToString()); }
            }
            return mstr;
        }
        
    }
}
