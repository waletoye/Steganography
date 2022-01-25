using System;
using System.Drawing;

namespace steganography
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap image = (Bitmap)Image.FromFile(Utilities.Settings.ImageLocation);

            var steg = new Utilities.Steganography();

            //conceal text
            string textToConceal = "crack the code";
            Bitmap modifiedImage = steg.ConcealText(textToConceal, image);

            //retrive text
            string result = steg.RetrieveConcealedText(modifiedImage);

            Console.WriteLine(result);

            Console.ReadKey();
        }
    }

}
