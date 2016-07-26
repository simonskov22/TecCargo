using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace TecCargo_Faktura.Class
{
    class XML_Files
    {
        public class DocData {

            public class Layout {
                
                public class AdresseInfo
                {
                    public bool isActive = true;
                    public bool isPayer = false;
                    public string telefon = "";
                    public string kontaktPerson = "";
                    public string firma = "";
                    public string vej = "";
                    public string zipCode = "";
                    public string city = "";
                }
                public class GenereltInfo
                {
                    public string reference = "";
                    public string fragtmand = "";
                    public DateTime datoRute1 = DateTime.Now;
                    public bool isSetDatoRute1 = false;
                    public string zipRute1 = "";
                    public DateTime datoRute2 = DateTime.Now;
                    public bool isSetDatoRute2 = false;
                    public string zipRute2 = "";
                    public int forsikringstype = -1;
                }

                public class ByttepallerInfo
                {
                    public bool useByttepalle = false;
                    public string palle1_1 = "";
                    public string palle1_2 = "";
                    public string palle1_4 = "";
                }
                public class EfterkravInfo
                {
                    public bool useEfterkrav = false;
                    public string efterkravGebyr = "";
                    public string forsikringssum = "";
                    public string premium = "";
                    public string total = "";
                }
                public class TransportInfo
                {
                    public int[] transportType = { -1, -1, -1 };
                    public List<Controls.PakkeControlItem> pakker = new List<Controls.PakkeControlItem>();
                }
                public class LukFragtbrevInfo
                {
                    public DateTime leveringsdato = DateTime.Now;
                    public bool isSetLeveringsdato = false;
                    public string rabat = "";
                    public string kilometer = "";
                    public string tidsforbrug1 = "";
                    public string tidsforbrug2 = "";
                    public string tidsforbrug3 = "";
                    public string tidsforbrug4 = "";
                    public string kommentar = "";
                }
                public class EkstraGebyrInfo
                {
                    public bool[] buttonsIsCheck = {
                        false, false, false,
                        false, false, false,
                        false, false, false,
                        false, false, false
                    };
                            public string[] textboxsValues = {
                        "","","","","","",""
                    };
                }

                public class DocumentData
                {
                    public int Invoice = -1;
                    public List<string> Initialer = new List<string>();

                    public AdresseInfo Afsender = new AdresseInfo();
                    public AdresseInfo Modtager = new AdresseInfo();
                    public AdresseInfo Modtager2 = new AdresseInfo();

                    public GenereltInfo Generelt = new GenereltInfo();
                    public ByttepallerInfo Byttepaller = new ByttepallerInfo();
                    public EfterkravInfo Efterkrav = new EfterkravInfo();

                    public TransportInfo Transport = new TransportInfo();
                    public string FragtbrevNotifications = "";

                    public LukFragtbrevInfo LukFragtbrev = new LukFragtbrevInfo();

                    public EkstraGebyrInfo EkstraGebyr = new EkstraGebyrInfo();


                    public string FakturaNotifications = "";
                    public string FakturaKommentar = "";
                }

            }

            private int GetNewInvoice()
            {
                int invoice = 1000;

                string[] FilesNames = Directory.GetFiles(Models.ImportantData.g_FolderSave).Select(item => Path.GetFileName(item)).ToArray();

                for (int i = 0; i < FilesNames.Length; i++)
                {
                    if (!FilesNames[i].StartsWith("document-"))
                        continue;
                                        
                    int fileInvoice = int.Parse(FilesNames[i].Replace("document-", "").Replace(".xml",""));

                    if (fileInvoice >= invoice)
                        invoice = fileInvoice + 1;
                }
                
                return invoice;
            }

            private DataSet CreateSaveFileLayout(DataSet file) {

                string[][] datasetVal = {
                    new string[] { "Indstillinger", "Invoice" },
                    new string[] { "Indstillinger", "Initialer" },
                    new string[] { "Afsender", "Telefon" },
                    new string[] { "Afsender", "Firma" },
                    new string[] { "Afsender", "KontaktPerson" },
                    new string[] { "Afsender", "Vej" },
                    new string[] { "Afsender", "ZipCode" },
                    new string[] { "Afsender", "IsPayer" },
                    new string[] { "Modtager", "Telefon" },
                    new string[] { "Modtager", "Firma" },
                    new string[] { "Modtager", "KontaktPerson" },
                    new string[] { "Modtager", "Vej" },
                    new string[] { "Modtager", "ZipCode" },
                    new string[] { "Modtager", "IsPayer" },
                    new string[] { "Modtager2", "IsActive" },
                    new string[] { "Modtager2", "Telefon" },
                    new string[] { "Modtager2", "Firma" },
                    new string[] { "Modtager2", "KontaktPerson" },
                    new string[] { "Modtager2", "Vej" },
                    new string[] { "Modtager2", "ZipCode" },
                    new string[] { "Modtager2", "IsPayer" },
                    new string[] { "Generelt", "Reference" },
                    new string[] { "Generelt", "Fragtmand" },
                    new string[] { "Generelt", "Forsikringstype" },
                    new string[] { "Generelt", "DatoRute1" },
                    new string[] { "Generelt", "IsSetDatoRute1" },
                    new string[] { "Generelt", "ZipRute1" },
                    new string[] { "Generelt", "DatoRute2" },
                    new string[] { "Generelt", "IsSetDatoRute2" },
                    new string[] { "Generelt", "ZipRute2" },
                    new string[] { "Byttepaller", "UseByttepalle" },
                    new string[] { "Byttepaller", "Palle1_1" },
                    new string[] { "Byttepaller", "Palle1_2" },
                    new string[] { "Byttepaller", "Palle1_4" },
                    new string[] { "Efterkrav", "UseEfterkrav" },
                    new string[] { "Efterkrav", "EfterkravGebyr" },
                    new string[] { "Efterkrav", "Forsikringssum" },
                    new string[] { "Efterkrav", "Premium" },
                    new string[] { "Efterkrav", "Total" },
                    new string[] { "TransportType", "Val1" },
                    new string[] { "TransportType", "Val2" },
                    new string[] { "TransportType", "Val3" },
                    new string[] { "TransportData", "IsDoneFragt" },
                    new string[] { "TransportData", "MrkNumb" },
                    new string[] { "TransportData", "Contains" },
                    new string[] { "TransportData", "CountI" },
                    new string[] { "TransportData", "CountS" },
                    new string[] { "TransportData", "WeightD" },
                    new string[] { "TransportData", "WeightS" },
                    new string[] { "TransportData", "Volume" },
                    new string[] { "TransportData", "ArtId" },
                    new string[] { "TransportData", "ArtName" },
                    new string[] { "TransportData", "TransportTypeId" },
                    new string[] { "TransportData", "TransportTypeName" },
                    new string[] { "TransportDataFaktura", "Index" },
                    new string[] { "TransportDataFaktura", "IsDoneFaktura" },
                    new string[] { "TransportDataFaktura", "TakstId" },
                    new string[] { "TransportDataFaktura", "TakstName" },
                    new string[] { "TransportDataFaktura", "BeregningstypeId" },
                    new string[] { "TransportDataFaktura", "BeregningstypeName" },
                    new string[] { "LukFragtbrev", "Leveringsdato" },
                    new string[] { "LukFragtbrev", "IsSetLeveringsdato" },
                    new string[] { "LukFragtbrev", "Rabat" },
                    new string[] { "LukFragtbrev", "Kilometer" },
                    new string[] { "LukFragtbrev", "Tidsforbrug1" },
                    new string[] { "LukFragtbrev", "Tidsforbrug2" },
                    new string[] { "LukFragtbrev", "Tidsforbrug3" },
                    new string[] { "LukFragtbrev", "Tidsforbrug4" },
                    new string[] { "LukFragtbrev", "Kommentar" },
                    new string[] { "EkstraGebyrButtons", "Button_0" },
                    new string[] { "EkstraGebyrButtons", "Button_1" },
                    new string[] { "EkstraGebyrButtons", "Button_2" },
                    new string[] { "EkstraGebyrButtons", "Button_3" },
                    new string[] { "EkstraGebyrButtons", "Button_4" },
                    new string[] { "EkstraGebyrButtons", "Button_5" },
                    new string[] { "EkstraGebyrButtons", "Button_6" },
                    new string[] { "EkstraGebyrButtons", "Button_7" },
                    new string[] { "EkstraGebyrButtons", "Button_8" },
                    new string[] { "EkstraGebyrButtons", "Button_9" },
                    new string[] { "EkstraGebyrButtons", "Button_10" },
                    new string[] { "EkstraGebyrButtons", "Button_11" },
                    new string[] { "EkstraGebyrTextboxs", "Text_0" },
                    new string[] { "EkstraGebyrTextboxs", "Text_1" },
                    new string[] { "EkstraGebyrTextboxs", "Text_2" },
                    new string[] { "EkstraGebyrTextboxs", "Text_3" },
                    new string[] { "EkstraGebyrTextboxs", "Text_4" },
                    new string[] { "EkstraGebyrTextboxs", "Text_5" },
                    new string[] { "EkstraGebyrTextboxs", "Text_6" },
                    new string[] { "andet", "FragtbrevNotifications" },
                    new string[] { "andet", "FakturaNotifications" },
                    new string[] { "andet", "FakturaKommentar" }
                };

                //hvis værdien ikke findes tilføj den
                foreach (var data in datasetVal)
                {
                    if (!file.Tables.Contains(data[0]))
                        file.Tables.Add(data[0]);

                    if (!file.Tables[data[0]].Columns.Contains(data[1]))
                        file.Tables[data[0]].Columns.Add(data[1]);
                }


                return file;
            }

            public void SaveFile(Layout.DocumentData docData)
            {
                //hvis det er en helt ny dokument
                //hent sidste ubrugt invoice id
                if (docData.Invoice == -1)
                    docData.Invoice = GetNewInvoice();

                DataSet file = CreateSaveFileLayout(new DataSet());

                string initialer = "";
                foreach (var item in docData.Initialer)
                    initialer += item + ",";

                file.Tables["Indstillinger"].Rows.Add(new string[] {
                    docData.Invoice.ToString(),
                    initialer
                });

                #region Adresse Info

                file.Tables["Afsender"].Rows.Add(new string[] {
                    docData.Afsender.telefon,
                    docData.Afsender.firma,
                    docData.Afsender.kontaktPerson,
                    docData.Afsender.vej,
                    docData.Afsender.zipCode,
                    docData.Afsender.isPayer.ToString()
                });
                file.Tables["Modtager"].Rows.Add(new string[] {
                    docData.Modtager.telefon,
                    docData.Modtager.firma,
                    docData.Modtager.kontaktPerson,
                    docData.Modtager.vej,
                    docData.Modtager.zipCode,
                    docData.Modtager.isPayer.ToString()
                });
                file.Tables["Modtager2"].Rows.Add(new string[] {
                    docData.Modtager2.isActive.ToString(),
                    docData.Modtager2.telefon,
                    docData.Modtager2.firma,
                    docData.Modtager2.kontaktPerson,
                    docData.Modtager2.vej,
                    docData.Modtager2.zipCode,
                    docData.Modtager2.isPayer.ToString()
                });
                #endregion

                #region Fragtbrev Generelt

                file.Tables["Generelt"].Rows.Add(new string[] {
                    docData.Generelt.reference,
                    docData.Generelt.fragtmand,
                    docData.Generelt.forsikringstype.ToString(),
                    docData.Generelt.datoRute1.ToString(),
                    docData.Generelt.isSetDatoRute1.ToString(),
                    docData.Generelt.zipRute1,
                    docData.Generelt.datoRute2.ToString(),
                    docData.Generelt.isSetDatoRute2.ToString(),
                    docData.Generelt.zipRute2
                });
                file.Tables["Byttepaller"].Rows.Add(new string[] {
                    docData.Byttepaller.useByttepalle.ToString(),
                    docData.Byttepaller.palle1_1,
                    docData.Byttepaller.palle1_2,
                    docData.Byttepaller.palle1_4
                });
                file.Tables["Efterkrav"].Rows.Add(new string[] {
                    docData.Efterkrav.useEfterkrav.ToString(),
                    docData.Efterkrav.efterkravGebyr,
                    docData.Efterkrav.forsikringssum,
                    docData.Efterkrav.premium,
                    docData.Efterkrav.total
                });
                #endregion

                #region Transport

                file.Tables["TransportType"].Rows.Add(new string[] {
                    docData.Transport.transportType[0].ToString(),
                    docData.Transport.transportType[1].ToString(),
                    docData.Transport.transportType[2].ToString()
                });
                for (int i = 0; i < docData.Transport.pakker.Count; i++)
                {
                    file.Tables["TransportData"].Rows.Add(new string[] {
                        docData.Transport.pakker[i].IsDoneFragt.ToString(),
                        docData.Transport.pakker[i].mrkNumb,
                        docData.Transport.pakker[i].contains,
                        docData.Transport.pakker[i].countI.ToString(),
                        docData.Transport.pakker[i].countS,
                        docData.Transport.pakker[i].weightD.ToString(),
                        docData.Transport.pakker[i].weightS,
                        docData.Transport.pakker[i].volume,
                        docData.Transport.pakker[i].artId.ToString(),
                        docData.Transport.pakker[i].artName,
                        docData.Transport.pakker[i].transportTypeId.ToString(),
                        docData.Transport.pakker[i].transportTypeName
                    });

                    for (int a = 0; a < docData.Transport.pakker[i].countI; a++)
                    {
                        //vær sikker på at den er sat
                        if (docData.Transport.pakker[i].IsDoneFaktura == null)
                            break;

                        file.Tables["TransportDataFaktura"].Rows.Add(new string[] {
                            i.ToString(),
                            docData.Transport.pakker[i].IsDoneFaktura[a].ToString(),
                            docData.Transport.pakker[i].takstId[a].ToString(),
                            docData.Transport.pakker[i].takstName[a].ToString(),
                            docData.Transport.pakker[i].beregningstypeId[a].ToString(),
                            docData.Transport.pakker[i].beregningstypeName[a].ToString(),
                        });
                    }
                }
                #endregion

                #region Luk Fragtbrev

                file.Tables["LukFragtbrev"].Rows.Add(new string[] {
                    docData.LukFragtbrev.leveringsdato.ToString(),
                    docData.LukFragtbrev.isSetLeveringsdato.ToString(),
                    docData.LukFragtbrev.rabat,
                    docData.LukFragtbrev.kilometer,
                    docData.LukFragtbrev.tidsforbrug1,
                    docData.LukFragtbrev.tidsforbrug2,
                    docData.LukFragtbrev.tidsforbrug3,
                    docData.LukFragtbrev.tidsforbrug4,
                    docData.LukFragtbrev.kommentar
                });
                #endregion

                #region Ekstra Gebyr

                file.Tables["EkstraGebyrButtons"].Rows.Add(new string[] {
                    docData.EkstraGebyr.buttonsIsCheck[0].ToString(),
                    docData.EkstraGebyr.buttonsIsCheck[1].ToString(),
                    docData.EkstraGebyr.buttonsIsCheck[2].ToString(),
                    docData.EkstraGebyr.buttonsIsCheck[3].ToString(),
                    docData.EkstraGebyr.buttonsIsCheck[4].ToString(),
                    docData.EkstraGebyr.buttonsIsCheck[5].ToString(),
                    docData.EkstraGebyr.buttonsIsCheck[6].ToString(),
                    docData.EkstraGebyr.buttonsIsCheck[7].ToString(),
                    docData.EkstraGebyr.buttonsIsCheck[8].ToString(),
                    docData.EkstraGebyr.buttonsIsCheck[9].ToString(),
                    docData.EkstraGebyr.buttonsIsCheck[10].ToString(),
                    docData.EkstraGebyr.buttonsIsCheck[11].ToString()
                });

                file.Tables["EkstraGebyrTextboxs"].Rows.Add(new string[] {
                    docData.EkstraGebyr.textboxsValues[0],
                    docData.EkstraGebyr.textboxsValues[1],
                    docData.EkstraGebyr.textboxsValues[2],
                    docData.EkstraGebyr.textboxsValues[3],
                    docData.EkstraGebyr.textboxsValues[4],
                    docData.EkstraGebyr.textboxsValues[5],
                    docData.EkstraGebyr.textboxsValues[6]
                });
                #endregion

                file.Tables["andet"].Rows.Add(new string[] {
                    docData.FragtbrevNotifications,
                    docData.FakturaNotifications,
                    docData.FakturaKommentar
                });

                file.WriteXml(Models.ImportantData.g_FolderSave + "document-"+ docData.Invoice +".xml");
            }


            public Layout.DocumentData ReadFile(string Filename)
            {
                Layout.DocumentData docData = new Layout.DocumentData();
                DataSet fileSet = new DataSet();
                FileStream readFile = new FileStream(Path.Combine(Models.ImportantData.g_FolderSave, Filename), FileMode.Open);
                fileSet.ReadXml(readFile);
                readFile.Close();

                //vær sikker på at den er opdateret
                fileSet = CreateSaveFileLayout(fileSet);

                docData.Invoice = int.Parse(fileSet.Tables["Indstillinger"].Rows[0]["Invoice"].ToString());

                string initialer = fileSet.Tables["Indstillinger"].Rows[0]["Initialer"].ToString();
                string[] initialerList = initialer.Split(',');

                for (int i = 0; i < initialerList.Length; i++)
                {
                    if (initialerList[i] != "" && !docData.Initialer.Contains(initialerList[i])) 
                    docData.Initialer.Add(initialerList[i]);
                }

                #region Adresse Info

                docData.Afsender.telefon = fileSet.Tables["Afsender"].Rows[0]["Telefon"].ToString();
                docData.Afsender.firma = fileSet.Tables["Afsender"].Rows[0]["Firma"].ToString();
                docData.Afsender.kontaktPerson = fileSet.Tables["Afsender"].Rows[0]["KontaktPerson"].ToString();
                docData.Afsender.vej = fileSet.Tables["Afsender"].Rows[0]["Vej"].ToString();
                docData.Afsender.zipCode = fileSet.Tables["Afsender"].Rows[0]["ZipCode"].ToString();
                docData.Afsender.isPayer = bool.Parse(fileSet.Tables["Afsender"].Rows[0]["IsPayer"].ToString());

                docData.Modtager.telefon = fileSet.Tables["Modtager"].Rows[0]["Telefon"].ToString();
                docData.Modtager.firma = fileSet.Tables["Modtager"].Rows[0]["Firma"].ToString();
                docData.Modtager.kontaktPerson = fileSet.Tables["Modtager"].Rows[0]["KontaktPerson"].ToString();
                docData.Modtager.vej = fileSet.Tables["Modtager"].Rows[0]["Vej"].ToString();
                docData.Modtager.zipCode = fileSet.Tables["Modtager"].Rows[0]["ZipCode"].ToString();
                docData.Modtager.isPayer = bool.Parse(fileSet.Tables["Modtager"].Rows[0]["IsPayer"].ToString());

                docData.Modtager2.isActive = bool.Parse(fileSet.Tables["Modtager2"].Rows[0]["IsActive"].ToString());
                docData.Modtager2.telefon = fileSet.Tables["Modtager2"].Rows[0]["Telefon"].ToString();
                docData.Modtager2.firma = fileSet.Tables["Modtager2"].Rows[0]["Firma"].ToString();
                docData.Modtager2.kontaktPerson = fileSet.Tables["Modtager2"].Rows[0]["KontaktPerson"].ToString();
                docData.Modtager2.vej = fileSet.Tables["Modtager2"].Rows[0]["Vej"].ToString();
                docData.Modtager2.zipCode = fileSet.Tables["Modtager2"].Rows[0]["ZipCode"].ToString();
                docData.Modtager2.isPayer = bool.Parse(fileSet.Tables["Modtager2"].Rows[0]["IsPayer"].ToString());
                #endregion

                #region Fragtbrev Generelt

                docData.Generelt.reference = fileSet.Tables["Generelt"].Rows[0]["Reference"].ToString();
                docData.Generelt.fragtmand = fileSet.Tables["Generelt"].Rows[0]["Fragtmand"].ToString();
                docData.Generelt.forsikringstype = int.Parse(fileSet.Tables["Generelt"].Rows[0]["Forsikringstype"].ToString());

                DateTime.TryParse(fileSet.Tables["Generelt"].Rows[0]["DatoRute1"].ToString(), out docData.Generelt.datoRute1);
                DateTime.TryParse(fileSet.Tables["Generelt"].Rows[0]["DatoRute2"].ToString(), out docData.Generelt.datoRute2);
                docData.Generelt.isSetDatoRute1 = bool.Parse(fileSet.Tables["Generelt"].Rows[0]["IsSetDatoRute1"].ToString());
                docData.Generelt.isSetDatoRute2 = bool.Parse(fileSet.Tables["Generelt"].Rows[0]["IsSetDatoRute2"].ToString());
                docData.Generelt.zipRute1 = fileSet.Tables["Generelt"].Rows[0]["ZipRute1"].ToString();
                docData.Generelt.zipRute2 = fileSet.Tables["Generelt"].Rows[0]["ZipRute2"].ToString();

                docData.Byttepaller.useByttepalle = bool.Parse(fileSet.Tables["Byttepaller"].Rows[0]["UseByttepalle"].ToString());
                docData.Byttepaller.palle1_1 = fileSet.Tables["Byttepaller"].Rows[0]["Palle1_1"].ToString();
                docData.Byttepaller.palle1_2 = fileSet.Tables["Byttepaller"].Rows[0]["Palle1_2"].ToString();
                docData.Byttepaller.palle1_4 = fileSet.Tables["Byttepaller"].Rows[0]["Palle1_4"].ToString();
                
                docData.Efterkrav.useEfterkrav = bool.Parse(fileSet.Tables["Efterkrav"].Rows[0]["UseEfterkrav"].ToString());
                docData.Efterkrav.efterkravGebyr = fileSet.Tables["Efterkrav"].Rows[0]["EfterkravGebyr"].ToString();
                docData.Efterkrav.forsikringssum = fileSet.Tables["Efterkrav"].Rows[0]["Forsikringssum"].ToString();
                docData.Efterkrav.premium = fileSet.Tables["Efterkrav"].Rows[0]["Premium"].ToString();
                docData.Efterkrav.total = fileSet.Tables["Efterkrav"].Rows[0]["Total"].ToString();
                #endregion

                #region Transport

                docData.Transport.transportType = new int[] {
                    int.Parse(fileSet.Tables["TransportType"].Rows[0]["Val1"].ToString()),
                    int.Parse(fileSet.Tables["TransportType"].Rows[0]["Val2"].ToString()),
                    int.Parse(fileSet.Tables["TransportType"].Rows[0]["Val3"].ToString())
                };

                
                for (int i = 0; i < fileSet.Tables["TransportData"].Rows.Count; i++)
                {
                    Controls.PakkeControlItem pakkeItem = new Controls.PakkeControlItem();

                    pakkeItem.IsDoneFragt = bool.Parse(fileSet.Tables["TransportData"].Rows[i]["IsDoneFragt"].ToString());
                    pakkeItem.mrkNumb = fileSet.Tables["TransportData"].Rows[i]["MrkNumb"].ToString();
                    pakkeItem.contains = fileSet.Tables["TransportData"].Rows[i]["Contains"].ToString();
                    pakkeItem.countI = int.Parse(fileSet.Tables["TransportData"].Rows[i]["CountI"].ToString());
                    pakkeItem.countS = fileSet.Tables["TransportData"].Rows[i]["CountS"].ToString();
                    pakkeItem.weightD = double.Parse(fileSet.Tables["TransportData"].Rows[i]["WeightD"].ToString());
                    pakkeItem.weightS = fileSet.Tables["TransportData"].Rows[i]["WeightS"].ToString();
                    pakkeItem.volume = fileSet.Tables["TransportData"].Rows[i]["Volume"].ToString();
                    pakkeItem.artId = int.Parse(fileSet.Tables["TransportData"].Rows[i]["ArtId"].ToString());
                    pakkeItem.artName = fileSet.Tables["TransportData"].Rows[i]["ArtName"].ToString();
                    pakkeItem.transportTypeId = int.Parse(fileSet.Tables["TransportData"].Rows[i]["TransportTypeId"].ToString());
                    pakkeItem.transportTypeName = fileSet.Tables["TransportData"].Rows[i]["TransportTypeName"].ToString();

                    for (int a = 0; a < fileSet.Tables["TransportDataFaktura"].Rows.Count; a++)
                    {
                        int index = int.Parse(fileSet.Tables["TransportDataFaktura"].Rows[a]["Index"].ToString());

                        if (i == index)
                        {
                            if (pakkeItem.IsDoneFaktura == null)
                                pakkeItem.IsDoneFaktura = new List<bool>();
                            if (pakkeItem.beregningstypeId == null)
                                pakkeItem.beregningstypeId = new List<int>();
                            if (pakkeItem.beregningstypeName == null)
                                pakkeItem.beregningstypeName = new List<string>();
                            if (pakkeItem.takstId == null)
                                pakkeItem.takstId = new List<int>();
                            if (pakkeItem.takstName == null)
                                pakkeItem.takstName = new List<string>();

                            pakkeItem.IsDoneFaktura.Add(bool.Parse(fileSet.Tables["TransportDataFaktura"].Rows[a]["IsDoneFaktura"].ToString()));
                            pakkeItem.beregningstypeId.Add(int.Parse(fileSet.Tables["TransportDataFaktura"].Rows[a]["TakstId"].ToString()));
                            pakkeItem.beregningstypeName.Add(fileSet.Tables["TransportDataFaktura"].Rows[a]["TakstName"].ToString());
                            pakkeItem.takstId.Add(int.Parse(fileSet.Tables["TransportDataFaktura"].Rows[a]["BeregningstypeId"].ToString()));
                            pakkeItem.takstName.Add(fileSet.Tables["TransportDataFaktura"].Rows[a]["BeregningstypeName"].ToString());
                        }
                    }

                    docData.Transport.pakker.Add(pakkeItem);
                }
                #endregion

                #region Luk fragtbrev

                DateTime.TryParse(fileSet.Tables["LukFragtbrev"].Rows[0]["leveringsdato"].ToString(), out docData.LukFragtbrev.leveringsdato);
                docData.LukFragtbrev.isSetLeveringsdato = bool.Parse(fileSet.Tables["LukFragtbrev"].Rows[0]["isSetLeveringsdato"].ToString());
                docData.LukFragtbrev.rabat = fileSet.Tables["LukFragtbrev"].Rows[0]["rabat"].ToString();
                docData.LukFragtbrev.kilometer = fileSet.Tables["LukFragtbrev"].Rows[0]["kilometer"].ToString();
                docData.LukFragtbrev.tidsforbrug1 = fileSet.Tables["LukFragtbrev"].Rows[0]["tidsforbrug1"].ToString();
                docData.LukFragtbrev.tidsforbrug2 = fileSet.Tables["LukFragtbrev"].Rows[0]["tidsforbrug2"].ToString();
                docData.LukFragtbrev.tidsforbrug3 = fileSet.Tables["LukFragtbrev"].Rows[0]["tidsforbrug3"].ToString();
                docData.LukFragtbrev.tidsforbrug4 = fileSet.Tables["LukFragtbrev"].Rows[0]["tidsforbrug4"].ToString();
                docData.LukFragtbrev.kommentar = fileSet.Tables["LukFragtbrev"].Rows[0]["Kommentar"].ToString();

                docData.FragtbrevNotifications = fileSet.Tables["Andet"].Rows[0]["FragtbrevNotifications"].ToString();
                docData.FakturaNotifications = fileSet.Tables["Andet"].Rows[0]["FakturaNotifications"].ToString();
                docData.FakturaKommentar = fileSet.Tables["Andet"].Rows[0]["FakturaKommentar"].ToString();
                #endregion

                #region Ekstra Gebyr

                for (int i = 0; i < docData.EkstraGebyr.buttonsIsCheck.Length; i++)
                    docData.EkstraGebyr.buttonsIsCheck[i] = bool.Parse(fileSet.Tables["EkstraGebyrButtons"].Rows[0]["Button_" + i].ToString());

                for (int i = 0; i < docData.EkstraGebyr.textboxsValues.Length; i++)
                    docData.EkstraGebyr.textboxsValues[i] = fileSet.Tables["EkstraGebyrTextboxs"].Rows[0]["Text_" + i].ToString();
                #endregion


                docData.FragtbrevNotifications = fileSet.Tables["Andet"].Rows[0]["FragtbrevNotifications"].ToString();
                docData.FakturaNotifications = fileSet.Tables["Andet"].Rows[0]["FakturaNotifications"].ToString();
                docData.FakturaKommentar = fileSet.Tables["Andet"].Rows[0]["FakturaKommentar"].ToString();

                return docData;
            }


        }


        public class Faktura
        {
            public class GodsTransport
            {
                public string Takst { get; set; }
                public string BeregnType { get; set; }
                public double BeregnKilo { get; set; }
                public double ReelleKilo { get; set; }
                public double Price { get; set; }
            }
            public class Gebyr
            {
                public bool Helper { get; set; }
                public bool Flytte { get; set; }
                public bool ADR { get; set; }
                public bool AftenOgNat { get; set; }
                public bool Weekend { get; set; }
                public bool Yderzone { get; set; }
                public bool Byttepalle { get; set; }
                public bool SMS { get; set; }
                public bool AdresseK { get; set; }
                public bool Bro { get; set; }
                public bool Vej { get; set; }
                public bool Faerge { get; set; }
                public int HelperN { get; set; }
                public int FlytteN { get; set; }
                public int ByttepalleN { get; set; }
                public int SMS_N { get; set; }
                public double BroPrice { get; set; }
                public double VejPrice { get; set; }
                public double FaergePrice { get; set; }
            }

            public class Layout
            {
                public string Filename { get; set; }
                public double Price { get; set; }

                public string Invoice { get; set; }
                public string FragtName { get; set; }
                public int TransportId { get; set; }

                public List<GodsTransport> GodsPackets = new List<GodsTransport>();

                public Gebyr Gebyr = new Gebyr();
            
                public bool UseComment { get; set; }
                public string CommentTekst { get; set; }
                public List<string> Owners = new List<string>();
                public bool IsClosed { get; set; }
            }

            /// <summary>
            /// Faktura Save
            /// Version 2.0.0+
            /// </summary>
            public void SaveFile(Layout fakturaInfo)
            {
                string folder = Models.ImportantData.g_FolderSave;
                FileStream documentCreate = new FileStream(folder + Models.ImportantData.Filename + ".xml", FileMode.Create, FileAccess.Write);

                DataSet SaveXML = new DataSet();

                #region XMl layout / table
                SaveXML.DataSetName = "Filnavn " + Models.ImportantData.Filename;

                //version
                SaveXML.Tables.Add("Version");
                SaveXML.Tables["Version"].Columns.Add("ver");

                //faktura Info
                SaveXML.Tables.Add("FakturaInfo");
                SaveXML.Tables["FakturaInfo"].Columns.Add("Price");
                SaveXML.Tables["FakturaInfo"].Columns.Add("IsClosed");
                SaveXML.Tables["FakturaInfo"].Columns.Add("ClosedText");

                //Generelt
                SaveXML.Tables.Add("FragtBrevInfo");
                SaveXML.Tables["FragtBrevInfo"].Columns.Add("Fragtnumber");
                SaveXML.Tables["FragtBrevInfo"].Columns.Add("FragtbrevNavn");
                SaveXML.Tables["FragtBrevInfo"].Columns.Add("TrasportId");


                //Pakker
                SaveXML.Tables.Add("Pakker");
                if (fakturaInfo.GodsPackets.Count != 0)
                {
                    SaveXML.Tables["Pakker"].Columns.Add("Takst");
                    SaveXML.Tables["Pakker"].Columns.Add("Beregningstype");
                    SaveXML.Tables["Pakker"].Columns.Add("Beregningsvægt");
                    SaveXML.Tables["Pakker"].Columns.Add("Reelle vægt");
                    SaveXML.Tables["Pakker"].Columns.Add("Pris");
                }


                //EkstraGebyr
                SaveXML.Tables.Add("Gebyr");
                SaveXML.Tables["Gebyr"].Columns.Add("Chauffør");
                SaveXML.Tables["Gebyr"].Columns.Add("Chauffør antal");
                SaveXML.Tables["Gebyr"].Columns.Add("Flytte");
                SaveXML.Tables["Gebyr"].Columns.Add("Flytte antal");
                SaveXML.Tables["Gebyr"].Columns.Add("ADR");
                SaveXML.Tables["Gebyr"].Columns.Add("Aften og nat");
                SaveXML.Tables["Gebyr"].Columns.Add("Weekend");
                SaveXML.Tables["Gebyr"].Columns.Add("Yderzone");
                SaveXML.Tables["Gebyr"].Columns.Add("Byttepalle");
                SaveXML.Tables["Gebyr"].Columns.Add("Byttepalle antal");
                SaveXML.Tables["Gebyr"].Columns.Add("SMS");
                SaveXML.Tables["Gebyr"].Columns.Add("SMS antal");
                SaveXML.Tables["Gebyr"].Columns.Add("Adresse korrektion");
                SaveXML.Tables["Gebyr"].Columns.Add("Bro afgift");
                SaveXML.Tables["Gebyr"].Columns.Add("Bro beløb");
                SaveXML.Tables["Gebyr"].Columns.Add("Vej afgift");
                SaveXML.Tables["Gebyr"].Columns.Add("Vej beløb");
                SaveXML.Tables["Gebyr"].Columns.Add("Færge afgift");
                SaveXML.Tables["Gebyr"].Columns.Add("Færge beløb");

                //Evt. Bemærkninger
                SaveXML.Tables.Add("ekstraComment");
                SaveXML.Tables["ekstraComment"].Columns.Add("Aktiver");
                SaveXML.Tables["ekstraComment"].Columns.Add("Bemærkninger");

                //Initialer
                SaveXML.Tables.Add("Initialer");
                SaveXML.Tables["Initialer"].Columns.Add("Id");

                #endregion XMl layout / table

                #region gem data
                SaveXML.Tables["Version"].Rows.Add(Models.ImportantData.PVersion.ToString());
                SaveXML.Tables["FakturaInfo"].Rows.Add(
                    fakturaInfo.Price,
                    Models.ImportantData.closeFakturaBool,
                    Models.ImportantData.closeFakturaText
                    );

                SaveXML.Tables["FragtBrevInfo"].Rows.Add(
                    fakturaInfo.Invoice,
                    fakturaInfo.FragtName,
                    fakturaInfo.TransportId
                );

                //gods
                if (fakturaInfo.GodsPackets.Count != 0)
                {
                    int godsCount = fakturaInfo.GodsPackets.Count;

                    for (int i = 0; i < godsCount; i++)
                    {
                        SaveXML.Tables["Pakker"].Rows.Add(
                            fakturaInfo.GodsPackets[i].Takst,
                            fakturaInfo.GodsPackets[i].BeregnType,
                            fakturaInfo.GodsPackets[i].BeregnKilo.ToString("F"),
                            fakturaInfo.GodsPackets[i].ReelleKilo.ToString("F"),
                            fakturaInfo.GodsPackets[i].Price.ToString("C")
                        );
                    }
                }

                SaveXML.Tables["Gebyr"].Rows.Add(
                    fakturaInfo.Gebyr.Helper, fakturaInfo.Gebyr.HelperN,
                    fakturaInfo.Gebyr.Flytte, fakturaInfo.Gebyr.FlytteN,
                    fakturaInfo.Gebyr.ADR, fakturaInfo.Gebyr.AftenOgNat,
                    fakturaInfo.Gebyr.Weekend, fakturaInfo.Gebyr.Yderzone,
                    fakturaInfo.Gebyr.Byttepalle, fakturaInfo.Gebyr.ByttepalleN,
                    fakturaInfo.Gebyr.SMS, fakturaInfo.Gebyr.SMS_N,
                    fakturaInfo.Gebyr.AdresseK,
                    fakturaInfo.Gebyr.Bro, fakturaInfo.Gebyr.BroPrice,
                    fakturaInfo.Gebyr.Vej, fakturaInfo.Gebyr.VejPrice,
                    fakturaInfo.Gebyr.Faerge, fakturaInfo.Gebyr.FaergePrice
                );

                SaveXML.Tables["ekstraComment"].Rows.Add(fakturaInfo.UseComment, fakturaInfo.CommentTekst);
                
                //Initialer
                for (int i = 0; i < fakturaInfo.Owners.Count; i++)
                {
                    SaveXML.Tables["Initialer"].Rows.Add(fakturaInfo.Owners[i]);
                }
                #endregion gem data

                SaveXML.WriteXml(documentCreate);
                documentCreate.Close();
            }

            /// <summary>
            /// Faktura Read
            /// Version 2.0.0+
            /// </summary>
            public Layout ReadFile(string filename)
            {
                string folder = Models.ImportantData.g_FolderSave;

                FileStream documentRead = new FileStream(folder + filename + ".xml", FileMode.Open, FileAccess.Read);

                DataSet fakturaData = new DataSet();

                fakturaData.ReadXml(documentRead);
                documentRead.Close();

                //app version kan læse
                string saveFileVersion = fakturaData.Tables["Version"].Rows[0].ItemArray[0].ToString();

                Layout fakturaInfo = new Layout();

                if (new Version(saveFileVersion) >= new Version("2.0.1"))
                {
                    fakturaInfo = GetFileData_V2(fakturaData);
                }
                else if (new Version(saveFileVersion) >= new Version("2.0.0"))
                {
                    fakturaInfo = GetFileData_V1(fakturaData);
                    fakturaInfo.IsClosed = false;
                }

                return fakturaInfo;
            }

            /// <summary>
            /// Hent data fra file
            /// </summary>
            /// <returns>Faktura Data: Version 2.0.0+</returns>
            private Layout GetFileData_V1(DataSet fakturaData)
            {
                Layout fakturaInfo = new Layout();

                #region Generelt

                fakturaInfo.FragtName = fakturaData.Tables["FragtBrevInfo"].Rows[0].ItemArray[1].ToString();
                fakturaInfo.Invoice = fakturaData.Tables["FragtBrevInfo"].Rows[0].ItemArray[0].ToString();
                fakturaInfo.TransportId = int.Parse(fakturaData.Tables["FragtBrevInfo"].Rows[0].ItemArray[2].ToString());

                fakturaInfo.UseComment = bool.Parse(fakturaData.Tables["ekstraComment"].Rows[0].ItemArray[0].ToString());
                fakturaInfo.CommentTekst = fakturaData.Tables["ekstraComment"].Rows[0].ItemArray[1].ToString();

                int ownersCount = fakturaData.Tables["Initialer"].Rows.Count;
                for (int i = 0; i < fakturaData.Tables["Initialer"].Rows.Count; i++)
                {
                    string creatorIdS = fakturaData.Tables["Initialer"].Rows[i].ItemArray[0].ToString();

                    if (!fakturaInfo.Owners.Contains(creatorIdS))
                    {
                        fakturaInfo.Owners.Add(creatorIdS);
                    }
                }
                #endregion

                #region Gods pakker

                if (fakturaInfo.TransportId == 3) //Kun hvis det er gods
                {
                    //antal pakker
                    int PakkeCount = fakturaData.Tables["Pakker"].Rows.Count;

                    for (int i = 0; i < PakkeCount; i++)
                    {
                        GodsTransport newGodsPack = new GodsTransport();
                        double kiloB = 0, kiloR = 0, price = 0;
                        double.TryParse(fakturaData.Tables["Pakker"].Rows[i].ItemArray[2].ToString(), out kiloB);
                        double.TryParse(fakturaData.Tables["Pakker"].Rows[i].ItemArray[3].ToString(), out kiloR);
                        double.TryParse(fakturaData.Tables["Pakker"].Rows[i].ItemArray[4].ToString(), out price);

                        newGodsPack.Takst = fakturaData.Tables["Pakker"].Rows[i].ItemArray[0].ToString();
                        newGodsPack.BeregnType = fakturaData.Tables["Pakker"].Rows[i].ItemArray[1].ToString();
                        newGodsPack.BeregnKilo = kiloB;
                        newGodsPack.ReelleKilo = kiloR;
                        newGodsPack.Price = price;

                        fakturaInfo.GodsPackets.Add(newGodsPack);
                    }
                }
                #endregion

                #region Gebyr

                fakturaInfo.Gebyr.Helper = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Chauffør"].ToString());
                fakturaInfo.Gebyr.Flytte = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Flytte"].ToString());
                fakturaInfo.Gebyr.ADR = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["ADR"].ToString());
                fakturaInfo.Gebyr.AftenOgNat = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Aften og nat"].ToString());
                fakturaInfo.Gebyr.Weekend = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Weekend"].ToString());
                fakturaInfo.Gebyr.Yderzone = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Yderzone"].ToString());
                fakturaInfo.Gebyr.Byttepalle = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Byttepalle"].ToString());
                fakturaInfo.Gebyr.SMS = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["SMS"].ToString());
                fakturaInfo.Gebyr.AdresseK = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Adresse korrektion"].ToString());
                fakturaInfo.Gebyr.Bro = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Bro afgift"].ToString());
                fakturaInfo.Gebyr.Vej = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Vej afgift"].ToString());
                fakturaInfo.Gebyr.Faerge = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Færge afgift"].ToString());

                fakturaInfo.Gebyr.HelperN = int.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Chauffør antal"].ToString());
                fakturaInfo.Gebyr.FlytteN = int.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Flytte antal"].ToString());
                fakturaInfo.Gebyr.ByttepalleN = int.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Byttepalle antal"].ToString());
                fakturaInfo.Gebyr.SMS_N = int.Parse(fakturaData.Tables["Gebyr"].Rows[0]["SMS antal"].ToString());
                fakturaInfo.Gebyr.BroPrice = double.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Bro beløb"].ToString());
                fakturaInfo.Gebyr.VejPrice = double.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Vej beløb"].ToString());
                fakturaInfo.Gebyr.FaergePrice = double.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Færge beløb"].ToString());
                #endregion

                return fakturaInfo;
            }

            /// <summary>
            /// Hent data fra file
            /// </summary>
            /// <returns>Faktura Data: Version 2.0.1+</returns>
            private Layout GetFileData_V2(DataSet fakturaData)
            {
                Layout fakturaInfo = new Layout();

                #region Generelt

                Models.ImportantData.closeFakturaBool = bool.Parse(fakturaInfo.FragtName = fakturaData.Tables["FakturaInfo"].Rows[0]["IsClosed"].ToString());
                Models.ImportantData.closeFakturaText = fakturaInfo.FragtName = fakturaData.Tables["FakturaInfo"].Rows[0]["ClosedText"].ToString();

                fakturaInfo.IsClosed = bool.Parse(fakturaInfo.FragtName = fakturaData.Tables["FakturaInfo"].Rows[0]["IsClosed"].ToString());
                fakturaInfo.FragtName = fakturaData.Tables["FragtBrevInfo"].Rows[0].ItemArray[1].ToString();
                fakturaInfo.Invoice = fakturaData.Tables["FragtBrevInfo"].Rows[0].ItemArray[0].ToString();
                fakturaInfo.TransportId = int.Parse(fakturaData.Tables["FragtBrevInfo"].Rows[0].ItemArray[2].ToString());

                fakturaInfo.UseComment = bool.Parse(fakturaData.Tables["ekstraComment"].Rows[0].ItemArray[0].ToString());
                fakturaInfo.CommentTekst = fakturaData.Tables["ekstraComment"].Rows[0].ItemArray[1].ToString();

                int ownersCount = fakturaData.Tables["Initialer"].Rows.Count;
                for (int i = 0; i < fakturaData.Tables["Initialer"].Rows.Count; i++)
                {
                    string creatorIdS = fakturaData.Tables["Initialer"].Rows[i].ItemArray[0].ToString();

                    if (!fakturaInfo.Owners.Contains(creatorIdS))
                    {
                        fakturaInfo.Owners.Add(creatorIdS);
                    }
                }
                #endregion

                #region Gods pakker

                if (fakturaInfo.TransportId == 3) //Kun hvis det er gods
                {
                    //antal pakker
                    int PakkeCount = fakturaData.Tables["Pakker"].Rows.Count;

                    for (int i = 0; i < PakkeCount; i++)
                    {
                        GodsTransport newGodsPack = new GodsTransport();
                        double kiloB = 0, kiloR = 0, price = 0;
                        double.TryParse(fakturaData.Tables["Pakker"].Rows[i].ItemArray[2].ToString(), out kiloB);
                        double.TryParse(fakturaData.Tables["Pakker"].Rows[i].ItemArray[3].ToString(), out kiloR);
                        double.TryParse(fakturaData.Tables["Pakker"].Rows[i].ItemArray[4].ToString(), out price);

                        newGodsPack.Takst = fakturaData.Tables["Pakker"].Rows[i].ItemArray[0].ToString();
                        newGodsPack.BeregnType = fakturaData.Tables["Pakker"].Rows[i].ItemArray[1].ToString();
                        newGodsPack.BeregnKilo = kiloB;
                        newGodsPack.ReelleKilo = kiloR;
                        newGodsPack.Price = price;

                        fakturaInfo.GodsPackets.Add(newGodsPack);
                    }
                }
                #endregion

                #region Gebyr

                fakturaInfo.Gebyr.Helper = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Chauffør"].ToString());
                fakturaInfo.Gebyr.Flytte = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Flytte"].ToString());
                fakturaInfo.Gebyr.ADR = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["ADR"].ToString());
                fakturaInfo.Gebyr.AftenOgNat = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Aften og nat"].ToString());
                fakturaInfo.Gebyr.Weekend = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Weekend"].ToString());
                fakturaInfo.Gebyr.Yderzone = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Yderzone"].ToString());
                fakturaInfo.Gebyr.Byttepalle = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Byttepalle"].ToString());
                fakturaInfo.Gebyr.SMS = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["SMS"].ToString());
                fakturaInfo.Gebyr.AdresseK = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Adresse korrektion"].ToString());
                fakturaInfo.Gebyr.Bro = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Bro afgift"].ToString());
                fakturaInfo.Gebyr.Vej = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Vej afgift"].ToString());
                fakturaInfo.Gebyr.Faerge = bool.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Færge afgift"].ToString());

                fakturaInfo.Gebyr.HelperN = int.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Chauffør antal"].ToString());
                fakturaInfo.Gebyr.FlytteN = int.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Flytte antal"].ToString());
                fakturaInfo.Gebyr.ByttepalleN = int.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Byttepalle antal"].ToString());
                fakturaInfo.Gebyr.SMS_N = int.Parse(fakturaData.Tables["Gebyr"].Rows[0]["SMS antal"].ToString());
                fakturaInfo.Gebyr.BroPrice = double.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Bro beløb"].ToString());
                fakturaInfo.Gebyr.VejPrice = double.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Vej beløb"].ToString());
                fakturaInfo.Gebyr.FaergePrice = double.Parse(fakturaData.Tables["Gebyr"].Rows[0]["Færge beløb"].ToString());
                #endregion

                return fakturaInfo;
            }
        }
        public class Fragtbrev
        {
            public class adresse
            {
                public string Kontakt = "";
                public string Firma = "";
                public string Adresse = "";
                public string Post = "";
                public bool Betaler = false;
                public string KontaktPerson = "";
            }

            public class GenereltData
            {
                public string Refercence { get; set; }
                public string Fragtmand { get; set; }
                public int Forsikringstype { get; set; }
                public string Date1 { get; set; }
                public string Date2 { get; set; }
                public string Rute1 { get; set; }
                public string Rute2 { get; set; }

                public bool UseEfterkrav { get; set; }
                public string Efterkrav { get; set; }
                public string ForSum { get; set; }
                public string Praemie { get; set; }
                public string All { get; set; }
            }

            public class Godslinjer
            {   
                public string Adresse { get; set; }
                public int Antal { get; set; }
                public string Art { get; set; }
                public string Indhold { get; set; }
                public double Kilo { get; set; }
                public string Rumfang { get; set; }
                public string Size { get; set; }
            }

            public class Close
            {
                public bool IsClosed { get; set; }
                public string TimeL { get; set; }
                public string TimeA { get; set; }
                public string TimeV { get; set; }
                public string TimeH { get; set; }
                public DateTime DateDay { get; set; }
                public DateTime DateTime { get; set; }
                public string Rabt { get; set; }
                public string Kilometer { get; set; }
            }

            public class Layout
            {
                public string Filename { get; set; }
                public string EkstraTekst { get; set; }
                public string CreateFaktura { get; set; }

                public bool UseByttePalle { get; set; }
                public string Palle1 { get; set; }
                public string Palle2 { get; set; }
                public string Palle3 { get; set; }

                public string CommentTekst { get; set; }
                public List<string> Owners = new List<string>();

                public adresse Afsender = new adresse();
                public adresse Modtager = new adresse();
                public bool UseAndenBetaler { get; set; }
                public adresse AndenBetaler = new adresse();
                public GenereltData Generelt = new GenereltData();

                public int Transport1 { get; set; }
                public int Transport2 { get; set; }
                public int Transport3 { get; set; }
                public List<Godslinjer> Godslinjer = new List<Godslinjer>();
                public Close Close = new Close();
            }


            /// <summary>
            /// Fragtbrev Save
            /// Version 2.0.0+
            /// </summary>
            public void SaveFile(Layout fragtInfo)
            {
                string folder = Models.ImportantData.g_FolderSave;

                DataSet SaveXML = new DataSet();

                #region XMl layout / table

                SaveXML.DataSetName = "Filnavn " + Models.ImportantData.Filename;

                //version
                SaveXML.Tables.Add("Version");
                SaveXML.Tables["Version"].Columns.Add("ver");

                //færdi
                SaveXML.Tables.Add("FileDone");
                SaveXML.Tables["FileDone"].Columns.Add("CreateFaktura");
                SaveXML.Tables["FileDone"].Columns.Add("EkstraTekst");

                //Afsender
                SaveXML.Tables.Add("Afsender");
                SaveXML.Tables["Afsender"].Columns.Add("Kontakt");
                SaveXML.Tables["Afsender"].Columns.Add("Firma");
                SaveXML.Tables["Afsender"].Columns.Add("Adresse");
                SaveXML.Tables["Afsender"].Columns.Add("Post/By");
                SaveXML.Tables["Afsender"].Columns.Add("Betaler");
                SaveXML.Tables["Afsender"].Columns.Add("pKont");

                //Modtager
                SaveXML.Tables.Add("Modtager");
                SaveXML.Tables["Modtager"].Columns.Add("Kontakt");
                SaveXML.Tables["Modtager"].Columns.Add("Firma");
                SaveXML.Tables["Modtager"].Columns.Add("Adresse");
                SaveXML.Tables["Modtager"].Columns.Add("Post/By");
                SaveXML.Tables["Modtager"].Columns.Add("Betaler");
                SaveXML.Tables["Modtager"].Columns.Add("pKont");

                SaveXML.Tables.Add("AndenBetaler");
                SaveXML.Tables["AndenBetaler"].Columns.Add("InUse");
                SaveXML.Tables["AndenBetaler"].Columns.Add("Kontakt");
                SaveXML.Tables["AndenBetaler"].Columns.Add("Firma");
                SaveXML.Tables["AndenBetaler"].Columns.Add("Adresse");
                SaveXML.Tables["AndenBetaler"].Columns.Add("Post/By");
                SaveXML.Tables["AndenBetaler"].Columns.Add("Betaler");
                SaveXML.Tables["AndenBetaler"].Columns.Add("pKont");

                //Generelt
                SaveXML.Tables.Add("Generelt");
                SaveXML.Tables["Generelt"].Columns.Add("Refercence");
                SaveXML.Tables["Generelt"].Columns.Add("Fragtmand");
                SaveXML.Tables["Generelt"].Columns.Add("Type");
                SaveXML.Tables["Generelt"].Columns.Add("Dato dag 1");
                SaveXML.Tables["Generelt"].Columns.Add("Rute dag 1");
                SaveXML.Tables["Generelt"].Columns.Add("Dato dag 2");
                SaveXML.Tables["Generelt"].Columns.Add("Rute dag 2");
                SaveXML.Tables["Generelt"].Columns.Add("Efterkrav-On");
                SaveXML.Tables["Generelt"].Columns.Add("Efterkrav");
                SaveXML.Tables["Generelt"].Columns.Add("Forsikringssum");
                SaveXML.Tables["Generelt"].Columns.Add("Præmie");
                SaveXML.Tables["Generelt"].Columns.Add("I Alt");

                //Godslinjer
                SaveXML.Tables.Add("Godslinjer Type");
                SaveXML.Tables["Godslinjer Type"].Columns.Add("Transport Type");
                SaveXML.Tables["Godslinjer Type"].Columns.Add("Pakke Type");
                    SaveXML.Tables["Godslinjer Type"].Columns.Add("Bil Type");

                int transportT = fragtInfo.Transport1;

                SaveXML.Tables.Add("Godslinjer");
                switch (transportT)
                {
                    case 0:
                        SaveXML.Tables["Godslinjer"].Columns.Add("Adresse");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Antal");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Art");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Indhold");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Vægt");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Rumfang");
                        break;
                    case 1:
                        SaveXML.Tables["Godslinjer"].Columns.Add("Adresse");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Antal");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Art");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Indhold");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Pakke size");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Vægt");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Rumfang");

                        break;
                    case 2:
                        SaveXML.Tables["Godslinjer"].Columns.Add("Adresse");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Antal");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Art");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Indhold");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Pakke size");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Vægt");
                        SaveXML.Tables["Godslinjer"].Columns.Add("Rumfang");
                        break;
                }

                //Byttepaller
                SaveXML.Tables.Add("Byttepaller");
                SaveXML.Tables["Byttepaller"].Columns.Add("Byttepalle-On");
                SaveXML.Tables["Byttepaller"].Columns.Add("1/1 palle");
                SaveXML.Tables["Byttepaller"].Columns.Add("1/2 palle");
                SaveXML.Tables["Byttepaller"].Columns.Add("1/4 palle");

                //Evt. Bemærkninger
                SaveXML.Tables.Add("ekstraComment");
                SaveXML.Tables["ekstraComment"].Columns.Add("Bemærkninger");


                //Initialer
                SaveXML.Tables.Add("Initialer");
                SaveXML.Tables["Initialer"].Columns.Add("Id");

                //Afslut Data
                SaveXML.Tables.Add("AfslutFragtBrev");
                SaveXML.Tables["AfslutFragtBrev"].Columns.Add("Læsse");
                SaveXML.Tables["AfslutFragtBrev"].Columns.Add("Aflæsse");
                SaveXML.Tables["AfslutFragtBrev"].Columns.Add("Vente");
                SaveXML.Tables["AfslutFragtBrev"].Columns.Add("Medhjælper");
                SaveXML.Tables["AfslutFragtBrev"].Columns.Add("Leveringsdato Dag");
                SaveXML.Tables["AfslutFragtBrev"].Columns.Add("Leveringsdato Tid");
                SaveXML.Tables["AfslutFragtBrev"].Columns.Add("Rabat");
                SaveXML.Tables["AfslutFragtBrev"].Columns.Add("Kilometer");

                #endregion XMl layout / table

                #region Default

                
                SaveXML.Tables["Version"].Rows.Add(Models.ImportantData.PVersion.ToString());

                SaveXML.Tables["FileDone"].Rows.Add(
                    Models.ImportantData.closeFragtbrevBool, 
                    Models.ImportantData.closeFragtbrevText
                );

                SaveXML.Tables["ekstraComment"].Rows.Add(fragtInfo.CommentTekst);

                //Initialer
                int ownersCount = fragtInfo.Owners.Count;
                for (int i = 0; i < ownersCount; i++)
                {
                    SaveXML.Tables["Initialer"].Rows.Add(fragtInfo.Owners[i]);
                }

                #endregion

                #region Adresse

                SaveXML.Tables["Afsender"].Rows.Add(
                    fragtInfo.Afsender.Kontakt,
                    fragtInfo.Afsender.Firma,
                    fragtInfo.Afsender.Adresse,
                    fragtInfo.Afsender.Post,
                    fragtInfo.Afsender.Betaler,
                    fragtInfo.Afsender.KontaktPerson
                );

                SaveXML.Tables["Modtager"].Rows.Add(
                    fragtInfo.Modtager.Kontakt,
                    fragtInfo.Modtager.Firma,
                    fragtInfo.Modtager.Adresse,
                    fragtInfo.Modtager.Post,
                    fragtInfo.Modtager.Betaler,
                    fragtInfo.Modtager.KontaktPerson
                );

                SaveXML.Tables["AndenBetaler"].Rows.Add(
                    fragtInfo.UseAndenBetaler.ToString(),
                    fragtInfo.AndenBetaler.Kontakt,
                    fragtInfo.AndenBetaler.Firma,
                    fragtInfo.AndenBetaler.Adresse,
                    fragtInfo.AndenBetaler.Post,
                    fragtInfo.AndenBetaler.Betaler,
                    fragtInfo.AndenBetaler.KontaktPerson
                );

                #endregion

                #region Generelt

                SaveXML.Tables["Generelt"].Rows.Add(
                    fragtInfo.Generelt.Refercence,
                    fragtInfo.Generelt.Fragtmand,
                    fragtInfo.Generelt.Forsikringstype,
                    fragtInfo.Generelt.Date1,
                    fragtInfo.Generelt.Rute1,
                    fragtInfo.Generelt.Date2,
                    fragtInfo.Generelt.Rute2,
                    fragtInfo.Generelt.UseEfterkrav,
                    fragtInfo.Generelt.Efterkrav,
                    fragtInfo.Generelt.ForSum,
                    fragtInfo.Generelt.Praemie,
                    fragtInfo.Generelt.All
                );

                #endregion

                #region Pakker

                SaveXML.Tables["Godslinjer Type"].Rows.Add(
                    fragtInfo.Transport1,
                    fragtInfo.Transport2,
                    fragtInfo.Transport3
                );

                int PakkeCount = fragtInfo.Godslinjer.Count;

                switch (transportT)
                {
                    case 0:
                        for (int i = 0; i < PakkeCount; i++)
                        {
                            SaveXML.Tables["Godslinjer"].Rows.Add(
                                fragtInfo.Godslinjer[i].Adresse,
                                fragtInfo.Godslinjer[i].Antal,
                                fragtInfo.Godslinjer[i].Art,
                                fragtInfo.Godslinjer[i].Indhold,
                                fragtInfo.Godslinjer[i].Kilo,
                                fragtInfo.Godslinjer[i].Rumfang
                            );
                        }

                        break;
                    case 1:
                    case 2:
                        for (int i = 0; i < PakkeCount; i++)
                        {
                            SaveXML.Tables["Godslinjer"].Rows.Add(
                                fragtInfo.Godslinjer[i].Adresse,
                                fragtInfo.Godslinjer[i].Antal,
                                fragtInfo.Godslinjer[i].Art,
                                fragtInfo.Godslinjer[i].Indhold,
                                fragtInfo.Godslinjer[i].Size,
                                fragtInfo.Godslinjer[i].Kilo,
                                fragtInfo.Godslinjer[i].Rumfang
                            );
                        }
                        break;
                }
                #endregion

                #region ByttePalle
                
                SaveXML.Tables["Byttepaller"].Rows.Add(
                    fragtInfo.UseByttePalle,
                    fragtInfo.Palle1,
                    fragtInfo.Palle2,
                    fragtInfo.Palle3
                );

                #endregion

                #region Afslut Fragtbrev

                SaveXML.Tables["AfslutFragtBrev"].Rows.Add(
                    fragtInfo.Close.TimeL,
                    fragtInfo.Close.TimeA,
                    fragtInfo.Close.TimeV,
                    fragtInfo.Close.TimeH,
                    fragtInfo.Close.DateDay,
                    fragtInfo.Close.DateTime,
                    fragtInfo.Close.Rabt,
                    fragtInfo.Close.Kilometer
                );

                #endregion


                FileStream documentCreate = new FileStream(folder + Models.ImportantData.Filename + ".xml", FileMode.Create, FileAccess.Write);
                SaveXML.WriteXml(documentCreate);

                documentCreate.Close();
            }

            /// <summary>
            /// Fragtbrev Read
            /// Version 2.0.0+
            /// </summary>
            public Layout ReadFile(string filename)
            {
                string folder = Models.ImportantData.g_FolderSave;

                FileStream documentRead = new FileStream(folder + filename + ".xml", FileMode.Open, FileAccess.Read);

                DataSet fragtData = new DataSet();

                fragtData.ReadXml(documentRead);
                documentRead.Close();

                //app version kan læse
                string saveFileVersion = fragtData.Tables["Version"].Rows[0].ItemArray[0].ToString();

                Layout fragtInfo = new Layout();

                if (new Version(saveFileVersion) >=  new Version("2.0.5"))
                {
                    fragtInfo = GetFileData_V2(fragtData);
                }
                else if (new Version(saveFileVersion) >= new Version("2.0.0"))
                {

                    fragtInfo = GetFileData_V1(fragtData);

                    fragtInfo.UseAndenBetaler = false;
                    fragtInfo.AndenBetaler.Kontakt = "";
                    fragtInfo.AndenBetaler.Firma = "";
                    fragtInfo.AndenBetaler.Adresse = "";
                    fragtInfo.AndenBetaler.Post = "";
                    fragtInfo.AndenBetaler.KontaktPerson = "";
                    fragtInfo.AndenBetaler.Betaler = false;
                }

                return fragtInfo;
            }


            /// <summary>
            /// Hent data fra file
            /// </summary>
            /// <returns>Fragtbrev Data: Version 2.0.5+</returns>
            private Layout GetFileData_V2(DataSet fragtData)
            {
                Layout fragtInfo = new Layout();

                Models.ImportantData.closeFragtbrevText = ""; // fragtData.Tables["FileDone"].Rows[0]["EkstraTekst"].ToString();
                Models.ImportantData.closeFragtbrevBool = bool.Parse(fragtData.Tables["FileDone"].Rows[0]["CreateFaktura"].ToString());

                #region defualt

                fragtInfo.CommentTekst = fragtData.Tables["ekstraComment"].Rows[0].ItemArray[0].ToString();

                //Initialer
                for (int i = 0; i < fragtData.Tables["Initialer"].Rows.Count; i++)
                {
                    string creatorIdS = fragtData.Tables["Initialer"].Rows[i].ItemArray[0].ToString();
                    if (!fragtInfo.Owners.Contains(creatorIdS))
                    {
                        fragtInfo.Owners.Add(creatorIdS);
                    }
                }
                #endregion

                #region Adresse

                fragtInfo.Afsender.Kontakt = fragtData.Tables["Afsender"].Rows[0].ItemArray[0].ToString();
                fragtInfo.Afsender.Firma = fragtData.Tables["Afsender"].Rows[0].ItemArray[1].ToString();
                fragtInfo.Afsender.Adresse = fragtData.Tables["Afsender"].Rows[0].ItemArray[2].ToString();
                fragtInfo.Afsender.Post = fragtData.Tables["Afsender"].Rows[0].ItemArray[3].ToString();
                fragtInfo.Afsender.KontaktPerson = fragtData.Tables["Afsender"].Rows[0].ItemArray[5].ToString();
                fragtInfo.Afsender.Betaler = bool.Parse(fragtData.Tables["Afsender"].Rows[0].ItemArray[4].ToString());
                
                fragtInfo.Modtager.Kontakt = fragtData.Tables["Modtager"].Rows[0].ItemArray[0].ToString();
                fragtInfo.Modtager.Firma = fragtData.Tables["Modtager"].Rows[0].ItemArray[1].ToString();
                fragtInfo.Modtager.Adresse = fragtData.Tables["Modtager"].Rows[0].ItemArray[2].ToString();
                fragtInfo.Modtager.Post = fragtData.Tables["Modtager"].Rows[0].ItemArray[3].ToString();
                fragtInfo.Modtager.KontaktPerson = fragtData.Tables["Modtager"].Rows[0].ItemArray[5].ToString();
                fragtInfo.Modtager.Betaler = bool.Parse(fragtData.Tables["Modtager"].Rows[0].ItemArray[4].ToString());

                fragtInfo.UseAndenBetaler = bool.Parse(fragtData.Tables["AndenBetaler"].Rows[0]["InUse"].ToString());
                fragtInfo.AndenBetaler.Kontakt =    fragtData.Tables["AndenBetaler"].Rows[0]["Kontakt"].ToString();
                fragtInfo.AndenBetaler.Firma =      fragtData.Tables["AndenBetaler"].Rows[0]["Firma"].ToString();
                fragtInfo.AndenBetaler.Adresse =    fragtData.Tables["AndenBetaler"].Rows[0]["Adresse"].ToString();
                fragtInfo.AndenBetaler.Post =       fragtData.Tables["AndenBetaler"].Rows[0]["Post/By"].ToString();
                fragtInfo.AndenBetaler.KontaktPerson = fragtData.Tables["AndenBetaler"].Rows[0]["pKont"].ToString();
                fragtInfo.AndenBetaler.Betaler = bool.Parse(fragtData.Tables["AndenBetaler"].Rows[0]["Betaler"].ToString());

                #endregion

                #region Generelt
                    
                fragtInfo.Generelt.Refercence = fragtData.Tables["Generelt"].Rows[0].ItemArray[0].ToString();
                fragtInfo.Generelt.Fragtmand = fragtData.Tables["Generelt"].Rows[0].ItemArray[1].ToString();
                fragtInfo.Generelt.Forsikringstype = int.Parse(fragtData.Tables["Generelt"].Rows[0].ItemArray[2].ToString());
                fragtInfo.Generelt.Date1 = fragtData.Tables["Generelt"].Rows[0].ItemArray[3].ToString();
                fragtInfo.Generelt.Rute1 = fragtData.Tables["Generelt"].Rows[0].ItemArray[4].ToString();
                fragtInfo.Generelt.Date2 = fragtData.Tables["Generelt"].Rows[0].ItemArray[5].ToString();
                fragtInfo.Generelt.Rute2 = fragtData.Tables["Generelt"].Rows[0].ItemArray[6].ToString();
                fragtInfo.Generelt.UseEfterkrav = bool.Parse(fragtData.Tables["Generelt"].Rows[0].ItemArray[7].ToString());
                fragtInfo.Generelt.Efterkrav = fragtData.Tables["Generelt"].Rows[0].ItemArray[8].ToString();
                fragtInfo.Generelt.ForSum = fragtData.Tables["Generelt"].Rows[0].ItemArray[9].ToString();
                fragtInfo.Generelt.Praemie = fragtData.Tables["Generelt"].Rows[0].ItemArray[10].ToString();
                fragtInfo.Generelt.All = fragtData.Tables["Generelt"].Rows[0].ItemArray[11].ToString();

                #endregion

                #region Godslinjer

                    //Godslinjer
                fragtInfo.Transport1 = int.Parse(fragtData.Tables["Godslinjer Type"].Rows[0].ItemArray[0].ToString());
                fragtInfo.Transport2 = int.Parse(fragtData.Tables["Godslinjer Type"].Rows[0].ItemArray[1].ToString());
                fragtInfo.Transport3 = int.Parse(fragtData.Tables["Godslinjer Type"].Rows[0].ItemArray[2].ToString());
                if (fragtInfo.Transport1 >= 0 && fragtInfo.Transport2 > 0)
                {
                    int pakkeCount = fragtData.Tables["Godslinjer"].Rows.Count;

                    switch (fragtInfo.Transport1)
                    {
                        case 0: //kurrer
                            if (fragtInfo.Transport3 <= 0)
                            {
                                break;
                            }
                            for (int i = 0; i < pakkeCount; i++)
                            {
                                var godsIndholdItems = new Godslinjer();

                                string adresse, antal, art, indhold, veagt, rumfangt;

                                adresse = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[0].ToString();
                                antal = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[1].ToString();
                                art = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[2].ToString();
                                indhold = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[3].ToString();
                                veagt = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[4].ToString();
                                rumfangt = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[5].ToString();

                                if (!(adresse == "" && (antal == "" || antal == "0") &&
                                    (art == "" || art == "-1") && indhold == "" &&
                                    (veagt == "" || veagt == "0") && rumfangt == ""))
                                {

                                    int antalInt = 0;
                                    double weightDouble = 0;
                                    //set values

                                    godsIndholdItems.Adresse = adresse;
                                    godsIndholdItems.Indhold = indhold;

                                    int.TryParse(antal, out antalInt);
                                    godsIndholdItems.Antal = antalInt;

                                    godsIndholdItems.Art = art;
                                    godsIndholdItems.Rumfang = rumfangt;

                                    double.TryParse(veagt, out weightDouble);
                                    godsIndholdItems.Kilo = weightDouble;

                                    //add pakke
                                    fragtInfo.Godslinjer.Add(godsIndholdItems);
                                }
                            }
                            break;

                        case 1: //Pakke
                        case 2: //Gods
                            for (int i = 0; i < pakkeCount; i++)
                            {
                                var godsIndholdItems = new Godslinjer();

                                string adresse, antal, art, indhold, transport, veagt, rumfangt;

                                adresse = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[0].ToString();
                                antal = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[1].ToString();
                                art = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[2].ToString();
                                indhold = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[3].ToString();
                                transport = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[4].ToString();
                                veagt = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[5].ToString();
                                rumfangt = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[6].ToString();

                                if (!(adresse == "" && (antal == "" || antal == "0") &&
                                    (art == "" || art == "-1") && indhold == "" &&
                                    (transport == "" || transport == "-1") &&
                                    (veagt == "" || veagt == "0") && rumfangt == ""))
                                {

                                    int antalInt = 0;
                                    double weightDouble = 0;
                                    //set values

                                    godsIndholdItems.Adresse = adresse;
                                    godsIndholdItems.Indhold = indhold;
                                    godsIndholdItems.Size = transport;

                                    int.TryParse(antal, out antalInt);
                                    godsIndholdItems.Antal = antalInt;

                                    godsIndholdItems.Art = art;
                                    godsIndholdItems.Rumfang = rumfangt;

                                    double.TryParse(veagt, out weightDouble);
                                    godsIndholdItems.Kilo = weightDouble;

                                    //add pakke
                                    fragtInfo.Godslinjer.Add(godsIndholdItems);
                                }
                            }
                            break;
                    }
                }
                    #endregion Godslinjer

                #region Byttepaller

                fragtInfo.UseByttePalle = bool.Parse(fragtData.Tables["Byttepaller"].Rows[0].ItemArray[0].ToString());
                fragtInfo.Palle1 = fragtData.Tables["Byttepaller"].Rows[0].ItemArray[1].ToString();
                fragtInfo.Palle2 = fragtData.Tables["Byttepaller"].Rows[0].ItemArray[2].ToString();
                fragtInfo.Palle3 = fragtData.Tables["Byttepaller"].Rows[0].ItemArray[3].ToString();

                #endregion

                #region Afslut fragtbrev

                fragtInfo.Close.IsClosed = bool.Parse(fragtData.Tables["FileDone"].Rows[0]["CreateFaktura"].ToString());
                
                fragtInfo.Close.TimeL = fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[0].ToString();
                fragtInfo.Close.TimeA = fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[1].ToString();
                fragtInfo.Close.TimeV = fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[2].ToString();
                fragtInfo.Close.TimeH = fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[3].ToString();
                fragtInfo.Close.DateDay = DateTime.Parse(fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[4].ToString());
                fragtInfo.Close.DateTime = DateTime.Parse(fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[5].ToString());
                fragtInfo.Close.Rabt = fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[6].ToString();
                fragtInfo.Close.Kilometer = fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[7].ToString();

                #endregion


                return fragtInfo;
            }
           
            /// <summary>
            /// Hent data fra file
            /// </summary>
            /// <returns>Fragtbrev Data: Version 2.0.0+</returns>
            private Layout GetFileData_V1(DataSet fragtData)
            {
                Layout fragtInfo = new Layout();

                Models.ImportantData.closeFragtbrevText = fragtData.Tables["FileDone"].Rows[0]["EkstraTekst"].ToString();
                Models.ImportantData.closeFragtbrevBool = bool.Parse(fragtData.Tables["FileDone"].Rows[0]["CreateFaktura"].ToString());

                #region defualt

                fragtInfo.CommentTekst = fragtData.Tables["ekstraComment"].Rows[0].ItemArray[0].ToString();

                //Initialer
                for (int i = 0; i < fragtData.Tables["Initialer"].Rows.Count; i++)
                {
                    string creatorIdS = fragtData.Tables["Initialer"].Rows[i].ItemArray[0].ToString();
                    if (!fragtInfo.Owners.Contains(creatorIdS))
                    {
                        fragtInfo.Owners.Add(creatorIdS);
                    }
                }
                #endregion

                #region Adresse

                fragtInfo.Afsender.Kontakt = fragtData.Tables["Afsender"].Rows[0].ItemArray[0].ToString();
                fragtInfo.Afsender.Firma = fragtData.Tables["Afsender"].Rows[0].ItemArray[1].ToString();
                fragtInfo.Afsender.Adresse = fragtData.Tables["Afsender"].Rows[0].ItemArray[2].ToString();
                fragtInfo.Afsender.Post = fragtData.Tables["Afsender"].Rows[0].ItemArray[3].ToString();
                fragtInfo.Afsender.KontaktPerson = fragtData.Tables["Afsender"].Rows[0].ItemArray[5].ToString();
                fragtInfo.Afsender.Betaler = bool.Parse(fragtData.Tables["Afsender"].Rows[0].ItemArray[4].ToString());
                
                fragtInfo.Modtager.Kontakt = fragtData.Tables["Modtager"].Rows[0].ItemArray[0].ToString();
                fragtInfo.Modtager.Firma = fragtData.Tables["Modtager"].Rows[0].ItemArray[1].ToString();
                fragtInfo.Modtager.Adresse = fragtData.Tables["Modtager"].Rows[0].ItemArray[2].ToString();
                fragtInfo.Modtager.Post = fragtData.Tables["Modtager"].Rows[0].ItemArray[3].ToString();
                fragtInfo.Modtager.KontaktPerson = fragtData.Tables["Modtager"].Rows[0].ItemArray[5].ToString();
                fragtInfo.Modtager.Betaler = bool.Parse(fragtData.Tables["Modtager"].Rows[0].ItemArray[4].ToString());

                #endregion

                #region Generelt
                    
                fragtInfo.Generelt.Refercence = fragtData.Tables["Generelt"].Rows[0].ItemArray[0].ToString();
                fragtInfo.Generelt.Fragtmand = fragtData.Tables["Generelt"].Rows[0].ItemArray[1].ToString();
                fragtInfo.Generelt.Forsikringstype = int.Parse(fragtData.Tables["Generelt"].Rows[0].ItemArray[2].ToString());
                fragtInfo.Generelt.Date1 = fragtData.Tables["Generelt"].Rows[0].ItemArray[3].ToString();
                fragtInfo.Generelt.Rute1 = fragtData.Tables["Generelt"].Rows[0].ItemArray[4].ToString();
                fragtInfo.Generelt.Date2 = fragtData.Tables["Generelt"].Rows[0].ItemArray[5].ToString();
                fragtInfo.Generelt.Rute2 = fragtData.Tables["Generelt"].Rows[0].ItemArray[6].ToString();
                fragtInfo.Generelt.UseEfterkrav = bool.Parse(fragtData.Tables["Generelt"].Rows[0].ItemArray[7].ToString());
                fragtInfo.Generelt.Efterkrav = fragtData.Tables["Generelt"].Rows[0].ItemArray[8].ToString();
                fragtInfo.Generelt.ForSum = fragtData.Tables["Generelt"].Rows[0].ItemArray[9].ToString();
                fragtInfo.Generelt.Praemie = fragtData.Tables["Generelt"].Rows[0].ItemArray[10].ToString();
                fragtInfo.Generelt.All = fragtData.Tables["Generelt"].Rows[0].ItemArray[11].ToString();

                #endregion

                #region Godslinjer

                    //Godslinjer
                fragtInfo.Transport1 = int.Parse(fragtData.Tables["Godslinjer Type"].Rows[0].ItemArray[0].ToString());
                fragtInfo.Transport2 = int.Parse(fragtData.Tables["Godslinjer Type"].Rows[0].ItemArray[1].ToString());
                fragtInfo.Transport3 = int.Parse(fragtData.Tables["Godslinjer Type"].Rows[0].ItemArray[2].ToString());
                if (fragtInfo.Transport1 >= 0 && fragtInfo.Transport2 > 0)
                {
                    int pakkeCount = fragtData.Tables["Godslinjer"].Rows.Count;

                    switch (fragtInfo.Transport1)
                    {
                        case 0: //kurrer
                            if (fragtInfo.Transport3 <= 0)
                            {
                                break;
                            }
                            for (int i = 0; i < pakkeCount; i++)
                            {
                                var godsIndholdItems = new Godslinjer();

                                string adresse, antal, art, indhold, veagt, rumfangt;

                                adresse = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[0].ToString();
                                antal = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[1].ToString();
                                art = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[2].ToString();
                                indhold = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[3].ToString();
                                veagt = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[4].ToString();
                                rumfangt = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[5].ToString();

                                if (!(adresse == "" && (antal == "" || antal == "0") &&
                                    (art == "" || art == "-1") && indhold == "" &&
                                    (veagt == "" || veagt == "0") && rumfangt == ""))
                                {

                                    int antalInt = 0;
                                    double weightDouble = 0;
                                    //set values

                                    godsIndholdItems.Adresse = adresse;
                                    godsIndholdItems.Indhold = indhold;

                                    int.TryParse(antal, out antalInt);
                                    godsIndholdItems.Antal = antalInt;

                                    godsIndholdItems.Art = art;
                                    godsIndholdItems.Rumfang = rumfangt;

                                    double.TryParse(veagt, out weightDouble);
                                    godsIndholdItems.Kilo = weightDouble;

                                    //add pakke
                                    fragtInfo.Godslinjer.Add(godsIndholdItems);
                                }
                            }
                            break;

                        case 1: //Pakke
                        case 2: //Gods
                            for (int i = 0; i < pakkeCount; i++)
                            {
                                var godsIndholdItems = new Godslinjer();

                                string adresse, antal, art, indhold, transport, veagt, rumfangt;

                                adresse = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[0].ToString();
                                antal = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[1].ToString();
                                art = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[2].ToString();
                                indhold = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[3].ToString();
                                transport = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[4].ToString();
                                veagt = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[5].ToString();
                                rumfangt = fragtData.Tables["Godslinjer"].Rows[i].ItemArray[6].ToString();

                                if (!(adresse == "" && (antal == "" || antal == "0") &&
                                    (art == "" || art == "-1") && indhold == "" &&
                                    (transport == "" || transport == "-1") &&
                                    (veagt == "" || veagt == "0") && rumfangt == ""))
                                {

                                    int antalInt = 0;
                                    double weightDouble = 0;
                                    //set values

                                    godsIndholdItems.Adresse = adresse;
                                    godsIndholdItems.Indhold = indhold;
                                    godsIndholdItems.Size = transport;

                                    int.TryParse(antal, out antalInt);
                                    godsIndholdItems.Antal = antalInt;

                                    godsIndholdItems.Art = art;
                                    godsIndholdItems.Rumfang = rumfangt;

                                    double.TryParse(veagt, out weightDouble);
                                    godsIndholdItems.Kilo = weightDouble;

                                    //add pakke
                                    fragtInfo.Godslinjer.Add(godsIndholdItems);
                                }
                            }
                            break;
                    }
                }
                    #endregion Godslinjer

                #region Byttepaller

                fragtInfo.UseByttePalle = bool.Parse(fragtData.Tables["Byttepaller"].Rows[0].ItemArray[0].ToString());
                fragtInfo.Palle1 = fragtData.Tables["Byttepaller"].Rows[0].ItemArray[1].ToString();
                fragtInfo.Palle2 = fragtData.Tables["Byttepaller"].Rows[0].ItemArray[2].ToString();
                fragtInfo.Palle3 = fragtData.Tables["Byttepaller"].Rows[0].ItemArray[3].ToString();

                #endregion

                #region Afslut fragtbrev
                fragtInfo.Close.IsClosed = bool.Parse(fragtData.Tables["FileDone"].Rows[0]["CreateFaktura"].ToString());
                
                fragtInfo.Close.TimeL = fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[0].ToString();
                fragtInfo.Close.TimeA = fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[1].ToString();
                fragtInfo.Close.TimeV = fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[2].ToString();
                fragtInfo.Close.TimeH = fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[3].ToString();
                fragtInfo.Close.DateDay = DateTime.Parse(fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[4].ToString());
                fragtInfo.Close.DateTime = DateTime.Parse(fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[5].ToString());
                fragtInfo.Close.Rabt = fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[6].ToString();
                fragtInfo.Close.Kilometer = fragtData.Tables["AfslutFragtBrev"].Rows[0].ItemArray[7].ToString();

                #endregion


                return fragtInfo;
            }
        }
        public class Customer
        {
            public class Layout
            {
                public int SaveUse = 5;
                public int TableId = 0;
                public int RowId = 0;
                public string ContactTlf = "";
                public string FirmName = "";
                public string ContactPer = "";
                public string Address = "";
                public string ZipCode = "";
            }
            
            public void SetTextBox(object[] textboxs, string seletorFirma)
            {
                //find valgte kunde
                List<Layout> customList = ReadCustomer();

                for (int i = 0; i < customList.Count; i++)
                {
                    if (seletorFirma == customList[i].FirmName)
                    {
                        (textboxs[0] as System.Windows.Controls.TextBox).Text = customList[i].ContactTlf;
                        (textboxs[1] as System.Windows.Controls.TextBox).Text = customList[i].FirmName;
                        (textboxs[2] as System.Windows.Controls.TextBox).Text = customList[i].ContactPer;
                        (textboxs[3] as System.Windows.Controls.TextBox).Text = customList[i].Address;
                        (textboxs[4] as System.Windows.Controls.TextBox).Text = customList[i].ZipCode;
                        break;
                    }
                }
            }
            public void SaveCustomer(Layout customerInfo, string folder = null)
            {
                string saveFolderFile = "";
                string saveFolder = "";

                if (folder == null)
                {
                    saveFolderFile = Models.ImportantData.g_FolderData + "KundeInfo.xml";
                    saveFolder = Models.ImportantData.g_FolderData;
                }
                else
                {
                    saveFolderFile = folder + "KundeInfo.xml";
                    saveFolder = folder;
                }

                #region Tjek om kunden findes i database

                //Hent fil info
                string inputKonIdNoSpace = customerInfo.ContactTlf.Replace(" ", "");
                List<Layout> readCustomer = this.ReadCustomer(saveFolder);
                int rowCount = readCustomer.Count;
                int index = -1;

                for (int i = 0; i < rowCount; i++)
                {
                    string saveKonIdNoSpace = readCustomer[i].ContactTlf;

                    if (inputKonIdNoSpace == saveKonIdNoSpace ||
                        customerInfo.FirmName.ToLower() == readCustomer[i].FirmName.ToLower())
                    {
                        index = i;
                        break;
                    }
                }
                #endregion

                DataSet saveDataSet = new DataSet();
                FileStream saveDataStreamR = new FileStream(saveFolderFile, FileMode.Open, FileAccess.Read);
                saveDataSet.ReadXml(saveDataStreamR);
                saveDataStreamR.Close();

                #region Slet hvis den findes
                if (index != -1)
                {
                    saveDataSet.Tables[readCustomer[index].TableId].Rows[readCustomer[index].RowId].Delete();
                }
                #endregion

                #region tilføj kunde
                if (!saveDataSet.Tables.Contains(Models.ImportantData.PVersion.ToString()))
                {
                    saveDataSet.Tables.Add(Models.ImportantData.PVersion.ToString());
                    for (int i = 0; i < customerInfo.SaveUse; i++)
			        {
			            saveDataSet.Tables[Models.ImportantData.PVersion.ToString()].Columns.Add("Info_" + i);
			        }
                    
                }

                saveDataSet.Tables[Models.ImportantData.PVersion.ToString()].Rows.Add(
                    customerInfo.ContactTlf,
                    customerInfo.FirmName,
                    customerInfo.ContactPer,
                    customerInfo.Address,
                    customerInfo.ZipCode
                );
                #endregion

                #region Slet ubrugte Tabeller

                for (int i = 0; i < saveDataSet.Tables.Count; i++)
			    {
			        if (saveDataSet.Tables[i].Rows.Count == 0)
	                {
		                saveDataSet.Tables.RemoveAt(i);
	                }
			    }

                #endregion

                FileStream saveDataStreamW = new FileStream(saveFolderFile, FileMode.Create, FileAccess.Write);
                saveDataSet.WriteXml(saveDataStreamW);
                saveDataStreamW.Close();
            }

            public List<Layout> ReadCustomer(string folder = null)
            {
                string saveFolderFile = "";

                if(folder == null)
                {
                    saveFolderFile = Models.ImportantData.g_FolderDB + "KundeInfo.xml";
                }
                else
	            {
                    saveFolderFile = folder + "KundeInfo.xml";
	            }
                

                if (!File.Exists(saveFolderFile))
                {
                    DataSet customerCreateDS = new DataSet();
                    customerCreateDS.DataSetName = "KundeInfo";
                    FileStream customerDataStreamW = new FileStream(saveFolderFile, FileMode.CreateNew, FileAccess.Write);
                    customerCreateDS.WriteXml(customerDataStreamW);
                    customerDataStreamW.Close();
                }


                DataSet cutomerDataSet = new DataSet();
                FileStream cutomerDataStreamR = new FileStream(saveFolderFile, FileMode.Open, FileAccess.Read);
                cutomerDataSet.ReadXml(cutomerDataStreamR);
                cutomerDataStreamR.Close();

                List<List<Layout>> customerAllList = new List<List<Layout>>();

                if (new Version("2.0.0") <= Models.ImportantData.PVersion)
                {
                    customerAllList.Add(GetSaveLayout_V1(cutomerDataSet));
                }


                //lave om så det kun vil være en liste
                List<Layout> layoutListFinal = new List<Layout>();

                int listCount = customerAllList.Count;
                for (int i = 0; i < listCount; i++)
                {
                    int layoutCount = customerAllList[i].Count;

                    for (int a = 0; a < layoutCount; a++)
                    {
                        layoutListFinal.Add(customerAllList[i][a]);
                    }
                }

                return layoutListFinal;
            }
            private List<Layout> GetSaveLayout_V1(DataSet customerData)
            {
                int allowColumn = 5;
                List<Layout> customerList = new List<Layout>();

                //Hent fil info
                int tableCount = customerData.Tables.Count;
                for (int i = 0; i < tableCount; i++)
			    {
			        int rowCount = customerData.Tables[i].Rows.Count;
                    int columnCount = customerData.Tables[i].Columns.Count;

			        if (columnCount == allowColumn)
	                {
                        for (int a = 0; a < rowCount; a++)
                        {
                            Layout newCustomerL = new Layout();
                            newCustomerL.TableId = i;
                            newCustomerL.RowId = a;

                            newCustomerL.ContactTlf = customerData.Tables[i].Rows[a]["Info_0"].ToString();
                            newCustomerL.FirmName = customerData.Tables[i].Rows[a]["Info_1"].ToString();
                            newCustomerL.ContactPer = customerData.Tables[i].Rows[a]["Info_2"].ToString();
                            newCustomerL.Address = customerData.Tables[i].Rows[a]["Info_3"].ToString();
                            newCustomerL.ZipCode = customerData.Tables[i].Rows[a]["Info_4"].ToString();

                            customerList.Add(newCustomerL);
                        }
                    }
                }

                return customerList;
            }
        }

        public class FileInfoKeyClass
        {
            public int Type_Fragt { get { return 0; } }
            public int Type_Faktura { get { return 1; } }

            public int Status_Open { get { return 0; } }
            public int Status_Middle { get { return 1; } }
            public int Status_Closed { get { return 2; } }


        }

        public class FileInfoClass
        {
            private FileInfoKeyClass keyvalues = new FileInfoKeyClass();

            private string priName;
            private int priFileType;
            private int priStatus;
            private int priPostA;
            private int priPostM;
            private double priPrice;
            private int priMonthNo;
            private string priMonthName;
            private int priYear;
            private DateTime priCreateDate;
            private DateTime priCreateDateXML;
            private List<string> priCreator;
            private string priKontaktP_Afsend;
            private string priKontaktP_Modtag;
            private int priInvoice;

            public string Filename { get { return priName; } }
            public string KontaktPersonAfsend
            {
                get
                {
                    string varName = "None";
                    if (priFileType == keyvalues.Type_Fragt)
                    {
                        varName = priKontaktP_Afsend;
                    }
                    return varName;
                }
            }
            public string KontaktPersonModtag
            {
                get
                {
                    string varName = "None";
                    if (priFileType == keyvalues.Type_Fragt)
                    {
                        varName = priKontaktP_Modtag;
                    }
                    return varName;
                }
            }
            public int FileType { get { return priFileType; } }
            public int Status { get { return priStatus; } }
            public int PostAfsend { get { return priPostA; } }
            public int PostModtag { get { return priPostM; } }
            public double Price
            {
                get
                {
                    double varPrice = -1;
                    if (priFileType == keyvalues.Type_Faktura && priStatus == keyvalues.Status_Closed)
                    {
                        varPrice = priPrice;
                    }
                    return varPrice;
                }
            }
            public int CreateMonth { get { return priMonthNo; } }
            public string CreateMonthName { get { return priMonthName; } }
            public int CreateYear { get { return priYear; } }
            public DateTime CreateDatePDF { get { return priCreateDate; } }
            public DateTime CreateDateXML { get { return priCreateDateXML; } }
            public List<string> FileOwners { get { return priCreator; } }
            public int Invoice { get { return priInvoice; } }

            public List<FileInfoClass> GetFileList()
            {
                List<FileInfoClass> allFileList = new List<FileInfoClass>();

                string FolderPDF = Models.ImportantData.g_FolderPdf;
                string FolderXML = Models.ImportantData.g_FolderSave;

                //hent fil info
                List<string> fileNamesPdf = Directory.GetFiles(FolderPDF).Select(path => System.IO.Path.GetFileName(path)).ToList();
                List<string> fileNamesSave = Directory.GetFiles(FolderXML).Select(path => System.IO.Path.GetFileName(path)).ToList();
                List<DateTime> createDatePDF = Directory.GetFiles(FolderPDF).Select(path => File.GetLastWriteTime(path)).ToList();
                List<DateTime> createDateXML = Directory.GetFiles(FolderXML).Select(path => File.GetLastWriteTime(path)).ToList();

                #region Opret info for en fil

                int fileCount = fileNamesSave.Count();

                for (int i = 0; i < fileCount; i++)
                {
                    FileInfoClass newFileInfo = new FileInfoClass();

                    //Filnavn uden fil type (.pdf/.xml)
                    string filename = fileNamesSave[i].Substring(0, fileNamesSave[i].Length - 4);

                    //om der er lavet en pdf version af filen
                    bool pdfFound = false;
                    int pdfIndex = 0;
                    if (fileNamesPdf.Contains(filename + ".pdf"))
                    {
                        pdfIndex = fileNamesPdf.FindIndex(fileItem => fileItem == filename + ".pdf");
                        pdfFound = true;
                    }

                    //hvad type fil det er (fragtbrev/faktura)
                    int filetypeId = 0;
                    string[] FileTypeName = { "Fragtbrev-", "Faktura-" };
                    bool[] fileTypeBool = new bool[FileTypeName.Count()];
                    for (int a = 0; a < FileTypeName.Count(); a++)
                    {
                        if (filename.StartsWith(FileTypeName[a]))
                        {
                            filetypeId = a;
                            fileTypeBool[a] = true;
                        }
                        else
                        {
                            fileTypeBool[a] = false;
                        }
                    }

                    //hent hvem der har lavet eller ændret på den
                    FileStream readFileStream = new FileStream(FolderXML + fileNamesSave[i], FileMode.Open, FileAccess.Read);
                    DataSet readXML = new DataSet();
                    readXML.ReadXml(readFileStream);
                    readFileStream.Close();

                    List<string> creatorPerson = new List<string>();
                    int creatorPersonCount = readXML.Tables["Initialer"].Rows.Count;

                    for (int a = 0; a < creatorPersonCount; a++)
                    {
                        creatorPerson.Add(readXML.Tables["Initialer"].Rows[0].ItemArray[0].ToString());
                    }


                    //faktura hent prisen
                    //Fragtbrev ellers find ud af om fragtbrevet er lukket
                    double price = 0;
                    bool fragtDone = false;
                    bool faktDone = false;
                    string kontaktP_Afsend = "None";
                    string kontaktP_Modtag = "None";
                    int post_Afsend = -1;
                    int post_Modtag = -1;
                    int invoice = -1;
                    if (fileTypeBool[1])
                    {
                        price = double.Parse(readXML.Tables["FakturaInfo"].Rows[0].ItemArray[0].ToString());
                        try
                        {
                            faktDone = bool.Parse(readXML.Tables["FakturaInfo"].Rows[0].ItemArray[1].ToString());
                        }
                        catch (Exception)
                        {
                        }

                        invoice = int.Parse(filename.Replace("Faktura-", ""));

                    }
                    else
                    {
                        int postA = -1;
                        int postM = -1;

                        fragtDone = bool.Parse(readXML.Tables["FileDone"].Rows[0].ItemArray[0].ToString());
                        kontaktP_Afsend = readXML.Tables["Afsender"].Rows[0].ItemArray[5].ToString();

                        int.TryParse(readXML.Tables["Afsender"].Rows[0].ItemArray[3].ToString(), out postA);
                        post_Afsend = postA;
                        kontaktP_Modtag = readXML.Tables["Modtager"].Rows[0].ItemArray[5].ToString();
                        int.TryParse(readXML.Tables["Modtager"].Rows[0].ItemArray[3].ToString(), out postM);
                        post_Modtag = postM;


                        invoice = int.Parse(filename.Replace("Fragtbrev-", ""));
                    }

                    //Find ud af hvad status er for fillen
                    int status = 0;

                    if (pdfFound)
                    {
                        if ((fileTypeBool[0] && !fragtDone) || (fileTypeBool[1] && !faktDone))
                        {
                            status = 1;
                        }
                        else
                        {
                            status = 2;
                        }
                    }

                    //Find opret dato, måned og år
                    DateTime createdate = DateTime.MinValue;
                    DateTime createdateSave = createDateXML[i];
                    if (pdfFound)
                    {
                        createdate = createDatePDF[pdfIndex];
                    }
                    int monthNo = createdate.Month;
                    string monthName = createdate.ToString("MMMM");
                    int year = createdate.Year;

                    //Set data
                    newFileInfo.priName = filename;
                    newFileInfo.priFileType = filetypeId;
                    newFileInfo.priStatus = status;
                    newFileInfo.priKontaktP_Afsend = kontaktP_Afsend;
                    newFileInfo.priKontaktP_Modtag = kontaktP_Modtag;
                    newFileInfo.priPostA = post_Afsend;
                    newFileInfo.priPostM = post_Modtag;
                    newFileInfo.priPrice = price;
                    newFileInfo.priMonthNo = monthNo;
                    newFileInfo.priMonthName = monthName;
                    newFileInfo.priYear = year;
                    newFileInfo.priCreateDate = createdate;
                    newFileInfo.priCreateDateXML = createdateSave;
                    newFileInfo.priCreator = creatorPerson;
                    newFileInfo.priInvoice = invoice;

                    allFileList.Add(newFileInfo);
                }
                #endregion

                //Send liste tilbage
                return allFileList;
            }

        }
    }
}
