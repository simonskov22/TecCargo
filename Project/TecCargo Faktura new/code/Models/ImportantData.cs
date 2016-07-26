using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TecCargo_Faktura.Models
{
    public class ImportantData
    {
        public static Version PVersion = new Version("2.5.0");
        public static string PAuthor = "Simon Skov";

        public static bool FileIsSaved = true;
        public static bool LoadFormFile = false;
        public static string Filename;

        public static string fragtBrevNumber;
        public static bool openStartMenuOnClose;

        public static string g_FolderPdf = Directory.GetCurrentDirectory() + @"\Pdf\";
        public static string g_FolderSave = Directory.GetCurrentDirectory() + @"\Gemte filer\";
        public static string g_FolderData = Directory.GetCurrentDirectory() + @"\Data\";
        public static string g_FolderDB = Directory.GetCurrentDirectory() + @"\Database\";

        //admin Login
        public static string AdminUsername = "Henrik";
        public static string AdminPassword = "henrikCargo2000";
        public static string Password = "5udRaWru";

        //settings
        //public static bool UsePdfBold = false;


        public static string closeFragtbrevText;
        public static bool closeFragtbrevBool;

        public static string closeFakturaText;
        public static bool closeFakturaBool;
    }
}
