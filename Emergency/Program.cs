using System.Drawing;

using System.Runtime.InteropServices;

#pragma warning disable CA1416 // Validate platform compatibility

class Program
{
    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

    private const uint MOUSE_EVENT_LEFT_DOWN = 0x0002;
    
    private const uint MOUSE_EVENT_LEFT_UP = 0x0004;

    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    [StructLayout(LayoutKind.Sequential)]
    struct POINT
    {
        public int X;
        public int Y;
    }

    static void Main(string[] args)
    {
        Thread.Sleep(2_000);
        StartTheGame();
    }

    static void StartTheGame()
    {
        SelectLevel();
        LoadingLobby();
        SelectEquipment();
        LoadingLevel();
    }
    
    static void SelectLevel()
    {
        (int startingX, int startingY) = (250, 170);
        (int endingX, int endingY) = (1230, 900);

        Color pixelToFind = ColorTranslator.FromHtml("#11EC3D");

        while (true)
        {
            Bitmap screen = new Bitmap(endingX-startingX, endingY-startingY);
            Graphics.FromImage(screen).CopyFromScreen(250, 170, 0, 0, screen.Size);

            for (int x = 0; x < endingX - startingX; x += 3)
            {
                for (int y = 0; y < endingY - startingY; y += 16)
                {
                    Color pixel = screen.GetPixel(x, y);
                    if (pixel.Equals(pixelToFind)) {
                        screen.Dispose();
                        Console.WriteLine($"{startingX+x}, {startingY+y}");
                        MouseClick(startingX+x, startingY+y);
                        Thread.Sleep(500);
                        SetCursorPos(776, 776);
                        Thread.Sleep(100);
                        SetCursorPos(777, 776);
                        Thread.Sleep(100);
                        MouseClick(777, 777);
                        return;
                    }
                }
            }

            screen.Dispose();
            Console.WriteLine("Not found! Waiting 5s");
            Thread.Sleep(5_000);
        }
    }

    static void LoadingLobby()
    {
        Thread.Sleep(10_000);
    }

    static void LoadingLevel()
    {
        Thread.Sleep(1_000);
    }

    static void MouseClick(int x, int y)
    {
        POINT cursorPosition;

        if (!SetCursorPos(x, y))
        {
            return;
        }

        if (GetCursorPos(out cursorPosition))
        {
            mouse_event(MOUSE_EVENT_LEFT_DOWN | MOUSE_EVENT_LEFT_UP, x, y, 0, 0);
        }
    }

    static void SelectEquipment()
    {
        string filePath = "C:/Users/Kvakino/Documents/VSCode/emergency/Emergency/positions.txt";

        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    Thread.Sleep(100);
                    string[] parts = line.Split(' ');
                    if (int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y)) {
                        Console.WriteLine($"{x} {y}");
                        MouseClick(x, y);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void PrintMousePosition()
    {
        POINT cursorPosition;
        if (GetCursorPos(out cursorPosition))
        {
            // Perform a left mouse button click at the current position
            Console.WriteLine("{0} {1}", cursorPosition.X, cursorPosition.Y);
        }
    }
}

// using System;
// using System.Drawing;
// using System.IO;
// using System.Threading;

// class Program
// {
//     static void Main(string[] args)
//     {
//         int x = 100; // Example x-coordinate
//         int y = 100; // Example y-coordinate
//         string filePath = Path.Combine(Environment.CurrentDirectory, "../pixel_log.txt");
//         int interval = 500; // Time interval in milliseconds (e.g., 5000 ms = 5 seconds)

//         while (true)
//         {
//             Color pixel = CapturePixel(x, y);
//             Console.WriteLine(pixel);
//             LogPixel(filePath, pixel);
//             Thread.Sleep(interval);
//             break;
//         }
//     }

//     static Color CapturePixel(int x, int y)
//     {
//         Bitmap screen = new Bitmap(1, 1);
//         Graphics g = Graphics.FromImage(screen);
//         g.CopyFromScreen(x, y, 0, 0, screen.Size);
//         Color pixel = screen.GetPixel(0, 0);
//         screen.Dispose();
//         return pixel;
//     }

//     static void LogPixel(string filePath, Color pixel)
//     {
//         try
//         {
//             using (StreamWriter writer = File.AppendText(filePath))
//             {
//                 writer.WriteLine($"R:{pixel.R}, G:{pixel.G}, B:{pixel.B}");
//             }
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Error logging pixel: {ex.Message}");
//         }
//     }
// }