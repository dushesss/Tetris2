using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;


namespace Tetris2
{
    public partial class Form1 : Form
    {
        public const int width = 15, height = 25, k = 15;
        public int[,] shape = new int[2, 4];
        public int[,] field = new int[width, height];
        public Bitmap bitfield = new Bitmap(k * (width + 1) + 1, k * (height + 3) + 1);
        public Graphics gr;
        public Brush colKirpich;
        public DateTime t1, t2;
        public TimeSpan ts;
        public int count, o = 200;
        System.Media.SoundPlayer sp2 = new System.Media.SoundPlayer();

        public Form1()
        {
            InitializeComponent();
            gr = Graphics.FromImage(bitfield);
            for (int i = 0; i < width; i++)
                field[i, height - 1] = 1;
            for (int i = 0; i < height; i++)
            {
                field[0, i] = 1;
                field[width - 1, i] = 1;
            }
            SetShape();
            
            timer2.Interval = 1000;
            timer2.Start();
            
            t1 = DateTime.Now;
            
            sp2.SoundLocation = "C:\\Users\\dushesss\\Desktop\\Practice\\Tetris2\\darude-sandstorm.wav";
            sp2.Load();
            sp2.Play();
            



        }
        public void FillField()
        {
            gr.Clear(Color.Black);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    if (field[i, j] == 1)
                    {
                        gr.FillRectangle(Brushes.Purple, i * k, j * k, k, k);
                        gr.DrawRectangle(Pens.Black, i * k, j * k, k, k);
                    }
            for (int i = 0; i < 4; i++)
            {
                gr.FillRectangle(colKirpich, shape[1, i] * k, shape[0, i] * k, k, k);
                gr.DrawRectangle(Pens.Black, shape[1, i] * k, shape[0, i] * k, k, k);
            }
            FieldPictureBox.Image = bitfield;
        }
       
       

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (field[8, 3] == 1)
            {
                timer1.Enabled = false;
                MessageBox.Show(
                    "Вы проиграли,сочувствуем",
                    "Упс!=D"
                    );
                this.Close();
                Form2 form = new Form2();
                //form.Show();
                //Environment.Exit(0);
            }
            for (int i = 0; i < 4; i++)
                shape[0, i]++;
            for (int i = height - 2; i > 2; i--)
            {
                var cross = (from t in Enumerable.Range(0, field.GetLength(0)).Select(j => field[j, i]).ToArray() where t == 1 select t).Count();
                if (cross == width)
                {
                    count++;
                    label6.Text = "Линии: " + count.ToString();
                    for (int k = i; k > 1; k--)
                        for (int l = 1; l < width - 1; l++)
                        {
                            field[l, k] = field[l, k - 1];

                            if (count == 5) { o = 150; }
                            if (count == 10) { o = 140; }
                            if (count == 15) { o = 130; }
                            if (count == 20) { o = 120; }
                            if (count == 25) { o = 110; }
                            if (count == 30) { o = 100; }
                            if (count == 35) { o = 90; }
                            if (count == 40) { o = 75; }
                            if (count == 45) { o = 60; }
                            if (count == 50) { o = 50; }
                            timer1.Interval = o;

                        }
                }
            }
            if (count == 50)
            {
                timer1.Enabled = false;
                MessageBox.Show(
                    "Вы выиграли, поздравляем",
                    "Congratulations!"
                    );
                this.Close();
                Form2 form = new Form2();
                //form.Show();
            }
            if (FindMistake())
            {
                for (int i = 0; i < 4; i++)
                    field[shape[1, i], --shape[0, i]]++;
                SetShape();
            }
            FillField();

            //// for(int i=0;i < ts;i++)
            
            //else if (ts.Seconds == 2)
            //    timer1.Interval -= 30;


        }

        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    for (int i = 0; i < 4; i++)
                        shape[1, i]--;
                    if (FindMistake())
                        for (int i = 0; i < 4; i++)
                            shape[1, i]++;
                    break;
                case Keys.D:
                    for (int i = 0; i < 4; i++)
                        shape[1, i]++;
                    if (FindMistake())
                        for (int i = 0; i < 4; i++)
                            shape[1, i]--;
                    break;
                case Keys.W:
                    var shapeT = new int[2, 4];
                    Array.Copy(shape, shapeT, shape.Length);
                    int maxx = 0, maxy = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (shape[0, i] > maxy)
                            maxy = shape[0, i];
                        if (shape[1, i] > maxx)
                            maxx = shape[1, i];
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        int temp = shape[0, i];
                        shape[0, i] = maxy - (maxx - shape[1, i]) - 1;
                        shape[1, i] = maxx - (3 - (maxy - temp)) + 1;
                    }
                    if (FindMistake())
                        Array.Copy(shapeT, shape, shape.Length);
                    break;
                case Keys.S:
                    timer1.Interval = 70;
                    break;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            sp2.Stop();
            this.Hide();
            Form2 form = new Form2();
            form.Show();

        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            //music = new Microsoft.DirectX.AudioVideoPlayback.Audio(@"C:\Users\dushesss\Downloads\darude-sandstorm.mp3");
            //music.Play();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.S:
                    timer1.Interval = o-20;
                    break;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            
            t2 = DateTime.Now;
            ts = t2 - t1;
            label5.Text = "Время:" + ts.Minutes.ToString() + ":" + ts.Seconds.ToString();
            
        }

        public void SetShape()
        {
            Random x = new Random(DateTime.Now.Millisecond);
            switch (x.Next(7))
            {
                case 0: shape = new int[,] { { 2, 3, 4, 5 }, { 8, 8, 8, 8 } }; colKirpich = Brushes.Aqua; break;//палка
                case 1: shape = new int[,] { { 2, 3, 2, 3 }, { 8, 8, 9, 9 } }; colKirpich = Brushes.Blue; break;//кирпич
                case 2: shape = new int[,] { { 2, 3, 4, 4 }, { 8, 8, 8, 9 } }; colKirpich = Brushes.Red; break;//L
                case 3: shape = new int[,] { { 2, 3, 4, 4 }, { 8, 8, 8, 7 } }; colKirpich = Brushes.Orange; break;//Г
                case 4: shape = new int[,] { { 3, 3, 4, 4 }, { 7, 8, 8, 9 } }; colKirpich = Brushes.Azure; break;//Z
                case 5: shape = new int[,] { { 3, 3, 4, 4 }, { 9, 8, 8, 7 } }; colKirpich = Brushes.PapayaWhip; break;//S
                case 6: shape = new int[,] { { 3, 4, 4, 4 }, { 8, 7, 8, 9 } }; colKirpich = Brushes.Violet; break;//T
            }
        }
        public bool FindMistake()
        {
            for (int i = 0; i < 4; i++)
                if (shape[1, i] >= width || shape[0, i] >= height ||
                    shape[1, i] <= 0 || shape[0, i] <= 0 ||
                    field[shape[1, i], shape[0, i]] == 1)
                    return true;
            return false;
        }
    }
}
