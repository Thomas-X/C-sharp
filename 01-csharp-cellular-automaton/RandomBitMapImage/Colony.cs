using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RandomBitMapImage
{
    class Colony
    {
        static int maxColonySize = 8;
        // initial random number generator
        static int maxAge = 40;
        // reproduce limit / initial random number
        static int maxStrength = 100;
        static int maxSpreadAroundColonySize = 4;

        public int id;
        public Color color;
        public string name;
        public int colonySize;
        public List<Person> people = new List<Person>();
        public int[] startPosition;

        public Colony (int id, Color color, string name)
        {
            this.id = id;
            this.color = color;
            this.name = name;
            this.colonySize = World.rand.Next(1, Colony.maxColonySize);
            int x = World.rand.Next(World.tiles.GetLength(0));
            int y = World.rand.Next(World.tiles.GetLength(1));
            this.startPosition = new int[] { x,y };
            for (int o = 0; o < this.colonySize;o++)
            {
                // init random stats
                int age = World.rand.Next(Colony.maxAge);
                int strength = World.rand.Next(Colony.maxStrength);
                bool isSick = false;
                
                // init person
                Person p = new Person(age, strength, isSick, this.color);

                // set person data
                p.setStartPosition(this.startPosition);
                p.setColonyID(this.id);
                p.spawnOnTheWorld(Colony.maxSpreadAroundColonySize);

                // update list
                this.people.Add(p);
            }
        }

        public void addPersonToColony(int x, int y)
        {
                this.colonySize++;
                // init random stats
                int age = World.rand.Next(Colony.maxAge);
                int strength = World.rand.Next(Colony.maxStrength);
                bool isSick = false;

                // init person
                Person p = new Person(age, strength, isSick, this.color);

                // set person data
                p.setStartPosition(this.startPosition);
                p.setColonyID(this.id);
                p.spawnOnTheWorld(Colony.maxSpreadAroundColonySize);
                this.people.Add(p);
        }

        public void removePersonFromColony (int x, int y)
        {
                this.colonySize--;
                Person p = null;
                for (int i = 0; i < this.people.Count; i++)
                {
                    if (this.people[i].currentX == x && this.people[i].currentY == y)
                    {
                        p = this.people[i];
                        break;
                    }
                }
                if (p != null)
                {
                    // remove from our people
                    this.people.Remove(p);
                    // update tile
                    TileGroup tilegroup = World.tiles[p.currentX, p.currentY];
                    tilegroup.removeOccupant();
                }
           
        }

        public void update()
        {
            for (int i = 0; i < this.people.Count; i++)
            {
                this.people[i].update(); 
            }
        }
    }
}
