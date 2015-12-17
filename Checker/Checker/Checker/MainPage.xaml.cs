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
        private bool moveKing = false;

        private bool isFirstClick; 
  
        private CheckerPiece myCheckerPiece;


        private SolidColorBrush myPieceColor = new SolidColorBrush();
        private SolidColorBrush myBorderColor = new SolidColorBrush(); 

        private List<CheckerPiece> WallYouCannotMove;
        private List<CheckerPiece> RedPiece;
        private List<CheckerPiece> OrgPiece;
        Canvas zero = new Canvas();

        //Multidimensional representation of the board 
        Canvas[,] boardSquares;


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
            int relHeightTile = (int)myGrid.ActualHeight / 12;
            int relWidthTile  = (int)myGrid.ActualWidth / 10; 

            boardSquares = new Canvas[,]{
                                       { Row1Col0, zero, Row1Col2, zero, Row1Col4, zero, Row1Col6, zero},    //0
                                       { zero, Row2Col1, zero, Row2Col3, zero, Row2Col5, zero, Row2Col7 },   //1 
                                       { Row3Col0, zero, Row3Col2, zero, Row3Col4, zero, Row3Col6, zero },   //2
                                       { zero, Row4Col1, zero, Row4Col3, zero, Row4Col5, zero, Row4Col7 },   //3 
                                       { Row5Col0, zero, Row5Col2, zero, Row5Col4, zero, Row5Col6, zero },   //4
                                       { zero, Row6Col1, zero, Row6Col3, zero, Row6Col5, zero, Row6Col7 },   //5
                                       { Row7Col0, zero, Row7Col2, zero, Row7Col4, zero, Row7Col6, zero },   //6 
                                       { zero, Row8Col1, zero, Row8Col3, zero, Row8Col5, zero, Row8Col7 } }; //7

            //Make the checker pieces relative to the board

            //foreach (var myCanvas in boardSquares)
            //{
            //    if (myCanvas.ToString() != "zero")
            //    {
            //        myCanvas.Children.ElementAt(0).SetValue(Ellipse.HeightProperty, relHeightTile);
            //        myCanvas.Children.ElementAt(0).SetValue(Ellipse.WidthProperty, relHeightTile);
            //        myCanvas.Children.ElementAt(0).SetValue(Ellipse.MarginProperty, new Thickness(50, 5, 50, 5));
            //        break;
            //    }
            //}


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
            
            try
            {
                if(isFirstClick)
                {
                    isFirstClick = false; //Select another coordinate to determine movement 
                    //Get the coordinates of the current selected part of the UI
                    prevColumn = (int)currSquareSelected.GetValue(Grid.ColumnProperty); //Prev Column
                    prevRow = (int)currSquareSelected.GetValue(Grid.RowProperty); //Prev Row

                    //Piece that was selected
               
                    //myPieceColor = (SolidColorBrush)currSquareSelected.Children.ElementAt(0).GetValue(Ellipse.StrokeProperty);
                    
                    boardSquares[prevRow - 1, prevColumn].Background = new SolidColorBrush(Colors.Yellow);

                    //Get the colour of the piece
                    myPieceColor = (SolidColorBrush)currSquareSelected.Children.ElementAt(0).GetValue(Ellipse.FillProperty);

                    //Check if the brush resembles king
                    myBorderColor = (SolidColorBrush)currSquareSelected.Children.ElementAt(0).GetValue(Ellipse.StrokeProperty);

                    if (myPieceColor.Color == Colors.Red)
                    {
                        colorOfPiece = true; //COLOR OF PIECE IS RED

                        if(myBorderColor.Color == Colors.White)
                        {
                            moveKing = true; 
                        }
                        else
                        {
                            moveKing = false;
                        }

                    }
                    else
                    {
                        colorOfPiece = false; //COLOR OF PIECE IS ORANGE

                        if(myBorderColor.Color == Colors.White)
                        {
                            moveKing = true; 
                        }
                        else
                        {
                            moveKing = false;
                        }
                    }
                }
                else
                {
                    boardSquares[prevRow - 1, prevColumn].Background = new SolidColorBrush(Colors.Black);

                    isFirstClick = true; //Allow to select new coordinates to click on for movement
                    //Get the coordinates of the current selected part of the UI
                    desColumnn = (int)currSquareSelected.GetValue(Grid.ColumnProperty); //Current Column
                    desRow = (int)currSquareSelected.GetValue(Grid.RowProperty); //Current Row

                    //Now send the information off to the server! 
                    moveAcceptable = CheckMove(colorOfPiece, moveKing, prevColumn + 1, prevRow, desColumnn + 1, desRow); //Send this info the game logic controller 
                                        
                    if(moveAcceptable) //Returns true, change checker position on board
                    {
                        if (myBorderColor.Color == Colors.White) //Check if there is a border color of white (meaining king piece)
                        {
                            MoveCheckerPiece(true); //Move the KING checker piece
                        }
                        else
                        {
                            //No stroke present, thus error, so imply that it's a regulat checker piece 

                            MoveCheckerPiece(false); //Move the REGULAR checker piece 

                            if (myPieceColor.Color == Colors.Orange) //Change into dark orange if required
                            {
                                CheckIfKing(false);
                            }
                            else //Change into dark red if required 
                            {
                                CheckIfKing(true);
                            }
                        }
                    }
                }
            }
            catch(Exception exp)
            {
                //Log the error

            }    
        }



        private bool CheckIfKing(bool colorStat)
        {
            bool kingStatus = false;
          
            if(colorStat) //For red piece
            {
                if((desRow == 8) && (desColumnn == 1 || desColumnn == 3 || desColumnn == 5 || desColumnn == 7))
                {
                    myPieceColor = new SolidColorBrush(Colors.Red); 
                    boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.FillProperty, myPieceColor);
                    boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, new SolidColorBrush(Colors.White)); //Border color of yellow
                    //boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeThicknessProperty, 3); //Border color of yellow
                    kingStatus = true; 
                }
            }
            else //For orange piece 
            {
                if ((desRow == 1) && (desColumnn == 0 || desColumnn == 2 || desColumnn == 4 || desColumnn == 6))
                {
                    myPieceColor = new SolidColorBrush(Colors.Orange);
                    boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.FillProperty, myPieceColor);
                    boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, new SolidColorBrush(Colors.White)); //Border color of yellow
                    //boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeThicknessProperty, 3); //Border color of yellow
                    kingStatus = true; 
                }
            }

            return kingStatus; 
        }



        private void MoveCheckerPiece(bool isChangeStrokeColor)
        {

            //Clear square if jump was done move was performed

            if ((prevRow - 2 == desRow && prevColumn + 2 == desColumnn))
            {
                boardSquares[prevRow - 1, prevColumn].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 0);
                boardSquares[prevRow - 2, prevColumn + 1].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 0);

                //boardSquares[prevRow - 2, prevColumn + 1].Background = new SolidColorBrush(Colors.Purple); 


                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.FillProperty, myPieceColor);
                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 100);
                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, myPieceColor); //Stroke Color


                if (isChangeStrokeColor)
                {
                    boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, new SolidColorBrush(Colors.White)); //Border color of yellow
                    //boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeThicknessProperty, 3); //Border color of yellow
                    boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, myBorderColor); //Stroke Color

                }

            }
            else if (prevRow - 2 == desRow && prevColumn - 2 == desColumnn)
            {
                boardSquares[prevRow - 1, prevColumn].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 0);
                boardSquares[prevRow - 2, prevColumn - 1].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 0);


                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.FillProperty, myPieceColor);
                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 100);
                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, myPieceColor); //Stroke Color


                if (isChangeStrokeColor)
                {
                    boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, new SolidColorBrush(Colors.White)); //Border color of yellow
                    //boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeThicknessProperty, 3); //Border color of yellow
                    boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, myBorderColor); //Stroke Color

                }
            }
            else if (prevRow + 2 == desRow && prevColumn - 2 == desColumnn)
            {
                boardSquares[prevRow - 1, prevColumn].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 0);
                boardSquares[prevRow, prevColumn - 1].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 0);
                //boardSquares[prevRow, prevColumn - 1].Background = new SolidColorBrush(Colors.Purple);

                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.FillProperty, myPieceColor);
                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 100);
                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, myPieceColor); //Stroke Color


                if (isChangeStrokeColor)
                {
                    boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, new SolidColorBrush(Colors.White)); //Border color of yellow
                    //boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeThicknessProperty, 3); //Border color of yellow
                    boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, myBorderColor); //Stroke Color

                }
            }
            else if (prevRow + 2 == desRow && prevColumn + 2 == desColumnn)
            {
                boardSquares[prevRow - 1, prevColumn].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 0);
                boardSquares[prevRow, prevColumn + 1].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 0);
                //boardSquares[prevRow, prevColumn + 1].Background = new SolidColorBrush(Colors.Purple); 

                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.FillProperty, myPieceColor);
                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 100);
                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, myPieceColor); //Stroke Color


                if (isChangeStrokeColor)
                {
                    boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, new SolidColorBrush(Colors.White)); //Border color of yellow
                    //boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeThicknessProperty, 3); //Border color of yellow
                    boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, myBorderColor); //Stroke Color

                }
            }
            else
            {
                boardSquares[prevRow - 1, prevColumn].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 0);

                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.FillProperty, myPieceColor);
                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.OpacityProperty, 100);
                boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, myPieceColor); //Stroke Color


                if (isChangeStrokeColor)
                {
                    boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, new SolidColorBrush(Colors.White)); //Border color of yellow
                    boardSquares[desRow - 1, desColumnn].Children.ElementAt(0).SetValue(Ellipse.StrokeProperty, myBorderColor); //Stroke Color

                }
            }
        }

        public bool moveOrgKing(int curX, int curY, int desX, int desY)
        {
            bool RedPieceThere = false;
            bool OrgPieceThere = false;
            bool status = false;
            bool canMove = true;
            if (((curX + 1 == desX) || (curX - 1 == desX)) && (curY != desY))
            {
                int count = 0;
                int myOrgCount = 0;
                foreach (CheckerPiece myOrg in OrgPiece)
                {
                    if ((curX == myOrg.XPos) && (curY == myOrg.YPos))
                    {
                        myOrgCount = count;
                    }
                    if ((desX == myOrg.XPos) && (desY == myOrg.YPos))
                    {
                        canMove = false;
                        break;
                    }
                    count++;
                }
                if (canMove == false)
                {
                    status = false;
                }
                else
                {
                    //check apponent pieces if it there 
                    foreach (CheckerPiece myRed in RedPiece)
                    {
                        if ((myRed.XPos == desX) && (myRed.YPos == desY))
                        {
                            canMove = false;
                            break;
                        }
                    }
                    if (canMove == false)
                    {
                        status = false;
                    }
                    else
                    {
                        OrgPiece.RemoveAt(myOrgCount);
                        OrgPiece.Add(new CheckerPiece(desX, desY, false, true));
                        status = true;
                    }
                }
            }
            if (((curY + 2 == desY) && ((curX + 2 == desX))) || ((curY + 2 == desY) && (curX - 2 == desX)))
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
                if (RedPieceThere == true)
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
                        int pos1X = 0;
                        int pos1Y = 0;
                        int pos2X = 0;
                        int pos2Y = 0;
                        bool statusCondition1 = true;
                        bool statusCondition2 = true;
                        int counterToDelete = 0;
                        int removeAtSet1 = 0;
                        int removeAtSet2 = 0;
                        foreach (CheckerPiece redPiecesDelete in RedPiece)
                        {

                                if (((redPiecesDelete.XPos == curX - 1) && (redPiecesDelete.YPos == curY - 1))
                                || ((redPiecesDelete.XPos == curX - 1) && (redPiecesDelete.YPos == curY + 1)))
                            {
                                pos1X = redPiecesDelete.XPos;
                                pos1Y = redPiecesDelete.YPos;
                                statusCondition1 = false;
                                removeAtSet1 = counterToDelete;
                            }
                            if (((redPiecesDelete.XPos == curX + 1) && (redPiecesDelete.YPos == curY - 1)) ||
                               ((redPiecesDelete.XPos == curX + 1) && (redPiecesDelete.YPos == curY + 1)))
                            {
                                pos2X = redPiecesDelete.XPos;
                                pos2Y = redPiecesDelete.YPos;
                                statusCondition2 = false;
                                removeAtSet2 = counterToDelete;
                            }
                            counterToDelete++;
                        }

                        int deleteOrgList = 0;
                        foreach (CheckerPiece OrgPiecesDelete in OrgPiece)
                        {
                            if ((OrgPiecesDelete.XPos == curX) && (OrgPiecesDelete.YPos == curY))
                            {
                                break;
                            }
                            deleteOrgList++;
                        }




                        if ((statusCondition1 == false) && (statusCondition2 == false))
                        {
                            if (pos1X - 1 == desX)
                            {
                                RedPiece.RemoveAt(removeAtSet1);
                            }
                            else
                            {

                                RedPiece.RemoveAt(removeAtSet2);
                            }
                            OrgPiece.RemoveAt(deleteOrgList);

                            OrgPiece.Add(new CheckerPiece(desX, desY, false, true));

                            status = true;
                        }
                        else if ((statusCondition1 == false) && (statusCondition2 == true))
                        {
                            OrgPiece.RemoveAt(deleteOrgList);

                            OrgPiece.Add(new CheckerPiece(desX, desY, false, true));

                            RedPiece.RemoveAt(removeAtSet1);
                            status = true;

                        }
                        else if ((statusCondition1 == true) && (statusCondition2 == false))
                        {

                            RedPiece.RemoveAt(removeAtSet2);
                            OrgPiece.RemoveAt(deleteOrgList);
                            OrgPiece.Add(new CheckerPiece(desX, desY, false, true));


                            status = true;
                        }
                        else
                        {
                            status = false;
                        }


                        //loop to delete
                    }
                }

            }

            //if ()






            return status;
        }
        public bool moveRedKing(int curX, int curY, int desX, int desY)
        {
            bool RedPieceThere = false;
            bool canMove = true;
            bool OrgPieceThere = false;
            bool status = false;



            if (((curX + 1 == desX) || (curX - 1 == desX)) && (curY != desY))
            {
                int count = 0;
                int myRedCount = 0;
                foreach (CheckerPiece myRed in RedPiece)
                {
                    if ((curX == myRed.XPos) && (curY == myRed.YPos))
                    {
                        myRedCount = count;
                    }
                    if ((desX == myRed.XPos) && (desY == myRed.YPos))
                    {
                        canMove = false;
                        break;
                    }
                    count++;
                }
                if (canMove == false)
                {
                    status = false;
                }
                else
                {
                    //check apponent pieces if it there 
                    foreach (CheckerPiece myOrg in OrgPiece)
                    {
                        if ((myOrg.XPos == desX) && (myOrg.YPos == desY))
                        {
                            canMove = false;
                            break;
                        }
                    }
                    if (canMove == false)
                    {
                        status = false;
                    }
                    else
                    {
                        RedPiece.RemoveAt(myRedCount);
                        RedPiece.Add(new CheckerPiece(desX, desY, true, true));
                        status = true;
                    }
                }
            }

            if (((curX + 2 == desX) || (curX - 2 == desX)) && ((curY + 2 == desY) || (curY - 2 == desY)))
            {
                //check oppnent pieces
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
                    //check your pieces if it interfear
                    foreach (CheckerPiece CheckRedPieces in RedPiece)
                    {
                        if ((desX == CheckRedPieces.XPos) && (desY == CheckRedPieces.YPos))
                        {
                            RedPieceThere = true;
                            break;
                        }

                    }

                    //change status
                    if (RedPieceThere == true)
                    {
                        status = false;
                    }
                    else
                    {
                        int pos1X = 0;
                        int pos1Y = 0;
                        int pos2X = 0;
                        int pos2Y = 0;
                        bool statusCondition1 = true;
                        bool statusCondition2 = true;
                        int counterToDelete = 0;
                        int removeAtSet1 = 0;
                        int removeAtSet2 = 0;
                        foreach (CheckerPiece orgPiecesDelete in OrgPiece)
                        {
                            if (((orgPiecesDelete.XPos == curX + 1) && (orgPiecesDelete.YPos == curY + 1))
                            || ((orgPiecesDelete.XPos == curX - 1) && (orgPiecesDelete.YPos == curY + 1)))
                            {
                                pos1X = orgPiecesDelete.XPos;
                                pos1Y = orgPiecesDelete.YPos;
                                statusCondition1 = false;
                                removeAtSet1 = counterToDelete;
                            }
                            if (((orgPiecesDelete.XPos == curX - 1) && (orgPiecesDelete.YPos == curY - 1)) ||
                               ((orgPiecesDelete.XPos == curX + 1) && (orgPiecesDelete.YPos == curY + 1)))

                            {
                                pos2X = orgPiecesDelete.XPos;
                                pos2Y = orgPiecesDelete.YPos;
                                statusCondition2 = false;
                                removeAtSet2 = counterToDelete;
                            }
                            counterToDelete++;
                        }
                        int deleteOrgList = 0;
                        foreach (CheckerPiece OrgPiecesDelete in RedPiece)
                        {
                            if ((OrgPiecesDelete.XPos == curX) && (OrgPiecesDelete.YPos == curY))
                            {
                                break;
                            }
                            deleteOrgList++;
                        }



                        if ((statusCondition1 == false) && (statusCondition2 == false))
                        {
                            if (pos1X - 1 == desX)
                            {
                                OrgPiece.RemoveAt(removeAtSet1);
                            }
                            else
                            {
                                OrgPiece.RemoveAt(removeAtSet2);
                            }
                            RedPiece.RemoveAt(deleteOrgList);
                            if (desX == 8)
                            {
                                RedPiece.Add(new CheckerPiece(desX, desY, true, true));
                            }
                            else
                            {
                                RedPiece.Add(new CheckerPiece(desX, desY, true, false));
                            }
                            status = true;
                        }
                        else if ((statusCondition1 == false) && (statusCondition2 == true))
                        {
                            OrgPiece.RemoveAt(removeAtSet1);
                            RedPiece.RemoveAt(deleteOrgList);

                            RedPiece.Add(new CheckerPiece(desX, desY, true, true));

                            status = true;

                        }
                        else if ((statusCondition1 == true) && (statusCondition2 == false))
                        {

                            OrgPiece.RemoveAt(removeAtSet2);
                            RedPiece.RemoveAt(deleteOrgList);

                            RedPiece.Add(new CheckerPiece(desX, desY, true, true));

                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }

            }
            return status;
        }





        /********************************* ENGINE LOGIC *********************************/


        bool CheckMove(bool color, bool isKingPiece, int curX, int curY, int desX, int desY)
        {
            //IF isKingPiece is TRUE that means its a king piece

            List<CheckerPiece> Piece = new List<CheckerPiece>();
            bool status = false; //Return to UI to see if move chosen was valid
            if (isKingPiece == true)
            {
                if (color == true)
                {
                    bool StatusRedKing = moveRedKing(curX, curY, desX, desY);
                    if (StatusRedKing == true)
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
                    bool statusOrgKing = true;
                    statusOrgKing = moveOrgKing(curX, curY, desX, desY);
                    if (statusOrgKing == true)
                    {
                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                }
            }
            else
            {
                

                if ((curX == desX) && (curY == desY)) //If user clicked the same square, then obviously the move was incorrect
                {
                    status = false;
                }
                else
                {
                    if (color == true) //Perform logic validation for the RED Piece 
                    {
                        bool statusRed = moveRedPiece(curX, curY, desX, desY);

                        if (statusRed == true)
                        {
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                    else //Perform logic validation for the ORANGE piece
                    {
                        bool statusOrange = moveOrgPiece(curX, curY, desX, desY); //Moves the orange piece, checks if move is valid

                        if (statusOrange == true)
                        {
                            status = true; //Move valid
                        }
                        else
                        {
                            status = false;
                        }
                    }
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
                        WallYouCannotMove.Add(new CheckerPiece(i, w, false, false));
                    }
                    else if (i == 8)
                    {
                        WallYouCannotMove.Add(new CheckerPiece(i, w, false, false));
                    }
                    else if (i<=7)
                    {
                        if ((w == 1) || (w == 8))
                        {
                            WallYouCannotMove.Add(new CheckerPiece(i, w, false, false));
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






        //LOGIC FOR MOVING ORANGE PIECE 


        public bool moveOrgPiece(int curX, int curY, int desX, int desY)
        {
            bool status = false; //Returns if the move was valid or not
            bool RedPieceThere = false;
            bool OrgPieceThere = false;

            //check the position of the pieces to see if its in the cordinate
            if ((curY - 1 == desY) && ((curX + 1 == desX) || (curX - 1 == desX)))
            {
                //check to see if you piece is there or not
                foreach (CheckerPiece CheckOrgPieces in OrgPiece)
                {
                    if ((desX == CheckOrgPieces.XPos) && (desY == CheckOrgPieces.YPos))
                    {
                        OrgPieceThere = true; //Found an orange piece on the space user wants to go to
                        break;
                    }
                }

                if (OrgPieceThere == true)
                {
                    status = false; //Move was invalid because of the same piece being on the spot
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
                        status = false; //Move was invalid because of the another piece being on the spot
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
                                if (desX == 1)
                                {
                                    OrgPiece.Add(new CheckerPiece(desX, desY, false, true));
                                }
                                else
                                {
                                    OrgPiece.Add(new CheckerPiece(desX, desY, false, false));
                                }

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
            if ((curY - 2 == desY) && ((curX + 2 == desX) || (curX - 2 == desX)))
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
                if (RedPieceThere == true)
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
                        int pos1X = 0;
                        int pos1Y = 0;
                        int pos2X = 0;
                        int pos2Y = 0;
                        bool statusCondition1 = true;
                        bool statusCondition2 = true;
                        int counterToDelete = 0;
                        int removeAtSet1 = 0;
                        int removeAtSet2 = 0;
                        foreach (CheckerPiece redPiecesDelete in RedPiece)
                        {
                            if ((redPiecesDelete.XPos == curX - 1) && (redPiecesDelete.YPos == curY - 1))
                            {
                                pos1X = redPiecesDelete.XPos;
                                pos1Y = redPiecesDelete.YPos;
                                statusCondition1 = false;
                                removeAtSet1 = counterToDelete;
                            }
                            if ((redPiecesDelete.XPos == curX + 1) && (redPiecesDelete.YPos == curY - 1))
                            {
                                pos2X = redPiecesDelete.XPos;
                                pos2Y = redPiecesDelete.YPos;
                                statusCondition2 = false;
                                removeAtSet2 = counterToDelete;
                            }
                            counterToDelete++;
                        }

                        int deleteOrgList = 0;
                        foreach (CheckerPiece OrgPiecesDelete in OrgPiece)
                        {
                            if ((OrgPiecesDelete.XPos == curX) && (OrgPiecesDelete.YPos == curY))
                                {
                                break;
                            }
                            deleteOrgList++;
                        }
                        



                        if ((statusCondition1 == false) && (statusCondition2 == false))
                        {
                            if (pos1X-1 == desX)
                            {
                                RedPiece.RemoveAt(removeAtSet1);
                            }
                            else
                            {

                                RedPiece.RemoveAt(removeAtSet2);
                            }
                            OrgPiece.RemoveAt(deleteOrgList);
                            if (desX == 1)
                            {
                                OrgPiece.Add(new CheckerPiece(desX, desY, false, true));
                            }
                            else
                            {
                                OrgPiece.Add(new CheckerPiece(desX, desY, false, false));
                            }
                            status = true;
                        }
                        else if ((statusCondition1 == false) && (statusCondition2 == true))
                        {
                            OrgPiece.RemoveAt(deleteOrgList);
                            if (desX == 1)
                            {
                                OrgPiece.Add(new CheckerPiece(desX, desY, false, true));
                            }
                            else
                            {
                                OrgPiece.Add(new CheckerPiece(desX, desY, false, false));
                            }
                            RedPiece.RemoveAt(removeAtSet1);
                            status = true;

                        }
                        else if ((statusCondition1 == true) && (statusCondition2 == false))
                        {

                                RedPiece.RemoveAt(removeAtSet2);
                                OrgPiece.RemoveAt(deleteOrgList);
                            if (desX == 1)
                            {
                                OrgPiece.Add(new CheckerPiece(desX, desY, false, true));
                            }
                            else
                            {
                                OrgPiece.Add(new CheckerPiece(desX, desY, false, false));
                            }

                            status = true;
                        }
                        else
                        {
                            status = false;
                        }


                        //loop to delete
                    }
                }

            }


            return status;
        }




        public bool moveRedPiece(int curX, int curY, int desX, int desY)
        {
            bool status = false; //Returns if the move was valid or not
            bool RedPieceThere = false;
            bool OrgPieceThere = false;
            if ((curY + 1 == desY) && ((curX + 1 == desX) || (curX - 1 == desX)))
            {
                foreach (CheckerPiece CheckRedPieces in RedPiece)
                {
                    if ((desX == CheckRedPieces.XPos) && (desY == CheckRedPieces.YPos))
                    {
                        RedPieceThere = true; //Found an orange piece on the space user wants to go to
                        break;
                    }
                }
                if (RedPieceThere == true)
                {
                    status = false; //Move was invalid because of the same piece being on the spot
                }
                else
                {
                    foreach (CheckerPiece CheckOrgPieces in OrgPiece)
                    {
                        if ((desX == CheckOrgPieces.XPos) && (desY == CheckOrgPieces.YPos))
                        {
                            OrgPieceThere = true;
                            break;
                        }
                    }
                    //if red piece is there that means you cannot move
                    if (OrgPieceThere == true)
                    {
                        status = false; //Move was invalid because of the another piece being on the spot
                    }
                    else
                    {
                        //if no pieces are there then move
                        int i = 0;
                        foreach (CheckerPiece CheckRedPieces in RedPiece)
                        {
                            if ((curX == CheckRedPieces.XPos) && (curY == CheckRedPieces.YPos))
                            {
                                RedPiece.RemoveAt(i);

                                if (desX == 8)
                                {
                                    RedPiece.Add(new CheckerPiece(desX, desY, true, true));
                                }
                                else
                                {
                                    RedPiece.Add(new CheckerPiece(desX, desY, true, false));
                                }
                                break;
                            }
                            i++;
                        }
                        //if no pieces are there then move
                        status = true;
                    }
                }

            }


            if ((curY + 2 == desY) && ((curX + 2 == desX) || (curX - 2 == desX)))
            {
                //check oppnent pieces
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
                    //check your pieces if it interfear
                    foreach (CheckerPiece CheckRedPieces in RedPiece)
                    {
                        if ((desX == CheckRedPieces.XPos) && (desY == CheckRedPieces.YPos))
                        {
                            RedPieceThere = true;
                            break;
                        }

                    }

                    //change status
                    if (RedPieceThere == true)
                    {
                        status = false;
                    }
                    else
                    {
                        int pos1X = 0;
                        int pos1Y = 0;
                        int pos2X = 0;
                        int pos2Y = 0;
                        bool statusCondition1 = true;
                        bool statusCondition2 = true;
                        int counterToDelete = 0;
                        int removeAtSet1 = 0;
                        int removeAtSet2 = 0;
                        foreach (CheckerPiece orgPiecesDelete in OrgPiece)
                        {
                            if ((orgPiecesDelete.XPos == curX - 1) && (orgPiecesDelete.YPos == curY + 1))
                            {
                                pos1X = orgPiecesDelete.XPos;
                                pos1Y = orgPiecesDelete.YPos;
                                statusCondition1 = false;
                                removeAtSet1 = counterToDelete;
                            }
                            if ((orgPiecesDelete.XPos == curX + 1) && (orgPiecesDelete.YPos == curY + 1))
                            {
                                pos2X = orgPiecesDelete.XPos;
                                pos2Y = orgPiecesDelete.YPos;
                                statusCondition2 = false;
                                removeAtSet2 = counterToDelete;
                            }
                            counterToDelete++;
                        }
                        int deleteOrgList = 0;
                        foreach (CheckerPiece OrgPiecesDelete in RedPiece)
                        {
                            if ((OrgPiecesDelete.XPos == curX) && (OrgPiecesDelete.YPos == curY))
                            {
                                break;
                            }
                            deleteOrgList++;
                        }



                        if ((statusCondition1 == false) && (statusCondition2 == false))
                        {
                            if (pos1X - 1 == desX)
                            {
                                OrgPiece.RemoveAt(removeAtSet1);
                            }
                            else
                            {
                                OrgPiece.RemoveAt(removeAtSet2);
                            }
                            RedPiece.RemoveAt(deleteOrgList);
                            if (desX == 8)
                            {
                                RedPiece.Add(new CheckerPiece(desX, desY, true, true));
                            }
                            else
                            {
                                RedPiece.Add(new CheckerPiece(desX, desY, true, false));
                            }
                            status = true;
                        }
                        else if ((statusCondition1 == false) && (statusCondition2 == true))
                        {
                            OrgPiece.RemoveAt(removeAtSet1);
                            RedPiece.RemoveAt(deleteOrgList);
                            if (desX == 8)
                            {
                                RedPiece.Add(new CheckerPiece(desX, desY, true, true));
                            }
                            else
                            {
                                RedPiece.Add(new CheckerPiece(desX, desY, true, false));
                            }
                            status = true;

                        }
                        else if ((statusCondition1 == true) && (statusCondition2 == false))
                        {

                            OrgPiece.RemoveAt(removeAtSet2);
                            RedPiece.RemoveAt(deleteOrgList);
                            if (desX == 8)
                            {
                                RedPiece.Add(new CheckerPiece(desX, desY, true, true));
                            }
                            else
                            {
                                RedPiece.Add(new CheckerPiece(desX, desY, true, false));
                            }
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }

            }


            return status;
        }



        //CREATING THE CHECKER PIECES ON THE GAME LOGIC SIDE 



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
                                RedPiece.Add(new CheckerPiece(w, i, false, false));
                            }
                        }
                        else
                        {
                            if (w % 2 == 0)
                            {
                                RedPiece.Add(new CheckerPiece(w, i, false, false));
                            }
                        }

                    }
                    if (i >= 6)
                    {
                        if (i % 2 == 0)
                        {
                            if (w % 2 == 0)
                            {
                                OrgPiece.Add(new CheckerPiece(w, i, false, false));
                            }
                        }
                        else
                        {
                            if (w % 2 != 0)
                            {
                                OrgPiece.Add(new CheckerPiece(w, i, false, false));
                            }
                        }
                    }
                }
            }
        }



















    }
}
