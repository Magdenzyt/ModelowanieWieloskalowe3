using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        Bitmap DrawArea;
        Graphics g;
        SolidBrush pinkBrush = new SolidBrush(Color.HotPink);

        List<List<int>> beginTab = new List<List<int>>();
        List<List<int>> nextTab = new List<List<int>>();
        List<Brush> brushes = new List<Brush>();
        Random rnd = new Random();
        int nextState = 1;

        Timer timer;

        int wys = 50;
        int szer = 50;

        public Form1()
        {
            
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 200;
            timer.Tick += new EventHandler(timer1_Tick);
            DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = DrawArea;
            g = Graphics.FromImage(DrawArea);
            var rand = new Random();
            for (int i = 0; i < 101; i++)
            {
                Color randomColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                brushes.Add(new SolidBrush(randomColor));
            }
            beginTab = new List<List<int>>();
            for (int i = 0; i < szer; i++)
            {
                beginTab.Add(Enumerable.Repeat<int>(0, wys).ToList());
            }
            nextTab = new List<List<int>>();
            for (int i = 0; i < szer; i++)
            {
                nextTab.Add(Enumerable.Repeat<int>(0, wys).ToList());
            }
            GenerateRandomSeeds(2, 10);
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            beginTab = nextTab;
            nextTab = new List<List<int>>();
            for (int i = 0; i < szer; i++)
            {
                nextTab.Add(Enumerable.Repeat<int>(0, wys).ToList());
            }

            for(int i = 0; i<szer; i++)
            {
                for (int j = 0; j < wys; j++)
                {
                    if (beginTab[i][j] == 0)
                    {
                        int which = 0;
                        which = Neighbours(i, j);
                      
                        if (which > 0)
                        {
                            nextTab[i][j] = which;
                            Draw(i, j, which);
                        }
                    }
                    else
                    {
                        nextTab[i][j] = beginTab[i][j];
                    }
                }
            }
            pictureBox1.Image = DrawArea;
        }

        private int Neighbours(int x, int y)
        {
            List<int> counters = Enumerable.Repeat<int>(0, nextState + 1).ToList();

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if ((i != x || j != y) && (i >= 0 && i < szer) && (j >= 0 && j < wys))
                    {
                        if (beginTab[i][j] > 0)
                        {
                            counters[beginTab[i][j]]++;
                        }
                    }
                }
            }

            return counters.IndexOf(counters.Max());
        }
        public void Draw(int row, int col, int state)
        {       
           g.FillRectangle(brushes[state - 1], col * 5, row * 5, 5, 5);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Stop();
            }
            else
                timer.Start();
        }

        public void GenerateRandomSeeds(int numberOfTypes, int numberOfSeeds)
        {
            var rand = new Random();
            for (int i = 1; i <= numberOfTypes; i++)
            {
                for (int j = 0; j < numberOfSeeds; j++)
                {
                    int row = rand.Next(szer);
                    int col = rand.Next(wys);
                    if (nextTab[row][col] == 0)
                    {
                        nextTab[row][col] = nextState;
                        Draw(row, col, nextState);
                    }
                    else
                    {
                        i--;
                    }
                }
                nextState++;
            }
            pictureBox1.Image = DrawArea;
        }

    }
}
