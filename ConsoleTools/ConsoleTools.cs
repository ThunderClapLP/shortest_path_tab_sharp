using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathTAB.ConsoleTools
{
    public class ConsoleTools
    {
        public static int MaxWidth = 100;
        public static void WriteResult(bool success)
        {
            ConsoleColor col = Console.BackgroundColor;

            if (success)
            {
                Console.CursorLeft = DockRight() - 4;
                Console.Write("  ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("OK");
            }
            else
            {
                Console.CursorLeft = DockRight() - 8;
                Console.Write("  ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("FAILED");
            }

            //Console.BackgroundColor=col;
            Console.ResetColor();
        }

        public static void WriteError(string err, bool isWarning = false)
        {
            ConsoleColor col = Console.BackgroundColor;

            if (!isWarning)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.Write("ERROR: ");
            }
            else
            {
                //Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("WARN: ");
            }
            //Console.BackgroundColor = col;
            Console.ResetColor();

            Console.WriteLine(err);
        }

        public static string InputPrompt(string prompt, string[]? hints = null) {
            string hintsStr = "";
            if (hints != null) {
                for (int i = 0; i < hints.Length; i++) {
                    if (i != 0) {
                        hintsStr += "/";
                    }
                    hintsStr += hints[i];
                }
            }

            Console.Write(prompt + " [" + hintsStr + "]: ");

            return Console.ReadLine() ?? "";
        }

        public static bool ConfirmDialog(string prompt, bool defaultRes = false)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Write(prompt + " [" + (defaultRes?"Y":"y") + "/" + (defaultRes?"n":"N") + "]: ");

                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Y:
                        exit = true;
                        defaultRes = true;
                        Console.WriteLine();
                        break;
                    case ConsoleKey.N:
                        exit = true;
                        defaultRes = false;
                        Console.WriteLine();
                        break;
                    case ConsoleKey.Enter:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine();
                        Console.Write("Illegal Input");
                        break;
                }
                Console.WriteLine();
            }
            return defaultRes;
        }

        public static int DockRight()
        {
            if (Console.WindowWidth <= Console.BufferWidth && MaxWidth > Console.WindowWidth)
            {
                return Console.WindowWidth;
            }
            else if (MaxWidth < Console.BufferWidth)
            {
                return MaxWidth;
            }
            else
            {
                return Console.BufferWidth;
            }
        }

        public static int DockBottom()
        {
            if (Console.WindowTop + Console.WindowHeight < Console.BufferHeight)
            {
                return Console.WindowTop + Console.WindowHeight - 1;
            }
            else
            {
                return Console.BufferHeight - 1;
            }
        }

        public static string FillString(string str, int count) {
            string res = str;
            if (res == null)
                res = "";
            while (res.Length < count) {
                res += " ";
            }
            return res;
        }
    }
}
