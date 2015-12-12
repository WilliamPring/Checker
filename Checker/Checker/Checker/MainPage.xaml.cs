using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
            //cannot get the size dynamicly so....

            canvasXPos = (int)myCanvas.ActualWidth;
            canvasYPos = (int)myCanvas.ActualHeight;
            //loop through a grid for the y position
            for (int i = 1; i <= 8; i++)
            {
                //loop through the gird for the x position
                for (int w = 1; w <= 8; w++)
                {
                    //put a checker piece only if i is less then 3 because 3 represent the all the Black Piece
                    if (i <= 3)
                    {
                        if (i % 2 == 0)
                        {
                            if (w % 2 == 0)
                            {
                                BlackPiece.Add(new CheckerPiece(i, w, canvasXPos * i, canvasYPos * w, false));
                            }
                        }
                        else
                        {
                            if (w % 2 != 0)
                            {
                                BlackPiece.Add(new CheckerPiece(i, w, canvasXPos * i, canvasYPos * w, false));
                            }
                        }
                    }
                    if (i >= 6)
                    {
                        if (i % 2 == 0)
                        {
                            if (w % 2 == 0)
                            {
                                WhitePiece.Add(new CheckerPiece(i, w, canvasXPos * i, canvasYPos * w, true));
                            }
                        }
                        else
                        {
                            if (w % 2 != 0)
                            {
                                WhitePiece.Add(new CheckerPiece(i, w, canvasXPos * i, canvasYPos * w, true));
                            }
                        }
                    }
                }
            }
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            CreateCheckerPiece();
        }
    }
}
