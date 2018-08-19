using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ImageEDLib.Controlers.ImageEncryption;
using ImageEDLib.Controlers.ImageOperations;
using ImageEDLib.Models.Huffman;
using ImageEDLib.Models.ImageContainers;

namespace ImageEDFrontEnd
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;

                (new ImageHolder()).DisplayImage((new ImageHolder().OpenImage(OpenedFilePath)), pictureBox1);

                (new ImageHolder()).DisplayImage((new ImageEncryptor()).
                    Encrypt(key: new KeyValuePair<string, int>("001010101110010101010", 2), source: new ImageHolder().OpenImage(OpenedFilePath)), pictureBox2);

            }
            try
            {
                
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
        }
    }
}