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
        private List<Checker> WhitePiece;
        private List<Checker> BlackPiece;
        private int canvasXPos;
        private int canvasYPos;
        private int x;
        private int y; 

        public MainPage()
        {
            this.InitializeComponent();
            canvasXPos = 0;
            canvasYPos = 0;
            x = 8;
            y = 8;
            WhitePiece = new List<Checker>();
            BlackPiece = new List<Checker>();
            CreateCheckerPiece();
        }
        public void CreateCheckerPiece()
        {
            //cannot get the size dynamicly so....
            canvasXPos = 340/8;
            canvasYPos = 640/8;
            //loop through a grid for the y position
            for (int i =1; i<=8; i++)
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
                                BlackPiece.Add(new Checker(i, w, canvasXPos * i, canvasYPos * w, false));
                            }
                        }
                        else
                        {
                            if (w % 2 != 0)
                            {
                                BlackPiece.Add(new Checker(i, w, canvasXPos * i, canvasYPos * w, false));
                            }
                        }
                    }
                    if (i >= 6)
                    {
                        if (i % 2 == 0)
                        {
                            if (w % 2 == 0)
                            {
                                WhitePiece.Add(new Checker(i, w, canvasXPos * i, canvasYPos * w, true));
                            }
                        }
                        else
                        {
                            if (w % 2 != 0)
                            {
                                WhitePiece.Add(new Checker(i, w, canvasXPos * i, canvasYPos * w, true));
                            }
                        }
                    }    
                }
               
            }
            int wi = 0;





        }

    }
}
