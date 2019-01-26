using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomBitMapImage
{
    class Tile
    {
        public int x;
        public int y;

        private object setPixelLock = new object();


        public Tile(int x, int y, Color color)
        {
            this.x = x;
            this.y = y;
            lock (setPixelLock)
            {
                World.setPixel(x, y, color);
            }
        }

        public void editTile (Color color)
        {
            lock (setPixelLock)
            {
                World.setPixel(this.x, this.y, color);

            }
        }
    }
}