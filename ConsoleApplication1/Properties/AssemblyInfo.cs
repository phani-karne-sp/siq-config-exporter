using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SecurityIQ Configurtion Report Utility")]
[assembly: AssemblyDescription("This utility exports the SecurityIQ configuration to an Excel spreadsheet.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Sailpoint Technologies, Inc. All Rights Reserved.")]
[assembly: AssemblyProduct("SecurityIQ Configurtion Report Utility")]
[assembly: AssemblyCopyright("Copyright© 2016 SailPoint Technologies")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

//We must use this tag to be sure the unit tests have access to the internal methods
[assembly: InternalsVisibleTo("UnitTestProject1")]

//Tells log4j to look in default app file.
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("f93b8178-8668-4a28-89ae-198bdde61a0d")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("5.1.0.0")]
[assembly: AssemblyFileVersion("5.1.0.0")]
