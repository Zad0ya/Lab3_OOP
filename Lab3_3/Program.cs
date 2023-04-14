using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Lab3_3
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = @"C:\Images";
            string[] imagePaths = Directory.GetFiles(directoryPath, "*.jpg");

            Func<Bitmap, Bitmap> imageProcessingDelegate = ProcessImage;

            foreach (string imagePath in imagePaths)
            {
                Bitmap originalImage = new Bitmap(imagePath);
                Bitmap processedImage = imageProcessingDelegate(originalImage);
                DisplayImage(processedImage);

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
        //Приклад з підвищенням контрастності зображення:

        static Bitmap ProcessImage(Bitmap image)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);

            int maxPixelValue = 0;
            int minPixelValue = 255;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);
                    int brightness = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                    maxPixelValue = Math.Max(maxPixelValue, brightness);
                    minPixelValue = Math.Min(minPixelValue, brightness);
                }
            }

            double contrast = 255.0 / (maxPixelValue - minPixelValue);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);
                    int brightness = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                    brightness = (int)(contrast * (brightness - minPixelValue));
                    brightness = Math.Max(0, Math.Min(255, brightness));
                    Color newPixel = Color.FromArgb(pixel.A, brightness, brightness, brightness);
                    result.SetPixel(x, y, newPixel);
                }
            }

            return result;
        }

        //Кінець приклаад

        static void DisplayImage(Bitmap image)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".bmp");
            image.Save(tempPath);
            System.Diagnostics.Process.Start(tempPath);
        }
    }
}

