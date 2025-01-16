using System.Drawing;
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

            string output = "";
            for (int i = 0; i < asciiArt.Length; i++)
            {
                for (int j = 0; j < asciiArt[i].Length; j++)
                {
                    output += asciiArt[i][j];
                }
                output += "\n";
            }

            try
            {
                File.WriteAllText(@"C:\Users\Marc\ascii.txt", output, Encoding.UTF8);
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

            char[][] asciiArt = new char[bitmap.Height][]; 

            for (int y = 0; y < bitmap.Height; y++)
            {
                asciiArt[y] = new char[bitmap.Width];

                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    int grayValue = (int)((pixelColor.R + pixelColor.G + pixelColor.B)/3);
                    char asciiChar = GetAsciiCharForGrayValue(grayValue, asciiChars);
                    asciiArt[y][x] = asciiChar;
                }
            }
            return asciiArt;
        }
    }

    public static char GetAsciiCharForGrayValue(int grayValue, string asciiChars)
    {
        int index = (int)((grayValue / 255.0) * (asciiChars.Length - 1));
        return asciiChars[index];
    }
}
