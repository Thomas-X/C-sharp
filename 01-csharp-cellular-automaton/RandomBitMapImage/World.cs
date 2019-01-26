using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomBitMapImage
{
    class World
    {
        public static int pixelSize = 8;
        static int chanceToGetLand = 100;
        public static int height = 0;
        public static int width = 0;
        public static int amountOfColonies = 8;
        public static Random rand = new Random();
        public static Bitmap world = null;
        public static int multiplier = 0;
        public static Colony[] colonies;
        public static List<Thread> threads = new List<Thread>();

        public static ThreadedWorld threadedWorld;

        // in how many squares the bitmap should be split up in 
        // x=y so if you want 4 threads, use 2. 
        public static int howManyDrawThreads = 2;


        private static object worldSetPixelLock = new object();

        // since there is a setting for pixelsize meaning the tiles are not 1:1 to pixels, depending on the 
        // pixelSize this tiles ratio is 1:pixelSize. 
        // so when a bitmap is 400x400 and the pixelSize = 8 the tiles tilegroup would be a size of 50,50 (400/8)

        public static TileGroup[,] tiles;

        public World (int height, int width)
        {
            World.height = height;
            World.width = width;
            World.multiplier = World.pixelSize;

            World.threadedWorld = new ThreadedWorld();

        }

        public static void setPixel(int x, int y, Color color)
        {

            lock(World.worldSetPixelLock)
            {
                World.threadedWorld.setPixel(x, y, color);
                World.world.SetPixel(x, y, color);
            }
        }

        public string getColonyStats()
        {
            string str = "";
            for (int o = 0; o < colonies.Length; o++)
            {
                Colony colony = colonies[o];
                

                str += colony.name;
                str += Environment.NewLine;
                int sum = 0;
                for (int h = 0; h < colony.people.Count; h++)
                {
                    sum += colony.people[h].strength;
                }
                str += "Total strength of colony: " + sum.ToString();
                str += Environment.NewLine;
                str += "Size: " + colony.colonySize.ToString();
                str += Environment.NewLine;
                str += Environment.NewLine;
            }
            return str;
        }
        
        public Bitmap regenerateWorld()
        {
            World.tiles = new TileGroup[World.height / pixelSize, World.width / pixelSize];
            World.world = new Bitmap(World.height, World.width);
            this.createTileGroups();
            World.colonies = this.createColonies();
            return World.world;
        }
        

        public void onTick()
        {
            
            
           
        }

        private Colony[] createColonies()
        {
            Colony[] colonies = new Colony[World.amountOfColonies];
            for (int o = 0; o < World.amountOfColonies;o++)
            {
                Colony colony = new Colony(
                        o,
                        Color.FromArgb(World.rand.Next(256), World.rand.Next(256), World.rand.Next(256)),
                        "Colony " + o.ToString()
                    );
                colonies.SetValue(colony, o);
            }
            return colonies;
        }

        private void createTileGroups()
        {
            for (int y = 0; y < World.height; y += World.multiplier)
            {
                for (int x = 0; x < World.width; x += World.multiplier)
                {
                    bool isLand = rand.Next(100) <= World.chanceToGetLand;
                    int tileCoordX = x / pixelSize;
                    int tileCoordY = y / pixelSize;
                    World.tiles[tileCoordX, tileCoordY] = this.createTileGroup(x, y, isLand);
                }
            }
        }

        private TileGroup createTileGroup(int x, int y, bool isLand)
        {
            return new TileGroup(x, y, isLand);
        }
    }
}
