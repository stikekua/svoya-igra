using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvoyaIgra.TestClient
{
    internal class Ui
    {
        public static void Clear()
        {
            Console.Clear();
        }
        public static void Write()
        {
            Console.WriteLine();
        }
        public static void Write(string text)
        {
            Console.WriteLine(text);
        }
        public static void WriteError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Write(text);
            Console.ResetColor();
        }
        public static void WriteWarning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Write(text);
            Console.ResetColor();
        }
        public static int Choice(int range)
        {
            do
            {
                System.Console.WriteLine("Your choice: ");
                var selection = System.Console.ReadLine();
                if (int.TryParse(selection, out var choice) && choice >= 0 && choice <= range)
                {
                    return choice;
                }
            } while (true);
        }
        public static void PressKey()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue .....");
            Console.ReadLine();
        }

        public static string Read(string text)
        {
            Console.Write($"{text}: ");
            return Console.ReadLine();
        }

        public static int ReadInt(string text)
        {
            do
            {
                Console.Write($"{text}: ");
                var str = Console.ReadLine();
                if (int.TryParse(str, out var ret))
                {
                    return ret;
                }
                WriteError("Error: Not a numeric format. Try again.");
            } while (true);
        }
        public static int ReadInt(string text, int[] allowedValues)
        {
            do
            {
                Console.Write($"{text}: ");
                var str = Console.ReadLine();
                if (int.TryParse(str, out var ret))
                {
                    if(allowedValues.Contains(ret)) return ret;

                    var msg = "";
                    foreach (var allowedValue in allowedValues)
                    {
                        msg += allowedValue;
                        msg += " ";
                    } 
                    WriteWarning($"Allowed values: {msg}. Try again.");
                }
                else
                {
                    WriteError("Error: Not a numeric format. Try again.");
                }
            } while (true);
        }

        public static string ReadString(string text, string[] allowedValues)
        {
            do
            {
                Console.Write($"{text}: ");
                var str = Console.ReadLine();
                if (allowedValues.Contains(str)) 
                    return str;
                else 
                    WriteWarning($"Invalid file name. Try again.");
            } while (true);
        }

        public static void Error(string text)
        {
            WriteError(text);
            PressKey();
        }
    }
}
