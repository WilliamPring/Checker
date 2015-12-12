using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checker
{
    class CheckerPiece
    {
        //true reprsent white and false represent black
        private bool color;
        private int xPos;
        private int yPos;
        private int CanvasPostY;
        private int CanvasPostX;
        public CheckerPiece(int xPos, int yPos, int CanvasPostY, int CanvasPostX, bool color)
        {
            this.XPos = xPos;
            this.YPos = yPos;
            this.CanvasPostX = CanvasPostX;
            this.CanvasPostY = CanvasPostY;
            this.color = color;
        }
        public int XPos
        {
            get
            {
                return xPos;
            }

            set
            {
                xPos = value;
            }
        }

        public int YPos
        {
            get
            {
                return yPos;
            }

            set
            {
                yPos = value;
            }
        }

        public bool Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public CheckerPiece()
        {

        }



    }
}
