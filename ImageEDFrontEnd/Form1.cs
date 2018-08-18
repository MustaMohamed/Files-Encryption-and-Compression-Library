using System;
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
            ImageEncryptor ie = new ImageEncryptor();
            try
            {
                ie.Encrypt("", "");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
        }
    }
}