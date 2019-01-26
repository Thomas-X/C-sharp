using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomBitMapImage
{
    class TileGroup
    {
        public bool isLand;
        public Color occupantColor;
        public Color tileColor;
        public Person occupant = null;
        public int x;
        public int y;
        public List<Tile> tiles = new List<Tile>();

        private object tileLock = new object();

        // TODO add tile properties like sea/land
        public TileGroup(int x, int y, bool isLand)
        {
           
                this.x = x;
                this.y = y;
                this.tileColor = Color.FromArgb(255, 0, 0, 0);
                if (isLand == true)
                {
                    //green
                    //this.tileColor = Color.FromArgb(255, 124, 252, 0);
                    // white (temp)
                    this.tileColor = Color.FromArgb(255, 255, 255, 255);
                }
                else if (isLand == false)
                {
                    this.tileColor = Color.FromArgb(255, 0, 0, 230);
                }
                this.isLand = isLand;

                for (int i = 0; i < World.pixelSize; i += 1)
                {
                    for (int o = 0; o < World.pixelSize; o += 1)
                    {
                        int newX = x + o;
                        int newY = y + i;

                        Tile tile = this.createTile(newX, newY, this.tileColor);
                        this.tiles.Add(tile);
                    }
                }
        }

        private void modifyTile (int x, int y, Color color)
        {
            lock (tileLock)
            {
                for (int i = 0; i < this.tiles.Count; i++)
                {
                    this.tiles[i].editTile(color);
                }
            }
            
        }

        public Tile createTile(int x, int y, Color color)
        {
            return new Tile(x, y, color);
        }

        public bool checkIfHasOccupant()
        {
            return this.occupant != null;
        }

        public void setOccupant(int x, int y, Color color, Person person)
        {
            
                if (this.isLand == false)
                {
                    Console.WriteLine("someone tried to move on me!! while i am sea!");
                }
                this.occupantColor = color;
                this.occupant = person;
                this.modifyTile(x * World.pixelSize, y * World.pixelSize, this.occupantColor);
            
        }

        public void removeOccupant()
        {
                this.occupant = null;
                this.occupantColor = this.tileColor;
                // non-nullable type, it'll just be overwritten next time its fine
                // this.occupantColor = null;
                this.modifyTile(this.x, this.y, this.tileColor);
        }
    }
}
