using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiqConfigExport
{
    public class ConsoleReader
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// A simple wrapper around ReadLine.  Encapsulation the Console.ReadLine makes things more testable
        /// </summary>
        /// <returns></returns>
        public virtual string ReadLine()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// Get the user input in a masked form.  If the user enters nothing then return the default password.
        /// </summary>
        /// <returns></returns>
        //public virtual string ReadPassword()
        //{
        //    Console.Write("Enter Password:");
        //    string password = "";
        //    ConsoleKeyInfo info = Console.ReadKey(true);
        //    while (info.Key != ConsoleKey.Enter)
        //    {
        //        if (info.Key != ConsoleKey.Backspace)
        //        {
        //            Console.Write("*");
        //            password += info.KeyChar;
        //        }
        //        else if (info.Key == ConsoleKey.Backspace)
        //        {
        //            if (!string.IsNullOrEmpty(password))
        //            {
        //                // remove one character from the list of password characters
        //                password = password.Substring(0, password.Length - 1);
        //                // get the location of the cursor
        //                int pos = Console.CursorLeft;
        //                // move the cursor to the left by one character
        //                Console.SetCursorPosition(pos - 1, Console.CursorTop);
        //                // replace it with space
        //                Console.Write(" ");
        //                // move the cursor to the left by one character again
        //                Console.SetCursorPosition(pos - 1, Console.CursorTop);
        //            }
        //        }
        //        info = Console.ReadKey(true);
        //    }
        //    if (String.IsNullOrEmpty(password))
        //    {
        //        password  = "Sailpoint1!";
        //    }
        //    Console.WriteLine();
        //    return password;
        //}



        /// <summary>
        /// Call this when the work is done due to success or error.
        /// </summary>
        /// <param name="str">A message to dosplay in the console</param>
        public static void PauseAndExit(string str)
        {
            Console.WriteLine(str);
            Console.ReadLine();
        }

    }

}
