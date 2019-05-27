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
        List<Brush> brushes = new List<Brush>();

        List<List<int>> beginTab = new List<List<int>>();
        List<List<int>> nextTab = new List<List<int>>();
        

        Random rnd = new Random();
        int nextState = 1;

        Timer timer;

        int wys;
        int szer;

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

            comboBox2.Items.Add("Periodyczne");
            comboBox2.Items.Add("Absorbujące");

            comboBox3.Items.Add("Moore");
            comboBox3.Items.Add("Von Neumann");
            comboBox3.Items.Add("Heks L");
            comboBox3.Items.Add("Heks P");
            comboBox3.Items.Add("Heks Los");
            comboBox3.Items.Add("Pent Los");


            comboBox1.Items.Add("Jednorodne");
            comboBox1.Items.Add("Z promieniem");
            comboBox1.Items.Add("Losowe");
            comboBox1.Items.Add("Klikanie");
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

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            g.Clear(Color.Transparent);

            beginTab = new List<List<int>>();
            nextTab = new List<List<int>>();

            wys = int.Parse(textBox3.Text);
            szer = int.Parse(textBox4.Text);

            for (int i = 0; i < szer; i++)
            {
                beginTab.Add(Enumerable.Repeat<int>(0, wys).ToList());
            }
            for (int i = 0; i < szer; i++)
            {
                nextTab.Add(Enumerable.Repeat<int>(0, wys).ToList());
            }

            if (comboBox1.Text == "Jednorodne")
            {
                int wiersz = int.Parse(textBox5.Text);
                int kolumna = int.Parse(textBox6.Text);
                GenerateHomogeneousSeeds(wiersz, kolumna);
            }
            else if (comboBox1.Text == "Z promieniem")
            {
                int promien = int.Parse(textBox2.Text);
                int promIle = int.Parse(textBox7.Text);
                GenerateRadiusSeeds(promien, promIle);
            }
            else if (comboBox1.Text == "Losowe")
            {
                int nrTypes = int.Parse(textBox1.Text);
                int losIle = int.Parse(textBox8.Text);
                GenerateRandomSeeds(nrTypes, losIle);
            }
            else if (comboBox1.Text == "Klikanie")
            {

            }
        }


        public void GenerateRandomSeeds(int numberOfTypes, int numberOfSeeds)
        {
            nextState = 1;
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
        public void GenerateHomogeneousSeeds(int numberRows, int numberCols)
        {
            nextState = 1;
            int rowOffset = szer / numberRows;
            int colOffset = wys / numberCols;

            for (int i = 0; i < numberRows; i++)
            {
                for (int j = 0; j < numberCols; j++)
                {
                    Draw((int)((i + 0.5) * rowOffset), (int)((j + 0.5) * colOffset), nextState);
                    nextTab[(int)((i + 0.5) * rowOffset)][(int)((j + 0.5) * colOffset)] = nextState++;
                }
            }
        }
        public void GenerateRadiusSeeds(int radius, int number)
        {
            nextState = 1;
            var rand = new Random();
          
            List<Tuple<int, int>> points = new List<Tuple<int, int>>();

            for (int i = 0; i < number; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    int row = rand.Next(szer);
                    int col = rand.Next(wys);
                    bool isValid = true;
                    for (int k = 0; k < points.Count; k++)
                    {
                        double distance = Math.Sqrt(Math.Pow(row- points[k].Item1,2)+Math.Pow(col-points[k].Item2,2));
                        if (distance <= radius * 2)
                        {
                            isValid = false;
                            break;
                        }
                    }
                    if (isValid)
                    {
                        points.Add(new Tuple<int, int>(row, col));
                        break;
                    }
                }
            }

            for (int i = 0; i < points.Count; i++)
            {
                Draw(points[i].Item1, points[i].Item2, nextState);
                nextTab[points[i].Item1][points[i].Item2] = nextState++;
            }


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs myszka = (MouseEventArgs)e;
            Point coordinates = myszka.Location;

            wys = int.Parse(textBox3.Text);
            szer = int.Parse(textBox4.Text);

            int x = coordinates.X / (500 / wys);
            int y = coordinates.Y / (500 / szer);

            beginTab[x][y] = nextState;

            g.Clear(Color.Transparent);
            for (int z = 0; z < wys; z++)
            {
                for (int j = 0; j < szer; j++)
                {
                    if (beginTab[z][j] == nextState)
                    {
                        if (wys > szer)
                            g.FillRectangle(pinkBrush, z * (500 / wys), j * (500 / wys), 500 / wys, 500 / wys);
                        if (szer > wys)
                            g.FillRectangle(pinkBrush, z * (500 / szer), j * (500 / szer), 500 / szer, 500 / szer);
                    }
                }
            }
            pictureBox1.Image = DrawArea;
        }
    }

}

