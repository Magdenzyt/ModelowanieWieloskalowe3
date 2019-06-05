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
        Bitmap DrawArea2;
        Graphics g;
        Graphics g2;
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
            DrawArea2 = new Bitmap(pictureBox2.Size.Width, pictureBox2.Size.Height);
            pictureBox1.Image = DrawArea;
            pictureBox2.Image = DrawArea2;
            g2 = Graphics.FromImage(DrawArea2);
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

            for (int i = 0; i < szer; i++)
            {
                for (int j = 0; j < wys; j++)
                {
                    if (beginTab[i][j] == 0)
                    {
                        int which = 0;

                        if (comboBox3.Text == "Moore")
                        {
                            which = Moore(i, j);
                        }
                        else if (comboBox3.Text == "Von Neumann")
                        {
                            which = Neumann(i, j);
                        }
                        else if (comboBox3.Text == "Heks L")
                        {
                            which = Heks(i, j, 0);
                        }
                        else if (comboBox3.Text == "Heks P")
                        {
                            which = Heks(i, j, 2);
                        }
                        else if (comboBox3.Text == "Heks Los")
                        {
                            which = Heks(i, j, rnd.Next(2) * 2);
                        }
                        else if (comboBox3.Text == "Pent Los")
                        {
                            which = PentLos(i, j);
                        }

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

        private int Moore(int x, int y)
        {
            List<int> counters = Enumerable.Repeat<int>(0, nextState + 1).ToList();
            if (comboBox2.Text == "Absorbujące")
            {
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if ((i != x || j != y) &&
                            (i >= 0 && i < szer) &&
                            (j >= 0 && j < wys))
                        {
                            if (beginTab[i][j] > 0)
                            {
                                counters[beginTab[i][j]]++;
                            }
                        }
                    }
                }
            }
            else if (comboBox2.Text == "Periodyczne")
            {
                for (int i = x - 1; i <= x + 1; i++)
                {
                    int row = (i + szer) % szer;
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        int col = (j + wys) % wys;
                        if ((row != x || col != y))
                        {
                            if (beginTab[row][col] > 0)
                            {
                                counters[beginTab[row][col]]++;
                            }
                        }
                    }
                }
            }             
            return counters.IndexOf(counters.Max());
        }
        private int Neumann(int x, int y)
        {
            List<int> counters = Enumerable.Repeat<int>(0, nextState + 1).ToList();

            if (comboBox2.Text == "Absorbujące")
            {
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if ((i != x || j != y) && (i == x || j == y) && (i >= 0 && i < szer) && (j >= 0 && j < wys))
                        {
                            if (beginTab[i][j] > 0)
                            {
                                counters[beginTab[i][j]]++;
                            }
                        }
                    }
                }
            }
            else if (comboBox2.Text == "Periodyczne")
            {
                for (int i = x - 1; i <= x + 1; i++)
                {
                    int row = (i + szer) % szer;
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        int col = (j + wys) % wys;
                        if ((row != x || col != y)&&(row == x || col == y))
                        {
                            if (beginTab[row][col] > 0)
                            {
                                counters[beginTab[row][col]]++;
                            }
                        }
                    }
                }
            }

                return counters.IndexOf(counters.Max());
        }
        private int Heks(int x, int y, int diff)
        {
            List<int> counters = Enumerable.Repeat<int>(0, nextState + 1).ToList();

            if (comboBox2.Text == "Absorbujące")
            {
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if ((i != x || j != y) &&
                            (Math.Abs(i - x - j + y) != diff) &&
                            (i >= 0 && i < szer) &&
                            (j >= 0 && j < wys))
                        {
                            if (beginTab[i][j] > 0)
                            {
                                counters[beginTab[i][j]]++;
                            }
                        }
                    }
                }
            }
            else if (comboBox2.Text == "Periodyczne")
            {
                for (int i = x - 1; i <= x + 1; i++)
                {
                    int row = (i + szer) % szer;
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        int col = (j + wys) % wys;
                        if ((row != x || col != y) &&
                            (Math.Abs(row - x - col + y) != diff))
                        {
                            if (beginTab[row][col] > 0)
                            {
                                counters[beginTab[row][col]]++;
                            }
                        }
                    }
                }
            }
                return counters.IndexOf(counters.Max());
        }
        private int PentLos(int x, int y)
        {
            List<int> counters = Enumerable.Repeat<int>(0, nextState + 1).ToList();

            int startX = x - 1, endX = x + 1;
            int startY = y - 1, endY = y + 1;

            switch (rnd.Next(4))
            {
                case 0:
                    endY = y;
                    break;
                case 1:
                    startY = y;
                    break;
                case 2:
                    endX = x;
                    break;
                case 3:
                    startX = x;
                    break;
            }

            if (comboBox2.Text == "Absorbujące")
            {
                for (int i = startX; i <= endX; i++)
                {
                    for (int j = startY; j <= endY; j++)
                    {
                        if ((i != x || j != y) &&
                            (i >= 0 && i < szer) &&
                            (j >= 0 && j < wys))
                        {
                            if (beginTab[i][j] > 0)
                            {
                                counters[beginTab[i][j]]++;
                            }
                        }
                    }
                }
            }
            else if (comboBox2.Text == "Periodyczne")
            {
                for (int i = startX; i <= endX; i++)
                {
                    int row = (i + szer) % szer;
                    for (int j = startY; j <= endY; j++)
                    {
                        int col = (j + wys) % wys;
                        if ((row != x || col != y))
                        {
                            if (beginTab[row][col] > 0)
                            {
                                counters[beginTab[row][col]]++;
                            }
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
                g.Clear(Color.Transparent);
                nextTab = new List<List<int>>();
                for (int i = 0; i < 500; i++)
                {
                    nextTab.Add(Enumerable.Repeat<int>(0, 500).ToList());
                }
            }

            pictureBox1.Image = DrawArea;
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
            int czyDodało = 0;
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
                        czyDodało++;
                        break;
                    }
                }
            }
            if (czyDodało < number)
            {
                MessageBox.Show("Uwaga! Dodało tylko: " + czyDodało);
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

            int row = coordinates.Y / 5;
            int col = coordinates.X / 5;

            nextTab[row][col] = nextState;
            Draw(row, col, nextState);
            pictureBox1.Image = DrawArea;
            nextState++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            g.Clear(Color.Transparent);
            nextTab = new List<List<int>>();
            for (int i = 0; i < szer; i++)
            {
                nextTab.Add(Enumerable.Repeat<int>(0, wys).ToList());
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int iterations = int.Parse(textBox9.Text);
            double kT = double.Parse(textBox10.Text);
            for (int i = 0; i < iterations; i++)
            {

                List<List<int>> energy = MonteCarlo(kT);
                DrawEnergy(energy);
                Application.DoEvents();
            }
        }
        public List<List<int>> MonteCarlo(double kt)
        {
            List<List<int>> returnEnergy = new List<List<int>>();
            for (int i = 0; i < szer; i++)
            {
                returnEnergy.Add(Enumerable.Repeat<int>(0, wys).ToList());
            }
            var rand = new Random();
            List<Tuple<int, int>> cells = new List<Tuple<int, int>>();
            for (int i = 0; i < nextTab.Count; i++)
            {
                for (int j = 0; j < nextTab[i].Count; j++)
                {
                    cells.Add(new Tuple<int, int>(i, j));
                }
            }
            while (cells.Count > 0)
            {
                int index = rand.Next(cells.Count);
                Tuple<int, int> cell = cells[index];
                cells.Remove(cell);
                int currentEnergy = OriginalEnergy(cell.Item1, cell.Item2);
                returnEnergy[cell.Item1][cell.Item2]=currentEnergy;
                List<int> neighbours = Neighbours(cell.Item1, cell.Item2);
                neighbours.Remove(nextTab[cell.Item1][cell.Item2]);
                if (neighbours.Count <= 0) continue;
                for (int i = 0; i < neighbours.Count; i++)
                {
                    int energy = AlteredEnergy(cell.Item1, cell.Item2, neighbours[i]);
                    int energyDiff = energy - currentEnergy;
                    if (energyDiff <= 0)
                    {
                        nextTab[cell.Item1][cell.Item2] = neighbours[i];
                        Draw(cell.Item1, cell.Item2, neighbours[i]);
                        break;
                    }
                    else
                    {
                        double probability = Math.Exp(-(energyDiff / kt));
                        if (rand.NextDouble() <= probability)
                        {
                            nextTab[cell.Item1][cell.Item2] = neighbours[i];
                            Draw(cell.Item1, cell.Item2, neighbours[i]);
                            break;
                        }
                    }
                }
            }
            pictureBox1.Image = DrawArea;
            return returnEnergy;
        }

        public void DrawEnergy(List<List<int>> energy)
        {
            int colorDiff = 255 / 9;
            List<Brush> brushe = new List<Brush>();

            for(int i = 0; i<9; i++)
            {
                brushe.Add(new SolidBrush(Color.FromArgb(colorDiff * i, colorDiff*i, colorDiff*i)));
            }
            g2.Clear(Color.White);
            for (int i=0; i<szer; i++)
            {
                for(int j=0; j<wys; j++)
                {
                    
                    g2.FillRectangle(brushe[energy[i][j]], i * 5, j * 5, 5, 5);
                }
            }
            pictureBox2.Image = DrawArea2;

        }

        private int OriginalEnergy(int row, int col)
        {
            int energy = 0;
            for (int i = Math.Max(row - 1, 0); i <= Math.Min(row + 1, szer - 1); i++)
            {
                for (int j = Math.Max(col - 1, 0); j <= Math.Min(col + 1, wys - 1); j++)
                {
                    if (beginTab[row][col] != beginTab[i][j])
                    {
                        energy++;
                    }
                }
            }
            return energy;
        }
        private int AlteredEnergy(int row, int col, int state)
        {
            int energy = 0;
            for (int i = Math.Max(row - 1, 0); i <= Math.Min(row + 1, szer - 1); i++)
            {
                for (int j = Math.Max(col - 1, 0); j <= Math.Min(col + 1, wys - 1); j++)
                {
                    if (nextTab[i][j] != state)
                    {
                        energy++;
                    }
                }
            }
            return energy;
        }
        private List<int> Neighbours(int row, int col)
        {
            List<int> neighbours = new List<int>();
            for (int i = Math.Max(row - 1, 0); i <= Math.Min(row + 1, szer - 1); i++)
            {
                for (int j = Math.Max(col - 1, 0); j <= Math.Min(col + 1, wys - 1); j++)
                {
                    if (row != i && col != j && !neighbours.Contains(nextTab[i][j]))
                    {
                        neighbours.Add(nextTab[i][j]);
                    }
                }
            }
            return neighbours;
        }
    }

}

