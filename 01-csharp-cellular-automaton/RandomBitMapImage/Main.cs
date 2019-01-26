using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Timer = System.Windows.Forms.Timer;

namespace RandomBitMapImage
{
    // TODO Add a method for updating a tile (TileGroup)
    // TODO keep track of the tile underneath when a person is on top of it.

    public partial class Main : Form
    {
        // how many MS 
        static int tickSpeed = 10;
        private readonly object imageLock = new object();
        Random rand = new Random();
        World world;


        public Main()
        {
            InitializeComponent();
        }

        public void tick()
        {
<<<<<<< HEAD
            this.world.onTick();
                string stats = this.world.getColonyStats();
                label1.Text = stats;
=======
            Task[] tasks = new Task[World.colonies.Length];
            for (int j = 0; j < World.colonies.Length; j++)
            {
                if (j < World.colonies.Length)
                {
                    Colony colony = World.colonies[j];
                    Task task = Task.Factory.StartNew(() => colony.update());
                    tasks.SetValue(task, j);
                }
            }
            Task.WaitAll(tasks);

            string stats = this.world.getColonyStats();

            this.label1.Invoke((MethodInvoker) delegate {
                // Running on the UI thread
                this.label1.Text = stats;
                this.pictureBox1.Image = World.world;
            });

            // label1.Text = stats;
>>>>>>> support-multithreading
            
            // this.pictureBox1.Image = World.world;
            
        }

        private void onTick()
        {
            while (true)
            {
                Thread.Sleep(Main.tickSpeed);
                this.tick();
            }
        }

        private void setupWorld()
        {
            this.world = new World(400, 400);
            Bitmap bmp = this.world.regenerateWorld();
            pictureBox1.Image = bmp;
            string stats = this.world.getColonyStats();
            label1.Text = stats;
            Task mainTicker = new Task(this.onTick); 
            mainTicker.Start();
        }

        private void onLoadWorld(object sender, EventArgs e)
        {
            this.setupWorld();
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  this.setupWorld();
          //  Console.WriteLine("you clicked on the button the re-render!!");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
