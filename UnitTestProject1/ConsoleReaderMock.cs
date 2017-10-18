using SiqConfigExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    class ConsoleReaderMock : ConsoleReader
    {
        string readlineString = null;
        string readlineStringPassword = null;

        /// <summary>
        /// Use this constructor for tests that mock where the user is not entering any input
        /// </summary>
        public ConsoleReaderMock()
        {

        }

        /// <summary>
        /// A mockup of the ConsoleReader class is for unit testing.
        /// </summary>
        /// <param name="readlineString">The string mockup of user console entry.</param>
        /// /// <param name="readlineStringPassword">The string mockup of user console entry for the password.</param>
        public ConsoleReaderMock(string readlineString, string readlineStringPassword)
        {
            this.readlineString = readlineString;
            this.readlineStringPassword = readlineStringPassword;
        }

        public string ReadlineReturnString
        {
            get
            {
                return readlineString;
            }
        }

        public string ReadlineReturnStringPassword
        {
            get
            {
                return readlineStringPassword;
            }
        }

        /// <summary>
        /// Fake a console readline
        /// </summary>
        /// <returns>ReadlineReturnString is always returned</returns>
        public override string ReadLine()
        {
            return ReadlineReturnString;
        }

        /// <summary>
        /// Fake a Console.ReadPassword readline
        /// </summary>
        /// <returns>ReadlineReturnString is always returned</returns>
        public override string ReadPassword()
        {
            return ReadlineReturnStringPassword;
        }
    }
}
