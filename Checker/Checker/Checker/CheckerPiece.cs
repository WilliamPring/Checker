//THIS IS A PRACTICE COMMIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Checker
{
    public class CheckerPiece
    {
        //true reprsent white and false represent black
        private bool colorStatus;

        private double height;
        private double width;  

        private int xPos;
        private int yPos;

        private int CanvasPostY;
        private int CanvasPostX;

        private SolidColorBrush checkerColor; 

        /** Contstructor **/

        public CheckerPiece(int xPos, int yPos, int CanvasPostY, int CanvasPostX, bool colorStatus, double height, double width)
        {
            //Positions
            this.xPos = CanvasPostX;
            this.yPos = CanvasPostY;
            this.CanvasPostX = CanvasPostX;
            this.CanvasPostY = CanvasPostY;

            //Image related data members
            this.colorStatus = colorStatus;
            this.height = height;
            this.width  = width;

            //Establish the color of the checker piece
            if(colorStatus == true) //Represents color Red
            {
                checkerColor = new SolidColorBrush(Colors.Red); 
            }
            else //Represents color Green
            {
                checkerColor = new SolidColorBrush(Colors.Green);
            }
        }

        //Default
        public CheckerPiece()
        {

        }


        /** Properties **/

        public int XPos
        {
            get
            {
                return xPos;
            }
        }

        public int YPos
        {
            get
            {
                return yPos;
            }
        }
           
        public double Height
        {
            get
            {
                return height; 
            }
        }

        public double Width
        {
            get
            {
                return width; 
            }
        }

        public SolidColorBrush CheckerColor
        {
            get
            {
                return checkerColor;
            }
        }
        
    }
}
