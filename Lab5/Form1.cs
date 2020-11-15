using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public partial class Form1 : Form
    {
        private UCDLRAOCI myclass = new UCDLRAOCI();

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
    }
}
