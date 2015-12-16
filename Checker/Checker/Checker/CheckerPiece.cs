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
        private int xPos;
        private int yPos;
        private SolidColorBrush checkerColor;
        bool isKing;
        /* Contstructor */

        public CheckerPiece(int xPos, int yPos, bool colorStatus, bool isKing)
        {
            //Positions
            this.xPos = xPos;
            this.yPos = yPos;
            this.isKing = isKing;
            //Image related data members
            this.colorStatus = colorStatus;


        }

        //Default
        public CheckerPiece()
        {

        }


        /* Properties */

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
           


        public SolidColorBrush CheckerColor
        {
            get
            {
                return checkerColor;
            }
        }
        
    }
}

