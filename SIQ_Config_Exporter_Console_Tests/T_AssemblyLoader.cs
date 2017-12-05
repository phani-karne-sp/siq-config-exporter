using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiqConfigExport;
using System.Reflection;
using SiqConfigReport.Properties;

namespace UnitTestProject1
{
    /// <summary>
    /// This class tests basic finctionality of the assembily loader
    /// </summary>
    [TestClass]
    public class T_AssemblyLoader
    {
        /// <summary>
        /// Update this to point to the place where the libraries live in your test environment
        /// </summary>
        const string basePath = @"C:\Users\matt.shirilla\Documents\SourceCode\SiqLibs42p1\";

        //SOme test values
        const string Syncfusion_XlsIO_Base = "Syncfusion.XlsIO.Base";
        const string Syncfusion_Compression_Base = "Syncfusion.Compression.Base";
        const string Syncfusion_Core = "Syncfusion.Core";
        const string WBXReportingService = "WBXReportingService";
        AssemblyLoader assemblyLoader = null;

        public T_AssemblyLoader()
        {
            assemblyLoader = new AssemblyLoader();
        }

        
        /// <summary>
        /// This test will pass if we get an assembly from a given path
        /// </summary>
        [TestMethod]
        public void Test_Resolve_GoodPath()
        {
            Assembly assembly = null;
            assembly = assemblyLoader.Resolve(basePath, Syncfusion_XlsIO_Base);
            Assert.IsNotNull(assembly);

            //now try the same, but with .dll on the end
            assembly = null;
            assembly = assemblyLoader.Resolve(basePath, Syncfusion_Compression_Base + Settings.Default.dllAssemblyFileExtension);
            Assert.IsNotNull(assembly);
        }

        /// <summary>
        /// This test will pass if we get teh expected IO error due to the bad path
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void Test_Resolve_BadPath()
        {
            assemblyLoader.Resolve("C:\\", Syncfusion_XlsIO_Base);
        }

        /// <summary>
        /// This test will pass if the assembily resolves when a bad path is provided, and the assembly is
        /// in the default path.
        /// </summary>
        [TestMethod]
        public void Test_ResolveOnServer()
        {
            Assembly assembly = null;
            AssemblyLoader badpath_assemblyLoader = new AssemblyLoader();
            badpath_assemblyLoader.ResolveOnServer(Syncfusion_XlsIO_Base);
            Assert.IsNull(assembly);
        }
    }
}
