using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomBitMapImage
{
    class Person
    {
        static int spawnRadiusAroundColony = 3;
        static int powerDivider = 5;
        static int damageRecieveModifier = 3;

        int colonyID;
        int[] startPosition;
        public int currentX;
        public int currentY;
        int oldX;
        int oldY;
        public int age;
        public int strength;
        int moveCount = 0;
        public bool isSick;
        Color color;

        private object moveLock = new object();

        public Person(int age, int strength, bool isSick, Color color)
        {
            this.color = color;
            this.age = age;
            this.strength = strength;
            this.isSick = isSick;
        }

        public void spawnOnTheWorld(int spawnRadius)
        {
            this.currentX = this.startPosition[0] + this.randomMove(spawnRadius);
            this.currentY = this.startPosition[1] + this.randomMove(spawnRadius);
            this.oldX = this.startPosition[0];
            this.oldY = this.startPosition[1];
            int[] coords = this.tryToMove(0, Person.spawnRadiusAroundColony);
            // really an edge case that results in the spawn failing because there's no room
            if (coords[0] == -1337 || coords[1] == -1337)
            {
                //Console.WriteLine("couldn't spawn person!!");
                return;
            }
            this.moveCount++;
            this.move(coords[0], coords[1]);
            
        }
        private int randomMove(int limit)
        {
            // limit = 2
            return World.rand.Next(-limit, limit + 1);
        }

        public int[] tryToMove(int attempts, int limit)
        {
            int xMove = this.currentX + this.randomMove(limit);
            int yMove = this.currentY + this.randomMove(limit);

            // to avoid going out of the index range of the array
            int[] coords = this.limitMovement(xMove, yMove);
            int x = coords[0];
            int y = coords[1];
            TileGroup tilegroup = World.tiles[x, y];
            // to avoid stack overflow error :)
            // since this is ran for each 'pixel'
            if (attempts > 10)
            {
                return new int[2] { -1337,-1337 };
            }
            // fighting
            // currently cells dont fight someone with a strength higher/equals than 30 than their own strength
            // and ofcourse don't fight our own brethren
            if (tilegroup.checkIfHasOccupant() == true && tilegroup.occupant.strength + 30 <= this.strength + 30 && tilegroup.occupant.colonyID != this.colonyID)
            {
                FightCondition fightcondition = this.fight(tilegroup.occupant.currentX, tilegroup.occupant.currentY);
                switch (fightcondition.condition)
                {
                    case "nobattle":
                        break;

                    case "win":
                        // because if we have won we can move over and take his spot
                        //return coords;
                        break;

                    case "lose":
                        return new int[2] { -1337, -1337 };

                    case "stalemate":
                        break;

                }
            }
            if (tilegroup.checkIfHasOccupant() == true)
            {
                attempts++;
                return this.tryToMove(attempts, limit);
            } else
            {
                return coords;
            }
        }

        public FightCondition fight(int enemyX, int enemyY)
        {
                Person enemy = World.tiles[enemyX, enemyY].occupant;
                // enemy shouldn't be null technically but still check for it.
                if (enemy == null)
                {
                    return new FightCondition("nobattle");
                }
                int ownPower = this.strength / Person.powerDivider;
                int enemyPower = enemy.strength / Person.powerDivider;

                // if we lose
                if (ownPower < enemyPower)
                {
                    this.strength -= enemyPower * Person.damageRecieveModifier;
                }
                // if we win
                if (ownPower > enemyPower)
                {
                    enemy.strength -= ownPower * Person.damageRecieveModifier;
                }
                // if we stalemate
                if (ownPower == enemyPower)
                {
                    this.strength -= ownPower * Person.damageRecieveModifier;
                    enemy.strength -= enemyPower * Person.damageRecieveModifier;
                    // dont return stalemate value here in case we both died
                }

                // we die :(
                if (this.strength < 0)
                {
                    World.colonies[this.colonyID].removePersonFromColony(this.currentX, this.currentY);
                    return new FightCondition("lose");
                }
                // enemy dies! :)
                if (enemy.strength < 0)
                {
                    Colony enemyColony = World.colonies[enemy.colonyID];
                    if (enemyColony != null)
                    {
                        enemyColony.removePersonFromColony(enemy.currentX, enemy.currentY);
                    }
                    return new FightCondition("win");
                }
                return new FightCondition("stalemate");
           
        }


        public void update ()
        {
            this.moveCount++;
            int[] coords = this.tryToMove(0, 1);
            if (coords[0] == -1337 || coords[1] == -1337)
            {
                return;
            }
                this.move(coords[0], coords[1]);

            // reproduce
            // 30% chance to increase chance on move
            // 1% chance to increase age on move
            bool increaseStrength = World.rand.Next(0, 100) < 31 ? true : false;
            bool increaseAge = World.rand.Next(0, 100) < 1 ? true : false;

            if (increaseStrength == true)
            {
                this.strength++;
            }
            if (increaseAge == true)
            {
                this.age++;
            }

            // minimum strength to make a baby / age to die
            // TODO add traits
            if (this.strength == 101 && this.age < 90)
            {
                World.colonies[this.colonyID].addPersonToColony(this.currentX, this.currentY);
                this.strength = 0;
            }
            if (this.age >= 90)
            {
                World.colonies[this.colonyID].removePersonFromColony(this.currentX, this.currentY);
            }
        }

        public int[] limitMovement(int xMove, int yMove)
        {
            int modifiedX = xMove;
            int modifiedY = yMove;
            if (modifiedX < 0)
            {
                modifiedX = 0;
            }
            if (modifiedY < 0)
            {
                modifiedY = 0;
            }
            int x = modifiedX;
            int y = modifiedY;

            if (x >= World.tiles.GetLength(0))
            {
                x = World.tiles.GetLength(0) - 1;
            }
            if (y >= World.tiles.GetLength(1))
            {
                y = World.tiles.GetLength(1) - 1;
            }
            return new int[2] { x, y };
        }

        public void move (int x, int y)
        {
            lock (moveLock)
            {
                this.currentX = x;
                this.currentY = y;
                // move to tile
                TileGroup tilegroup = World.tiles[x, y];
                // avoid moving on sea
                if (tilegroup.isLand == false)
                {
                    return;
                }
                tilegroup.setOccupant(x, y, this.color, this);
                if (this.moveCount > 0)
                {
                    TileGroup tilegroupOld = World.tiles[this.oldX, this.oldY];
                    tilegroupOld.removeOccupant();
                    this.oldX = x;
                    this.oldY = y;
                }
            }       }

        public void setStartPosition(int[] startPosition)
        {
            this.startPosition = startPosition;
        }

        public void setColonyID(int colonyID)
        {
            this.colonyID = colonyID;
        }
    }
}
