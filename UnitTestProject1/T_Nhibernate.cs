using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using System;
using WBX.Common.Utilities;

namespace UnitTestProject1
{
    [TestClass]
    public class T_Nhibernate
    {

        const string basePath =      @"C:\Users\matt.shirilla\Documents\SourceCode\SiqLibs42p1\";

        public T_Nhibernate()
        {
        }

        [TestMethod]
        public void NHibernateTest()
        {

            //string filename = @"C:\Users\matt.shirilla\Documents\Visual Studio 2015\Projects\SIQ_Install_Report\ConsoleApplication1\resources\hibernate.cfg.xml";
            //TODO Developer hack to get a hibernate file.  Update for your test environment
            string filename = @"C:\Users\matt.shirilla\Documents\Visual Studio 2015\Projects\SIQ_Install_Report_5.0\ConsoleApplication1\resources\hibernate.cfg.xml";

            Configuration config = new Configuration().Configure(filename);
            string connectionString = null;
            // Get the connection string from the NHibernate.config.xml
            connectionString = config.GetProperty(global::NHibernate.Cfg.Environment.ConnectionString);
            connectionString = connectionString.Replace("\r\n", "").Trim();
            string[] splittedConnectionString = connectionString.Split(';');
            connectionString = string.Empty;


            // Split the connection string and go over the it's properties
            foreach (string property in splittedConnectionString)
            {
                if (property.StartsWith("password", StringComparison.CurrentCultureIgnoreCase))
                {
                    string ttt = property.Replace("Password=", "");
                    byte[] pkcs7ToDecrypt = System.Convert.FromBase64String(ttt);
                    string result = RSAHelper.decryptStringPKCS7(pkcs7ToDecrypt);

                }

            }
            
        }

    }
}
