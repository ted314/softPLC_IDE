using System;
using System.Collections;
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
        public void AOV_Clicked(object sender, RoutedEventArgs e)
        {
            MaptoAOV();
            //GetsubAOV(this.btn[1, 0]);
            //Test_AOV();
        }

        
        public void MaptoAOV()
        {
            IList<BlockButton> Rungs = new List<BlockButton>();  //标记被激活的母线单元，每个单元代表有一条程序
            IList<BlockButton> Non_Empty = new List<BlockButton>();  //标记非空的方格，最后用于判断遍历是否有遗漏
            IList<BlockButton> Visitedbtn = new List<BlockButton>();  //标记已经访问过的方格
            
            foreach (BlockButton bbtn in this.btn)
            {
                if (bbtn.type == -11)
                {
                    Rungs.Add(bbtn);
                    //往全局的根节点列表中添加新的根节点，为后续的转二叉树做准备
                    Treenode rungnode = new Treenode();  //声明一个新的根节点
                    rungnode.nodedata = bbtn;  //使根节点的data为母线单元
                    this.Rungnodes.Add(rungnode);  //把根节点加入全局的根节点列表

                    //bbtn.Background = Brushes.Blue;
                    //MessageBox.Show("找到一个子程序");  //用于调试
                }
                if (bbtn.type != 0 && bbtn.type != -3 && bbtn.type != -11)
                {
                    Non_Empty.Add(bbtn);
                }
            }

            foreach (BlockButton rungbtn in Rungs)
            {
                Stack<BlockButton> myStack = new Stack<BlockButton>();
                BlockButton Curbtn = rungbtn;

                for (int i = 0; i < 100; i++)  //每个子程序的单元个数不能超过100个，否则将提示修改
                {
                    int Currow = Curbtn.row;
                    int Curcol = Curbtn.column;
                    int[] CurConnected = GetConnect_Array(this.btn[Currow, Curcol]);
                    //遇到一个三面孤立的结点，就结束扫描
                    if (CurConnected.Length == 0
                        | (CurConnected[1] == 0 && Visitedbtn.Contains(this.btn[Currow + 1, Curcol]) == true 
                            && Visitedbtn.Contains(this.btn[Currow - 1, Curcol]) == true))
                    { break; }

                    else
                    {
                        //若上方、左方有连接，且右方、下方非空（十字或倒T连接），就把上下两个模块从下到上建立关系，但不跳转
                        if (CurConnected[0] == 1 && CurConnected[2]==1 && (CurConnected[3] == 1 | CurConnected[1] == 1))
                        {
                            this.btn[Currow, Curcol].rightBlocks.Add(this.btn[Currow - 1, Curcol]);
                            //this.btn[Currow, Curcol].right_num += 1;
                            this.btn[Currow - 1, Curcol].leftBlocks.Add(this.btn[Currow, Curcol]);
                            //this.btn[Currow - 1, Curcol].left_num += 1;
                        }
                        //如果下方有连接块，而且没有被访问过，则需要转入下一行扫描
                        if (CurConnected[1] == 1 
                            && Visitedbtn.Contains(this.btn[Currow+1,Curcol])==false)  
                        {
                            this.btn[Currow, Curcol].rightBlocks.Add(this.btn[Currow + 1, Curcol]);
                            //this.btn[Currow, Curcol].right_num += 1;
                            this.btn[Currow + 1, Curcol].leftBlocks.Add(this.btn[Currow, Curcol]);
                            //this.btn[Currow + 1, Curcol].left_num += 1;
                            myStack.Push(this.btn[Currow, Curcol]);
                            Curbtn = this.btn[Currow + 1, Curcol];
                            //MessageBox.Show("我朝下走了！");
                            continue;
                        }
                        //右边、下边都没有连接，只有上方有，则回到上一个分支点
                        if (CurConnected[3] == 0 && (CurConnected[1] == 0 | Visitedbtn.Contains(this.btn[Currow + 1, Curcol]))
                            && CurConnected[0] == 1) 
                        {
                            this.btn[Currow, Curcol].rightBlocks.Add(this.btn[Currow - 1, Curcol]);
                            //this.btn[Currow, Curcol].right_num += 1;
                            this.btn[Currow - 1, Curcol].leftBlocks.Add(this.btn[Currow, Curcol]);
                            //this.btn[Currow - 1, Curcol].left_num += 1;
                            Visitedbtn.Add(this.btn[Currow, Curcol]);  //把当前单元标记为已访问单元
                            Non_Empty.Remove(this.btn[Currow, Curcol]);  //从非空单元集中把当前单元去掉
                            Curbtn = myStack.Pop();
                            //this.btn[Currow, Curcol].Background = Brushes.Red; //用于调试，遍历过后就将该单元背景设为红色
                            //MessageBox.Show("我这行扫描结束了！");
                            continue;
                        }
                        //如果遇到线圈单元，如果栈是空的，就结束扫描；若栈非空，就回到上一个分支点
                        if (this.btn[Currow, Curcol].type == 5)
                        {
                            Visitedbtn.Add(this.btn[Currow, Curcol]);  //把当前单元标记为已访问单元
                            Non_Empty.Remove(this.btn[Currow, Curcol]);  //从非空单元集中把当前单元去掉
                            //this.btn[Currow, Curcol].Background = Brushes.Red; //用于调试，遍历过后就将该单元背景设为红色
                            //MessageBox.Show("我遇到线圈了！");
                            if (myStack.Count == 0)
                            {
                                //MessageBox.Show("子程序扫描结束了！");
                                break;
                            }
                            else
                            { Curbtn = myStack.Pop(); }
                            continue;
                        }
                        //若以上两种情况都没有发生，就默认向右扫描
                        if (CurConnected[3] != 0)
                        {
                            this.btn[Currow, Curcol].rightBlocks.Add(this.btn[Currow, Curcol + 1]);
                            //this.btn[Currow, Curcol].right_num += 1;
                            this.btn[Currow, Curcol + 1].leftBlocks.Add(this.btn[Currow, Curcol]);
                            //this.btn[Currow, Curcol + 1].left_num += 1;
                            Visitedbtn.Add(this.btn[Currow, Curcol]);  //把当前单元标记为已访问单元
                            Non_Empty.Remove(this.btn[Currow, Curcol]);  //从非空单元集中把当前单元去掉
                            Curbtn = this.btn[Currow, Curcol + 1];
                            //MessageBox.Show("我朝右走了！");
                        }
                    }

                    //this.btn[Currow, Curcol].Background = Brushes.Red; //用于调试，遍历过后就将该单元背景设为红色
                }
                Simplify_AOV(rungbtn);
            }
            //要排序
            foreach (BlockButton bbtn in this.btn)
            {
                if (bbtn.IsAOVPoint == true)
                { bbtn.rightAOVs = Sort_RightAOV(bbtn); }
            }
            
            if (Non_Empty.Count == 0)
            { MessageBox.Show("所有单元已扫描完毕！"); }
            else
            { MessageBox.Show("扫描出错，有非空单元未扫描！"); }
        }

        //经过初始扫描之后，对一个元件单元调用，可忽略线元，直接建立元件与元件之间的AOV网络
        //原理：对单个元件进行深度优先的有向图遍历
        public void Simplify_AOV(BlockButton CurAOV)
        {
            IList<BlockButton> Visitedbtn = new List<BlockButton>();  //标记已经访问过的方格
            Stack<BlockButton> myStack = new Stack<BlockButton>();
            BlockButton Curbtn = CurAOV;
            CurAOV.IsAOVPoint = true;

            for (int i = 0; i < 50; i++)
            {
                int crow = Curbtn.row;
                int ccol = Curbtn.column;
                Visitedbtn.Add(this.btn[crow, ccol]);
                //this.btn[crow,ccol].Background = Brushes.Yellow;  //作为输出，便于调试
                //若当前节点是个非母元件的元件，则添加连接关系，并回溯到上一节点
                if (this.btn[crow, ccol].type > 0 && this.btn[crow,ccol].ID != CurAOV.ID)
                {
                    if (this.btn[crow, ccol].type != 5)
                    {
                        this.btn[crow, ccol].IsAOVPoint = true;  //线圈的AOV标记需要在这里设置
                        Simplify_AOV(this.btn[crow, ccol]);  //如果不是线圈，就对该元件递归调用本函数
                    }
                    else
                    { this.btn[crow, ccol].IsAOVPoint = true; }  //线圈的AOV标记需要在这里设置

                    if (CurAOV.rightAOVs.Contains(this.btn[crow, ccol])==false)  //由于递归调用，需要避免重复添加
                    {
                        CurAOV.rightAOVs.Add(this.btn[crow, ccol]);
                        CurAOV.right_num += 1;
                        this.btn[crow, ccol].leftAOVs.Add(CurAOV);
                        this.btn[crow, ccol].left_num += 1;
                    }
                    Curbtn = myStack.Pop();
                    continue;
                }

                //IList < BlockButton > Unvisited= new List<BlockButton>();
                bool Emptytag = true;
                //只要是线元，右连接集就不可能为空
                foreach (BlockButton rbtn in this.btn[crow, ccol].rightBlocks)
                {
                    if (Visitedbtn.Contains(rbtn) == false)  //如果右集中还有未遍历的节点，就会直接指向那个节点
                    {
                        Emptytag = false;
                        myStack.Push(this.btn[crow, ccol]);
                        Curbtn = rbtn;
                        break;
                    }
                }
                //若栈为空，说明能遍历的都遍历了
                if (myStack.Count == 0)
                {
                    //MessageBox.Show("我没地方可走！");
                    break;
                }
                //若右连接集中所有节点都被访问过了，就回溯到上一节点
                if (Emptytag==true)
                {
                    Curbtn = myStack.Pop();
                }
            }
        }

        
        //需要通过选择排序将右AOV集中的元素排序，以便后续二叉树算法的操作
        public IList<BlockButton> Sort_RightAOV(BlockButton bbtn)
        {
            IList<BlockButton> rAOVs = bbtn.rightAOVs;
            IList<BlockButton> final_AOVs = new List<BlockButton>();
            for (int i = 0; i < 30; i++)
            {
                if (rAOVs.Count == 0)
                { break; }
                int rowmin = rAOVs[0].row;
                int delindex = 0;
                for (int j = 0; j < rAOVs.Count; j++)
                {
                    if (rAOVs[j].row < rowmin)
                    {
                        rowmin = rAOVs[j].row;
                        delindex = j;
                    }
                }
                final_AOVs.Add(rAOVs[delindex]);
                rAOVs.Remove(rAOVs[delindex]);
            }
            return final_AOVs;
        }

        //显示经过简化后的元件AOV网络
        public void Test_AOV()
        {
            for (int r = 0; r < LDrow; r++)
            {
                for (int c = 0; c < LDcol; c++)
                {
                    if (btn[r,c].IsAOVPoint == true)
                    {
                        this.btn[r,c].Background = Brushes.Green;  //作为输出，便于调试
                        //IList<BlockButton> Connectbtns = new List<BlockButton>();
                        string leftstr = "";
                        foreach (BlockButton lbtn in btn[r, c].leftAOVs)
                        {
                            leftstr += ("[" + lbtn.row.ToString() + "," + lbtn.column.ToString() + "], ");
                        }
                        leftstr += "----";
                        string rightstr = "";
                        rightstr += ("[" + btn[r, c].row.ToString() + "," + btn[r, c].column.ToString() + "]----");
                        foreach (BlockButton rbtn in btn[r, c].rightAOVs)
                        {
                            rightstr += ("[" + rbtn.row.ToString() + "," + rbtn.column.ToString() + "], ");
                        }
                        string Destr = "";
                        Destr += ("\n left:"+this.btn[r,c].left_num.ToString()+"  right:"+ this.btn[r, c].right_num.ToString());
                        MessageBox.Show(leftstr + rightstr+Destr);
                    }
                }
            }
        }

        //用于获取单元上下左右的连通性，返回一个数组，元素分别代表[上，下，左，右]
        public int[] GetConnect_Array(BlockButton bbtn)
        {
            int[] Connection = new int[4];
            int r = bbtn.row;
            int c = bbtn.column;
            if (r != 0) //判断上方是否有连接
            {
                if (this.LD_Map[r - 1, c] == -2 | (this.LD_Map[r - 1, c] == -1 && this.LD_Map[r, c] == -2))
                {
                    if (this.LD_Map[r - 1, c] == -2 && this.LD_Map[r, c] == -1) ;
                    else
                    { Connection[0] = 1; }
                }
            }
            if (r < LDrow - 1) //判断下方是否有连接
            {
                if (this.LD_Map[r + 1, c] == -2)
                {
                    Connection[1] = 1;
                }
            }
            if (c != 0)  //判断左方是否有连接
            {
                if (this.LD_Map[r, c - 1] != 0)
                {
                    if (this.LD_Map[r, c] == -2 && (this.LD_Map[r, c - 1] == -3 | this.LD_Map[r, c - 1] == -11)
                            | (this.LD_Map[r, c] == -2 && this.LD_Map[r, c - 1] == -2)) ; //无事发生
                    else
                    { Connection[2] = 1; }
                }
            }
            if (c < LDcol - 1) //判断右方是否有连接
            {
                if (this.LD_Map[r, c + 1] != 0)
                {
                    if (this.LD_Map[r, c] == -2 && this.LD_Map[r, c + 1] == -2) ;  //无事发生
                    else
                    { Connection[3] = 1; }
                }
            }
            return Connection;
        }


        /*
         //废案
         //利用广度优先搜索遍历有向图
        public void Simplify2()
        {
            IList<BlockButton> Rungs = new List<BlockButton>();  //标记被激活的母线单元，每个单元代表有一条程序
            IList<BlockButton> Visitedbtn = new List<BlockButton>();  //标记已经访问过的方格
            foreach (BlockButton bbtn in this.btn)
            {
                if (bbtn.type == -11)
                {
                    Rungs.Add(bbtn);
                    bbtn.Background = Brushes.Green;
                    bbtn.IsAOVPoint = true;
                    MessageBox.Show("找到一个子程序");  //用于调试
                }
            }

            foreach (BlockButton rungbtn in Rungs)
            {
                BlockButton CurBlock = rungbtn;
                BlockButton CurAOV = rungbtn;
                //队列的主要函数：Dequque()和Enqueue(object)
                Queue<BlockButton> myQueue = new Queue<BlockButton>();  //广度优先遍历所需要的队列
                myQueue.Enqueue(rungbtn);
                
                for (int i = 0; i < 100; i++)  //每个子程序的单元个数不能超过100个，否则将提示修改
                {
                    if (myQueue.Count == 0)
                    {
                        break;
                    }  //如果队列为空，说明已经没有需要遍历的节点了，可退出循环

                    int Currow = CurBlock.row;
                    int Curcol = CurBlock.column;
                    foreach (BlockButton rbtn in this.btn[Currow,Curcol].rightBlocks)
                    {
                        if (Visitedbtn.Contains(rbtn) == false)
                        {
                            myQueue.Enqueue(rbtn);  //若这个连接单元没有被访问过，就压入队列末尾
                        }
                    }

                    this.btn[Currow, Curcol].Background = Brushes.Red; //用于调试，遍历过后就将该单元背景设为红色

                    Visitedbtn.Add(this.btn[Currow, Curcol]);  //把当前单元标记为已访问单元
                    CurBlock = myQueue.Dequeue();  //使当前单元指向队列中的头一个单元
                }
            }
        }
         */

        /*
        public void MaptoAOV4()
        {
            IList<BlockButton> Rungs = new List<BlockButton>();  //标记被激活的母线单元，每个单元代表有一条程序
            IList<BlockButton> Non_Empty = new List<BlockButton>();  //标记非空的方格，最后用于判断遍历是否有遗漏
            IList<BlockButton> Visitedbtn = new List<BlockButton>();  //标记已经访问过的方格

            foreach (BlockButton bbtn in this.btn)
            {
                if (bbtn.type == -11)
                {
                    Rungs.Add(bbtn);
                    bbtn.Background = Brushes.Blue;
                    MessageBox.Show("找到一个子程序");  //用于调试
                }
                if (bbtn.type != 0 && bbtn.type != -3 && bbtn.type != -11)
                {
                    Non_Empty.Add(bbtn);
                }
            }

            foreach (BlockButton rungbtn in Rungs)
            {
                Stack<BlockButton> myStack = new Stack<BlockButton>();
                BlockButton Curbtn = rungbtn;
                Stack<BlockButton> AOVStack = new Stack<BlockButton>();
                BlockButton CurAOV = rungbtn;

                for (int i = 0; i < 100; i++)  //每个子程序的单元个数不能超过100个，否则将提示修改
                {
                    int Currow = Curbtn.row;
                    int Curcol = Curbtn.column;
                    int[] CurConnected = GetConnect_Array(this.btn[Currow, Curcol]);
                    //遇到一个三面孤立的结点，就结束扫描
                    if (CurConnected.Length == 0
                        | (CurConnected[1] == 0 && Visitedbtn.Contains(this.btn[Currow + 1, Curcol]) == true
                            && Visitedbtn.Contains(this.btn[Currow - 1, Curcol]) == true))
                    {
                        break;
                    }

                    else
                    {
                        //若上方、左方有连接，且右方、下方非空（十字或倒T连接），就把上下两个模块从下到上建立关系，但不跳转
                        if (CurConnected[0] == 1 && CurConnected[2] == 1 && (CurConnected[3] == 1 | CurConnected[1] == 1))
                        {
                            
                        }
                        //如果下方有连接块，而且没有被访问过，则需要转入下一行扫描
                        if (CurConnected[1] == 1
                            && Visitedbtn.Contains(this.btn[Currow + 1, Curcol]) == false)
                        {
                            
                            myStack.Push(this.btn[Currow, Curcol]);
                            Curbtn = this.btn[Currow + 1, Curcol];
                            MessageBox.Show("我朝下走了！");
                            continue;
                        }
                        //右边、下边都没有连接，只有上方有，则回到上一个分支点
                        if (CurConnected[3] == 0 && CurConnected[1] == 0 && CurConnected[0] == 1)
                        {
                            
                            Visitedbtn.Add(this.btn[Currow, Curcol]);  //把当前单元标记为已访问单元
                            Non_Empty.Remove(this.btn[Currow, Curcol]);  //从非空单元集中把当前单元去掉
                            Curbtn = myStack.Pop();
                            this.btn[Currow, Curcol].Background = Brushes.Red; //用于调试，遍历过后就将该单元背景设为红色
                            MessageBox.Show("我这行扫描结束了！");
                            continue;
                        }
                        //如果遇到线圈单元，如果栈是空的，就结束扫描；若栈非空，就回到上一个分支点
                        if (this.btn[Currow, Curcol].type == 5)
                        {
                            Visitedbtn.Add(this.btn[Currow, Curcol]);  //把当前单元标记为已访问单元
                            Non_Empty.Remove(this.btn[Currow, Curcol]);  //从非空单元集中把当前单元去掉
                            this.btn[Currow, Curcol].Background = Brushes.Red; //用于调试，遍历过后就将该单元背景设为红色
                            MessageBox.Show("我遇到线圈了！");
                            if (myStack.Count == 0)
                            {
                                MessageBox.Show("子程序扫描结束了！");
                                break;
                            }
                            else
                            {
                                Curbtn = myStack.Pop();
                            }
                            continue;
                        }
                        //若以上两种情况都没有发生，就默认向右扫描
                        if (CurConnected[3] != 0)
                        {
                            
                            Visitedbtn.Add(this.btn[Currow, Curcol]);  //把当前单元标记为已访问单元
                            Non_Empty.Remove(this.btn[Currow, Curcol]);  //从非空单元集中把当前单元去掉
                            Curbtn = this.btn[Currow, Curcol + 1];
                            MessageBox.Show("我朝右走了！");
                        }
                    }

                    this.btn[Currow, Curcol].Background = Brushes.Red; //用于调试，遍历过后就将该单元背景设为红色
                }

                if (Non_Empty.Count == 0)
                { MessageBox.Show("所有单元已扫描完毕！"); }
                else
                { MessageBox.Show("扫描出错，有非空单元未扫描！"); }
            }
        }*/





        /**********
        //用广度优先遍历算法把每个单元的左右连接集确定好，梯形图转化为AOV网络
        public void MaptoAOV2()
        {
            IList<BlockButton> Rungs = new List<BlockButton>();  //标记被激活的母线单元，每个单元代表有一条程序
            IList<BlockButton> Non_Empty = new List<BlockButton>();  //标记非空的方格，最后用于判断遍历是否有遗漏
            IList<BlockButton> Visitedbtn = new List<BlockButton>();  //标记已经访问过的方格
            foreach (BlockButton bbtn in this.btn)
            {
                if (bbtn.type == -11)
                {
                    Rungs.Add(bbtn);
                    bbtn.Background = Brushes.Blue;
                    MessageBox.Show("找到一个子程序");  //用于调试
                }
                if (bbtn.type != 0 && bbtn.type != -3 && bbtn.type != -11)
                {
                    Non_Empty.Add(bbtn);
                }
            }

            foreach (BlockButton rungbtn in Rungs)
            {
                //队列的主要函数：Dequque()和Enqueue(object)
                Queue<BlockButton> myQueue = new Queue<BlockButton>();  //广度优先遍历所需要的队列
                BlockButton Curbtn = rungbtn;
                myQueue.Enqueue(rungbtn);

                bool Wrong_Tag = true;
                for (int i = 0; i < 100; i++)  //每个子程序的单元个数不能超过100个，否则将提示修改
                {
                    if (myQueue.Count == 0)
                    {
                        Wrong_Tag = false;
                        break;
                    }  //如果队列为空，说明已经没有需要遍历的节点了，可退出循环

                    int Currow = Curbtn.row;
                    int Curcol = Curbtn.column;
                    IList<BlockButton> CurConnected = GetConnected(this.btn[Currow,Curcol]);
                    if (CurConnected.Count == 0)
                    {
                        //无事发生
                    }
                    else
                    {
                        foreach (BlockButton conbtn in CurConnected)
                        {
                            this.btn[Currow, Curcol].rightBlocks.Add(conbtn);  //先将连接单元加入当前单元的右连接集中
                            conbtn.leftBlocks.Add(this.btn[Currow, Curcol]);
                            if (Visitedbtn.Contains(conbtn) == false)
                            {
                                myQueue.Enqueue(conbtn);  //若这个连接单元没有被访问过，就压入队列末尾
                            }
                        }
                    }

                    this.btn[Currow, Curcol].Background = Brushes.Red; //用于调试，遍历过后就将该单元背景设为红色

                    Visitedbtn.Add(this.btn[Currow, Curcol]);  //把当前单元标记为已访问单元
                    Non_Empty.Remove(this.btn[Currow, Curcol]);  //从非空单元集中把当前单元去掉
                    Curbtn = myQueue.Dequeue();  //使当前单元指向队列中的头一个单元
                }
                if (Wrong_Tag == true)
                { MessageBox.Show("单个程序使用超过100个单元格，应当修改！"); }
            }
            
        }

        public IList<BlockButton> GetConnected(BlockButton bbtn)
        {
            IList<BlockButton> Connected = new List<BlockButton>();
            int r = bbtn.row;
            int c = bbtn.column;
            if (r != 0) //判断上方是否有连接
            {
                if (this.LD_Map[r - 1, c] == -2 | (this.LD_Map[r - 1, c] == -1 && this.LD_Map[r, c] == -2))
                {
                    if (this.LD_Map[r - 1, c] == -2 && this.LD_Map[r, c] == -1) ;
                    else
                    { Connected.Add(this.btn[r - 1, c]); }
                }
            }
            if (r < LDrow - 1) //判断下方是否有连接
            {
                if (this.LD_Map[r + 1, c] == -2)
                {
                    Connected.Add(this.btn[r + 1, c]);
                }
            }
            if (c < LDcol - 1) //判断右方是否有连接
            {
                if (this.LD_Map[r, c + 1] != 0)
                {
                    if (this.LD_Map[r, c] == -2 && this.LD_Map[r, c + 1] == -2) ;  //无事发生
                    else
                    { Connected.Add(this.btn[r, c + 1]); }
                }
            }
            return Connected;
        }
        ********/

    }

}
