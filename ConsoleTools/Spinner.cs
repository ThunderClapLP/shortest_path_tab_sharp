using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathTAB.ConsoleTools
{
    internal class Spinner
    {
        public int left;
        public int top;

        public bool dockRight = true;

        private int state = 0;
        private char curr = '/';

        public long lastUpdate = 0;
        public int minSpinnerTime = 50;

        public bool result = false;
        public bool success = false;
        public bool finished = false;

        public Spinner(int left, int top)
        {
            this.left = left;
            this.top = top;

            dockRight = false;
        }

        public Spinner(int top)
        {
            this.left = ConsoleTools.DockRight();
            this.top = top;

            dockRight = true;
        }

        public void Update()
        {
            if (lastUpdate > Environment.TickCount64 - minSpinnerTime && !result || finished)
                return;
            lastUpdate = Environment.TickCount64;

            switch (state)
            {
                case 0:
                    curr = '-';
                    break;
                case 1:
                    curr = '\\';
                    break;
                case 2:
                    curr = '|';
                    break;
                case 3:
                    curr = '/';
                    state = -1;
                    break;
                default:
                    state = -1;
                    break;
            }
            state++;

            Draw();
        }

        public void Draw()
        {
            bool prvVisible = OperatingSystem.IsWindows() ? Console.CursorVisible : true;
            int prvLeft = Console.CursorLeft;
            int prvTop = Console.CursorTop;

            Console.CursorVisible = false;
            Console.CursorLeft = dockRight ? ConsoleTools.DockRight() - 1 : left;
            Console.CursorTop = top;


            if (!result)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(curr);
                Console.ResetColor();
            } else
            {
                if (success)
                {
                    Console.CursorLeft = left - 4;
                    Console.Write("  ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("OK");
                }
                else
                {
                    Console.CursorLeft = left - 8;
                    Console.Write("  ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("FAILED");
                }

                //Console.BackgroundColor=col;
                Console.ResetColor();
                finished = true;
            }

            Console.CursorLeft = prvLeft;
            Console.CursorTop = prvTop;
            Console.CursorVisible = prvVisible;

        }

        public void WriteResult(bool success)
        {
            result = true;
            this.success = success;
        }

        public void Clean()
        {
            finished = false;
            char prv = curr;
            curr = ' ';
            Draw();
            curr = prv;
        }
    }
}
