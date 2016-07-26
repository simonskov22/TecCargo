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
            /// Gem faktura til nyeste version
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
            /// Læs fraktura alle versioner
            /// </summary>
            public Layout ReadFile(string filename)
            {
                Layout fakturaInfo = new Layout();

                //hent fil xml data
                string folder = Models.ImportantData.g_FolderSave;
                FileStream documentRead = new FileStream(folder + filename + ".xml", FileMode.Open, FileAccess.Read);
                DataSet fakturaData = new DataSet();
                fakturaData.ReadXml(documentRead);
                documentRead.Close();

                //tjek om man kan læse denne fil version
                string saveFileVersion = fakturaData.Tables["Version"].Rows[0].ItemArray[0].ToString();

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
            /// gemt fragtbrev til nyeste version
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
                public int SaveUse { get { return 5; } }
                public int TableId { get; set; }
                public int RowId { get; set; }
                public string KontaktId { get; set; }
                public string Firma { get; set; }
                public string KontaktPerson { get; set; }
                public string Adresse { get; set; }
                public string Post { get; set; }
            }
            /// <summary>
            /// auto udflyde tesktbokse med data om 
            /// en kunde
            /// </summary>
            public void SetTextBox(object[] textboxs, string seletorFirma)
            {
                //find valgte kunde
                List<Layout> customList = ReadCustomer();

                for (int i = 0; i < customList.Count; i++)
                {
                    if (seletorFirma == customList[i].Firma)
                    {
                        (textboxs[0] as System.Windows.Controls.TextBox).Text = customList[i].KontaktId;
                        (textboxs[1] as System.Windows.Controls.TextBox).Text = customList[i].Firma;
                        (textboxs[2] as System.Windows.Controls.TextBox).Text = customList[i].KontaktPerson;
                        (textboxs[3] as System.Windows.Controls.TextBox).Text = customList[i].Adresse;
                        (textboxs[4] as System.Windows.Controls.TextBox).Text = customList[i].Post;
                        break;
                    }
                }
            }
            
            /// <summary>
            /// gem data fra tekstbokse
            /// hvis det ikke findes
            /// </summary>
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
                string inputKonIdNoSpace = customerInfo.KontaktId.Replace(" ", "");
                List<Layout> readCustomer = this.ReadCustomer(saveFolder);
                int rowCount = readCustomer.Count;
                int index = -1;

                for (int i = 0; i < rowCount; i++)
                {
                    string saveKonIdNoSpace = readCustomer[i].KontaktId;

                    if (inputKonIdNoSpace == saveKonIdNoSpace ||
                        customerInfo.Firma.ToLower() == readCustomer[i].Firma.ToLower())
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
                    customerInfo.KontaktId,
                    customerInfo.Firma,
                    customerInfo.KontaktPerson,
                    customerInfo.Adresse,
                    customerInfo.Post
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

            /// <summary>
            /// hent alle kunder i databasen
            /// </summary>
            public List<Layout> ReadCustomer(string folder = null)
            {
                //kunde fil pladsering
                string saveFolderFile = "";

                if(folder == null)
                {
                    saveFolderFile = Models.ImportantData.g_FolderData + "KundeInfo.xml";
                }
                else
	            {
                    saveFolderFile = folder + "KundeInfo.xml";
	            }
                
                //hvis filen ikke findes opret den
                if (!File.Exists(saveFolderFile))
                {
                    DataSet customerCreateDS = new DataSet();
                    customerCreateDS.DataSetName = "KundeInfo";
                    FileStream customerDataStreamW = new FileStream(saveFolderFile, FileMode.CreateNew, FileAccess.Write);
                    customerCreateDS.WriteXml(customerDataStreamW);
                    customerDataStreamW.Close();
                }

                //hent data fra fil
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
            
            /// <summary>
            /// læse kunde data fra dataset
            /// (version 2.0.0+)
            /// </summary>
            private List<Layout> GetSaveLayout_V1(DataSet customerData)
            {
                List<Layout> customerList = new List<Layout>();

                //Hent fil info
                int tableCount = customerData.Tables.Count;
                for (int i = 0; i < tableCount; i++)
			    {
			        int rowCount = customerData.Tables[i].Rows.Count;
                    int columnCount = customerData.Tables[i].Columns.Count;

                    //hent alle kunde informationer
                    //og tilføj dem til en liste
                    for (int a = 0; a < rowCount; a++)
                    {
                        Layout newCustomerL = new Layout();
                        newCustomerL.TableId = i;
                        newCustomerL.RowId = a;

                        newCustomerL.KontaktId = customerData.Tables[i].Rows[a]["Info_0"].ToString();
                        newCustomerL.Firma = customerData.Tables[i].Rows[a]["Info_1"].ToString();
                        newCustomerL.KontaktPerson = customerData.Tables[i].Rows[a]["Info_2"].ToString();
                        newCustomerL.Adresse = customerData.Tables[i].Rows[a]["Info_3"].ToString();
                        newCustomerL.Post = customerData.Tables[i].Rows[a]["Info_4"].ToString();

                        customerList.Add(newCustomerL);
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


            /// <summary>
            /// hent information data
            /// for fragtbreve og fakture
            /// 
            /// (bliver ikke brugt)
            /// </summary>
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
                    

                    FileStream readFileStream = new FileStream(FolderXML + fileNamesSave[i], FileMode.Open, FileAccess.Read);
                    DataSet readXML = new DataSet();
                    readXML.ReadXml(readFileStream);
                    readFileStream.Close();

                    List<string> creatorPerson = new List<string>();
                    int creatorPersonCount = readXML.Tables["Initialer"].Rows.Count;

                    for (int a = 0; a < creatorPersonCount; a++)
                    {
                        creatorPerson.Add(readXML.Tables["Initialer"].Rows[a].ItemArray[0].ToString());
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
