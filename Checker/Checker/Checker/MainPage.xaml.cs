using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409


namespace Checker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int prevRow;
        private int prevColumn;

        private int desRow;
        private int desColumnn; 

        private bool colorOfPiece; //TRUE is RED    FALSE is ORANGE

        private bool isFirstClick; 
  
        private CheckerPiece myCheckerPiece;
        SolidColorBrush myPieceColor = new SolidColorBrush();
        private List<CheckerPiece> WallYouCannotMove;
        private List<CheckerPiece> RedPiece;
        private List<CheckerPiece> OrgPiece;


        public MainPage()
        {
            this.InitializeComponent();

            isFirstClick = true; //Select the first click option

            prevRow = 0;
            prevColumn = 0;
            desRow = 0;
            desColumnn = 0;
            WallYouCannotMove = new List<CheckerPiece>();
            RedPiece = new List<CheckerPiece>();
            OrgPiece = new List<CheckerPiece>();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            CreateCheckerPiece();
            createWall();
        }

        private void Clicked(object sender, TappedRoutedEventArgs e)
        {
      
        }


        private void clickBoard(object sender, TappedRoutedEventArgs e)
        {
            bool moveAcceptable = false;
            Canvas currSquareSelected = sender as Canvas;
            Canvas zero = new Canvas();

            //Multidimensional representation of the board 
            Canvas[,] boardSquares = { { Row1Col0, zero, Row1Col2, zero, Row1Col4, zero, Row1Col6, zero},
                                       { zero, Row2Col1, zero, Row2Col3, zero, Row2Col5, zero, Row2Col7 },
                                       { Row3Col0, zero, Row3Col2, zero, Row3Col4, zero, Row3Col6, zero },
                                       { zero, Row4Col1, zero, Row4Col3, zero, Row4Col5, zero, Row4Col7 },
                                       { Row5Col0, zero, Row5Col2, zero, Row5Col4, zero, Row5Col6, zero },
                                       { zero, Row6Col1, zero, Row6Col3, zero, Row6Col5, zero, Row6Col7 },
                                       { Row7Col0, zero, Row7Col2, zero, Row7Col4, zero, Row7Col6, zero },
                                       { zero, Row8Col1, zero, Row8Col3, zero, Row8Col5, zero, Row8Col7 }   }; 
            
            try
            {
                if(isFirstClick)
                {
                    isFirstClick = false; //Select another coordinate to determine movement 
                    //Get the coordinates of the current selected part of the UI
                    prevColumn = (int)currSquareSelected.GetValue(Grid.ColumnProperty); //Prev Column
                    prevRow = (int)currSquareSelected.GetValue(Grid.RowProperty); //Prev Row

                    //Get the colour of the piece
                    myPieceColor = (SolidColorBrush)currSquareSelected.Children.ElementAt(0).GetValue(Ellipse.FillProperty);


                    if(myPieceColor.Color == Colors.Red)
                    {
                        colorOfPiece = true; //COLOR OF PIECE IS RED
                    }
                    else
                    {
                        colorOfPiece = false; //COLOR OF PIECE IS ORANGE
                    }
                }
                else
                {
                    isFirstClick = true; //Allow to select new coordinates to click on for movement
                    //Get the coordinates of the current selected part of the UI
                    desColumnn = (int)currSquareSelected.GetValue(Grid.ColumnProperty); //Current Column
                    desRow = (int)currSquareSelected.GetValue(Grid.RowProperty); //Current Row

                    //Now send the information off to the server! 
                    moveAcceptable = CheckMove(colorOfPiece, prevColumn + 1, prevRow, desColumnn + 1, desRow); //Send this info the game logic controller 
                                        
                    if(moveAcceptable) //Returns true, change checker position on board
                    {
                        //boardSquares[prevRow - 1, prevColumn].Background = new SolidColorBrush(Colors.Purple);

                        boardSquares[prevRow - 1, prevColumn].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 0);

                        boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.FillProperty, myPieceColor);
                        boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 100);

                    }
                }
            }
            catch(Exception exp)
            {


            }    
        }



        /********************************* ENGINE LOGIC *********************************/


        bool CheckMove(bool color, int curX, int curY, int desX, int desY)
        {
            List<CheckerPiece> Piece = new List<CheckerPiece>();
            bool status = false;
            if (color == true)
            {
                Piece = RedPiece;
            }
            else
            {
                Piece = OrgPiece;
            }
            if ((curX == desX) && (curY == desY))
            {
                status = false;
            }
            else
            {
                if (status == false)
                {
                    bool statusRed = moveOrgPiece(curX, curY, desX, desY);
                    if (statusRed == true)
                    {
                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                }
                else
                {

                }
            }

            return status;
        }

        public void createWall()
        {
            for (int i = 1; i <= 8; i++)
            {
                for (int w =1; w <=8; w++)
                {
                    if ((w >= 1) && (i == 1))
                    {
                        WallYouCannotMove.Add(new CheckerPiece(i, w, false));
                    }
                    else if (i == 8)
                    {
                        WallYouCannotMove.Add(new CheckerPiece(i, w, false));
                    }
                    else if (i<=7)
                    {
                        if ((w == 1) || (w == 8))
                        {
                            WallYouCannotMove.Add(new CheckerPiece(i, w, false));
                        }
                    }


                }
            }
        }


        public bool CheckWall(int curX, int curY, int desX, int desY)
        {
            bool status = true;


            return status; 
        }


        public bool moveOrgPiece(int curX, int curY, int desX, int desY)
        {
            bool status = false;
            bool RedPieceThere = false;
            bool OrgPieceThere = false;

            //check the position of the pieces to see if it in the cordinate
            if ((curY - 1 == desY) && ((curX + 1 == desX) || (curX - 1 == desX)))
            {
                //check to see if you piece is there or not
                foreach (CheckerPiece CheckOrgPieces in OrgPiece)
                {
                    if ((desX == CheckOrgPieces.XPos) && (desY == CheckOrgPieces.YPos))
                    {
                        OrgPieceThere = true;
                        break;
                    }
                }
                if (OrgPieceThere == true)
                {
                    status = false;
                }
                else
                {
                    //check to see if there red pices there or not
                    foreach (CheckerPiece CheckRedPieces in RedPiece)
                    {
                        if ((desX == CheckRedPieces.XPos) && (desY == CheckRedPieces.YPos))
                        {
                            RedPieceThere = true;
                            break;
                        }
                    }
                    //if red piece is there that means you cannot move
                    if (RedPieceThere == true)
                    {
                        status = false;
                    }
                    else
                    {
                        //if no pieces are there then move
                        int i = 0;
                        foreach (CheckerPiece CheckOrgPieces in OrgPiece)
                        {
                            if ((curX == CheckOrgPieces.XPos) && (curY == CheckOrgPieces.YPos))
                            {
                                OrgPiece.RemoveAt(i);
                                OrgPiece.Add(new CheckerPiece(desX, desY, false));
                                break;
                            }
                            i++;
                        }
                        //if no pieces are there then move
                        status = true;
                    }
                }

            }
            //check for a kill
            if ((curY-2 == desY) && ((curX+2 == desX) || (curX - 2 == desX)))
            {
                //check oppnent pieces
                foreach (CheckerPiece CheckRedPieces in RedPiece)
                {
                    if ((desX == CheckRedPieces.XPos) && (desY == CheckRedPieces.YPos))
                    {
                        RedPieceThere = true;
                        break;
                    }
                }
                if(RedPieceThere == true)
                {
                    status = false;
                }
                else
                {
                    //check your pieces if it interfear
                    foreach (CheckerPiece CheckOrgPieces in OrgPiece)
                    {
                        if ((desX == CheckOrgPieces.XPos) && (desY == CheckOrgPieces.YPos))
                        {
                            OrgPieceThere = true;
                            break;
                        }
                    }
                    //change status
                    if (OrgPieceThere == true)
                    {
                        status = false;
                    }
                    else
                    {
                        status = true;
                    }
                }

            }


            return status;
        }



        public void CreateCheckerPiece()
        {
            //loop through a grid for the y position
            for (int i = 1; i <= 8; i++)
            {
                //loop through the gird for the x position
                for (int w = 1; w <= 8; w++)
                {
                    //put a checker piece only if i is less then 3 because 3 represent the all the Black Piece
                    if (i <= 3)
                    {
                        if (i % 2 != 0)
                        {
                            if (w % 2 != 0)
                            {
                                RedPiece.Add(new CheckerPiece(w, i, false));
                            }
                        }
                        else
                        {
                            if (w % 2 == 0)
                            {
                                RedPiece.Add(new CheckerPiece(w, i, false));
                            }
                        }

                    }
                    if (i >= 6)
                    {
                        if (i % 2 == 0)
                        {
                            if (w % 2 == 0)
                            {
                                OrgPiece.Add(new CheckerPiece(w, i, false));
                            }
                        }
                        else
                        {
                            if (w % 2 != 0)
                            {
                                OrgPiece.Add(new CheckerPiece(w, i, false));
                            }
                        }
                    }
                }
            }
        }



















    }
}
