using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.util
{
    public class Util
    {
        /// <summary>
        /// Returns the path to the current unit testing project. This method uses the path of the executing assembly and traverses upwards
        /// </summary>
        /// <returns></returns>
        public static string GetProjectDir()
        {
            string pathAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string folderAssembly = System.IO.Path.GetDirectoryName(pathAssembly);
            if (folderAssembly.EndsWith("\\") == false) folderAssembly = folderAssembly + "\\";
            string folderProjectLevel = System.IO.Path.GetFullPath(folderAssembly + "..\\..\\");
            Trace.WriteLine($"Project directory:{folderProjectLevel}");
            return folderProjectLevel;
        }

    }
}
