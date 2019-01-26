using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomBitMapImage
{
    class ThreadMetadata
    {
        public int[,] position;

        public ThreadMetadata(int[,] position)
        {
            this.position = position;
            Console.WriteLine("hello world");
        }
    }
}
