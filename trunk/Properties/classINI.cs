using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace KingsDamageMeter
{
    class classINI
    {
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string FileName);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string FileName);

        // using strReturn = cfgFile.GetIniValue( "SDM Setup", "systemovrdel", "sdm.cfg")
        // cfgFile.setIniValue( "SDM Setup", "systemovrdel", "0", "sdm.cfg")

        public string GetIniValue(string section, string key, string FileName)
        {
            if(!File.Exists(FileName)) throw new FileNotFoundException();

            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, FileName);
            return temp.ToString();
        }

        public void SetInIValue(string section, string key, string val, string FileName)
        {
            WritePrivateProfileString(section, key, val, FileName);
        }
    }
}
