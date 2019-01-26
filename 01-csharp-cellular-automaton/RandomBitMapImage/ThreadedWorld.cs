using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomBitMapImage
{
    class ThreadedWorld
    {
        public static List<ThreadMetadata> threadMetaData = new List<ThreadMetadata>();
        public static int[,] pixelDimensions; 

        public ThreadedWorld()
        {
            // set multi-threaded repaints
            int[] tilegroupAdjustedLengthAndHeight = new int[2] { World.width / World.pixelSize, World.height / World.pixelSize };
            int threadDimensions = World.width / World.howManyDrawThreads;
            int oneSquareLengthAndHeight = tilegroupAdjustedLengthAndHeight[0] / World.howManyDrawThreads;

            // TODO find a better solution than hardcoding.
            threadMetaData.Add(new ThreadMetadata(
                    new int[0, 0]
               ));
            threadMetaData.Add(new ThreadMetadata(
                   new int[199, 0]
              ));
            threadMetaData.Add(new ThreadMetadata(
                   new int[0, 1]
              ));
            threadMetaData.Add(new ThreadMetadata(
                   new int[199, 1]
              ));
        }

        
        private int determineThread (int x, int y)
        {
            ThreadMetadata thmeta = null;
            for (int i = 0; i < threadMetaData.Count;i++)
            {
                if (threadMetaData[i].position.GetLength(0) - 1 <= x && y >= threadMetaData[i].position.GetLength(1))
                {
                    thmeta = threadMetaData[i];
                    break;
                }
            }
            if (thmeta != null)
            {
                Console.WriteLine(thmeta);
            }
            // temp
            return 1;
        }

        // check which thread to use via deterMineThread
        // determineThread returns the index of the thread that should be used.
        // every thread should be locked
        public void setPixel(int x, int y, Color color)
        {
            this.determineThread(x, y);
        }

        // return stitched bitmap here
        public void getStitchedBitmap ()
        {

        }

    }
}
