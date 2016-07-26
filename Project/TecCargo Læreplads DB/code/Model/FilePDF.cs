using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace TecCargo_Læreplads_DB.Model
{
    class FilePDF
    {
        //Hvor dagbogen gemmes og hvad den hedder
        public static string FileName = "General\\printToStudents.pdf";

        /// <summary>
        /// Opretter en læreplads som pdf 
        /// så man senere kan printe den
        /// </summary>
        public void CreateDocument() 
        {
            FileStream documentCreate = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, documentCreate);

            PdfPTable document = makePdfTable(); //Tilføj indhold
            document.WidthPercentage = 100;

            //lav document
            doc.Open();
            doc.Add(document);
            doc.Close();
            writer.Close();
            documentCreate.Close();
        }

        /// <summary>
        /// Opretter læreplads layout
        /// </summary>
        private PdfPTable makePdfTable()
        {
            Model.FileClass.fileInput fileInfo = Inc.Settings.fileInput;

            int[] paddingTextLeft = new int[]{10,0,0,0};
            PdfPCell whiteSpace = new PdfPCell();
            whiteSpace.Border = 0;
            whiteSpace.FixedHeight = 10;

            #region virsomhed info

            PdfPTable virksomhedAdresse = new PdfPTable(1);
            virksomhedAdresse.DefaultCell.Border = 0;

            virksomhedAdresse.AddCell(InsertText("Virksomhed", true));
            virksomhedAdresse.AddCell(InsertText(fileInfo.name, false, paddingTextLeft));
            virksomhedAdresse.AddCell(InsertText(fileInfo.adresse, false, paddingTextLeft));
            virksomhedAdresse.AddCell(InsertText(fileInfo.tjenestested, false, paddingTextLeft));

            PdfPTable virksomhedProfil = new PdfPTable(1);
            virksomhedProfil.DefaultCell.Border = 0;

            if (fileInfo.profil.Trim().Length != 0)
            {
                virksomhedProfil.AddCell(InsertText("Virksomhedsprofil", true));
                virksomhedProfil.AddCell(InsertText(fileInfo.profil, false, paddingTextLeft));   
            }

            PdfPTable virksomhed = new PdfPTable(1);
            virksomhed.DefaultCell.Border = 0;

            virksomhed.AddCell(ConvertTableToCell(virksomhedAdresse));
            virksomhed.AddCell(whiteSpace);
            virksomhed.AddCell(ConvertTableToCell(virksomhedProfil));

            #endregion

            #region Kontaktperson

            PdfPTable kontaktInfo = new PdfPTable(4);
            kontaktInfo.SetWidths(new int[] { 2,1,2,2 });
            kontaktInfo.DefaultCell.Border = 0;
            kontaktInfo.AddCell(InsertText("kontaktperson", true));
            kontaktInfo.AddCell("");
            kontaktInfo.AddCell("");
            kontaktInfo.AddCell("");

            foreach (var item in fileInfo.contactPerson)
            {
                if (item.print)
                {
                    kontaktInfo.AddCell(InsertText(item.name, false, paddingTextLeft));
                    kontaktInfo.AddCell(InsertText(item.mobile, false, paddingTextLeft));
                    kontaktInfo.AddCell(InsertText(item.mail, false, paddingTextLeft));
                    kontaktInfo.AddCell(InsertText(item.post, false, paddingTextLeft));
                }
            }
            #endregion

            #region krav

            PdfPTable language = new PdfPTable(1);
            language.DefaultCell.Border = 0;
            language.AddCell(InsertText("Sproglige kundskaber og færdigheder:",true));

            foreach (var item in fileInfo.language)
            {
                language.AddCell(InsertText(item, false, paddingTextLeft));
            }

            PdfPTable math = new PdfPTable(1);
            math.DefaultCell.Border = 0;
            math.AddCell(InsertText("Matematiske kundskaber og færdigheder:",true));
            math.AddCell(InsertText(fileInfo.math, false, paddingTextLeft));

            PdfPTable elevInfo = new PdfPTable(2);
            elevInfo.DefaultCell.Border = 0;

            string recordText = "";
            for (int i = 0; i < fileInfo.record.Count; i++)
            {
                if (i != 0)
                {
                    recordText += ", ";
                }
                recordText += fileInfo.record[i];
            }

            elevInfo.AddCell(InsertTextExpert("<b>Alder: </b>" + fileInfo.age));
            elevInfo.AddCell(InsertTextExpert("<b>Kørekort: </b>" + fileInfo.driverLicense));
            elevInfo.AddCell(InsertTextExpert("<b>Straffeattest: </b>" + recordText));
            elevInfo.AddCell(InsertTextExpert("<b>Fysiske forudsætninger: </b>" + fileInfo.physical));

            elevInfo.AddCell(whiteSpace);
            elevInfo.AddCell(whiteSpace);

            elevInfo.AddCell(ConvertTableToCell(language));
            elevInfo.AddCell(ConvertTableToCell(math));

            #endregion

            #region Godkendt

            #region variable

            string[][] godkentCheckbox = new string[][] {
                new string[] {"lagerHelp","Lagermedhjælper", "0","0"},
                new string[] {"lagerLogistik","Lager og logistik", "0","1"},
                new string[] {"logerTransport","Lager og transport", "0","1"},
                new string[] {"lastbil","Lastbilchauffør", "1","2"},
                new string[] {"lastbilGods","Godschauffør", "1","3"},
                new string[] {"lastbilFlytte","Flyttechauffør", "1","3"},
                new string[] {"lastbilRenovation","Renovationschauffør", "1","3"},
                new string[] {"lastbilKran","Kranfører", "1","3"},
                new string[] {"lufthavn","Transportarbejder i lufthavn", "2","4"},
                new string[] {"lufthavnBagage","Bagage", "2","5"},
                new string[] {"lufthavnCargo","Cargo", "2","5"},
                new string[] {"lufthavnAircraft","Aircraft servicing", "2","5"},
                new string[] {"lufthavnAirport","Airport servicing<", "2","5"},
                new string[] {"lufthavnBrand","Brand og redning", "2","5"},
                new string[] {"lufthavnFuel","Fuel", "2","5"},
                new string[] {"lufthavnClean","Cleaning", "2","5"},
                new string[] {"lufthavnGround","Groundhandling", "2","5"},
                new string[] {"lufthavnRampe","Rampeservice", "2","5"}
            };

            string[] godkentCategory = new string[] { 
                "<u>Lager: "+fileInfo.uddannelses_lager+"</u>",
                "Lageroperatør med specialet:",
                "<u>Chauffør: "+fileInfo.uddannelses_chauffor+"</u>",
                "Lastbilchauffør med specialet:",
                "<u>Lufthavn: "+fileInfo.uddannelses_lufthavn+"</u>",
                "Lufthavnsoperatør med specialet:",
            };


            List<List<string>> godkendtList = new List<List<string>>();
            godkendtList.Add(new List<string>());
            godkendtList.Add(new List<string>());
            godkendtList.Add(new List<string>());
            List<int> godkendtStatus = new List<int>();
            List<int> paddingStatus = new List<int>();
            int[][] paddingVal = new int[][] { new int[] { 0, 3, 0, 0 }, paddingTextLeft };
            #endregion

            PdfPTable godkendt = new PdfPTable(3);
            godkendt.DefaultCell.Border = 0;

            if (fileInfo.uddannelsesCheckBoxs.Count > 0)
            {
                godkendt.AddCell(InsertText("Godkendt til:", true));
                godkendt.AddCell("");
                godkendt.AddCell("");

                foreach (var checkboxInfo in godkentCheckbox)
                {
                    if (fileInfo.uddannelsesCheckBoxs.Contains(checkboxInfo[0]))
                    {
                        int listIndex = int.Parse(checkboxInfo[2]);
                        int statusIndex = int.Parse(checkboxInfo[3]);

                        if (!godkendtStatus.Contains(statusIndex))
                        {
                            if (!godkendtStatus.Contains(statusIndex - 1) && (statusIndex - 1 == 0 || statusIndex - 1 == 2 || statusIndex - 1 == 4))
                            {
                                godkendtStatus.Add(statusIndex - 1);
                                godkendtList[listIndex].Add(godkentCategory[statusIndex - 1]);
                                paddingStatus.Add(0);
                                godkendtList[listIndex].Add("$$SPACE$$");
                                paddingStatus.Add(0);
                            }
                            godkendtStatus.Add(statusIndex);
                            godkendtList[listIndex].Add(godkentCategory[statusIndex]);
                            paddingStatus.Add(0);
                            godkendtList[listIndex].Add("$$SPACE$$");
                            paddingStatus.Add(0);
                        }

                        godkendtList[listIndex].Add(checkboxInfo[1]);
                        paddingStatus.Add(1);
                    }
                }

                int tableAdd = 0;
                int paddingNumb = 0;
                for (int i = 0; i < 3; i++)
                {
                    bool valuesFound = false;
                    PdfPTable newTable = new PdfPTable(1);
                    newTable.DefaultCell.Border = 0;

                    foreach (var checkboxContent in godkendtList[i])
                    {
                        if (checkboxContent == "$$SPACE$$")
                        {
                            newTable.AddCell(whiteSpace);
                        }
                        else
                        {
                            valuesFound = true;
                            //System.Windows.MessageBox.Show(checkboxContent+" - "+paddingStatus[paddingNumb] + " -- " +paddingNumb);
                            newTable.AddCell(InsertTextExpert(checkboxContent, paddingVal[paddingStatus[paddingNumb]]));
                        }
                        paddingNumb++;
                    }

                    if (valuesFound)
                    {
                        tableAdd++;
                        godkendt.AddCell(ConvertTableToCell(newTable));
                    }
                }

                //gør så der ikke er et felt der ikke er udflydt
                for (int i = tableAdd; i < 3; i++)
                {
                    godkendt.AddCell("");
                }
            }

            #endregion

            #region Aftale Type

            #region variable

            string[][] aftaleCheckbox = new string[][] {
                new string[] {"checkbox_aml","Almindelig aftale"},
                new string[] {"checkbox_rest","Restaftale"},
                new string[] {"checkbox_kort","Kort aftale"},
                new string[] {"checkbox_kombi","Kombinationsaftale"},
                new string[] {"checkbox_mester","Ny mesterlære"},
                new string[] {"checkbox_del","Delaftale"}
            };

            List<string> aftaleList = new List<string>();

            #endregion

            PdfPTable aftale = new PdfPTable(3);
            aftale.DefaultCell.Border = 0;

            if (fileInfo.agreementType.Count > 0)
            {
                aftale.AddCell(InsertText("Uddannelsesaftale Type:", true));
                aftale.AddCell("");
                aftale.AddCell("");



                foreach (var aftaleInfo in aftaleCheckbox)
                {
                    if (fileInfo.agreementType.Contains(aftaleInfo[0]))
                    {

                        aftaleList.Add(aftaleInfo[1]);
                    }
                }


                int tableAddAftale = (int)(Math.Ceiling(aftaleList.Count / 3f) * 3) - aftaleList.Count - 1;
                foreach (var checkboxContent in aftaleList)
                {
                    aftale.AddCell(InsertTextExpert(checkboxContent, paddingTextLeft));
                }

                //gør så der ikke er et felt der ikke er udflydt
                for (int i = tableAddAftale; i < 3; i++)
                {
                    aftale.AddCell("");
                }
            }
            #endregion

            #region Virk onsker

            #region variable

            string[][] virkOnskerCheckbox = new string[][] {
                new string[] {"checkbox_motiveret","Motiveret ansøgning"},
                new string[] {"checkbox_cv","Curriculum vitae (CV)"},
                new string[] {"checkbox_perTele","Personligt telefonopkald"},
                new string[] {"checkbox_perMeet","Personligt fremmøde"},
                new string[] {"checkbox_andet",fileInfo.firmReceive.andet},
            };


            #endregion

            PdfPTable virkOnsker = new PdfPTable(1);
            aftale.DefaultCell.Border = 0;

            if (fileInfo.firmReceive.Checkbox.Count > 0)
            {
                virkOnsker.AddCell(InsertText("Virksomheden ønsker at modtage:", true));

                foreach (var CheckboxContent in virkOnskerCheckbox)
                {
                    if (fileInfo.firmReceive.Checkbox.Contains(CheckboxContent[0]))
                    {
                        virkOnsker.AddCell(InsertTextExpert(CheckboxContent[1], paddingTextLeft));
                    }
                }
            }
            #endregion

            #region Lav Table

            PdfPTable document = new PdfPTable(1);
            document.DefaultCell.Border = 0;

            document.AddCell(ConvertTableToCell(virksomhed));

            if (fileInfo.jobDescription.Trim().Length != 0)
            {
                document.AddCell(whiteSpace);
                document.AddCell(InsertText("Jobbeskrivelse:", true));
                document.AddCell(InsertText(fileInfo.jobDescription, false, paddingTextLeft));
            }
            
            document.AddCell(whiteSpace);
            document.AddCell(ConvertTableToCell(godkendt));

            document.AddCell(whiteSpace);
            document.AddCell(ConvertTableToCell(aftale));
            
            document.AddCell(whiteSpace);
            document.AddCell(ConvertTableToCell(elevInfo));

            document.AddCell(whiteSpace);
            document.AddCell(ConvertTableToCell(virkOnsker));

            if (fileInfo.other.Trim().Length != 0)
            {
                document.AddCell(whiteSpace);
                document.AddCell(InsertText("Evt. bemærkninger:", true));
                document.AddCell(InsertText(fileInfo.other, false, paddingTextLeft));
            }

            if (fileInfo.contactPerson.Count != 0)
            {
                document.AddCell(whiteSpace);
                document.AddCell(ConvertTableToCell(kontaktInfo));
            }

            document.AddCell(whiteSpace);
            document.AddCell(InsertTextExpert("<b>Ansøgningsfrist: </b>" + fileInfo.firmReceive.lastDay));
            document.AddCell(InsertTextExpert("<b>Start dato for uddannelsesaftale: </b>" + fileInfo.firmReceive.StartDate));
            document.AddCell(InsertTextExpert("<b>Antal kandidater ønskes i udvælgelse processen: </b>" + fileInfo.firmReceive.antalPersoner));

            #endregion

            return document;
        }

        /// <summary>
        /// Laver en table om til en cell uden border
        /// </summary>
        private PdfPCell ConvertTableToCell(PdfPTable table) {

            PdfPCell cellTable = new PdfPCell(table);
            cellTable.Border = 0;

            return cellTable;
        }

        /// <summary>
        /// Tilføj noget tekst hurtigt
        /// </summary>
        private PdfPCell InsertText(string text, bool bold = false, int[] padding = null)
        {
            Font textStyle = new Font(Font.FontFamily.HELVETICA, 12f);

            if (bold)
            {
                textStyle.SetStyle(Font.BOLD);
            }

            PdfPCell textCell = new PdfPCell(new Phrase(text, textStyle));
            textCell.Border = 0;

            if (padding != null)
            {
                textCell.PaddingLeft = padding[0];
                textCell.PaddingTop = padding[1];
                textCell.PaddingRight = padding[2];
                textCell.PaddingBottom = padding[3];
            }


            return textCell;
        }

        /// <summary>
        /// Tilføj noget tekst hurtigt
        /// 
        /// med text coder så man kan lave noget af teksten fed eller andet
        /// </summary>
        private PdfPCell InsertTextExpert(string text, int[] padding = null)
        {

            Font fontNormal = new Font(Font.FontFamily.HELVETICA, 12f);
            Font fontBold = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD);
            Font fontUnderline = new Font(Font.FontFamily.HELVETICA, 12f, Font.UNDERLINE);
            Font fontItalic = new Font(Font.FontFamily.HELVETICA, 12f, Font.ITALIC);
            Phrase phraseText = new Phrase();

            bool tagFound = false;
            string[] tagStart = new string[] { "<b>", "<u>", "<i>","<p>" };
            string[] tagEnd = new string[] { "</b>", "</u>", "</i>","</p>" };
            int tagStartFound = -1;
            int tagEndFound = -1;
            string textValue = "";

            //<b>Test</b>Hallo
            //så der ikke sker fejl ved at bruge substring
            text += "0000";
            for (int i = 0; i < text.Length-4; i++)
            {
                string startTag = text.Substring(i, 3);
                string endTag = text.Substring(i, 4);

                //System.Windows.MessageBox.Show(text + "\n" +
                //    "StartTag: " + startTag + "\n" +
                //    "EndTag: "+endTag+"\n" +
                //"Tagfound: "+tagFound+"\n" +
                //"StartTag ID: "+tagStartFound+ "\n" +
                //"EndTag ID: " + tagEndFound + "\n");

                if (!tagFound && tagStart.Contains(startTag))
                {
                    i += 2; //ungå at få tag med
                    tagStartFound = Array.IndexOf(tagStart, startTag);

                    if (textValue.Length > 0)
                    {
                        phraseText.Add(new Chunk(textValue, fontNormal));
                        textValue = "";
                    }

                    tagFound = true;
                }
                else if (tagFound && tagEnd.Contains(endTag))
                {
                    i += 3; //ungå at få tag med
                    tagEndFound = Array.IndexOf(tagEnd, endTag);

                    //hvis det er det samme tag tillad at luk
                    if (tagStartFound == tagEndFound) 
                    { 
                        tagFound = false;


                        if (textValue.Length > 0)
                        {
                            Font tagFont = fontNormal;
                            switch (tagStartFound)
                            {
                                case 0:
                                    tagFont = fontBold;
                                    break;
                                case 1:
                                    tagFont = fontUnderline;
                                    break;
                                case 2:
                                    tagFont = fontItalic;
                                    break;
                                case 3:
                                    tagFont = fontItalic;
                                    break;
                            }
                            phraseText.Add(new Chunk(textValue, tagFont));
                            textValue = "";
                        }
                    }
                    

                    
                }
                else //tilføj tekst
                {
                    //System.Windows.MessageBox.Show(i.ToString() + " < " + (text.Length - 4).ToString() + "\n" + i.ToString() + " == " + (text.Length - 5).ToString());
                    
                    textValue += text.Substring(i, 1);
                }
            }

            //Hvis der ikke blev fundet nogle tag
            if (textValue.Length != 0) 
            {
                phraseText.Add(new Chunk(textValue, fontNormal));
            }


            PdfPCell textCell = new PdfPCell(phraseText);
            textCell.Border = 0;

            if (padding != null)
            {
                textCell.PaddingLeft = padding[0];
                textCell.PaddingTop = padding[1];
                textCell.PaddingRight = padding[2];
                textCell.PaddingBottom = padding[3];
            }


            return textCell;
        }
    }
}
