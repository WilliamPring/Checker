using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checker
{
    class Checker
    {
        //true reprsent white and false represent black
        bool color;
        int xPos;
        int yPos;
        int CanvasPostY;
        int CanvasPostX;

        public Checker()
        {

        }

        public Checker(int xPos, int yPos, int CanvasPostY, int CanvasPostX, bool color)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.CanvasPostX = CanvasPostX;
            this.CanvasPostY = CanvasPostY;
            this.color = color; 
        }



    }
}
