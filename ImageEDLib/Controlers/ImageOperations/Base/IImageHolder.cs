using System.Windows.Forms;

namespace ImageEDLib.Controlers.ImageOperations.Base
{
    public interface IImageHolder
    {
        object OpenImage(string imagePath);
        void DisplayImage(object imageMatrix, PictureBox picBox);
        bool SaveImage(object imageMatrix, string locatioPath);
        int GetWidth(object imageMatrix);
        int GetHeight(object imageMatrix);
    }
}