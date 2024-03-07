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


using System;

using System.Runtime.InteropServices;
using System.Threading;

class Program
{
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

    // static void Main(string[] args)
    // {
    //     int interval = 2000; // Click interval in milliseconds (e.g., 2000 ms = 2 seconds)

    //     Console.WriteLine("Press Ctrl + C to stop the program.");

    //     while (true)
    //     {
    //         PrintMousePosition();
    //         Thread.Sleep(interval);
    //     }
    // }
    
    static void Main(string[] args)
    {
        string filePath = "C:/Users/Kvakino/Documents/VSCode/emergency/Emergency/positions.txt";

        try
        {
            Thread.Sleep(1000);
            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    Thread.Sleep(500);
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

    static void MouseClick(int x, int y)
    {
        POINT cursorPosition;
        if (!SetCursorPos(x, y)) {
            return;
        }
        if (GetCursorPos(out cursorPosition))
        {
            // Perform a left mouse button click at the current position
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }
    }

    // Import the mouse_event function from the user32.dll library
    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

    // Define constants for mouse events
    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;
}