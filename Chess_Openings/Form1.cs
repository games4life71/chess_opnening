using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace chess_openings
{
    public partial class Form1 : Form
    {



        public Form1()
        {
            InitializeComponent();
        }
        //dictionar pentru a tine evidenta pieselor 



        private static Dictionary<char, int> NumToLett = new Dictionary<char, int>();


        private void Piece_Click(object sender, EventArgs e)
        {

            if (sender is PictureBox)
            {
                PictureBox picBox = (PictureBox)sender;

                ValueTuple<int, int> picBox_coord;


                picBox_coord.Item1 = boardTableLayout.GetPositionFromControl(picBox).Row;
                picBox_coord.Item2 = boardTableLayout.GetPositionFromControl(picBox).Column;


                //second click is triggered 
              
                    
              
            }
            else return;

        }

      private static void MoveParser(string move )
        {


            string allLower = move.ToLower();
            char Piece;
            if (allLower.Length == 2) Piece = 'p';
             else Piece = allLower[0];
            int pos_x = NumToLett[allLower[1]]; //pozitia pe axa  x 
            int pos_y = allLower[2] - '0';

             
        }
        /// <summary>
        /// A method that checks if the path is clear or not 
        /// </summary>
        /// <param name="start">Start coords </param>
        /// <param name="end">End coords </param>
        /// <param name="dir">Direction that the search will go 1 = horizontal 2 = vertical 3 = diagonal</param>
        /// <returns></returns>
        private static bool isPathFree(ValueTuple<int ,int> start , ValueTuple<int,int> end  , int dir ,TableLayoutPanel board )
        {
            switch (dir)
            {

                case 1:
                    //the direction is horizontal 
                    for(int i = start.Item1+1; i<end.Item1;i++)
                    {
                        for(int j =start.Item1+1; j<end.Item2;j++)
                        {
                            if (board.GetControlFromPosition(j, i).Tag.ToString() != null) return false; 
                        }
                    }
                    break; 

                case 2:
                    //the direction is vertical 
                    for(int j = start.Item2; j<end.Item2;j++)
                        for(int i = start.Item1;i<end.Item1; i++)
                        {
                            if (board.GetControlFromPosition(j, i).Tag.ToString() != null) return false;

                        }
                    break;
                
                case 3:
                    //the direction is diagonal 
                      
                     if(start.Item1<end.Item1)
                    {

                        //up right  
                        if(start.Item2  < end.Item2)
                        {
                            for(int  i = start.Item1 , j =start.Item2; i<end.Item1;i++,j++)
                            {
                                if (board.GetControlFromPosition(j,i).Tag != null) return false;

                            }
                        }
                        else
                        {
                            //up left
                           for(int i = start.Item1  , j=start.Item2;i<end.Item1;i++ , j--)
                            {

                                if (board.GetControlFromPosition(j, i).Tag.ToString() != null) return false;
                            }

                        }

                     } 
                    else
                    {
                        if(start.Item2 > end.Item2 )
                        {
                            //down right 
                            for (int i = start.Item1, j = start.Item2; i > end.Item1; i--, j++)
                            {
                                if (board.GetControlFromPosition(j, i).Tag.ToString() != null) return false;
                            }
                        }

                        else
                        {
                            for (int i = start.Item1, j = start.Item2; i > end.Item1; i--, j--)
                            {
                                if (board.GetControlFromPosition(j, i).Tag.ToString() != null) return false;
                            }

                        }

                    }                     


                     
                    break;

                default: break; 
            }

            return true; 
        }
          
            //every time a piece moves into a  position we change the tag to that piece name i.e.  B or K
            /// <summary>
            /// Function that tells whether a piece can be moved at the given position 
            /// </summary>
            /// <param name="dest">tuple that registeres final positons</param>
            /// <param name="board">the virtual boaard </param>
            /// <param name="isWhite">type of piece </param>
            /// <param name="pos_x"> initial x position </param>
            /// <param name="pos_y">initial y position </param>
            /// <param name="typeOfMove"> 1- horizontal , 2- vertical  , 3-diagonal  , 4-L shape </param>
            /// <returns></returns>
         private static bool CanMove(ValueTuple<int , int> dest , TableLayoutPanel board , bool isWhite , int pos_x , int pos_y , int typeOfMove )
        {
           

            TableLayoutControlCollection  controls = board.Controls;

            foreach (Control c in controls)
            {

                if (c.Tag.ToString().Contains('e'.ToString()))
                { //a pawn can every time move 
                    return true;
                }

                else
                {
                   TableLayoutPanelCellPosition piece_curr_pos = board.GetPositionFromControl(c);//position of the piece 
                    string Piece = c.Tag.ToString();
                    if (isWhite)
                    { //piece its white 
                        switch (Piece[1])
                        {

                            case 'n':
                                {
                                        //knight 
                                    //check if the knight can move to that position , if not return false 
                                    //not need to check if the path is clear 
                                    int[] y_val = { 2,2,-2,-2 ,1,1,-1,-1 };
                                    int[] x_val = { 1,-1,1,-1 , 2,-2,2,-2 };

                                for(int i = 0; i<8; i++)
                                    {

                                        try
                                        {

                                            if(piece_curr_pos.Column+x_val[i] == dest.Item2 && piece_curr_pos.Row+y_val[i] == dest.Item1)
                                                return true; 
                                        }

                                        catch(IndexOutOfRangeException e )
                                        {
                                            Console.WriteLine(e.StackTrace);
                                        }

                                    }



                                    return false;

                                }
                            case 'b':
                                {
                                    //bishop
                                    //main diag 
                                     for ( int i = 0; i<8;i++)
                                    {

                                            if (piece_curr_pos.Column + i == dest.Item1 && piece_curr_pos.Row + i == dest.Item2 || piece_curr_pos.Column - i == dest.Item1 && piece_curr_pos.Row - i == dest.Item2)

                                                return true; 
                                       }

                                     //secondary diag 
                                     for (int  i =  0; i < 8; i++ )
                                    {
                                        if (piece_curr_pos.Row + i == dest.Item1 && piece_curr_pos.Column - i == dest.Item2) return true;
                                        else if (piece_curr_pos.Row - i == dest.Item1 && piece_curr_pos.Column + i == dest.Item1) return true;   
                                        //and the path is empty !!!!! 
                                    }


                                    return false;                                   }
                            case 'r':
                                {
                                    //rook 

                                    bool pathFree = isPathFree((pos_x, pos_y), dest, typeOfMove, board);
                                    if (!pathFree) return false;
                                    
                                    else
                                    {

                                        if (board.GetControlFromPosition(dest.Item2, dest.Item1).Tag != null) return false;

                                        else return true;

                                    }
 
                                }
                            case 'q':
                                {
                                    //queen
                                    bool PathFree = isPathFree((pos_x, pos_y), dest, typeOfMove, board);

                                    if (!PathFree) return false;
                                    else
                                    {
                                        if (board.GetControlFromPosition(dest.Item2, dest.Item1).Tag != null) return false;
                                        else return true;

                                    }

                                }

                        }

                    }
                    else
                    {
                        //it's black 



                    }

                }

            }




            return false;
        }

        /// <summary>
        /// A function that  draws all the pieces into their initial place 
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="isWhite"></param>
        public static void DrawTable(TableLayoutPanel board)
        {
            //set the pawns 
            for (int i = 0; i < 8; i++)
            {
                board.GetControlFromPosition(i, 1).BackgroundImage = Properties.Resources.Chess_pdt60;
                board.GetControlFromPosition(i, 1).Tag = "bP";
                board.GetControlFromPosition(i, 6).BackgroundImage = Properties.Resources.white_pawn;
                board.GetControlFromPosition(i, 6).Tag = "wP";
            }
            //black pieces 
            board.GetControlFromPosition(0, 0).BackgroundImage = Properties.Resources.Chess_rdt60;
            board.GetControlFromPosition(0,0).Tag = "bR";
            board.GetControlFromPosition(7, 0).BackgroundImage = Properties.Resources.Chess_rdt60;
            board.GetControlFromPosition(7, 0).Tag = "bR";
            board.GetControlFromPosition(1, 0).BackgroundImage = Properties.Resources.Chess_ndt60;
            board.GetControlFromPosition(1, 0).Tag = "bN";
            board.GetControlFromPosition(6, 0).BackgroundImage = Properties.Resources.Chess_ndt60;
            board.GetControlFromPosition(1, 0).Tag = "bN";
            board.GetControlFromPosition(2, 0).BackgroundImage = Properties.Resources.Chess_bdt60;
            board.GetControlFromPosition(2, 0).Tag = "bB";
            board.GetControlFromPosition(5, 0).BackgroundImage = Properties.Resources.Chess_bdt60;
            board.GetControlFromPosition(5, 0).Tag = "bB";
            board.GetControlFromPosition(3, 0).BackgroundImage = Properties.Resources.Chess_qdt60;
            board.GetControlFromPosition(3, 0).Tag = "bQ";
            board.GetControlFromPosition(4, 0).BackgroundImage = Properties.Resources.Chess_kdt60;
            board.GetControlFromPosition(4, 0).Tag = "bK";
            //white pieces 
            board.GetControlFromPosition(0, 7).BackgroundImage = Properties.Resources.whtite_rook;
            board.GetControlFromPosition(0, 7).Tag = "wR";
            board.GetControlFromPosition(7, 7).BackgroundImage = Properties.Resources.whtite_rook;
            board.GetControlFromPosition(7, 7).Tag = "wR";
            board.GetControlFromPosition(1, 7).BackgroundImage = Properties.Resources.white_knight;
            board.GetControlFromPosition(1, 7).Tag = "wN";
            board.GetControlFromPosition(6, 7).BackgroundImage = Properties.Resources.white_knight;
            board.GetControlFromPosition(6, 7).Tag = "wN";
            board.GetControlFromPosition(2, 7).BackgroundImage = Properties.Resources.white_bishop;
            board.GetControlFromPosition(2, 7).Tag = "wB";
            board.GetControlFromPosition(5, 7).BackgroundImage = Properties.Resources.white_bishop;
            board.GetControlFromPosition(5, 7).Tag = "wB";
            board.GetControlFromPosition(4, 7).BackgroundImage = Properties.Resources.white_king;
            board.GetControlFromPosition(4, 7).Tag = "wK";
            board.GetControlFromPosition(3, 7).BackgroundImage = Properties.Resources.white_queen;
            board.GetControlFromPosition(3, 7).Tag = "wQ";


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //populate the num to lett dictionary 
            NumToLett.Add('A', 1);
            NumToLett.Add('B', 2);
            NumToLett.Add('C', 3);
            NumToLett.Add('D', 4);
            NumToLett.Add('E', 5);
            NumToLett.Add('F', 6);
            NumToLett.Add('G', 7);
            NumToLett.Add('H', 8);
            //creting chess board visually by adding picture boxes    
            for (int i = 0; i < boardTableLayout.RowCount; i++)
                for (int j = 0; j < boardTableLayout.ColumnCount; j++)
                {
                    PictureBox picBox = new PictureBox();
                    picBox.BackgroundImageLayout = ImageLayout.Stretch;
                    picBox.Dock = DockStyle.Fill;
                   
                    picBox.Margin = new Padding(0);
                    picBox.Click += Piece_Click;
                    picBox.BorderStyle = BorderStyle.FixedSingle;
                    boardTableLayout.Controls.Add(picBox);

                    if ((j + i) % 2 == 0) picBox.BackColor = Color.White; else picBox.BackColor = Color.DarkGray;
                }
             //draws all the pieces into their initial place 
            DrawTable(boardTableLayout);



        }

       
        private void boardTableLayout_Paint(object sender, PaintEventArgs e)
        {

        }

        private void boardTableLayout_Click(object sender, EventArgs e)
        {

        }
    }
}