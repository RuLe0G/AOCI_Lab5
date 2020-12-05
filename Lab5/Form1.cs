using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing.Imaging;
using System.Drawing;
using Emgu.CV.OCR;

namespace Lab5
{
    public partial class Form1 : Form
    {
        private UCDLRAOCI myclass = new UCDLRAOCI();
        public VideoCapture capture;
        CascadeClassifier face;
        Mat image = new Mat();
        Image<Bgr, byte> input;
        Mat frame;

        public Form1()
        {
            InitializeComponent();
        }
        private string OpenFile()
        {
            string fileName = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = ("Файлы изображений | *.jpg; *.jpeg; *.jpe; *.jfif; *.png");
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла            
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                fileName = openFileDialog.FileName;
            }
            return fileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myclass.Source(OpenFile());
            imageBox1.Image = myclass.sourceImage;
            imageBox2.Image = myclass.sourceImage;
            Clear();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void Clear()
        {
            myclass.MainImage();
            imageBox2.Image = myclass.MainImageExp;
            listBox1.Items.Clear();
            label1.Text = "";
            myclass.RoiList.Clear();
        }
        private void button17_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = ("Файлы изображений | *.jpg; *.jpeg; *.jpe; *.jfif; *.png");
            var result = saveFileDialog.ShowDialog(); // открытие диалога выбора файла            
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = saveFileDialog.FileName;
                myclass.saveJpeg(fileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clear();
            imageBox1.Image = myclass.Lab5Process(trackBar1.Value);
            for( int i = 0; i < myclass.RoiList.Count; i++)
            {
                listBox1.Items.Add("Roi "+ i.ToString());
            }            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            imageBox2.Image = myclass.GetROI(listBox1.SelectedIndex);
            if (checkBox1.Checked)
            {
                label1.Text = myclass.Translate(myclass.GetROI(listBox1.SelectedIndex),"eng");
            }
            if (checkBox2.Checked)
            {
                label1.Text = myclass.Translate(myclass.GetROI(listBox1.SelectedIndex), "rus");
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label2.Text = trackBar1.Value.ToString();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            face = new CascadeClassifier(@"D:\Stud\TEMP\tessdata\haarcascade_frontalface_default.xml");

            OpenFileDialog f = new OpenFileDialog();
            f.ShowDialog();

            frame = CvInvoke.Imread(f.FileName, ImreadModes.Unchanged);

            imageBox1.Image = frame.Split()[3];

        }

        
        public void ProcessFrame(object sender, EventArgs e)
        {
            capture.Retrieve(image);

            input = image.ToImage<Bgr, byte>();
            List<Rectangle> faces = new List<Rectangle>();

            Image<Bgra, byte> res = input.Convert<Bgra, byte>();

            using (Mat ugray = new Mat())
            {
                CvInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                Rectangle[] facesDetected = face.DetectMultiScale(ugray, 1.1, 10, new Size(10, 10));
                faces.AddRange(facesDetected);
            }

            //foreach (Rectangle rect in faces)
            //    input.Draw(rect, new Bgr(Color.Yellow), 2);


            foreach (Rectangle rect in faces) //для каждого лица
            {
                res.ROI = rect; //для области содержащей лицо
                Image<Bgra, byte> small = frame.ToImage<Bgra, byte>().Resize(rect.Width, rect.Height, Inter.Nearest); //создание
                                                                                                                      //копирование изображения small на изображение res с использованием маски копирования mask
                CvInvoke.cvCopy(small, res, small.Split()[3]);
                res.ROI = System.Drawing.Rectangle.Empty;
            }

            imageBox2.Image = res;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            capture = new VideoCapture();
            capture.ImageGrabbed += ProcessFrame;
            capture.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            imageBox2.Image = myclass.Lab5Face();
        }
    }
}
