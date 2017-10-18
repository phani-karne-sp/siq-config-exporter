using System;
using WBX.Common.Utilities;

namespace SiqConfigExport
{
    /// <summary>
    /// This class generates a new connection string based on the Hibernate string passed and 
    /// decrypteds the password using the WBX-Nhibernate cetificate.  Developers must install
    /// an SIQ component on thier own system to use this method
    /// </summary>
    public class ConnStringer
    {
        private string hibernateConnectionString = null;
        private string generatedConnectionString = null;
        private ConsoleReader consoleReader = null;
        private string[] connArray = null;
        //To store the values that we will put into the connection string
        string dbuser = null;
        string dbpass = null;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The connection string that this class has created based on the Hibernate string and the user input
        /// </summary>
        public string GeneratedConnectionString
        {
            get
            {
                return generatedConnectionString;
            }
        }

        /// <summary>
        /// The original hibernate string, but with the cyphertext password shortened for easier display.
        /// </summary>
        public string HibernateConnectionString
        {
            get
            {
                return hibernateConnectionString.Remove(hibernateConnectionString.IndexOf("Password=")+9) + "********";
            }
        }


        /// <summary>
        /// Use this constructor
        /// </summary>
        /// <param name="hibernateConnectionString"></param>
        public ConnStringer(string hibernateConnectionString, ConsoleReader consoleReader)
        {

            this.hibernateConnectionString = hibernateConnectionString;
            this.consoleReader = consoleReader;
            connArray = hibernateConnectionString.Split(';');
            
        }

        /// <summary>
        /// Convert the nHibernate connection string to a usable connection string. User is prompted to decrypt the nHib connection string or 
        /// </summary>
        public void GenerateConnectionString(bool useHibernate)
        {
            if (useHibernate)
            {
                int passwordIndex = hibernateConnectionString.IndexOf("Password=");
                String encryptedPassword = hibernateConnectionString.Replace(";Pooling=true", "").Substring(passwordIndex + 9);
                byte[] encryptedPasswordBytes = Convert.FromBase64String(encryptedPassword);
                String decryptedPasword = RSAHelper.decryptStringPKCS7(encryptedPasswordBytes);
                generatedConnectionString = hibernateConnectionString.Replace(encryptedPassword, decryptedPasword);
            }
            else
            {
                GenerateConnectionStringFromUserInput();
            }
        }


        #region getUserInputs
        
        /// <summary>
        /// Get a connection string to use for the report using the Hibernate file and user input
        /// </summary>
        private void GenerateConnectionStringFromUserInput()
        {
            //Get the username from the coonection string so that we can use it as a default value
            dbuser = GetUsernameFromConnStr(hibernateConnectionString);

            //Get the username from the user
            dbuser = GetUsernameFromUser(dbuser);
            //Replace a single quote with 2 single-quotes, that is how we esacpe a single quote in a connection string
            dbpass = consoleReader.ReadPassword().Replace("'", "''");
            
            for (int i = 0; i < connArray.Length; i++)
            {
                if (!String.IsNullOrEmpty(dbpass) && connArray[i].ToLower().StartsWith("password="))
                    connArray[i] = "Password='" + dbpass + "'"; //wrap the password in single quotes
                if (!String.IsNullOrEmpty(dbuser) && connArray[i].ToLower().StartsWith("user="))
                    connArray[i] = "User=" + dbuser;
            }

            generatedConnectionString = String.Join(";", connArray);

            #region developerHack
            //TODO:update this hack that I put in for my own testing
            if(Environment.MachineName.ToLower().EndsWith("-xps"))
                generatedConnectionString = generatedConnectionString.Replace("I1,1433", "I1,31433").Replace(@"SQL1", @"localhost");
            #endregion developerHack
        }

        /// <summary>
        /// Get the username from the connection string so that we can show it to the user
        /// </summary>
        /// <param name="connString">The connection string</param>
        /// <returns>The username from the connection string or null if not found</returns>
        internal string GetUsernameFromConnStr(string connString)
        {
            string username = null;
            for (int i = 0; i < connArray.Length; i++)
            {
                if (connArray[i].ToLower().StartsWith("user="))
                    username = connArray[i].Replace("User=", "");
            }
            return username;
        }

        /// <summary>
        /// Prompt the user for the username to use for the connection.
        /// </summary>
        /// <param name="dbuser">The default username if the user does not enter one.</param>
        /// <returns></returns>
        internal string GetUsernameFromUser(string dbuser)
        {
            Console.Write("Enter Username[" + dbuser + "]:");
            string userInput = consoleReader.ReadLine();
            if (!String.IsNullOrEmpty(userInput))
            {
                dbuser = userInput;
            }
            return dbuser;
        }

        #endregion


        #region defaultConstructor
        /// <summary>
        /// Hide the default constructor
        /// </summary>
        private ConnStringer() { }
        #endregion


    }
}
