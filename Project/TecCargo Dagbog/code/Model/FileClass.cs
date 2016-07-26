using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace TecCargo_Dagbog.Model
{
    public class FileClass
    {
        #region Class

        public class Links
        {
            public string name = "";
            public string path = "";
            public bool isLink = false;
        }
        public class fileInput 
        {
            public string filename = "";
            public string name = "";
            public string cpr = "";
            public List<string> dato = new List<string>();
            public List<string> freeText = new List<string>();
            public List<List<Links>> files = new List<List<Links>>();
        }

        private class UpdateFileData
        {
            public string table = "";
            public string column = "";
            public string value = "";
        }
        public class General 
        {
            public string printer = "";
        }

        #endregion Class

        /// <summary>
        /// Opret/hent general indstiliger
        /// </summary>
        public static General GetGeneral()
        {
            if (!File.Exists("General\\General.xml"))
            {
                return new General();
            }

            General settings = new General();

            FileStream XMLRead = new FileStream("General\\General.xml", FileMode.Open, FileAccess.Read);
            DataSet readFil = new DataSet();
            readFil.ReadXml(XMLRead);

            settings.printer = readFil.Tables["general"].Rows[0]["printer"].ToString();

            return settings;
        }

        /// <summary>
        /// Gem general indsilinger
        /// </summary>
        public static void SaveGeneral()
        {
            General settings = Inc.Settings.Gerenal;

            FileStream XMLWrite = new FileStream("General\\General.xml", FileMode.Create, FileAccess.Write);
            DataSet createFil = new DataSet();

            createFil.Tables.Add("general");
            createFil.Tables["general"].Columns.Add("printer");

            createFil.Tables["general"].Rows.Add(settings.printer);

            createFil.WriteXml(XMLWrite);
        }

        /// <summary>
        /// hvis der kommer nye version som bruger 
        /// nogle andre dataset kan de blive addet her
        /// 
        /// lige bliver ikke brugt
        /// </summary>
        private DataSet UpdateFile(DataSet datafile, Version ver) 
        {
            List<UpdateFileData> ListUpdateFileData = new List<UpdateFileData>();

            /* example         
             *
            if (ver <= new Version("2.0")) {
                UpdateFileData ver2Filedata = new UpdateFileData();
                ver2Filedata.table = "table";
                ver2Filedata.column = "column";
                ver2Filedata.value = "default";
              
              ListUpdateFileData.add(ver2Filedata);
            }
             */
            foreach (var newUpdate in ListUpdateFileData)
            {
                if (!datafile.Tables.Contains(newUpdate.table))
                {
                    datafile.Tables.Add(newUpdate.table);
                }

                if (!datafile.Tables[newUpdate.table].Columns.Contains(newUpdate.column))
                {
                    datafile.Tables[newUpdate.table].Columns.Add(newUpdate.column);

                    for (int i = 0; i < datafile.Tables[newUpdate.table].Rows.Count; i++)
                    {
                        datafile.Tables[newUpdate.table].Rows[i][newUpdate.column] = newUpdate.value;
                    }
                }

            }

            return datafile;
        }

        /// <summary>
        /// Henter alle gemte dagbøger
        /// og gemmer dem i en liste så man hurtigt kan hente dem
        /// </summary>
        public List<fileInput> GetFiles()
        {
            //hvis mappen ikke findes
            if (!Directory.Exists("Saves")) 
            {
                return new List<fileInput>();
            }


            List<string> fileName = Directory.GetFiles("Saves").ToList(); //liste af alle fil navne
            List<fileInput> fileInputs = new List<fileInput>(); //new fil data list

            //for hver filnavn hent dets data
            foreach (var item in fileName)
            {

                FileStream XMLRead = new FileStream(item, FileMode.Open, FileAccess.Read);
                DataSet readFil = new DataSet();
                readFil.ReadXml(XMLRead);

                Version fileVersion = Version.Parse(readFil.Tables["general"].Rows[0]["version"].ToString());

                //gør det muligt at opdatere dataset så der ikke vil ske felj
                //ved nye versioner
                readFil = UpdateFile(readFil, fileVersion);

                string filename = item.Substring(6);
                fileInputs.Add(GetFileValues(readFil, filename)); //tilføj dagbog data
                
                XMLRead.Close();
            }


            return fileInputs;
        }

        /// <summary>
        /// læser dataset indhold og tilføjer dem
        /// til en class til dagbog data
        /// </summary>
        private fileInput GetFileValues(DataSet readxml, string filename)
        {
            fileInput fileData = new fileInput();
            fileData.filename = filename;


            if (readxml.Tables.Contains("indhold"))
            {
                fileData.name = readxml.Tables["indhold"].Rows[0]["navn"].ToString();
                fileData.cpr = readxml.Tables["indhold"].Rows[0]["cvr"].ToString();
            }

            if (readxml.Tables.Contains("log"))
            {

                for (int i = 0; i <  readxml.Tables["log"].Rows.Count; i++)
                {
                    fileData.dato.Add(readxml.Tables["log"].Rows[i]["date"].ToString());
                    fileData.freeText.Add(readxml.Tables["log"].Rows[i]["text"].ToString());
                }
            }

            if (readxml.Tables.Contains("files"))
            {
                for (int i = 0; i < readxml.Tables["files"].Rows.Count; i++)
                {
                    Model.FileClass.Links newLink = new Links();

                    int fileIndex = int.Parse(readxml.Tables["files"].Rows[i]["index"].ToString());
                    newLink.name = readxml.Tables["files"].Rows[i]["name"].ToString();
                    newLink.path = readxml.Tables["files"].Rows[i]["path"].ToString();
                    newLink.isLink = bool.Parse(readxml.Tables["files"].Rows[i]["isLink"].ToString());

                    for (int a = fileData.files.Count; a <= fileIndex; a++)
                    {
                        fileData.files.Add(new List<Links>());
                    }

                    fileData.files[fileIndex].Add(newLink);
                }
            }


            return fileData;
        }

        /// <summary>
        /// Gemmer dagbog data class til fil af xml type
        /// </summary>
        public void SaveFile()
        {

            if (!Directory.Exists("Saves"))
            {
                Directory.CreateDirectory("Saves");
            }
            fileInput input = Inc.Settings.fileInput;
            string fileName = input.filename;

            if (fileName == "")
            {

                int fileCount = Directory.GetFiles("Saves").Count();
                fileCount++;
                fileName = "saves-" + fileCount + ".xml";
                input.filename = fileName;
            }




            DataSet writeXml = new DataSet();


            #region createTemplate

            writeXml.Tables.Add("general");
            writeXml.Tables["general"].Columns.Add("version");

            writeXml.Tables.Add("indhold");
            writeXml.Tables["indhold"].Columns.Add("navn");
            writeXml.Tables["indhold"].Columns.Add("cvr");


            writeXml.Tables.Add("log");
            writeXml.Tables["log"].Columns.Add("date");
            writeXml.Tables["log"].Columns.Add("text");

            writeXml.Tables.Add("files");
            writeXml.Tables["files"].Columns.Add("index");
            writeXml.Tables["files"].Columns.Add("name");
            writeXml.Tables["files"].Columns.Add("path");
            writeXml.Tables["files"].Columns.Add("isLink");
            #endregion

            #region AddValues

            writeXml.Tables["general"].Rows.Add(Inc.Settings._version.ToString());

            writeXml.Tables["indhold"].Rows.Add(
                input.name,                                     //navn
                input.cpr                                       //cvr
            );

            for (int i = 0; i < input.freeText.Count; i++)
            {
                writeXml.Tables["log"].Rows.Add(
                input.dato[i],                                     //dato
                input.freeText[i]                                       //fri tekst
            );
            }

            for (int i = 0; i < input.files.Count; i++)
            {
                foreach (var item in input.files[i])
                {
                     writeXml.Tables["files"].Rows.Add(
                        i.ToString(),                               //index
                        item.name,                                  //name
                        item.path,                                  //path
                        item.isLink.ToString()                      //isLink
                    );
                }   
            }

            #endregion

            FileStream XMLCreate = new FileStream("Saves/" + fileName, FileMode.Create, FileAccess.Write);

            writeXml.WriteXml(XMLCreate, XmlWriteMode.WriteSchema);
            XMLCreate.Close();
        }


        public class function 
        {
            /// <summary>
            /// sorter efter dato
            /// </summary>
            public sortDateArray DateSort(Model.FileClass.fileInput fileInput)
            {
                List<DateTime> datetimeVal = new List<DateTime>();
                List<string> textVal = new List<string>();

                for (int i = 0; i < fileInput.dato.Count; i++)
                {
                    int index = datetimeVal.Count;
                    DateTime fileDateVal = new DateTime();
                    DateTime.TryParse(fileInput.dato[i], out fileDateVal);

                    for (int a = 0; a < datetimeVal.Count; a++)
                    {
                        if (datetimeVal[a].CompareTo(fileDateVal) > 0)
                        {
                            index = a;
                            break;
                        }
                    }


                    datetimeVal.Insert(index, fileDateVal);
                    textVal.Insert(index, fileInput.freeText[i]);
                }

                sortDateArray returnVal = new sortDateArray();
                returnVal.dato = datetimeVal;
                returnVal.text = textVal;

                return returnVal;
            }

            public class sortDateArray
            {
                public List<DateTime> dato = new List<DateTime>();
                public List<string> text = new List<string>();
            }
        }
    }
}
