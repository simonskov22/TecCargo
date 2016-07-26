using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace TecCargo_Læreplads_DB.Model
{
    public class FileClass
    {
        #region class

        public class contactPerson
        {
            public string name = "";
            public string mobile = "";
            public string mail = "";
            public string post = "";
            public bool print = false;
        }
        public class uddannelsesCheckBoxs
        {
            public bool lager { get; set; }
            public bool lager_Logistik { get; set; }
            public bool lager_Transport { get; set; }

            public bool lastbil { get; set; }
            public bool lastbil_Gods { get; set; }
            public bool lastbil_Flytte { get; set; }
            public bool lastbil_Renovations { get; set; }
            public bool lastbil_Kran { get; set; }

            public bool lufthavn { get; set; }
            public bool lufthavn_Bagage { get; set; }
            public bool lufthavn_Cargo { get; set; }
            public bool lufthavn_Aircraft { get; set; }
            public bool lufthavn_Airport { get; set; }
            public bool lufthavn_Brand { get; set; }
            public bool lufthavn_Fuel { get; set; }
            public bool lufthavn_Cleaning { get; set; }
            public bool lufthavn_Ground { get; set; }
            public bool lufthavn_Rampe { get; set; }
        }
        public class agreementType
        {
            public bool alm { get; set; }
            public bool kombi { get; set; }
            public bool rest { get; set; }
            public bool newMaster { get; set; }
            public bool kort { get; set; }
            public bool del { get; set; }
        }
        public class files
        {
            public string name = "";
            public string path = "";
            public bool link = false;
        }
        public class firmReceive
        {
            public List<string> Checkbox = new List<string>();
            public string andet = "";

            public string antalAftaler = "";
            public string lastDay = "";
            public string antalPersoner = "";
            public string StartDate = "";

        }

        public class fileInput {

            public Version version = new Version();
            public string filename = "";
            public string name = "";
            public string adresse = "";
            public string tjenestested = "";
            public string cvr = "";
            public string profil = "";
            public List<contactPerson> contactPerson = new List<contactPerson>();
            public List<string> uddannelsesCheckBoxs = new List<string>();

            public string uddannelses_lager = "";
            public string uddannelses_chauffor = "";
            public string uddannelses_lufthavn = "";

            public List<string> agreementType = new List<string>();
            public firmReceive firmReceive = new firmReceive();
            public string jobDescription = "";
            public string age = "";
            public string driverLicense = "";
            public string physical = "";
            public List<string> record = new List<string>();
            public List<string> language = new List<string>();
            public string math = "";
            public string other = "";
            public List<string> candidates = new List<string>();
            public List<files> files = new List<files>();
            public string VUF = "";
            public string signedBy = "";
        }
        private class UpdateFileData
        {
            public string table = "";
            public string column = "";
            public string value = "";
        }

        #endregion class

        /// <summary>
        /// hvis der kommer nye version som bruger 
        /// nogle andre dataset kan de blive addet her
        /// 
        /// lige bliver ikke brugt
        /// </summary>
        private DataSet UpdateFile(DataSet datafile, Version fileVersion)
        {
            #region UpdateDataSet

            List<UpdateFileData> ListUpdateFileData = new List<UpdateFileData>();
            ListUpdateFileData.Add(new UpdateFileData { table = "kontaktPerson", column = "print", value = "False" });
            ListUpdateFileData.Add(new UpdateFileData { table = "andet", column = "adresse", value = "" });
            ListUpdateFileData.Add(new UpdateFileData { table = "andet", column = "andetTjenestested", value = "" });
            ListUpdateFileData.Add(new UpdateFileData { table = "andet", column = "uddannelses_lager", value = "" });
            ListUpdateFileData.Add(new UpdateFileData { table = "andet", column = "uddannelses_chauffor", value = "" });
            ListUpdateFileData.Add(new UpdateFileData { table = "andet", column = "uddannelses_lufthavn", value = "" });
            ListUpdateFileData.Add(new UpdateFileData { table = "FirmaØnsker", column = "checkbox", value = "" });
            ListUpdateFileData.Add(new UpdateFileData { table = "record", column = "value", value = "" });

            #endregion

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

            if (fileVersion <= new Version("1.5"))
            {
                string[][] firmCheckbox = { 
                                              new string[] {"motiveretAnsoegning","checkbox_motiveret"},
                                              new string[] {"cv","checkbox_cv"},
                                              new string[] {"perOpkald","checkbox_perTele"},
                                              new string[] {"perMoede","checkbox_perMeet"},
                                          };

                foreach (var item in firmCheckbox)
                {
                    bool checkboxIsCheck = bool.Parse(datafile.Tables["andet"].Rows[0][item[0]].ToString());
                    if (checkboxIsCheck)
                    {
                        datafile.Tables["FirmaØnsker"].Rows.Add(item[1]);
                    }
                }
            }

            if (fileVersion <= new Version("1.7"))
            {
                if (datafile.Tables.Contains("andet"))
                {
                    string record = datafile.Tables["andet"].Rows[0]["straffet"].ToString();
                    datafile.Tables["record"].Rows.Add(record);
                }
            }

            return datafile;
        }

        /// <summary>
        /// Henter alle gemte lærepladser
        /// og gemmer dem i en liste så man 
        /// hurtigt kan hente dem
        /// </summary>
        public List<fileInput> GetFiles() {

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

                fileInputs.Add(GetFilesValues(readFil, filename));

                XMLRead.Close();
            }


            return fileInputs;
        }

        /// <summary>
        /// læser dataset indhold og tilføjer dem
        /// til en class til data
        /// </summary>
        private fileInput GetFilesValues(DataSet readxml,string filename) 
        {
            fileInput fileData = new fileInput();
            fileData.filename = filename;

            if (readxml.Tables.Contains("kontaktPerson"))
            {

                for (int i = 0; i < readxml.Tables["kontaktPerson"].Rows.Count; i++)
                {
                    contactPerson contactPer = new contactPerson();

                    contactPer.name = readxml.Tables["kontaktPerson"].Rows[i]["navn"].ToString();
                    contactPer.mobile = readxml.Tables["kontaktPerson"].Rows[i]["mobil"].ToString();
                    contactPer.mail = readxml.Tables["kontaktPerson"].Rows[i]["email"].ToString();
                    contactPer.post = readxml.Tables["kontaktPerson"].Rows[i]["stilling"].ToString();
                    contactPer.print = bool.Parse(readxml.Tables["kontaktPerson"].Rows[i]["print"].ToString());

                    fileData.contactPerson.Add(contactPer);
                }
            }

            if (readxml.Tables.Contains("kandidater"))
            {
                for (int i = 0; i < readxml.Tables["kandidater"].Rows.Count; i++)
                {
                    fileData.candidates.Add(readxml.Tables["kandidater"].Rows[i]["navn"].ToString());
                }
            }

            if (readxml.Tables.Contains("uddannelsesaftale1"))
            {
                for (int i = 0; i < readxml.Tables["uddannelsesaftale1"].Rows.Count; i++)
                {
                    fileData.uddannelsesCheckBoxs.Add(readxml.Tables["uddannelsesaftale1"].Rows[i]["checkbox"].ToString());
                }
            }

            if (readxml.Tables.Contains("uddannelsesaftale2"))
            {
                for (int i = 0; i < readxml.Tables["uddannelsesaftale2"].Rows.Count; i++)
                {
                    fileData.agreementType.Add(readxml.Tables["uddannelsesaftale2"].Rows[i]["checkbox"].ToString());
                }
            }

            if (readxml.Tables.Contains("files"))
            {
                for (int i = 0; i < readxml.Tables["files"].Rows.Count; i++)
                {
                    files newFile = new files();

                    newFile.name = readxml.Tables["files"].Rows[i]["name"].ToString();
                    newFile.path = readxml.Tables["files"].Rows[i]["path"].ToString();
                    newFile.link = bool.Parse(readxml.Tables["files"].Rows[i]["link"].ToString());
                    
                    fileData.files.Add(newFile);
                }
            }
            
            if (readxml.Tables.Contains("sproglige"))
            {
                for (int i = 0; i < readxml.Tables["sproglige"].Rows.Count; i++)
                {
                    string value = readxml.Tables["sproglige"].Rows[i]["value"].ToString();
                    fileData.language.Add(value);
                }
            }

            if (readxml.Tables.Contains("record"))
            {
                for (int i = 0; i < readxml.Tables["record"].Rows.Count; i++)
                {
                    string value = readxml.Tables["record"].Rows[i]["value"].ToString();
                    fileData.record.Add(value);
                }
            }

            if (readxml.Tables.Contains("FirmaØnsker"))
            {
                for (int i = 0; i < readxml.Tables["FirmaØnsker"].Rows.Count; i++)
                {
                    string value = readxml.Tables["FirmaØnsker"].Rows[i]["checkbox"].ToString();
                    fileData.firmReceive.Checkbox.Add(value);
                }
            }

            if (readxml.Tables.Contains("andet"))
            {
                fileData.name = readxml.Tables["andet"].Rows[0]["virsomhed"].ToString();
                fileData.cvr = readxml.Tables["andet"].Rows[0]["CVR"].ToString();
                fileData.adresse = readxml.Tables["andet"].Rows[0]["adresse"].ToString();
                fileData.tjenestested = readxml.Tables["andet"].Rows[0]["andetTjenestested"].ToString();
                fileData.profil = readxml.Tables["andet"].Rows[0]["virsomhedProfil"].ToString();
                fileData.uddannelses_lager = readxml.Tables["andet"].Rows[0]["uddannelses_lager"].ToString();
                fileData.uddannelses_chauffor = readxml.Tables["andet"].Rows[0]["uddannelses_chauffor"].ToString();
                fileData.uddannelses_lufthavn = readxml.Tables["andet"].Rows[0]["uddannelses_lufthavn"].ToString();
                fileData.firmReceive.andet = readxml.Tables["andet"].Rows[0]["other"].ToString();
                fileData.firmReceive.antalAftaler = readxml.Tables["andet"].Rows[0]["antalAftaler"].ToString();
                fileData.firmReceive.lastDay = readxml.Tables["andet"].Rows[0]["datoFrist"].ToString();
                fileData.firmReceive.antalPersoner = readxml.Tables["andet"].Rows[0]["antalKandidater"].ToString();
                fileData.firmReceive.StartDate = readxml.Tables["andet"].Rows[0]["startDato"].ToString();
                fileData.jobDescription = readxml.Tables["andet"].Rows[0]["jobBeskrivelse"].ToString();
                fileData.age = readxml.Tables["andet"].Rows[0]["alder"].ToString();
                fileData.driverLicense = readxml.Tables["andet"].Rows[0]["koerekort"].ToString();
                fileData.physical = readxml.Tables["andet"].Rows[0]["fysiske"].ToString();
                fileData.math = readxml.Tables["andet"].Rows[0]["matematiske"].ToString();
                fileData.other = readxml.Tables["andet"].Rows[0]["bemaerkninger"].ToString();
                fileData.VUF = readxml.Tables["andet"].Rows[0]["VUF"].ToString();
                fileData.signedBy = readxml.Tables["andet"].Rows[0]["aftaleHvem"].ToString();
            }
            return fileData;
        }

        /// <summary>
        /// Gemmer lærepladsens data class til fil af xml type
        /// </summary>
        public void SaveFile() {

            if(!Directory.Exists("Saves")){

                Directory.CreateDirectory("Saves");
            }
            fileInput input = Inc.Settings.fileInput;
            string fileName = input.filename;

            if (fileName == "") {

                int fileCount = Directory.GetFiles("Saves").Count();
                
                do
                {
                    fileCount++;
                    fileName = "saves-" + fileCount + ".xml";
                } while (File.Exists(Directory.GetCurrentDirectory() + @"\Saves\" + fileName));

                input.filename = fileName;
            }

            


            DataSet writeXml = new DataSet();


            #region createTemplate

            writeXml.Tables.Add("general");
            writeXml.Tables["general"].Columns.Add("version");

            writeXml.Tables.Add("kontaktPerson");
            writeXml.Tables["kontaktPerson"].Columns.Add("navn");
            writeXml.Tables["kontaktPerson"].Columns.Add("mobil");
            writeXml.Tables["kontaktPerson"].Columns.Add("email");
            writeXml.Tables["kontaktPerson"].Columns.Add("stilling");
            writeXml.Tables["kontaktPerson"].Columns.Add("print");  //1.5

            writeXml.Tables.Add("kandidater");
            writeXml.Tables["kandidater"].Columns.Add("navn");

            writeXml.Tables.Add("uddannelsesaftale1");
            writeXml.Tables["uddannelsesaftale1"].Columns.Add("checkbox");

            writeXml.Tables.Add("uddannelsesaftale2");
            writeXml.Tables["uddannelsesaftale2"].Columns.Add("checkbox");

            writeXml.Tables.Add("FirmaØnsker"); //1.6
            writeXml.Tables["FirmaØnsker"].Columns.Add("checkbox");//1.6

            writeXml.Tables.Add("sproglige");
            writeXml.Tables["sproglige"].Columns.Add("value");


            writeXml.Tables.Add("record");
            writeXml.Tables["record"].Columns.Add("value");


            writeXml.Tables.Add("files");
            writeXml.Tables["files"].Columns.Add("name");
            writeXml.Tables["files"].Columns.Add("path");
            writeXml.Tables["files"].Columns.Add("link");

            writeXml.Tables.Add("andet");
            writeXml.Tables["andet"].Columns.Add("virsomhed");
            writeXml.Tables["andet"].Columns.Add("CVR");
            writeXml.Tables["andet"].Columns.Add("adresse"); //1.5
            writeXml.Tables["andet"].Columns.Add("andetTjenestested"); //1.5
            writeXml.Tables["andet"].Columns.Add("virsomhedProfil");
            
            writeXml.Tables["andet"].Columns.Add("uddannelses_lager"); //1.5
            writeXml.Tables["andet"].Columns.Add("uddannelses_chauffor"); //1.5
            writeXml.Tables["andet"].Columns.Add("uddannelses_lufthavn"); //1.5
            
            writeXml.Tables["andet"].Columns.Add("other");

            writeXml.Tables["andet"].Columns.Add("antalAftaler");
            writeXml.Tables["andet"].Columns.Add("datoFrist");
            writeXml.Tables["andet"].Columns.Add("antalKandidater");
            writeXml.Tables["andet"].Columns.Add("startDato");

            writeXml.Tables["andet"].Columns.Add("jobBeskrivelse");
            writeXml.Tables["andet"].Columns.Add("alder");
            writeXml.Tables["andet"].Columns.Add("straffet");

            writeXml.Tables["andet"].Columns.Add("koerekort");
            writeXml.Tables["andet"].Columns.Add("fysiske");
            writeXml.Tables["andet"].Columns.Add("matematiske");
            writeXml.Tables["andet"].Columns.Add("bemaerkninger");
            writeXml.Tables["andet"].Columns.Add("VUF");
            writeXml.Tables["andet"].Columns.Add("aftaleHvem");
            #endregion

            #region AddValues

            writeXml.Tables["general"].Rows.Add(Inc.Settings._version.ToString());



            foreach (var item in input.contactPerson)
            {
                writeXml.Tables["kontaktPerson"].Rows.Add(item.name,
                    item.mobile,
                    item.mail,
                    item.post,
                    item.print
                );
            }

            foreach (var item in input.candidates)
            {
                writeXml.Tables["kandidater"].Rows.Add(item);   
            }

            foreach (var item in input.uddannelsesCheckBoxs)
            {
                writeXml.Tables["uddannelsesaftale1"].Rows.Add(item);
            }

            foreach (var item in input.agreementType)
            {
                writeXml.Tables["uddannelsesaftale2"].Rows.Add(item);
            }

            foreach (var item in input.files)
            {
                writeXml.Tables["files"].Rows.Add(item.name, item.path, item.link.ToString());
            }

            foreach (var item in input.language)
            {
                writeXml.Tables["sproglige"].Rows.Add(item);
            }

            foreach (var item in input.record)
            {
                writeXml.Tables["record"].Rows.Add(item);
            }

            foreach (var item in input.firmReceive.Checkbox)
            {
                writeXml.Tables["FirmaØnsker"].Rows.Add(item);   
            }



            writeXml.Tables["andet"].Rows.Add(
                input.name,                                     //virsomhed
                input.cvr,                                      //CVR
                input.adresse,                                  //andresse
                input.tjenestested,                             //andetTjenestested
                input.profil,                                   //virsomhedProfil

                input.uddannelses_lager,                        //uddannelses_lager
                input.uddannelses_chauffor,                     //uddannelses_chauffor
                input.uddannelses_lufthavn,                     //uddannelses_ludthavn

                input.firmReceive.andet,                        //other
                input.firmReceive.antalAftaler,                 //antalAftaler
                input.firmReceive.lastDay,                      //datoFrist
                input.firmReceive.antalPersoner,                //antalKandidater
                input.firmReceive.StartDate,                    //startDato
                input.jobDescription,                           //jobBeskrivelse
                input.age,                                      //alder
                input.driverLicense,                            //koerekort
                input.physical,                                 //fysiske
                input.math,                                     //matematiske
                input.other,                                    //bemaerkninger
                input.VUF,                                      //VUF
                input.signedBy                                  //aftaleHvem
            );
            #endregion

            FileStream XMLCreate = new FileStream("Saves/" + fileName, FileMode.Create, FileAccess.Write);

            writeXml.WriteXml(XMLCreate, XmlWriteMode.WriteSchema);
            XMLCreate.Close();
        }

        #region general File

        public class General
        {
            public string printer = "";
        }


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
        public static void SaveGeneral()
        {
            General settings = Inc.Settings.Gerenal; ;

            FileStream XMLWrite = new FileStream("General\\General.xml", FileMode.Create, FileAccess.Write);
            DataSet createFil = new DataSet();

            createFil.Tables.Add("general");
            createFil.Tables["general"].Columns.Add("printer");

            createFil.Tables["general"].Rows.Add(settings.printer);

            createFil.WriteXml(XMLWrite);
        }


        #endregion

    }
}
