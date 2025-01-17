using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Enter the path of the image you want to convert to ASCII art:");
            string imagePath = Console.ReadLine();
            Console.WriteLine("Enter the path where you want to save the ASCII art:");
            string outputPath = Console.ReadLine();
            Console.WriteLine("Converting image to ASCII art...");

            char[][] asciiArt = GetAsciiArtFromImage(imagePath);

            StringBuilder output = new StringBuilder();
            for (int i = 0; i < asciiArt.Length; i++)
            {
                for (int j = 0; j < asciiArt[i].Length; j++)
                {
                    output.Append(asciiArt[i][j]);
                    output.Append(' ');
                }
                output.AppendLine();
            }

            try
            {
                File.WriteAllText(outputPath, output.ToString(), Encoding.UTF8);
                Console.WriteLine($"ASCII art saved in '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.WriteLine();
        }
    }

    public static char[][] GetAsciiArtFromImage(string imagePath)
    {
        using (Bitmap bitmap = new Bitmap(imagePath))
        {
            string asciiChars = "@#S%?*+;:,. ";
            int width = bitmap.Width;
            int height = bitmap.Height;

            char[][] asciiArt = new char[height][];
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb
            );

            unsafe
            {
                byte* ptr = (byte*)bitmapData.Scan0;
                int stride = bitmapData.Stride;

                for (int y = 0; y < height; y++)
                {
                    asciiArt[y] = new char[width];
                    for (int x = 0; x < width; x++)
                    {
                        byte b = ptr[y * stride + x * 3];
                        byte g = ptr[y * stride + x * 3 + 1];
                        byte r = ptr[y * stride + x * 3 + 2];
                        int grayValue = (r + g + b) / 3;
                        asciiArt[y][x] = GetAsciiCharForGrayValue(grayValue, asciiChars);
                    }
                }
            }

            bitmap.UnlockBits(bitmapData);
            return asciiArt;
        }
    }

    public static char GetAsciiCharForGrayValue(int grayValue, string asciiChars)
    {
        int index = (int)((grayValue / 255.0) * (asciiChars.Length - 1));
        return asciiChars[index];
    }
}
