using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecCargo_Dagbog.Inc
{
    class Settings
    {

        public static Version _version = new Version("1.8");
        public static string CreateBy = "Simon Skov";
        public static Model.FileClass.General Gerenal = Model.FileClass.GetGeneral();

        public static Model.FileClass.fileInput fileInput = new Model.FileClass.fileInput();
    }
}
