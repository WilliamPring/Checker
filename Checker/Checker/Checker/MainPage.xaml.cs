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
        private List<CheckerPiece> WhitePiece;
        private List<CheckerPiece> BlackPiece;
        private int canvasXPos;
        private int canvasYPos;

        private int totRows;
        private int totColumns;

        private CheckerPiece myCheckerPiece; 

        public MainPage()
        {
            this.InitializeComponent();
            canvasXPos = 0;
            canvasYPos = 0;
            WhitePiece = new List<CheckerPiece>();
            BlackPiece = new List<CheckerPiece>();




        }         




        public void CheckMove(List<CheckerPiece> MovePiece, CheckerPiece CurrentPost, CheckerPiece DesirePost)
        {
            bool status;
            //check to see if it on the bondary
            if (CurrentPost.YPos - 1 == 0)
            {
                if (CurrentPost.YPos +1 == DesirePost.YPos)
                {
                    //do stuff in here because logic is done
                }
            }
            else
            {
                //check the postion the two position a piece can move
                if ((CurrentPost.XPos + 1 == DesirePost.XPos) && ((CurrentPost.YPos+2 == DesirePost.YPos) || (CurrentPost.YPos -1 == DesirePost.YPos)))
                {
                    //then check to see if pieces are there or not so interate thought the list if there a piece that can be killed then
                }
            }
        }

        public void KillPiece()
        {
            //
        }


        public void CreateCheckerPiece()
        {        
            //loop through a grid for the y position
            for (int i = 1; i <= 8; i++)
            {
                //loop through the gird for the x position
                for (int w = 0; w < 8; w++)
                {
                    //put a checker piece only if i is less then 3 because 3 represent the all the Black Piece
                    if (i <= 3)
                    {
                        if (i % 2 == 0)
                        {
                            if (w % 2 != 0)
                            {
                                myCheckerPiece = new CheckerPiece(i, w, canvasXPos * i, canvasYPos * w, false, canvasXPos, canvasXPos);
                                BlackPiece.Add(myCheckerPiece);
                                DrawCheckerPiece(myCheckerPiece, i, w);
                            }
                        }
                        else
                        {
                            if (w % 2 == 0)
                            {
                                myCheckerPiece = new CheckerPiece(i, w, canvasXPos * i, canvasYPos * w, false, canvasXPos, canvasXPos);
                                BlackPiece.Add(myCheckerPiece);
                                DrawCheckerPiece(myCheckerPiece, i, w);
                            }
                        }
                    }
                    if (i >= 6)
                    {
                        if (i % 2 == 0)
                        {
                            if (w % 2 != 0)
                            {
                                myCheckerPiece = new CheckerPiece(i, w, canvasXPos * i, canvasYPos * w, true, canvasXPos, canvasXPos);
                                WhitePiece.Add(myCheckerPiece);
                                DrawCheckerPiece(myCheckerPiece, i, w);
                            }
                        }
                        else
                        {
                            if (w % 2 == 0)
                            {
                                myCheckerPiece = new CheckerPiece(i, w, canvasXPos * i, canvasYPos * w, true, canvasXPos, canvasXPos);
                                WhitePiece.Add(myCheckerPiece);
                                DrawCheckerPiece(myCheckerPiece, i, w);
                            }
                        }
                    }
                }
            } 
        }


        private void DrawCheckerPiece(CheckerPiece myPiece, int rowNum, int colNum)
        {
            //Add canvas to the grid
            Canvas myCanvas = new Canvas();
            myGrid.Children.Add(myCanvas);

            Ellipse newCheckerImage = new Ellipse();

            newCheckerImage.Height = myPiece.Height;
            newCheckerImage.Width = myPiece.Width;

            newCheckerImage.Margin = new Thickness(0, 5, 0, 0); //Always the same distance apart
            myCanvas.SetValue(Grid.ColumnProperty, colNum);
            myCanvas.SetValue(Grid.RowProperty, rowNum);

            newCheckerImage.Fill = myPiece.CheckerColor;
            myCanvas.Children.Add(newCheckerImage); //Finally draw the ellipse onto the canvas
        }


        private void CreateBoard()
        {
            Rectangle squareOnBoard;
            int row = 1;
            int column = 0;

            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (i % 2 == 0) //Black square
                    {
                        if (j % 2 == 0)
                        {
                            squareOnBoard = new Rectangle();
                            squareOnBoard.Fill = new SolidColorBrush(Colors.Black);
                            squareOnBoard.SetValue(Grid.ColumnProperty, column);
                            squareOnBoard.SetValue(Grid.RowProperty, row);
                            myGrid.Children.Add(squareOnBoard);
                        }


                    }
                    else //White Square
                    {
                        if (j % 2 != 0)
                        {
                            squareOnBoard = new Rectangle();
                            squareOnBoard.Fill = new SolidColorBrush(Colors.Black);
                            squareOnBoard.SetValue(Grid.ColumnProperty, column);
                            squareOnBoard.SetValue(Grid.RowProperty, row);
                            myGrid.Children.Add(squareOnBoard);
                        }
                    }
                    column++;
                }
                row++;
                column = 0;
            }
        }     


        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {       
            ////Break up the board into pieces
            //totColumns = (int)myGrid.ActualWidth;
            //totRows = (int)myGrid.ActualHeight;

            //canvasXPos = totColumns / 8;
            //canvasYPos = totRows / 8;

            //CreateBoard(); //Create the board
            //CreateCheckerPiece(); //Assign the pieces to the board 
        }

        private void Button_Clicked(object sender, RoutedEventArgs e)
        {
            myGrid.Background = new SolidColorBrush(Colors.Purple);




        }

        private void Clicked(object sender, TappedRoutedEventArgs e)
        {
            Rectangle squareOnBoard = sender as Rectangle;

            int row = (int)squareOnBoard.GetValue(Grid.RowProperty);
            int column = (int)squareOnBoard.GetValue(Grid.ColumnProperty);
            bool blackSquareStatus = false;

            if(row % 2 == 0) //Even Number
            {
                if(column % 2 != 0) //Ding ding Black Square
                {
                    blackSquareStatus = true; //Highlight the square 
                }
            }
            else //Odd Number 
            {
                if(column % 2 == 0) //Ding ding Black Square
                {
                    blackSquareStatus = true; //Highlight the square
                }
            }


            if(blackSquareStatus)
            {
                SolidColorBrush compareBrush = new SolidColorBrush(Colors.Yellow);
                SolidColorBrush squareBrush = (SolidColorBrush)squareOnBoard.Fill;

                if (squareBrush.Color == compareBrush.Color)
                {
                    squareOnBoard.Fill = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    squareOnBoard.Fill = new SolidColorBrush(Colors.Yellow);                    
                }
            }
        }

        //private void Play_Game_Clicked(object sender, RoutedEventArgs e)
        //{
        //    totColumns = (int)myGrid.ActualWidth;
        //    totRows = (int)myGrid.ActualHeight;

        //    canvasXPos = totColumns / 8;
        //    canvasYPos = totRows / 8;

        //    playButton.IsEnabled = false;
        //    playButton.Opacity = 0;

        //    CreateCheckerPiece();

        //}

        private void YOO(object sender, TappedRoutedEventArgs e)
        {
            Canvas squareOnBoard = sender as Canvas;

            //int row = (int)squareOnBoard.GetValue(Grid.RowProperty);
            int column = (int)squareOnBoard.GetValue(Grid.ColumnProperty);
            int row = (int)squareOnBoard.GetValue(Grid.RowProperty);
            int i = 0;

            Ellipse myRobot = new Ellipse();

            myRobot.Height = 50;
            myRobot.Width = 50;

            myRobot.Fill = new SolidColorBrush(Colors.Green);

            squareOnBoard.SetValue(Grid.ColumnProperty, 4);

            squareOnBoard.Children.Add(myRobot);

        }
    }
}
