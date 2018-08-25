using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ImageEDLib;
using ImageEDLib.Controlers.ImageCompression;
using ImageEDLib.Controlers.ImageEncryption;
using ImageEDLib.Controlers.ImageOperations;
using ImageEDLib.Models.Huffman;
using ImageEDLib.Models.ImageContainers;

namespace ImageEDFrontEnd
{
    public partial class Form1 : Form
    {
        private RGBPixel[,] imageMatrix;
        long[,] mFrequencies = new long[3, 260];
        readonly ImageOperations _imageOperations = new ImageOperations();
        readonly ImageEncryptor _imageEncryptor = new ImageEncryptor();
        readonly ImageComprosser _imageComprosser = new ImageComprosser();
        private string encryptKey;
        private int tapIdx = 0;

        public Form1()
        {
            InitializeComponent();
            ImageEncComp.Initialize(_imageOperations, _imageEncryptor, _imageComprosser);
        }
        
        private bool ValidateInput()
        {
            encryptKey = txtInitSeed.Text;
            try
            {

                if (encryptKey == "")
                {
                    MessageBox.Show("Please enter valid tap index !");
                    return false;
                }
                tapIdx = Int32.Parse(txtTapIdx.Text);
                if (tapIdx > encryptKey.Length || tapIdx <= 0)
                {
                    MessageBox.Show("The tab index must be less than or equals the key length and more than zero!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter valid tap index !");
                return false;
            }

            return true;
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                imageMatrix = ImageEncComp.OpenImage(OpenedFilePath) as RGBPixel[,];
                ImageEncComp.DisplayImage(imageMatrix, pictureBox1);
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            imageMatrix = ImageEncComp.Encrypt(
                new KeyValuePair<string, int>(encryptKey, tapIdx), imageMatrix) as RGBPixel[,];
            ImageEncComp.DisplayImage(imageMatrix, pictureBox2);
        }

        private void btnFastEncAndCom_Click(object sender, EventArgs e)
        {

            if (!ValidateInput())
                return;

            // TODO: refactor fast algorithms again
            var key = new KeyValuePair<string, int>(encryptKey, tapIdx);
            imageMatrix = ImageEncComp.FastEncryptionWithCompression(
               ref key, ref imageMatrix,
                ref mFrequencies) as RGBPixel[,];
            ImageEncComp.DisplayImage(imageMatrix, pictureBox2);
            var compressedResultObject =
                ImageEncComp.FastCompressionWithPreEncryption(ref imageMatrix, ref mFrequencies);
            SaveFile(compressedResultObject);
            
        }

        private void btnCompress_Click(object sender, EventArgs e)
        {
            
            var compressedResultObject =
                ImageEncComp.Compress(imageMatrix);
            SaveFile(compressedResultObject);
        }

        private void SaveFile(object compressedResultObject)
        {
            var saveResult = MessageBox.Show("The image has been encrypted and compressed do you want to save ?",
                "Save Image ?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (saveResult == DialogResult.Yes)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = @"C:\Users\Musta\Desktop\";
                saveFileDialog.Title = "Save text Files";
                saveFileDialog.DefaultExt = "bin";
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate);
                    BinaryWriter binaryWriter = new BinaryWriter(fs);
                    binaryWriter.Write(compressedResultObject as byte[]);
                    binaryWriter.Close();
                    MessageBox.Show("The file has been saved.");
                }
            }
            else if (saveResult == DialogResult.No)
            {
                compressedResultObject = null;
            }
        }
    }
}