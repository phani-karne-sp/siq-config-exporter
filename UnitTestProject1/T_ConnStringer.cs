using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;
using UnitTestProject1;

namespace SiqConfigExport
{
    /// <summary>
    /// This class tests 
    /// </summary>
    [TestClass]
    public class T_ConnStringer
    {

        string cnStr_defaultUser = "Server=SQL1\\I1,1433;initial catalog=SecurityIQDB;User=SecurityIQ_User;Password=7LA==;Pooling=true";
        string cnStr_customUser = "Server=SQL1\\I1,1433;initial catalog=SecurityIQDB;User=customUser;Password=7LA==;Pooling=true";
        string defaultPassword = "defaultPassword";
        string securityiq_custom_user = "customUser";
        string securityiq_default_user = "SecurityIQ_User";
        string securityiq_custom_pass = "customPass";
        
        //TODO - ad these accounts to SQL Server as part of the test setup.  FOr now the tester has to do this manually
        string loginHasSemiColonPassword = "loginHasSemiColonPassword";
        string passwordWithSemiColon = "Pass;word123";
        string loginHasSingleQuotePassword = "LoginHasSingleQuotePassword";
        string passwordWithSingleQuote = "Pass'word123";
        string loginHasDblQuotePassword = "LoginHasDblQuotePassword";
        string passwordWithDoubleQuote = @"Pass" + '\u0022' + "word123";

        
        /// <summary>
        /// Tests to be sure that the password is surrounded by single quotes, single quotes are escaped, etc
        /// </summary>
        [TestMethod]
        public void Test_GetPasswordFromUser_TestSpecialChars()
        {
            //Test passwords that have a semicolon
            bool cnWasOpened = false;
            global::SiqConfigExport.ConnStringer connStringer = new ConnStringer(cnStr_defaultUser, new ConsoleReaderMock(loginHasSemiColonPassword, passwordWithSemiColon));
            connStringer.GenerateConnectionString(false);
            using (SqlConnection sqlCn = new SqlConnection(connStringer.GeneratedConnectionString))
            {
                sqlCn.Open();
                if (sqlCn.State == ConnectionState.Open)
                    cnWasOpened = true;
            }
            Assert.IsTrue(cnWasOpened, "The connection should have opend successfully for this test to pass");

            //Test passwords that have a single quote
            cnWasOpened = false;
            connStringer = new ConnStringer(cnStr_defaultUser, new ConsoleReaderMock(loginHasSingleQuotePassword, passwordWithSingleQuote));
            connStringer.GenerateConnectionString(false);
            using (SqlConnection sqlCn = new SqlConnection(connStringer.GeneratedConnectionString))
            {
                sqlCn.Open();
                if (sqlCn.State == ConnectionState.Open)
                    cnWasOpened = true;
            }
            Assert.IsTrue(cnWasOpened, "The connection should have opend successfully for this test to pass");
            
        }

        /// .NET always adds an escape \ to my double quote when I create the string, no matter what I try (even tried unicode). So 
        /// the wrong password is generated.  That's okay, because all we really want to test is that the conn string is not 
        /// mangled by a double-quote in a password.  Expect a sql exception
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void Test_GetPasswordFromUser_TestDoubleQuote()
        {
            //Test passwords that have a doublequote
            bool cnWasOpened = false;
            global::SiqConfigExport.ConnStringer connStringer = new ConnStringer(cnStr_defaultUser, new ConsoleReaderMock(loginHasDblQuotePassword, passwordWithDoubleQuote));
            connStringer.GenerateConnectionString(false);
            using (SqlConnection sqlCn = new SqlConnection(connStringer.GeneratedConnectionString))
            {
                sqlCn.Open();
                if (sqlCn.State == ConnectionState.Open)
                    cnWasOpened = true;
            }
            Assert.IsTrue(cnWasOpened, "The connection should have opend successfully for this test to pass");
        }



        [TestMethod]
        public void Test_GetUsernameFromUser()
        {
            global::SiqConfigExport.ConnStringer connStringer = new ConnStringer(cnStr_defaultUser, new ConsoleReaderMock("someuser", "somePass"));
            string username = connStringer.GetUsernameFromUser(defaultPassword);
            Assert.AreEqual(username, "someuser", true, "The resulting username should match the user-provided string");

            connStringer = new ConnStringer(cnStr_defaultUser, new ConsoleReaderMock(null, null));
            username = connStringer.GetUsernameFromUser(defaultPassword);
            Assert.AreEqual(username, defaultPassword, true, "The user had no input, therefore we should get the default password back.");

        }

        [TestMethod]
        public void Test_GenerateConnectionStringFromAllInputs()
        {
            
            global::SiqConfigExport.ConnStringer connStringer = new ConnStringer(cnStr_defaultUser, new ConsoleReaderMock(securityiq_custom_user, securityiq_custom_pass));
            connStringer.GenerateConnectionString(false);

            Assert.IsTrue(
                connStringer.GeneratedConnectionString.Contains(securityiq_custom_user),
                "The user supplied username should be in the connection string.");
            Assert.IsTrue(connStringer.GeneratedConnectionString.Contains(securityiq_custom_pass),
                "The user supplied password should be in the connection string.");

        }

        /// <summary>
        /// Test getting the username from the cn str.  Does not test the user input.
        /// </summary>
        [TestMethod]
        public void Test_GetUsernameFromConnStr()
        {
            global::SiqConfigExport.ConnStringer connStringer = new ConnStringer(cnStr_defaultUser, new ConsoleReaderMock());
            string username = connStringer.GetUsernameFromConnStr(cnStr_defaultUser);
            Assert.AreEqual(username, securityiq_default_user, true, "The username should match 'default' SecurityIQ user in the cn string");

            connStringer = new ConnStringer(cnStr_customUser, new ConsoleReaderMock());
            username = connStringer.GetUsernameFromConnStr(cnStr_customUser);
            Assert.AreEqual(username, securityiq_custom_user, true, "The username should match the custom user in the cn string");
        }

    }
}
