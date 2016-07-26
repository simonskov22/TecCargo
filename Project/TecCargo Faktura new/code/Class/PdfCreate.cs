using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;



namespace TecCargo_Faktura.Class
{
    class PdfCreate
    {
        class TecCargoPdfFont
        {
            public Font textNormal = new Font(Font.FontFamily.HELVETICA, 10f);
            public Font textNormalBold = new Font(Font.FontFamily.HELVETICA, 10f, Font.BOLD);
            public Font textHeadlineWhiteBold = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.WHITE);
            //public Font textHeadlineBlackBold = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD);
            public Font textCargo = new Font(Font.FontFamily.HELVETICA, 15f);
            public Font textInvoiceBold = new Font(Font.FontFamily.HELVETICA, 24f, Font.BOLD, BaseColor.BLUE);
        }
        public class AdresseInfo
        {
            public string Name,
                Adresse,
                Telefon,
                Post;
            public List<string> AdresseList = new List<string>(),
                PostList = new List<string>();
        }
        public class PakkeInfo {
            public string description,
                prices;

            public int count = 1;

        }

        #region PDF Faktura

        public class FakturaLayout
        {
            public AdresseInfo FakturaAdresse = new AdresseInfo();
            public AdresseInfo AfsenderAdresse = new AdresseInfo();
            public AdresseInfo ModtagerAdresse = new AdresseInfo();

            public string SaveTo = null,
                PdfName,
                Dato,
                KundeID,
                FragtNumber,
                Invoice,
                LeveringDag,
                LeveringTid,
                TransportType_1_String,
                TransportType_2_String,
                TransportType_3_String;

            public int pakkeCount;

            public double Rabat,
                Kilo;

            public List<string> PakkeCategoryName = new List<string>();
            public List<List<PakkeInfo>> PakkeInfo = new List<List<PakkeInfo>>();

            public void AddPakkeItem(string Category, string Description, string Price, bool SavePaper)
            {
                //find kategori id
                //tilføj hvis ikke findes
                int categoryId = this.PakkeCategoryName.FindIndex(id=> id == Category);
                if (categoryId == -1)
                {
                    this.PakkeCategoryName.Add(Category);
                    this.PakkeInfo.Add(new List<PdfCreate.PakkeInfo>());
                    categoryId = this.PakkeCategoryName.Count;
                }

                
                //tilføj eller opdater pakke
                int pakkeId = this.PakkeInfo[categoryId].FindIndex(pakke => pakke.description == Description && pakke.prices == Price);
                if (SavePaper && pakkeId > -1)
                {
                    this.PakkeInfo[categoryId][pakkeId].count++;
                }
                else {
                    this.PakkeInfo[categoryId].Add(new PdfCreate.PakkeInfo()
                    {
                        description = Description,
                        prices = Price
                    });
                }
            }
        }

        public class Faktura
        {
            public void CreateFaktura(FakturaLayout FakturaInfo)
            {
                //mappe placering
                string folder;

                if (FakturaInfo.SaveTo == null)
                {
                    folder = Models.ImportantData.g_FolderPdf;
                }
                else
                {
                    folder = FakturaInfo.SaveTo + @"\";
                }


                FileStream documentCreate = new FileStream(folder + FakturaInfo.PdfName + ".pdf", FileMode.Create, FileAccess.Write);
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, documentCreate);
                documentCreate.Close();

                PdfPTable FakturaTableDone = CreateFakturaTable(FakturaInfo);

                FakturaTableDone.DefaultCell.Border = 0;
                FakturaTableDone.WidthPercentage = 100f;

                //lav faktura
                doc.Open();
                doc.Add(FakturaTableDone);
                doc.Close();
                writer.Close();
                documentCreate.Close();
            }

            private PdfPTable CreateFakturaTable(FakturaLayout FakturaInfo)
            {
                TecCargoPdfFont styleFont = new TecCargoPdfFont();
                //bruges som mellemrum
                PdfPCell whiteSpace = new PdfPCell();
                whiteSpace.FixedHeight = 10f;
                whiteSpace.Border = 0;
                //whiteSpace.BackgroundColor = BaseColor.RED;

                #region Information/Faktura Adresse

                PdfPTable informationTable = new PdfPTable(1);
                informationTable.DefaultCell.Border = Rectangle.NO_BORDER;

                //tilføjer tekst til tablen
                informationTable.AddCell(new Paragraph("CARGO", styleFont.textCargo));
                informationTable.AddCell(new Paragraph("TEC-Distribution Center", styleFont.textNormalBold));
                informationTable.AddCell(whiteSpace);
                informationTable.AddCell(new Paragraph("Stamholmen 201, 2650 Hvidovre", styleFont.textNormal));
                informationTable.AddCell(new Paragraph("(Kontakt: +45 2545 3288)", styleFont.textNormal));
                informationTable.AddCell(new Paragraph("CVR: 12345678", styleFont.textNormal));
                informationTable.AddCell(whiteSpace);

                //laver en celle med baggrund farve
                PdfPCell infoCell = new PdfPCell(new Paragraph("BILL To:", styleFont.textHeadlineWhiteBold));
                infoCell.BackgroundColor = BaseColor.DARK_GRAY;
                infoCell.Border = 0;

                informationTable.AddCell(infoCell);
                informationTable.AddCell(new Paragraph(FakturaInfo.FakturaAdresse.Name, styleFont.textNormal));
                informationTable.AddCell(new Paragraph(FakturaInfo.FakturaAdresse.Adresse, styleFont.textNormal));
                informationTable.AddCell(new Paragraph(FakturaInfo.FakturaAdresse.Post, styleFont.textNormal));
                informationTable.AddCell(new Paragraph("(Kontakt: +45 " + FakturaInfo.FakturaAdresse.Telefon + ")", styleFont.textNormal));


                #endregion Information/Faktura Adresse

                #region Invoice

                PdfPTable invoiceTabel = new PdfPTable(2);
                invoiceTabel.DefaultCell.Border = 0;

                //opretter felter med speciale styles
                //
                //invoice med blåt
                PdfPCell invoiceCell = new PdfPCell(new Paragraph("INVOICE", styleFont.textInvoiceBold));
                invoiceCell.Border = 0;
                invoiceCell.Colspan = 2;
                invoiceCell.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                //datoen
                PdfPCell dateCell = new PdfPCell(new Paragraph(FakturaInfo.Dato, styleFont.textNormal));
                dateCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                dateCell.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                //invoice nummer
                PdfPCell numberCell = new PdfPCell(new Paragraph(FakturaInfo.Invoice, styleFont.textNormal));
                numberCell.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                //customer id
                PdfPCell customerCell = new PdfPCell(new Paragraph(FakturaInfo.KundeID, styleFont.textNormal));
                customerCell.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                //billedet
                Image cargo_logo = Image.GetInstance(Directory.GetCurrentDirectory() + @"\Images\Pdf\logo.png");
                cargo_logo.ScaleToFit(100f, 100f);

                PdfPCell logocell = new PdfPCell(cargo_logo);
                logocell.Colspan = 2;
                logocell.Border = Rectangle.NO_BORDER;
                logocell.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                //tilføjer tekst til tablen
                invoiceTabel.AddCell(invoiceCell);
                invoiceTabel.AddCell(whiteSpace);
                invoiceTabel.AddCell(whiteSpace);
                invoiceTabel.AddCell(new Paragraph("DATE:", styleFont.textNormal));
                invoiceTabel.AddCell(dateCell);
                invoiceTabel.AddCell(new Paragraph("INVOICE#", styleFont.textNormal));
                invoiceTabel.AddCell(numberCell);
                invoiceTabel.AddCell(new Paragraph("Customer ID", styleFont.textNormal));
                invoiceTabel.AddCell(customerCell);
                invoiceTabel.AddCell(whiteSpace);
                invoiceTabel.AddCell(whiteSpace);
                invoiceTabel.AddCell(logocell);

                #endregion Invoice

                #region Afsender Adresse

                //laver celler med indryk

                PdfPCell vejSendCell = new PdfPCell(new Paragraph(FakturaInfo.AfsenderAdresse.Adresse, styleFont.textNormal));
                vejSendCell.Border = Rectangle.NO_BORDER;
                vejSendCell.PaddingLeft = 5f;

                PdfPCell postSendCell = new PdfPCell(new Paragraph(FakturaInfo.AfsenderAdresse.Post, styleFont.textNormal));
                postSendCell.Border = Rectangle.NO_BORDER;
                postSendCell.PaddingLeft = 5f;


                PdfPTable afsenderTable = new PdfPTable(1);
                afsenderTable.DefaultCell.Border = Rectangle.NO_BORDER;

                //tilføjer tekst til tablen
                afsenderTable.AddCell(new Paragraph(FakturaInfo.AfsenderAdresse.Name, styleFont.textNormalBold));
                afsenderTable.AddCell(vejSendCell);
                afsenderTable.AddCell(postSendCell);

                #endregion Afsender Adresse

                #region Modtager Adresse

                PdfPTable modtagerTable = new PdfPTable(1);
                modtagerTable.DefaultCell.Border = 0;
                PdfPCell vejModCell = new PdfPCell(new Paragraph(FakturaInfo.ModtagerAdresse.Adresse, styleFont.textNormal));
                vejModCell.Border = 0;
                vejModCell.PaddingLeft = 5f;

                PdfPCell postModCell = new PdfPCell(new Paragraph(FakturaInfo.ModtagerAdresse.Post, styleFont.textNormal));
                postModCell.Border = 0;
                postModCell.PaddingLeft = 5f;

                //tilføjer tekst til tablen
                modtagerTable.AddCell(new Paragraph(FakturaInfo.ModtagerAdresse.Name, styleFont.textNormalBold));
                modtagerTable.AddCell(vejModCell);
                modtagerTable.AddCell(postModCell);
                modtagerTable.AddCell(whiteSpace);


                #endregion Modtager Adresse

                #region Control number Og Sæt afsender og modtager adresse sammen
                //transport control
                string trasportInfoText = "Transport Leveret " + FakturaInfo.LeveringDag + "(" + FakturaInfo.LeveringTid + ")" + " Transportation Control Number: " + FakturaInfo.FragtNumber;
                PdfPCell transportInfo = new PdfPCell(new Paragraph(trasportInfoText, styleFont.textNormalBold));
                transportInfo.Border = 0;
                transportInfo.Colspan = 2;
                
                //afsender adresse
                PdfPCell afsenderCell = new PdfPCell(afsenderTable);              
                afsenderCell.Border = Rectangle.NO_BORDER;

                //modtager adresse
                PdfPCell modtagerCell = new PdfPCell(modtagerTable);              
                modtagerCell.Border = Rectangle.NO_BORDER;
                
                //sætter transportinfo afsender og modtager i en table også en cell
                PdfPTable senderModtagTable = new PdfPTable(2);
                senderModtagTable.DefaultCell.Border = 0;
                senderModtagTable.AddCell(transportInfo);
                senderModtagTable.AddCell(whiteSpace);
                senderModtagTable.AddCell(whiteSpace);

                senderModtagTable.AddCell(afsenderCell);
                senderModtagTable.AddCell(modtagerCell);

                senderModtagTable.AddCell(whiteSpace);
                senderModtagTable.AddCell(whiteSpace);

                senderModtagTable.AddCell(new Paragraph("Reference: 578000000" + FakturaInfo.FragtNumber, styleFont.textNormal));
                senderModtagTable.AddCell("");

                /*
                    senderModtagTable.AddCell(afsenderCell);
                    senderModtagTable.AddCell(modtagerCell);
                */
                //lav ramme til højre
                PdfPCell senderModtagCell = new PdfPCell(senderModtagTable);
                //senderModtagCell.Border = Rectangle.LEFT_BORDER;
                senderModtagCell.Border = Rectangle.RIGHT_BORDER;

                #endregion Control number Og Sæt afsender og modtager adresse sammen

                #region Produkter / Evt. Bemærkninger

                whiteSpace.Border = Rectangle.RIGHT_BORDER;

                //description
                PdfPCell des = new PdfPCell(new Paragraph("DESCRIPTION", styleFont.textHeadlineWhiteBold));
                des.BackgroundColor = BaseColor.DARK_GRAY;
                des.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                //amount
                PdfPCell amo = new PdfPCell(new Paragraph("AMOUNT", styleFont.textHeadlineWhiteBold));
                amo.BackgroundColor = BaseColor.DARK_GRAY;
                amo.HorizontalAlignment = Rectangle.ALIGN_CENTER;


                //transport type
                string trasportTypeText = "* * * CARGO " + 
                    FakturaInfo.TransportType_1_String + " " +
                    FakturaInfo.TransportType_2_String + FakturaInfo.TransportType_3_String + 
                    " * * * " + FakturaInfo.pakkeCount + " stk * * * " + 
                    DecimalTwo(FakturaInfo.Kilo.ToString()) + " KG * * *";

                PdfPCell trasportType = new PdfPCell(new Paragraph(trasportTypeText, styleFont.textNormalBold));
                //trasportType.Border = Rectangle.LEFT_BORDER;
                trasportType.Border = Rectangle.RIGHT_BORDER;

                //
                PdfPTable middelTable = new PdfPTable(2);
                middelTable.DefaultCell.Border = 0; // Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER;

                //bredden mellem DESCRIPTION og AMOUNT
                float[] widths = { 4f, 0.7f };
                middelTable.SetWidths(widths);

                //tilføj teskt
                middelTable.AddCell(des);
                middelTable.AddCell(amo);

                middelTable.AddCell(whiteSpace);
                middelTable.AddCell("");

                middelTable.AddCell(senderModtagCell);
                middelTable.AddCell("");

                middelTable.AddCell(trasportType);
                middelTable.AddCell("");

                middelTable.AddCell(whiteSpace);
                middelTable.AddCell("");

                //hvormange kategorier der er
                int kateCount = FakturaInfo.PakkeCategoryName.Count;


                for (int i = 0; i < kateCount; i++)
                {
                    //gør så katogori står med fed
                    PdfPCell headLine = new PdfPCell(new Paragraph(FakturaInfo.PakkeCategoryName[i], styleFont.textNormalBold));
                    //headLine.Border = Rectangle.LEFT_BORDER;
                    headLine.Border = Rectangle.RIGHT_BORDER;
                    headLine.PaddingLeft = 10f;

                    middelTable.AddCell(headLine);
                    middelTable.AddCell("");

                    //hvormange values der er i den valgte katogori
                    int kateValueCount = FakturaInfo.PakkeInfo[i].Count;

                    for (int a = 0; a < kateValueCount; a++)
                    {
                        //
                        int doublePacket = FakturaInfo.PakkeInfo[i][a].count;
                        string gangeText = "";

                        if (doublePacket != 1)
                        {
                            gangeText = " x" + doublePacket;
                        }

                        PdfPCell ProdukIndhold = new PdfPCell(new Paragraph(FakturaInfo.PakkeInfo[i][a].description + gangeText, styleFont.textNormal));
                        ProdukIndhold.Border = Rectangle.RIGHT_BORDER;

                        //hent hele prisen
                        
                        double pakkerPriceAllDouble = 0;
                        string pakkerPriceAllString = "-";
                        for (int b = 0; b < doublePacket; b++)
                        {
                            if (FakturaInfo.PakkeInfo[i][a].prices == "-" || FakturaInfo.PakkeInfo[i][a].prices == "")
                            {
                                pakkerPriceAllString = FakturaInfo.PakkeInfo[i][a].prices;
                                break;
                            }
                            else
                            {
                                double tryPriceOut = 0;
                                double.TryParse(FakturaInfo.PakkeInfo[i][a].prices, out tryPriceOut);
                                pakkerPriceAllDouble += tryPriceOut;
                                pakkerPriceAllString = pakkerPriceAllDouble.ToString();
                            }
                        }
                        
                        //gem prisen som eks. 0,00 eller -
                        string kommaTal;
                        addZero(pakkerPriceAllString, out kommaTal);

                        //så priserne står tilhøjre
                        PdfPCell priceRight = new PdfPCell(new Paragraph(kommaTal, styleFont.textNormal));
                        priceRight.Border = Rectangle.LEFT_BORDER;
                        //priceRight.Border += Rectangle.RIGHT_BORDER;
                        priceRight.HorizontalAlignment = Rectangle.ALIGN_RIGHT;


                        //tilføj dem
                        middelTable.AddCell(ProdukIndhold);
                        middelTable.AddCell(priceRight);
                        
                    }

                    //mellemrum mellem kategorier
                    middelTable.AddCell(whiteSpace);
                    middelTable.AddCell("");
                }

                //lav list mellemrum i bunden
                for (int i = 0; i < 2; i++)
                {
                    middelTable.AddCell(whiteSpace);
                    middelTable.AddCell("");
                }

                ////cell så kassen bliver lukket
                //PdfPCell closeBorder = new PdfPCell();
                //closeBorder.Border = Rectangle.LEFT_BORDER;
                //closeBorder.Border += Rectangle.RIGHT_BORDER;
                //closeBorder.Border += Rectangle.BOTTOM_BORDER;

                //middelTable.AddCell(closeBorder);
                //middelTable.AddCell(closeBorder);

                whiteSpace.Border = 0;

                #endregion Produkter / Evt. Bemærkninger

                #region Other Comments

                //opretter felter med speciale styles 
                PdfPCell commentsCell = new PdfPCell(new Paragraph("OTHER COMMENTS", styleFont.textHeadlineWhiteBold));
                commentsCell.BackgroundColor = BaseColor.DARK_GRAY;

                PdfPTable comments_tabel = new PdfPTable(1);
                comments_tabel.DefaultCell.Border = 0;

                comments_tabel.AddCell(commentsCell);
                comments_tabel.AddCell(new Paragraph("1. Total payment due in 30 days", styleFont.textNormal));
                comments_tabel.AddCell(new Paragraph("2. Please include the invoice number on your check", styleFont.textNormal));
                comments_tabel.AddCell(new Paragraph("3. Danske Bank reg: 5211 konto 5211478956477", styleFont.textNormal));

                //laver et mellem rum mellem pakke info og comments
                PdfPCell commentFcell = new PdfPCell(comments_tabel);

                PdfPTable comments_final = new PdfPTable(1);
                comments_final.AddCell(whiteSpace);
                comments_final.AddCell(commentFcell);

                #endregion Other Comments

                #region Total / Beløb

                //find prisen uden moms
                double subTotal = 0;

                //hent prisen fra de enkelte kategorier
                for (int i = 0; i < kateCount; i++)
                {
                    //hvormange values der er i valgte kategori
                    int kateValueCount = FakturaInfo.PakkeInfo[i].Count;

                    for (int a = 0; a < kateValueCount; a++)
                    {
                        
                        for (int b = 0; b < FakturaInfo.PakkeInfo[i][a].count; b++)
                        {
                            //hvis det er et tal add til subtotal
                            double price;

                            bool isNumber = double.TryParse(FakturaInfo.PakkeInfo[i][a].prices, out price);
                            if (isNumber)
                            {
                                //System.Windows.MessageBox.Show(price.ToString());
                                subTotal += price;
                            }
                        }
                        
                    }
                }

                //beregner moms ud
                double tax = (subTotal * 0.25);

                //beregner prisen ud med moms
                double prisIaltMoms = (subTotal + tax);

                //beregn rabatten ud
                double rabatTotal = (prisIaltMoms * (FakturaInfo.Rabat / 100));

                //træk rabatten fra
                double total = (prisIaltMoms - rabatTotal);

                //laver celler
                PdfPCell subtotal_1 = new PdfPCell(new Paragraph("SUBTOTAL", styleFont.textNormal));
                PdfPCell subtotal_2 = new PdfPCell(new Paragraph(subTotal.ToString("F"), styleFont.textNormal));
                subtotal_1.Border = Rectangle.NO_BORDER;
                subtotal_2.Border = Rectangle.NO_BORDER;
                subtotal_1.BackgroundColor = BaseColor.LIGHT_GRAY;
                subtotal_2.BackgroundColor = BaseColor.LIGHT_GRAY;
                subtotal_2.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                PdfPCell taxrate_1 = new PdfPCell(new Paragraph("TAX RATE", styleFont.textNormal));
                PdfPCell taxrate_2 = new PdfPCell(new Paragraph("25%", styleFont.textNormal));
                taxrate_1.Colspan = 2;
                taxrate_1.Border = Rectangle.NO_BORDER;
                taxrate_2.Border = Rectangle.NO_BORDER;
                taxrate_2.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                PdfPCell tax_1 = new PdfPCell(new Paragraph("TAX", styleFont.textNormal));
                PdfPCell tax_2 = new PdfPCell(new Paragraph(tax.ToString("F"), styleFont.textNormal));
                tax_1.Border = Rectangle.NO_BORDER;
                tax_2.Border = Rectangle.NO_BORDER;
                tax_1.BackgroundColor = BaseColor.LIGHT_GRAY;
                tax_2.BackgroundColor = BaseColor.LIGHT_GRAY;
                tax_2.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                PdfPCell rabatrate_1 = new PdfPCell(new Paragraph("RABAT RATE", styleFont.textNormal));
                PdfPCell rabatrate_2 = new PdfPCell(new Paragraph(FakturaInfo.Rabat + "%", styleFont.textNormal));
                rabatrate_1.Colspan = 2;
                rabatrate_1.Border = Rectangle.NO_BORDER;
                rabatrate_2.Border = Rectangle.NO_BORDER;
                rabatrate_2.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                PdfPCell rabat_1 = new PdfPCell(new Paragraph("RABAT", styleFont.textNormal));
                PdfPCell rabat_2 = new PdfPCell(new Paragraph(rabatTotal.ToString("F"), styleFont.textNormal));
                rabat_1.Border = Rectangle.NO_BORDER;
                rabat_2.Border = Rectangle.NO_BORDER;
                rabat_1.BackgroundColor = BaseColor.LIGHT_GRAY;
                rabat_2.BackgroundColor = BaseColor.LIGHT_GRAY;
                rabat_2.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                PdfPCell total_1 = new PdfPCell(new Paragraph("TOTAL", styleFont.textNormalBold));
                PdfPCell total_2 = new PdfPCell(new Paragraph(total.ToString("F"), styleFont.textNormalBold));
                total_1.Border = Rectangle.NO_BORDER;
                total_2.Border = Rectangle.NO_BORDER;
                total_1.BackgroundColor = BaseColor.LIGHT_GRAY;
                total_2.BackgroundColor = BaseColor.LIGHT_GRAY;
                total_2.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                //DKR
                PdfPCell dkr_1 = new PdfPCell(new Paragraph("DKR", styleFont.textNormal));
                PdfPCell dkr_2 = new PdfPCell(new Paragraph("DKR", styleFont.textNormalBold));
                dkr_1.Border = Rectangle.NO_BORDER;
                dkr_2.Border = Rectangle.NO_BORDER;
                dkr_1.BackgroundColor = BaseColor.LIGHT_GRAY;
                dkr_2.BackgroundColor = BaseColor.LIGHT_GRAY;
                dkr_1.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                dkr_2.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                //double linje
                PdfPCell doubleLine = new PdfPCell();
                doubleLine.Border = Rectangle.TOP_BORDER;
                doubleLine.Border += Rectangle.BOTTOM_BORDER;
                doubleLine.Colspan = 3;
                doubleLine.BorderWidth = 0.8f;
                doubleLine.FixedHeight = 2f;

                //opretter table
                float[] widthh = { 5f, 2f, 3f };

                PdfPTable totalTable = new PdfPTable(3);
                totalTable.SetWidths(widthh);

                totalTable.AddCell(subtotal_1);
                totalTable.AddCell(dkr_1);
                totalTable.AddCell(subtotal_2);
                totalTable.AddCell(taxrate_1);
                totalTable.AddCell(taxrate_2);
                totalTable.AddCell(tax_1);
                totalTable.AddCell(dkr_1);
                totalTable.AddCell(tax_2);
                totalTable.AddCell(rabatrate_1);
                totalTable.AddCell(rabatrate_2);
                totalTable.AddCell(rabat_1);
                totalTable.AddCell(dkr_1);
                totalTable.AddCell(rabat_2);
                totalTable.AddCell(doubleLine);
                totalTable.AddCell(total_1);
                totalTable.AddCell(dkr_2);
                totalTable.AddCell(total_2);


                #endregion Total / Beløb

                #region Sæt tabellerne sammen til en

                //laver tabler om til celler så de kan leve sat ind i en tabel
                PdfPCell infoTableCell = new PdfPCell(informationTable);
                infoTableCell.Border = Rectangle.NO_BORDER;

                PdfPCell invoiceTableCell = new PdfPCell(invoiceTabel);
                invoiceTableCell.Border = Rectangle.NO_BORDER;

                PdfPCell middelTableCell = new PdfPCell(middelTable);
                middelTableCell.Border = Rectangle.BOX;
                middelTableCell.Colspan = 4;

                PdfPCell commentTableCell = new PdfPCell(comments_final);
                commentTableCell.Border = Rectangle.NO_BORDER;
                commentTableCell.Colspan = 2;

                PdfPCell totolTableCell = new PdfPCell(totalTable);
                totolTableCell.Border = Rectangle.NO_BORDER;

                //laver den sidste table der skal indehold alle cellerne

                float[] widthss = { 5f, 3f, 1.2f, 5.5f };

                PdfPTable fakturaTable = new PdfPTable(4);
                fakturaTable.DefaultCell.Border = Rectangle.NO_BORDER;
                fakturaTable.WidthPercentage = 90;
                fakturaTable.KeepTogether = false;
                fakturaTable.SetWidths(widthss);

                //tilføj celler
                fakturaTable.AddCell(infoTableCell);
                fakturaTable.AddCell("");
                fakturaTable.AddCell("");
                fakturaTable.AddCell(invoiceTableCell);
                fakturaTable.AddCell(whiteSpace);
                fakturaTable.AddCell(whiteSpace);
                fakturaTable.AddCell(whiteSpace);
                fakturaTable.AddCell(whiteSpace);
                fakturaTable.AddCell(middelTableCell);
                fakturaTable.AddCell(commentTableCell);
                fakturaTable.AddCell("");
                fakturaTable.AddCell(totolTableCell);

                #endregion Sæt tabellerne sammen til en

                return fakturaTable;
            }

            private void addZero(string number, out string decTal)
            {
                double doubleNumber;
                bool isAnumber = double.TryParse(number, out doubleNumber);

                if (isAnumber)
                {
                    decTal = doubleNumber.ToString("F");
                }
                else
                {
                    decTal = number;
                }
            }

            private string DecimalTwo(string value)
            {
                double doubleValue = 0;
                double.TryParse(value, out doubleValue);

                return doubleValue.ToString("#.##");
            }
        }
        #endregion PDF Faktura

        #region PDF Fragtbrev

        public class FragtBrevLayout
        {

            public string SaveTo = null,
                pdfName,
                kundeNumber,
                fragtNumber,
                referenceText,
                forsikringSum,
                praemieSum,
                iAltSum,
                efterKrav,
                fragtmand,
                date1,
                date2,
                rute1,
                rute2,
                pakkeTypeText,
                EkstraComment,
                creatorIds;

            public bool emptyPDF = false,
                UseBoldOnInput = false,
                senderPay,
                isByttePalle;
            public bool[] transportKurer = { false, false },
                transportPakke = { false, false },
                transportGods = { false, false },
                /* 0 - 6 kurrer 
                 * 7 - 15 pakke
                 * 16 - 20 gods
                 */
                transportTypeUse = { false, false, false, false, false, false, false, false, false, false,
                                   false, false, false, false, false, false, false, false, false, false,
                                   false };

            public AdresseInfo Afsender = new AdresseInfo(),
                Modtager = new AdresseInfo(),
                AndenBetaler = new AdresseInfo();

            public int typeABCD,
                pakkeTransport;

            public int[] palleNumber = { 0, 0, 0 };

            public List<string> pakkeIndhold = new List<string>(),
                pakkeART = new List<string>(),
                pakkeAdresseAndNumber = new List<string>(),
                pakkeRumfang = new List<string>();

            public List<int> pakkeCount = new List<int>();

            public List<double> pakkeWidth = new List<double>();
            public List<bool> pakkeRumOrWidth = new List<bool>(),
                pakkeRumOrWidthBoth = new List<bool>();




        }

        public class FragtBrev
        {
            class PdfStyle
            {
                public int[] AlingCenter = { 1, 5 };
                public int[] AlingRightCenter = { 2, 5 };
                public int[] AlingLeftBottom = { 0, 6 };
                public int[] AlingLeftCenter = { 0, 5 };
                public int[] AlingCenterBottom = { 1, 6 };

                //border Size
                public float[] BorderSize_05f = { 0.5f, 0.5f, 0.5f, 0.5f };
                public float[] BorderSize_1f = { 0.5f, 0.5f, 0.5f, 0.5f };
                public float[] BorderSize_2f = { 0.5f, 0.5f, 0.5f, 0.5f };
                public float[] BorderSize_3f = { 0.5f, 0.5f, 0.5f, 0.5f };
                public float[] BorderSize_Top_3f_side_1f = { 0.5f, 0.5f, 0.5f, 0.5f };

                //padding
                public float[] Padding_5fLeft_5fTopBottom = { 5f, 5f, 0, 5f };
                public float[] Padding_5fTopBottom = { 0, 5f, 0, 5f };
                public float[] Padding_5fLeft = { 5f, 0, 0, 0 };
            }

            class FragtText
            {
                //public static Font HeaderBold = new Font(Font.FontFamily.HELVETICA, 20f, Font.BOLD, BaseColor.BLACK);
                public Font InfoRed = new Font(Font.FontFamily.HELVETICA, 11f, Font.BOLD, BaseColor.RED);
                //public static Font mediumText = new Font(Font.FontFamily.HELVETICA, 9f, Font.NORMAL, BaseColor.BLACK);
                //public static Font mediumBigText = new Font(Font.FontFamily.HELVETICA, 9f, Font.NORMAL, BaseColor.BLACK);
                public Font StregkodeText = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.BLACK);
                public Font StregkodeTextInput = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.BLACK);
                public Font UserInputMediumSmallText = new Font(Font.FontFamily.HELVETICA, 7.3f, Font.NORMAL, BaseColor.BLACK);
                public Font mediumSmallText = new Font(Font.FontFamily.HELVETICA, 7.3f, Font.NORMAL, BaseColor.BLACK);
                public Font mediumSmallTextUnderLine = new Font(Font.FontFamily.HELVETICA, 7.3f, Font.UNDERLINE, BaseColor.BLACK);
                public Font smallText = new Font(Font.FontFamily.HELVETICA, 5.7f, Font.NORMAL, BaseColor.BLACK);
                public Font smallTextRed = new Font(Font.FontFamily.HELVETICA, 7f, Font.NORMAL, BaseColor.RED);

                public Font normalText = new Font(Font.FontFamily.HELVETICA, 9f, Font.NORMAL, BaseColor.BLACK);
                public Font normalTextBold = new Font(Font.FontFamily.HELVETICA, 9f, Font.BOLD, BaseColor.BLACK);

                public Font FrankoText = new Font(Font.FontFamily.HELVETICA, 11f, Font.NORMAL, BaseColor.BLACK);
                public Font WhiteText = new Font(Font.FontFamily.HELVETICA, 11f, Font.NORMAL, BaseColor.WHITE);
                public Font mediumSmallTextBold = new Font(Font.FontFamily.HELVETICA, 7.3f, Font.BOLD, BaseColor.BLACK);
                public Font mediumTextBold = new Font(Font.FontFamily.HELVETICA, 9f, Font.BOLD, BaseColor.BLACK);


                //hvid text farve

                public Font mediumSmallTextWhite = new Font(Font.FontFamily.HELVETICA, 7.3f, Font.NORMAL, BaseColor.WHITE);
            }



            //gemmer pdf med alle de ændringer der er lavet til
            public void SaveFragtBrev(FragtBrevLayout InformationClass)
            {
                //mappe placering
                string folder;

                if (InformationClass.SaveTo == null)
                {
                    folder = Models.ImportantData.g_FolderPdf;
                }
                else
                {
                    folder = InformationClass.SaveTo + @"\";
                }


                FileStream documentCreate = new FileStream(folder + InformationClass.pdfName + ".pdf", FileMode.Create, FileAccess.Write);
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, documentCreate);

                ITextEvents footerTextEvent = new ITextEvents();
                writer.PageEvent = footerTextEvent;


                PdfPTable fragtBrevTableDone;
                fragtBrevTableDone = CreatePDFTableOfFragtbrev(InformationClass);


                fragtBrevTableDone.DefaultCell.Border = 0;
                fragtBrevTableDone.WidthPercentage = 100f;

                //lav fragtBrev

                string[] footText = { "Afsenderens Kvittering", "Modtagerens Kvittering", "Chaufførens Kvittering" };
                doc.Open();
                for (int i = 0; i < 3; i++)
                {
                    doc.ResetPageCount();
                    doc.PageCount = 1;
                    footerTextEvent.FooterText = footText[i];
                    doc.Add(fragtBrevTableDone);
                    doc.NewPage();
                }
                doc.Close();

                writer.Close();
                documentCreate.Close();
            }

            private PdfPTable CreatePDFTableOfFragtbrev(FragtBrevLayout InformationClass)
            {
                FragtText styleFont = new FragtText();
                PdfStyle stylePDF = new PdfStyle();

                if (useBold())
                {
                    styleFont.UserInputMediumSmallText = new Font(Font.FontFamily.HELVETICA, 7.3f, Font.BOLD, BaseColor.BLACK);
                }
                else
                {
                    styleFont.UserInputMediumSmallText = new Font(Font.FontFamily.HELVETICA, 7.3f, Font.NORMAL, BaseColor.BLACK);
                }

                if (InformationClass.emptyPDF)
                {
                    styleFont.StregkodeTextInput = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.WHITE);
                }
                else
                {
                    styleFont.StregkodeTextInput = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.BLACK);
                }

                #region Default functions, stregKodeImage

                PdfPCell whiteSpace = new PdfPCell();
                whiteSpace.Border = Rectangle.NO_BORDER;
                whiteSpace.FixedHeight = 5f;


                PdfPCell whiteSpaceNormalText = new PdfPCell();
                whiteSpaceNormalText.Border = Rectangle.NO_BORDER;
                whiteSpaceNormalText.BackgroundColor = BaseColor.RED;
                whiteSpaceNormalText.FixedHeight = 5f;

                //stregkode billede
                Image stregKodeImage = Image.GetInstance(this.CreateBarcode("*578000000" + InformationClass.fragtNumber + "*"), BaseColor.WHITE);
                stregKodeImage.ScaleToFit(220f, 60f);

                PdfPCell stregKodeCell1 = new PdfPCell(stregKodeImage);
                stregKodeCell1.Border = Rectangle.NO_BORDER;
                stregKodeCell1.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                PdfPTable stregKodeTable = new PdfPTable(1);
                stregKodeTable.DefaultCell.Border = 0;
                stregKodeTable.DefaultCell.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                /*
                //lav mellemrum i fragtnumber
                string fragtNumberWhitSpace = "";
                for (int i = 0; i < InformationClass.fragtNumber.Length; i++)
                {
                    fragtNumberWhitSpace += InformationClass.fragtNumber.Substring(i, 1) + " ";
                }

                PdfPTable fragtNumberTable = new PdfPTable(6);
                fragtNumberTable.DefaultCell.Border = 0;
                fragtNumberTable.SetWidths(new float[] { 0.3f, 0.1f, 0.64f, 0.3f, 0.2f, 0.3f });

                fragtNumberTable.AddCell("");
                fragtNumberTable.AddCell(FastCell2("*", new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD), stylePDF.AlingRightCenter, 0, null, new float[] { 0, 5f, 0, 0 }));
                fragtNumberTable.AddCell(FastCell2("5 7 8 0 0 0 0 0 0", styleFont.StregkodeText, stylePDF.AlingCenter));
                fragtNumberTable.AddCell(FastCell2(fragtNumberWhitSpace, styleFont.StregkodeTextInput, stylePDF.AlingCenter));
                fragtNumberTable.AddCell(FastCell2("*", new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD), stylePDF.AlingLeftCenter, 0, null, new float[] { 0, 5f, 0, 0 }));
                fragtNumberTable.AddCell("");
                */
                stregKodeTable.AddCell(stregKodeCell1);
                //stregKodeTable.AddCell(FastCell2(fragtNumberTable, stylePDF.AlingCenter));
                //stregKodeTable.AddCell(new Paragraph("* 5 7 8 0 0 0 0 0 0 " + fragtNumberWhitSpace + " *", styleFont.StregkodeText));

                PdfPCell stregKodeCell = new PdfPCell(stregKodeTable);
                stregKodeCell.Border = Rectangle.NO_BORDER;
                stregKodeCell.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                #endregion Default functions, stregKodeImage

                #region Afsender siden

                //lav tabel for postnummer også om til en cell
                PdfPTable postTable = new PdfPTable(2);
                postTable.DefaultCell.Border = Rectangle.NO_BORDER;
                postTable.SetWidths(new float[] { 10f, 20f });

                postTable.AddCell(FastCell2("POSTNUMMER", styleFont.mediumSmallText, stylePDF.AlingLeftBottom, 0, null, new float[] { 3f, 0, 0, 3f }));

                if (InformationClass.emptyPDF)
                {
                    postTable.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingLeftBottom, 0, null, new float[] { 0, 0, 0, 3f }));
                }
                else
                {
                    postTable.AddCell(FastCell2(InformationClass.Afsender.Post, styleFont.UserInputMediumSmallText, stylePDF.AlingLeftBottom, 0, null, new float[] { 0, 0, 0, 3f }));
                }

                //lav afsender cell
                PdfPTable adresseTable = new PdfPTable(1);
                adresseTable.DefaultCell.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                adresseTable.DefaultCell.Border = Rectangle.NO_BORDER;
                adresseTable.DefaultCell.PaddingBottom = 5f;

                adresseTable.AddCell(new Paragraph("AFSENDER, TELEFONNR.", styleFont.mediumSmallText));

                if (InformationClass.emptyPDF)
                {
                    adresseTable.AddCell(new Paragraph("§§§", styleFont.mediumSmallTextWhite));
                    adresseTable.AddCell(new Paragraph("§§§", styleFont.mediumSmallTextWhite));
                    adresseTable.AddCell(new Paragraph("§§§", styleFont.mediumSmallTextWhite));
                    adresseTable.AddCell(new Paragraph("§§§", styleFont.mediumSmallTextWhite));
                    adresseTable.AddCell(new Paragraph("§§§", styleFont.mediumSmallTextWhite));
                    adresseTable.AddCell(new Paragraph("§§§", styleFont.mediumSmallTextWhite));
                }
                else
                {
                    adresseTable.AddCell(new Paragraph(InformationClass.Afsender.Name, styleFont.UserInputMediumSmallText));
                    adresseTable.AddCell(new Paragraph(InformationClass.Afsender.Adresse, styleFont.UserInputMediumSmallText));
                    adresseTable.AddCell(new Paragraph(InformationClass.Afsender.Telefon, styleFont.UserInputMediumSmallText));
                }
                adresseTable.AddCell(FastCell2(postTable));

                //PdfPCell adresseCell = new PdfPCell(adresseTable);
                //adresseCell.BorderColor = FragtColor.BlueBorder;
                //adresseCell.BorderWidth = 3f;

                //lav infomation cell
                string infoText = "VED EFTERKRAV MÅ FORSENDELSEN UDLEVERES MOD BETALING MED SÆDVANLIGE BE-\n" +
                    "TALINGSMIDLER, HERUNDER KONTANTER OG ALMINDELIG CHECK. HAR AFSENDEREN PÅ-\n" +
                    "FØRT SIN UNDERSKRIFT NEDENFOR, MÅ UDLEVERING DOG KUN SKE MED KONTANTER EL-\n" +
                    "LER BANKNOTERET CHECK.";

                PdfPCell infomationCell = FastCell2(infoText, styleFont.smallText, null, Rectangle.TOP_BORDER, stylePDF.BorderSize_3f);
                infomationCell.FixedHeight = 50f;

                //lav border lige
                PdfPTable borderTable = new PdfPTable(1);
                borderTable.DefaultCell.Border = 0;

                //borderTable.AddCell(FastCell2(tidforbrugtable, null, Rectangle.BOTTOM_BORDER));
                borderTable.AddCell(FastCell2(adresseTable));
                borderTable.AddCell(infomationCell);
                borderTable.AddCell(FastCell2("AFSENDERS UNDERSKRIFT", styleFont.mediumSmallText, stylePDF.AlingCenterBottom, Rectangle.TOP_BORDER, stylePDF.BorderSize_1f, new float[] { 0, 10f, 0, 0 }));


                //sæt højre del ind i en table som skal laves om til en cell
                PdfPTable AfsenderTableDone = new PdfPTable(1);

                AfsenderTableDone.AddCell(FastCell2(borderTable));

                #endregion Afsender siden

                #region TecCargo Logo Billede

                //teccargo text billede
                Image tecCargoTextImage = Image.GetInstance(Directory.GetCurrentDirectory() + "/Images/Pdf/logo textOnly.png");
                tecCargoTextImage.ScaleToFit(140f, 70f);

                PdfPCell tecCargoTextCell = new PdfPCell(tecCargoTextImage);
                tecCargoTextCell.Border = Rectangle.NO_BORDER;
                tecCargoTextCell.VerticalAlignment = Rectangle.ALIGN_MIDDLE;

                //tecargo logo billede
                Image tecCargoImage = Image.GetInstance(Directory.GetCurrentDirectory() + "/Images/Pdf/logo logoOnly.png");
                tecCargoImage.ScaleToFit(70f, 130f);

                PdfPCell tecCargoCell = new PdfPCell(tecCargoImage);
                tecCargoCell.Border = Rectangle.NO_BORDER;
                tecCargoCell.HorizontalAlignment = Rectangle.ALIGN_LEFT;


                PdfPTable tacCargoLogoTableDone = new PdfPTable(2);
                tacCargoLogoTableDone.DefaultCell.Border = 0;
                tacCargoLogoTableDone.SetWidths(new float[] { 0.65f, 2f });

                tacCargoLogoTableDone.AddCell(tecCargoCell);
                tacCargoLogoTableDone.AddCell(tecCargoTextCell);

                #endregion TecCargo Logo Billede

                #region Kunde nummer siden


                //Reference cell
                PdfPTable regerenceTextTable = new PdfPTable(2);
                regerenceTextTable.DefaultCell.Border = Rectangle.NO_BORDER;

                regerenceTextTable.AddCell(FastCell2("REFERENCE", styleFont.mediumSmallText, stylePDF.AlingLeftBottom));
                if (InformationClass.emptyPDF)
                {
                    regerenceTextTable.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite));
                }
                else
                {
                    regerenceTextTable.AddCell(FastCell2(InformationClass.referenceText, styleFont.mediumSmallText));
                }

                //lav fragt del 1
                PdfPTable KundeNumberTableDone = new PdfPTable(2);
                KundeNumberTableDone.DefaultCell.Border = 0;

                //teccargo logo
                KundeNumberTableDone.AddCell(FastCell2(tacCargoLogoTableDone));

                //sæt afsender delen ind her
                KundeNumberTableDone.AddCell(FastCell2(AfsenderTableDone, null, Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER, stylePDF.BorderSize_3f, null, 1, 9));

                if (InformationClass.emptyPDF)
                {
                    KundeNumberTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingCenter, Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_3f, new float[] { 0f, 5f, 0, 5f }));
                }
                else
                {
                    KundeNumberTableDone.AddCell(FastCell2(InformationClass.kundeNumber, styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_3f, new float[] { 0f, 5f, 0, 5f }));
                }

                KundeNumberTableDone.AddCell(whiteSpace);

                KundeNumberTableDone.AddCell(FastCell2("KUNDENUMMER", styleFont.InfoRed, stylePDF.AlingCenter));

                KundeNumberTableDone.AddCell(whiteSpace);

                KundeNumberTableDone.AddCell(stregKodeCell);
                KundeNumberTableDone.AddCell(whiteSpace);
                KundeNumberTableDone.AddCell(FastCell2(regerenceTextTable, stylePDF.AlingCenter, Rectangle.TOP_BORDER, stylePDF.BorderSize_1f, new float[] { 0, 5f, 0, 5f }));

                #endregion Kunde nummer siden

                #region fragtman, dato, rute og byttepaller delen

                //lav antal eur paller
                PdfPTable eurPallerTable = new PdfPTable(6);
                eurPallerTable.DefaultCell.Border = 0;
                if (InformationClass.emptyPDF)
                {
                    eurPallerTable.AddCell(FastCell2("1/1", styleFont.mediumSmallText, stylePDF.AlingLeftBottom));
                    eurPallerTable.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingLeftBottom));
                    eurPallerTable.AddCell(FastCell2("1/2", styleFont.mediumSmallText, stylePDF.AlingLeftBottom));
                    eurPallerTable.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingLeftBottom));
                    eurPallerTable.AddCell(FastCell2("1/4", styleFont.mediumSmallText, stylePDF.AlingLeftBottom));
                    eurPallerTable.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingLeftBottom));
                }
                else
                {
                    eurPallerTable.AddCell(FastCell2("1/1", styleFont.mediumSmallText, stylePDF.AlingLeftBottom));
                    eurPallerTable.AddCell(FastCell2(InformationClass.palleNumber[0].ToString(), styleFont.UserInputMediumSmallText, stylePDF.AlingLeftBottom));
                    eurPallerTable.AddCell(FastCell2("1/2", styleFont.mediumSmallText, stylePDF.AlingLeftBottom));
                    eurPallerTable.AddCell(FastCell2(InformationClass.palleNumber[1].ToString(), styleFont.UserInputMediumSmallText, stylePDF.AlingLeftBottom));
                    eurPallerTable.AddCell(FastCell2("1/4", styleFont.mediumSmallText, stylePDF.AlingLeftBottom));
                    eurPallerTable.AddCell(FastCell2(InformationClass.palleNumber[2].ToString(), styleFont.UserInputMediumSmallText, stylePDF.AlingLeftBottom));
                }
                //lav om der er byttepaller

                PdfPTable palleYesNoTable = new PdfPTable(6);
                palleYesNoTable.DefaultCell.Border = 0;
                palleYesNoTable.SetWidths(new float[] { 3f, 1.5f, 1f, 3f, 1.5f, 1f });

                string isByttePalleYesX = "", isByttePalleNoX = "";

                if (!InformationClass.emptyPDF)
                {
                    if (InformationClass.isByttePalle)
                    {
                        isByttePalleYesX = "X";
                    }
                    else
                    {
                        isByttePalleNoX = "X";
                    }
                }
                palleYesNoTable.AddCell(FastCell2("JA", styleFont.mediumSmallText));
                palleYesNoTable.AddCell(FastCell2(isByttePalleYesX, styleFont.normalTextBold, stylePDF.AlingCenter, Rectangle.BOX));
                palleYesNoTable.AddCell("");
                palleYesNoTable.AddCell(FastCell2("NEJ", styleFont.mediumSmallText));
                palleYesNoTable.AddCell(FastCell2(isByttePalleNoX, styleFont.normalTextBold, stylePDF.AlingCenter, Rectangle.BOX));
                palleYesNoTable.AddCell("");

                //table til at indeholde byttepalle og dato/rute
                PdfPTable FragtMandDatoRuteTableDone = new PdfPTable(4);
                FragtMandDatoRuteTableDone.SetWidths(new float[] { 1f, 1f, 1f, 0.5f });

                FragtMandDatoRuteTableDone.AddCell(FastCell2("FRAGTMAND I", styleFont.mediumSmallText, null, 0, null, null, 2));
                FragtMandDatoRuteTableDone.AddCell(FastCell2("DATO", styleFont.mediumSmallText, null, Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER, null));
                FragtMandDatoRuteTableDone.AddCell(FastCell2("RUTE", styleFont.mediumSmallText, null, 0, null));

                if (InformationClass.emptyPDF)
                {
                    FragtMandDatoRuteTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, null, 0, null, null, 2));
                    FragtMandDatoRuteTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingCenter, Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER));
                    FragtMandDatoRuteTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingCenter));
                }
                else
                {
                    FragtMandDatoRuteTableDone.AddCell(FastCell2(InformationClass.fragtmand, styleFont.UserInputMediumSmallText, null, 0, null, null, 2));
                    FragtMandDatoRuteTableDone.AddCell(FastCell2(InformationClass.date1, styleFont.UserInputMediumSmallText, stylePDF.AlingCenter, Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER));
                    FragtMandDatoRuteTableDone.AddCell(FastCell2(InformationClass.rute1, styleFont.UserInputMediumSmallText, stylePDF.AlingCenter));
                }

                FragtMandDatoRuteTableDone.AddCell(FastCell2("EUR.PALLER(ANTAL)", styleFont.mediumSmallText, null, Rectangle.TOP_BORDER, null));
                FragtMandDatoRuteTableDone.AddCell(FastCell2("BYTTEPALLE", styleFont.mediumSmallText, null, Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER));
                FragtMandDatoRuteTableDone.AddCell(FastCell2("DATO", styleFont.mediumSmallText, null, Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER));
                FragtMandDatoRuteTableDone.AddCell(FastCell2("RUTE", styleFont.mediumSmallText, null, Rectangle.TOP_BORDER));

                FragtMandDatoRuteTableDone.AddCell(FastCell2(eurPallerTable, null, 0, null));
                FragtMandDatoRuteTableDone.AddCell(FastCell2(palleYesNoTable, null, Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER));

                if (InformationClass.emptyPDF)
                {
                    FragtMandDatoRuteTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingCenter, Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER));
                    FragtMandDatoRuteTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingCenter));
                }
                else
                {
                    FragtMandDatoRuteTableDone.AddCell(FastCell2(InformationClass.date2, styleFont.UserInputMediumSmallText, stylePDF.AlingCenter, Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER));
                    FragtMandDatoRuteTableDone.AddCell(FastCell2(InformationClass.rute2, styleFont.UserInputMediumSmallText, stylePDF.AlingCenter));
                }

                #endregion fragtman, dato, rute og byttepaller delen

                #region Godslinjer / pakke indhold


                PdfPTable pakkerTypeCheckBoxTable = new PdfPTable(2);
                pakkerTypeCheckBoxTable.DefaultCell.Border = 0;

                if (InformationClass.emptyPDF)
                {
                    //kurrer transport type
                    pakkerTypeCheckBoxTable.AddCell(FastCell2("Kurretransport", null, stylePDF.AlingCenter, 0, null, null, 2));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("GoRush", false));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("GoFlex", false));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("GoVIP", false));
                    pakkerTypeCheckBoxTable.AddCell("");
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("Grp 1", false, new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("Grp 2", false, new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("Grp 3", false, new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("Grp 4", false, new float[] { 10f, 0, 0, 0 }));

                    pakkerTypeCheckBoxTable.AddCell(FastCell2("Pakketransport", null, stylePDF.AlingCenter, 0, null, null, 2));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("GoGreen", false));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("GoPlus", false));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("XS", false, new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("S", false, new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("M", false, new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("L", false, new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("XL", false, new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("2XL", false, new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("3XL", false, new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell("");

                    pakkerTypeCheckBoxTable.AddCell(FastCell2("Godstransport", null, stylePDF.AlingCenter, 0, null, null, 2));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("GoFull", false));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("GoPart", false));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("LDM", false, new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("PLL", false, new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("M\u00b3", false, new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell("");

                }
                else
                {
                    //kurrer transport type
                    pakkerTypeCheckBoxTable.AddCell(FastCell2("Kurretransport", null, stylePDF.AlingCenter, 0, null, null, 2));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("GoRush", InformationClass.transportTypeUse[0]));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("GoFlex", InformationClass.transportTypeUse[1]));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("GoVIP", InformationClass.transportTypeUse[2]));
                    pakkerTypeCheckBoxTable.AddCell("");
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("Grp 1", InformationClass.transportTypeUse[3], new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("Grp 2", InformationClass.transportTypeUse[4], new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("Grp 3", InformationClass.transportTypeUse[5], new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("Grp 4", InformationClass.transportTypeUse[6], new float[] { 10f, 0, 0, 0 }));

                    pakkerTypeCheckBoxTable.AddCell(FastCell2("Pakketransport", null, stylePDF.AlingCenter, 0, null, null, 2));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("GoGreen", InformationClass.transportTypeUse[7]));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("GoPlus", InformationClass.transportTypeUse[8]));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("XS", InformationClass.transportTypeUse[9], new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("S", InformationClass.transportTypeUse[10], new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("M", InformationClass.transportTypeUse[11], new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("L", InformationClass.transportTypeUse[12], new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("XL", InformationClass.transportTypeUse[13], new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("2XL", InformationClass.transportTypeUse[14], new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("3XL", InformationClass.transportTypeUse[15], new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell("");

                    pakkerTypeCheckBoxTable.AddCell(FastCell2("Godstransport", null, stylePDF.AlingCenter, 0, null, null, 2));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("GoFull", InformationClass.transportTypeUse[16]));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("GoPart", InformationClass.transportTypeUse[17]));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("LDM", InformationClass.transportTypeUse[18], new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("PLL", InformationClass.transportTypeUse[19], new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell(FastCellBox("M\u00b3", InformationClass.transportTypeUse[20], new float[] { 10f, 0, 0, 0 }));
                    pakkerTypeCheckBoxTable.AddCell("");
                }
                ////bil pakke table
                //PdfPTable pakkerCountTable = new PdfPTable(2);
                //pakkerCountTable.DefaultCell.Border = 0;

                //string[] pakkeCountTypeNames;

                ////hvilken transport type der bruges
                //if (InformationClass.transportKurer[0])
                //{
                //    pakkeCountTypeNames = new string[] { "Grp. 1", "Grp. 2", "Grp. 3", "Grp. 4" };
                //}
                //else if (InformationClass.transportPakke[0])
                //{
                //    pakkeCountTypeNames = new string[] { "XS", "S", "M", "L", "XL", "2XL", "3XL" };
                //}

                //else
                //{
                //    pakkeCountTypeNames = new string[] { "LDM", "PLL", "M\u00b3" };
                //}

                ////hvor mange pakke typer der er
                //int pakkeCountTypeNameCount = 0;

                //for (int i = 0; i < pakkeCountTypeNames.Count(); i++)
                //{
                //    //skal kun hvis hvis der er en pakke
                //    if (InformationClass.pakkeStrCount[i] != 0)
                //    {
                //        pakkerCountTable.AddCell(FastCell2(pakkeCountTypeNames[i], styleFont.mediumBigText, PdfStyle.AlingCenter, Rectangle.BOTTOM_BORDER, PdfStyle.BorderSize_2f));
                //        pakkerCountTable.AddCell(FastCell2(InformationClass.pakkeStrCount[i].ToString(), styleFont.normalText, PdfStyle.AlingCenter, Rectangle.BOTTOM_BORDER, PdfStyle.BorderSize_2f));
                //        pakkeCountTypeNameCount++;
                //    }
                //}

                //tabel af godlinje indhold
                PdfPTable GodsLinjeTableDone = new PdfPTable(6);
                GodsLinjeTableDone.DefaultCell.Border = 0;

                float[] widthGods = { 50f, 50f, 60f, 210f, 110f, 80f };
                GodsLinjeTableDone.SetWidths(widthGods);

                //tilføj header
                GodsLinjeTableDone.AddCell(FastCell2("MRK./NR.", styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_Top_3f_side_1f));
                GodsLinjeTableDone.AddCell(FastCell2("ANTAL", styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_Top_3f_side_1f));
                GodsLinjeTableDone.AddCell(FastCell2("ART", styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_Top_3f_side_1f));
                GodsLinjeTableDone.AddCell(FastCell2("INDHOLD", styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_Top_3f_side_1f));
                GodsLinjeTableDone.AddCell(FastCell2("TRANSPORT TYPE", styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_Top_3f_side_1f));
                GodsLinjeTableDone.AddCell(FastCell2("VÆGT/\nRUMFANG", styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_Top_3f_side_1f, stylePDF.Padding_5fTopBottom));


                //antal af pakker
                int pakkeCount = 0;
                double kiloAll = 0;

                if (InformationClass.emptyPDF)
                {
                    GodsLinjeTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER));
                    GodsLinjeTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingRightCenter, Rectangle.RIGHT_BORDER));
                    GodsLinjeTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER));
                    GodsLinjeTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER));
                    GodsLinjeTableDone.AddCell(FastCell2(pakkerTypeCheckBoxTable, null, 0, null, null, 1, 16));
                    GodsLinjeTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingCenter, Rectangle.LEFT_BORDER));
                }
                else
                {

                    pakkeCount = InformationClass.pakkeIndhold.Count;

                    for (int i = 0; i < pakkeCount; i++)
                    {
                        GodsLinjeTableDone.AddCell(FastCell2(InformationClass.pakkeAdresseAndNumber[i], styleFont.UserInputMediumSmallText, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER));
                        GodsLinjeTableDone.AddCell(FastCell2(InformationClass.pakkeCount[i].ToString(), styleFont.UserInputMediumSmallText, stylePDF.AlingRightCenter, Rectangle.RIGHT_BORDER));
                        GodsLinjeTableDone.AddCell(FastCell2(InformationClass.pakkeART[i], styleFont.UserInputMediumSmallText, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER));
                        GodsLinjeTableDone.AddCell(FastCell2(InformationClass.pakkeIndhold[i], styleFont.UserInputMediumSmallText, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER));


                        //tilføj bilpakke hvis det først og hvis der er flere end pakkeCountTypeNameCount pakker skal der også til en tom cell
                        if (i == 0)
                        {
                            GodsLinjeTableDone.AddCell(FastCell2(pakkerTypeCheckBoxTable, null, 0, null, null, 1, 15));
                        }
                        if (i >= 15)
                        {
                            GodsLinjeTableDone.AddCell("");
                        }


                        //om den skal skrive både vægt og rumfang
                        if (InformationClass.pakkeRumOrWidthBoth[i])
                        {
                            GodsLinjeTableDone.AddCell(FastCell2(InformationClass.pakkeWidth[i].ToString() + "kg \n" + InformationClass.pakkeRumfang[i], styleFont.UserInputMediumSmallText, stylePDF.AlingCenter, Rectangle.LEFT_BORDER));
                            kiloAll += InformationClass.pakkeWidth[i];
                        }
                        //om det er kilo eller rumfang
                        //hvis true er det kilo
                        else
                        {
                            if (InformationClass.pakkeRumOrWidth[i])
                            {
                                GodsLinjeTableDone.AddCell(FastCell2(InformationClass.pakkeWidth[i].ToString() + "kg", styleFont.UserInputMediumSmallText, stylePDF.AlingCenter, Rectangle.LEFT_BORDER));
                                kiloAll += InformationClass.pakkeWidth[i];
                            }
                            else
                            {
                                GodsLinjeTableDone.AddCell(FastCell2(InformationClass.pakkeRumfang[i], styleFont.UserInputMediumSmallText, stylePDF.AlingCenter, Rectangle.LEFT_BORDER));
                            }
                        }
                    }
                }
                //tjek om der er mindre pakker end pakkeCountTypeNameCount
                if (pakkeCount < 15)
                {
                    int pakkerLeft = 15 - pakkeCount;

                    for (int i = 0; i < pakkerLeft; i++)
                    {
                        GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.RIGHT_BORDER));
                        GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.RIGHT_BORDER));
                        GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.RIGHT_BORDER));
                        GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.RIGHT_BORDER));
                        GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.LEFT_BORDER));
                    }
                }

                //tilføj vægt i alt

                GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_05f));
                GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_05f));
                GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_05f));
                GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_05f));
                GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_05f));
                GodsLinjeTableDone.AddCell(FastCell2("VÆGT I ALT", styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.TOP_BORDER, stylePDF.BorderSize_3f));

                GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_05f));
                GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_05f));
                GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_05f));
                GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_05f));
                GodsLinjeTableDone.AddCell(FastCell2("", null, null, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_05f));

                if (InformationClass.emptyPDF)
                {
                    GodsLinjeTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingCenter, Rectangle.TOP_BORDER));
                }
                else
                {
                    GodsLinjeTableDone.AddCell(FastCell2(kiloAll.ToString() + "kg", styleFont.UserInputMediumSmallText, stylePDF.AlingCenter, Rectangle.TOP_BORDER));
                }
                #endregion Godslinjer / pakke indhold

                #region Modtager tabel

                PdfPTable ModtagerTableDone = new PdfPTable(2);
                ModtagerTableDone.DefaultCell.Border = 0;

                ModtagerTableDone.AddCell(FastCell2("MODTAGER", styleFont.mediumSmallText, null, 0, null, stylePDF.Padding_5fLeft_5fTopBottom));
                ModtagerTableDone.AddCell(FastCell2("TELEFONNR.", styleFont.mediumSmallText, null, 0, null, stylePDF.Padding_5fTopBottom));

                if (InformationClass.emptyPDF)
                {
                    ModtagerTableDone.AddCell(FastCell2("§§§\n§§§\n§§§", styleFont.mediumSmallTextWhite, null, 0, null, stylePDF.Padding_5fLeft));
                    ModtagerTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, null, 0, null, stylePDF.Padding_5fLeft));
                }
                else
                {
                    ModtagerTableDone.AddCell(FastCell2(InformationClass.Modtager.Name + "\n" + InformationClass.Modtager.Adresse + "\n" + InformationClass.Modtager.Post, styleFont.UserInputMediumSmallText, null, 0, null, stylePDF.Padding_5fLeft));
                    ModtagerTableDone.AddCell(FastCell2(InformationClass.Modtager.Telefon, styleFont.UserInputMediumSmallText, null, 0, null, stylePDF.Padding_5fLeft));
                }

                ModtagerTableDone.AddCell(whiteSpace);
                ModtagerTableDone.AddCell(whiteSpace);

                ModtagerTableDone.AddCell(FastCell2("POSTNUMMER", styleFont.mediumSmallText, stylePDF.AlingLeftBottom, 0, null, stylePDF.Padding_5fLeft));

                if (InformationClass.emptyPDF)
                {
                    ModtagerTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite));
                }
                else
                {
                    ModtagerTableDone.AddCell(FastCell2(InformationClass.Modtager.Post.Substring(0, 4), styleFont.UserInputMediumSmallText));
                }

                #endregion Modtager tabel

                #region Efterkrav tabel

                PdfPTable EfterkrevTableDone = new PdfPTable(2);
                EfterkrevTableDone.DefaultCell.Border = 0;
                float[] modtagerWidhtDel2 = { 1.5f, 1f };
                EfterkrevTableDone.SetWidths(modtagerWidhtDel2);

                EfterkrevTableDone.AddCell(FastCell2("EFTERKRAV\n(INKL. GEBYR)", styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER, stylePDF.BorderSize_1f, null, 1, 1, 30f));

                if (InformationClass.emptyPDF)
                {
                    EfterkrevTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingCenter, Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_1f));
                }
                else
                {
                    EfterkrevTableDone.AddCell(FastCell2(InformationClass.efterKrav, styleFont.UserInputMediumSmallText, stylePDF.AlingCenter, Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_1f));
                }
                EfterkrevTableDone.AddCell(FastCell2("", styleFont.mediumSmallText, null, 0, null, null, 1, 1, 30f));
                EfterkrevTableDone.AddCell(FastCell2("", styleFont.mediumSmallText));

                #endregion Efterkrav tabel

                #region Franko / UFranko table

                PdfPTable FrankoUfrankoTableDone = new PdfPTable(2);
                FrankoUfrankoTableDone.DefaultCell.Border = 0;

                Chunk FrankoChunk = new Chunk("FRANKO", styleFont.FrankoText);
                Chunk UFrankoChunk = new Chunk("UFRANKO", styleFont.FrankoText);


                FrankoUfrankoTableDone.AddCell(FastCell2("DET UBENYTTEDE BEDES OVERSTREGET", styleFont.mediumSmallTextUnderLine, null, 0, null, null, 2));


                //hvad der bliver sktreget over
                if (!InformationClass.emptyPDF)
                {
                    if (InformationClass.senderPay)
                    {
                        UFrankoChunk.SetUnderline(2f, 5f);
                    }
                    else
                    {
                        FrankoChunk.SetUnderline(2f, 4f);
                    }
                }
                PdfPCell FrankoCell = new PdfPCell(new Paragraph(FrankoChunk));
                FrankoCell.Border = 0;
                FrankoCell.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                PdfPCell UFrankoCell = new PdfPCell(new Paragraph(UFrankoChunk));
                UFrankoCell.Border = 0;
                UFrankoCell.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                FrankoUfrankoTableDone.AddCell(FrankoCell);
                FrankoUfrankoTableDone.AddCell(UFrankoCell);


                #endregion Franko / UFranko table

                #region ABCD - Kvittering

                //
                //ABCD
                //
                PdfPTable modtagerTableDel5 = new PdfPTable(5);
                modtagerTableDel5.DefaultCell.Border = 0;

                string typeIsA = "",
                    typeIsB = "",
                    typeIsC = "",
                    typeIsD = "";

                if (!InformationClass.emptyPDF)
                {
                    switch (InformationClass.typeABCD)
                    {
                        case 1:
                            typeIsA = "X";
                            break;
                        case 2:
                            typeIsB = "X";
                            break;
                        case 3:
                            typeIsC = "X";
                            break;
                        case 4:
                            typeIsD = "X";
                            break;
                    }
                }
                modtagerTableDel5.AddCell(FastCell2("A", styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_2f));
                modtagerTableDel5.AddCell(FastCell2(typeIsA, styleFont.normalTextBold, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_2f));
                modtagerTableDel5.AddCell(FastCell2("C", styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_2f));
                modtagerTableDel5.AddCell(FastCell2(typeIsC, styleFont.normalTextBold, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_2f));
                modtagerTableDel5.AddCell(whiteSpace);

                modtagerTableDel5.AddCell(FastCell2("B", styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER, stylePDF.BorderSize_2f));
                modtagerTableDel5.AddCell(FastCell2(typeIsB, styleFont.normalTextBold, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER, stylePDF.BorderSize_2f));
                modtagerTableDel5.AddCell(FastCell2("D", styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER, stylePDF.BorderSize_2f));
                modtagerTableDel5.AddCell(FastCell2(typeIsD, styleFont.normalTextBold, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER, stylePDF.BorderSize_2f));
                modtagerTableDel5.AddCell(whiteSpace);

                //
                //FORSIKRINGSSUM delen og acbd
                //
                PdfPTable AbcdForsikringsumTableDone = new PdfPTable(4);
                AbcdForsikringsumTableDone.DefaultCell.Border = 0;
                float[] modtagerWidhtDel6 = { 1.7f, 2.9f, 1.6f, 1.6f };
                AbcdForsikringsumTableDone.SetWidths(modtagerWidhtDel6);

                AbcdForsikringsumTableDone.AddCell(FastCell2(modtagerTableDel5, null, 0, null, null, 1, 2));

                AbcdForsikringsumTableDone.AddCell(FastCell2("FORSIKRINGSSUM KR.", styleFont.mediumSmallText, null, Rectangle.RIGHT_BORDER + Rectangle.LEFT_BORDER, stylePDF.BorderSize_2f));
                AbcdForsikringsumTableDone.AddCell(FastCell2("PRÆMIE KR.", styleFont.mediumSmallText, null, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_2f));
                AbcdForsikringsumTableDone.AddCell(FastCell2("KVITTERING", styleFont.mediumSmallText));

                if (InformationClass.emptyPDF)
                {
                    AbcdForsikringsumTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, null, Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER + Rectangle.LEFT_BORDER, stylePDF.BorderSize_2f));
                    AbcdForsikringsumTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, null, Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER, stylePDF.BorderSize_2f));
                }
                else
                {
                    AbcdForsikringsumTableDone.AddCell(FastCell2(InformationClass.forsikringSum, styleFont.UserInputMediumSmallText, null, Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER + Rectangle.LEFT_BORDER, stylePDF.BorderSize_2f));
                    AbcdForsikringsumTableDone.AddCell(FastCell2(InformationClass.praemieSum, styleFont.UserInputMediumSmallText, null, Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER, stylePDF.BorderSize_2f));
                }

                AbcdForsikringsumTableDone.AddCell(FastCell2("", styleFont.normalText, null, Rectangle.TOP_BORDER, stylePDF.BorderSize_2f));



                #endregion ABCD - Kvittering

                #region Fragt / sum ialt table

                PdfPTable FragtSumTableDone = new PdfPTable(2);
                FragtSumTableDone.DefaultCell.Border = 0;

                FragtSumTableDone.AddCell(FastCell2("FRAGT", styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_2f, null, 1, 1, 25f));
                FragtSumTableDone.AddCell(whiteSpace);

                FragtSumTableDone.AddCell(FastCell2("I ALT. KR.\nINKL. MOMS", styleFont.mediumSmallText, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER, stylePDF.BorderSize_2f, null, 1, 1, 25f));

                if (InformationClass.emptyPDF)
                {
                    FragtSumTableDone.AddCell(FastCell2("§§§", styleFont.mediumSmallTextWhite, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER, stylePDF.BorderSize_2f, null, 1, 1, 25f));
                }
                else
                {
                    FragtSumTableDone.AddCell(FastCell2(InformationClass.iAltSum, styleFont.UserInputMediumSmallText, stylePDF.AlingCenter, Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER, stylePDF.BorderSize_2f, null, 1, 1, 25f));
                }
                #endregion Fragt / sum ialt table

                #region Forvalter - Modtagers kundenummer table

                PdfPTable ForvalterModtagersNumberTableDone = new PdfPTable(4);
                ForvalterModtagersNumberTableDone.DefaultCell.Border = 0;
                //float[] modtagerWidhtDel7 = { 1f, 2f, 2f, 2f };
                //modtagerTableDel7.SetWidths(modtagerWidhtDel7);

                ForvalterModtagersNumberTableDone.AddCell(FastCell2("FORVALTER/DISPONENT", styleFont.mediumSmallText, null, Rectangle.RIGHT_BORDER, null, stylePDF.Padding_5fLeft_5fTopBottom));
                ForvalterModtagersNumberTableDone.AddCell(FastCell2("KVITTERING FOR MODTAGELSEN", styleFont.mediumSmallText, null, Rectangle.RIGHT_BORDER, null, stylePDF.Padding_5fTopBottom));
                ForvalterModtagersNumberTableDone.AddCell(FastCell2("FRAGTMANDENS KVITTERING", styleFont.mediumSmallText, null, Rectangle.RIGHT_BORDER, null, stylePDF.Padding_5fTopBottom));
                ForvalterModtagersNumberTableDone.AddCell(FastCell2("MODTAGERS KUNDENUMMER", styleFont.mediumSmallText, null, 0, null, stylePDF.Padding_5fTopBottom));

                if (InformationClass.creatorIds == null)
                {
                    ForvalterModtagersNumberTableDone.AddCell(FastCell2("§§§", styleFont.WhiteText, null, Rectangle.RIGHT_BORDER));
                }
                else
                {
                    ForvalterModtagersNumberTableDone.AddCell(FastCell2(InformationClass.creatorIds, styleFont.mediumSmallText, null, Rectangle.RIGHT_BORDER));
                }

                ForvalterModtagersNumberTableDone.AddCell(FastCell2("§§§", styleFont.WhiteText, null, Rectangle.RIGHT_BORDER));
                ForvalterModtagersNumberTableDone.AddCell(FastCell2("§§§", styleFont.WhiteText, null, Rectangle.RIGHT_BORDER));

                if (InformationClass.emptyPDF)
                {
                    ForvalterModtagersNumberTableDone.AddCell(FastCell2("§§§", styleFont.WhiteText, null, 0, null, new float[] { 3f, 3f, 3f, 3f }));
                }
                else
                {
                    ForvalterModtagersNumberTableDone.AddCell(FastCell2(InformationClass.Modtager.Telefon, styleFont.UserInputMediumSmallText, null, 0, null, new float[] { 3f, 3f, 3f, 3f }));
                }

                #endregion Forvalter - Modtagers kundenummer table

                #region Etv bemærkninger

                PdfPTable ekstraCommentTable = new PdfPTable(1);
                ekstraCommentTable.DefaultCell.Border = 0;

                ekstraCommentTable.AddCell(new Paragraph("Evt. Bemærkninger", styleFont.mediumSmallText));

                if (InformationClass.emptyPDF)
                {
                    ekstraCommentTable.AddCell(new Paragraph("§§§", styleFont.mediumSmallTextWhite));
                }
                else
                {
                    ekstraCommentTable.AddCell(new Paragraph(InformationClass.EkstraComment, styleFont.UserInputMediumSmallText));
                }

                PdfPCell ekstraCommentCell = new PdfPCell(ekstraCommentTable);
                ekstraCommentCell.Border = Rectangle.BOX;

                ekstraCommentCell.MinimumHeight = 70f;

                #endregion Etv bemærkninger

                #region tidsforbrug

                PdfPTable tidforbrugtable = new PdfPTable(2);
                //tidforbrugtable.DefaultCell.Border = 0;

                PdfPTable tidforbrugtableAfsender = new PdfPTable(1);
                //tidforbrugtableAfsender.DefaultCell.Border = Rectangle.BOX;

                //afsender box
                tidforbrugtableAfsender.AddCell(FastCell2("Hos Afsender", styleFont.mediumTextBold, stylePDF.AlingCenter, Rectangle.BOTTOM_BORDER, null, null, 2));

                tidforbrugtableAfsender.AddCell(WriteBox("Ankomst"));
                tidforbrugtableAfsender.AddCell(WriteBox("Afgang"));
                tidforbrugtableAfsender.AddCell(WriteBox("Total"));



                PdfPTable tidforbrugtableModtager = new PdfPTable(1);
                //tidforbrugtableModtager.DefaultCell.Border = 0;

                tidforbrugtableModtager.AddCell(FastCell2("Hos Modtager", styleFont.mediumTextBold, stylePDF.AlingCenter, Rectangle.BOTTOM_BORDER, null, null, 2));

                tidforbrugtableModtager.AddCell(WriteBox("Ankomst"));
                tidforbrugtableModtager.AddCell(WriteBox("Afgang"));
                tidforbrugtableModtager.AddCell(WriteBox("Total"));

                tidforbrugtable.AddCell(tidforbrugtableAfsender);
                tidforbrugtable.AddCell(tidforbrugtableModtager);
                /*
                tidforbrugtable.AddCell(whiteSpace);
                tidforbrugtable.AddCell(whiteSpace);
                tidforbrugtable.AddCell(whiteSpace);

                tidforbrugtable.AddCell(FastCell2("Hos Modtager", styleFont.mediumTextBold, stylePDF.AlingCenter, Rectangle.BOX, null, null, 2));
                tidforbrugtable.AddCell(FastCell2("Ankomst", styleFont.mediumSmallTextBold, PdfStyle.AlingCenter));
                tidforbrugtable.AddCell(FastCell2("Afgang", styleFont.mediumSmallTextBold, PdfStyle.AlingCenter));
                tidforbrugtable.AddCell(writeText);
                tidforbrugtable.AddCell(writeText);
                tidforbrugtable.AddCell(FastCell2("Total", styleFont.mediumSmallTextBold, PdfStyle.AlingCenter));
                tidforbrugtable.AddCell(whiteSpace);
                tidforbrugtable.AddCell(writeText);
                tidforbrugtable.AddCell(whiteSpace);
                */
                #endregion tidsforbrug

                #region Sæt alle tabllerne sammen


                //sæt kundenummer sammen med afsender siden
                PdfPTable fragtTableDoneDel1 = new PdfPTable(2);
                fragtTableDoneDel1.DefaultCell.Border = 0;

                fragtTableDoneDel1.AddCell(FastCell2(KundeNumberTableDone, null, 0, null, null, 2));
                //fragtTableDoneDel1.AddCell(FastCell2(AfsenderTableDone));

                //sæt modtager info sammen med efterkrav
                PdfPTable fragtTableDoneDel2 = new PdfPTable(2);
                fragtTableDoneDel2.DefaultCell.Border = 0;
                fragtTableDoneDel2.SetWidths(new float[] { 2f, 1f });

                fragtTableDoneDel2.AddCell(FastCell2(ModtagerTableDone, null, Rectangle.RIGHT_BORDER, stylePDF.BorderSize_3f));
                fragtTableDoneDel2.AddCell(FastCell2(EfterkrevTableDone));

                //sæt franko, ABCD/Kvittering og fragt/Sum ialt sammen
                PdfPTable fragtTableDoneDel3 = new PdfPTable(2);
                fragtTableDoneDel3.DefaultCell.Border = 0;
                fragtTableDoneDel3.SetWidths(new float[] { 3f, 1.4f });

                fragtTableDoneDel3.AddCell(FastCell2(FrankoUfrankoTableDone, null, Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_3f));
                fragtTableDoneDel3.AddCell(FastCell2(FragtSumTableDone, null, Rectangle.LEFT_BORDER, stylePDF.BorderSize_3f, null, 1, 2));
                fragtTableDoneDel3.AddCell(FastCell2(AbcdForsikringsumTableDone));

                //sæt tidforbrug vedsiden af bemærkninger

                PdfPTable fragtTableDoneDel4 = new PdfPTable(2);
                fragtTableDoneDel4.DefaultCell.Border = 0;
                fragtTableDoneDel4.AddCell(tidforbrugtable);
                fragtTableDoneDel4.AddCell(ekstraCommentCell);


                //
                //table der indeholder de andre tabler
                //
                PdfPTable AllTableFinalDone = new PdfPTable(1);
                AllTableFinalDone.DefaultCell.Border = 0;

                AllTableFinalDone.AddCell(FastCell2(fragtTableDoneDel1));
                AllTableFinalDone.AddCell(FastCell2(FragtMandDatoRuteTableDone, null, Rectangle.BOX - Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_3f));
                AllTableFinalDone.AddCell(FastCell2(GodsLinjeTableDone, null, Rectangle.BOX - Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_3f));
                AllTableFinalDone.AddCell(FastCell2(fragtTableDoneDel2, null, Rectangle.BOX - Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_3f));
                AllTableFinalDone.AddCell(FastCell2(fragtTableDoneDel3, null, Rectangle.BOX - Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_3f));
                AllTableFinalDone.AddCell(FastCell2(ForvalterModtagersNumberTableDone, null, Rectangle.BOX - Rectangle.BOTTOM_BORDER, stylePDF.BorderSize_3f));

                string RedSmallBottomText = "TRANSPORTEN UDFØRES I HENHOLD TIL CMR-REGLERNE";
                AllTableFinalDone.AddCell(FastCell2(RedSmallBottomText, styleFont.smallTextRed, stylePDF.AlingCenter, Rectangle.TOP_BORDER, stylePDF.BorderSize_3f));

                AllTableFinalDone.AddCell(whiteSpace);
                AllTableFinalDone.AddCell(fragtTableDoneDel4);
                #endregion Sæt alle tabllerne sammen


                //gem tablen så den kan blive lavet senere
                return AllTableFinalDone;
            }

            private PdfPCell WriteBox(string header)
            {
                FragtText styleFont = new FragtText();
                PdfStyle stylePDF = new PdfStyle();
                PdfPTable writeTable = new PdfPTable(2);
                writeTable.DefaultCell.Border = 0;
                writeTable.SetTotalWidth(new float[] { 1f, 2f });


                PdfPCell writeText = new PdfPCell(FastCell2("", styleFont.WhiteText, stylePDF.AlingCenter, Rectangle.BOX));
                writeText.Padding = 5f;
                writeText.MinimumHeight = 20f;

                writeTable.AddCell(FastCell2(header, styleFont.mediumSmallTextBold, stylePDF.AlingRightCenter));
                writeTable.AddCell(writeText);

                PdfPCell writeCellbox = new PdfPCell(writeTable);
                writeCellbox.Border = 0;

                return writeCellbox;
            }

            private PdfPCell FastCellBox(string Text, bool IshakBox = false, float[] Padding = null, Font FontType = null)
            {
                FragtText styleFont = new FragtText();

                //Checkbox billede
                string checkboxB = "";
                if (IshakBox)
                {
                    checkboxB = "boxCheck.png";
                }
                else
                {
                    checkboxB = "box.png";
                }
                Image CheckboxImage = Image.GetInstance(Directory.GetCurrentDirectory() + "/Images/Pdf/" + checkboxB);
                CheckboxImage.ScaleToFit(10f, 10f);

                PdfPCell CheckboxCell = new PdfPCell(CheckboxImage);
                CheckboxCell.Border = Rectangle.NO_BORDER;
                CheckboxCell.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                //sæt null values
                FontType = FontType ?? styleFont.mediumSmallText;
                Padding = Padding ?? new float[] { 0, 0, 0, 0 };
                //opret cell
                PdfPCell cell = new PdfPCell(new Paragraph(Text, FontType));
                cell.Border = 0;

                PdfPTable table = new PdfPTable(2);
                table.DefaultCell.Border = 0;
                table.SetWidths(new float[] { 0.5f, 1f });

                table.AddCell(CheckboxImage);
                table.AddCell(cell);


                PdfPCell cellDone = new PdfPCell(table);
                cellDone.Border = 0;

                cellDone.PaddingLeft = Padding[0];
                cellDone.PaddingTop = Padding[1];
                cellDone.PaddingRight = Padding[2];
                cellDone.PaddingBottom = Padding[3];

                return cellDone;
            }
            private PdfPCell FastCell2(string Text, Font FontType = null, int[] TextAlign = null, int Border = 0, float[] BorderWidth = null, float[] Padding = null, int Colspan = 1, int Rowspan = 1, float Height = 0)
            {
                FragtText styleFont = new FragtText();

                //sæt null values
                FontType = FontType ?? styleFont.mediumSmallText;
                TextAlign = TextAlign ?? new int[] { 0, 0 };
                BorderWidth = BorderWidth ?? new float[] { 0.5f, 0.5f, 0.5f, 0.5f };

                //opret cell
                PdfPCell cell = new PdfPCell(new Paragraph(Text, FontType));

                //Cell align
                cell.HorizontalAlignment = TextAlign[0];
                cell.VerticalAlignment = TextAlign[1];


                if (Border != 0)
                {
                    cell.BorderColor = BaseColor.BLACK;

                    cell.BorderWidthLeft = BorderWidth[0];
                    cell.BorderWidthTop = BorderWidth[1];
                    cell.BorderWidthRight = BorderWidth[2];
                    cell.BorderWidthBottom = BorderWidth[3];
                }

                //border
                cell.Border = Border;

                //padding
                if (Padding != null)
                {
                    cell.PaddingLeft = Padding[0];
                    cell.PaddingTop = Padding[1];
                    cell.PaddingRight = Padding[2];
                    cell.PaddingBottom = Padding[3];
                }


                //span
                cell.Colspan = Colspan;
                cell.Rowspan = Rowspan;

                if (Height != 0)
                {
                    cell.MinimumHeight = Height;
                }

                return cell;
            }

            private PdfPCell FastCellBold(string Text, Font FontType = null, int[] TextAlign = null, int Border = 0, float[] BorderWidth = null, float[] Padding = null, int Colspan = 1, int Rowspan = 1, float Height = 0)
            {
                FragtText styleFont = new FragtText();

                //sæt null values
                FontType = FontType ?? styleFont.mediumSmallText;
                TextAlign = TextAlign ?? new int[] { 0, 0 };
                BorderWidth = BorderWidth ?? new float[] { 0.5f, 0.5f, 0.5f, 0.5f };


                Chunk TextChunk = new Chunk(Text, FontType);
                TextChunk.SetTextRise(10f);

                //opret cell
                PdfPCell cell = new PdfPCell(new Paragraph(TextChunk));

                //Cell align
                cell.HorizontalAlignment = TextAlign[0];
                cell.VerticalAlignment = TextAlign[1];


                if (Border != 0)
                {
                    cell.BorderColor = BaseColor.BLACK;

                    cell.BorderWidthLeft = BorderWidth[0];
                    cell.BorderWidthTop = BorderWidth[1];
                    cell.BorderWidthRight = BorderWidth[2];
                    cell.BorderWidthBottom = BorderWidth[3];
                }

                //border
                cell.Border = Border;

                //padding
                if (Padding != null)
                {
                    cell.PaddingLeft = Padding[0];
                    cell.PaddingTop = Padding[1];
                    cell.PaddingRight = Padding[2];
                    cell.PaddingBottom = Padding[3];
                }


                //span
                cell.Colspan = Colspan;
                cell.Rowspan = Rowspan;

                if (Height != 0)
                {
                    cell.MinimumHeight = Height;
                }

                return cell;
            }

            private PdfPCell FastCell2(PdfPTable PdfTableCell, int[] TextAlign = null, int Border = 0, float[] BorderWidth = null, float[] Padding = null, int Colspan = 1, int Rowspan = 1, bool UseBorderBaseColorAsBackBaseColor = false)
            {
                //sæt null values
                TextAlign = TextAlign ?? new int[] { 0, 0 };
                BorderWidth = BorderWidth ?? new float[] { 0.5f, 0.5f, 0.5f, 0.5f };

                //opret cell
                PdfPCell cell = new PdfPCell(PdfTableCell);

                //Cell align
                cell.HorizontalAlignment = TextAlign[0];
                cell.VerticalAlignment = TextAlign[1];


                if (Border != 0)
                {
                    cell.BorderColor = BaseColor.BLACK;

                    cell.BorderWidthLeft = BorderWidth[0];
                    cell.BorderWidthTop = BorderWidth[1];
                    cell.BorderWidthRight = BorderWidth[2];
                    cell.BorderWidthBottom = BorderWidth[3];
                }

                //border
                cell.Border = Border;

                //padding
                if (Padding != null)
                {
                    cell.PaddingLeft = Padding[0];
                    cell.PaddingTop = Padding[1];
                    cell.PaddingRight = Padding[2];
                    cell.PaddingBottom = Padding[3];
                }

                //span
                cell.Colspan = Colspan;
                cell.Rowspan = Rowspan;

                //baground BaseColor
                if (UseBorderBaseColorAsBackBaseColor)
                {
                    cell.BackgroundColor = BaseColor.BLACK;
                }

                return cell;
            }

            private PdfPCell EmptySpace(float height, int Border = 0, float[] BorderWidth = null, float[] Padding = null, int Colspan = 1, int Rowspan = 1)
            {
                BorderWidth = BorderWidth ?? new float[] { 0.5f, 0.5f, 0.5f, 0.5f };

                PdfPCell whiteSpaceText = new PdfPCell();
                whiteSpaceText.Border = Rectangle.NO_BORDER;
                //whiteSpaceText.BackgroundColor = BaseColor.BLUE;

                whiteSpaceText.FixedHeight = height;

                if (Border != 0)
                {
                    whiteSpaceText.BorderColor = BaseColor.BLACK;

                    whiteSpaceText.BorderWidthLeft = BorderWidth[0];
                    whiteSpaceText.BorderWidthTop = BorderWidth[1];
                    whiteSpaceText.BorderWidthRight = BorderWidth[2];
                    whiteSpaceText.BorderWidthBottom = BorderWidth[3];
                }

                //border
                whiteSpaceText.Border = Border;

                //padding
                if (Padding != null)
                {
                    whiteSpaceText.PaddingLeft = Padding[0];
                    whiteSpaceText.PaddingTop = Padding[1];
                    whiteSpaceText.PaddingRight = Padding[2];
                    whiteSpaceText.PaddingBottom = Padding[3];
                }

                //span
                whiteSpaceText.Colspan = Colspan;
                whiteSpaceText.Rowspan = Rowspan;

                return whiteSpaceText;
            }


            private System.Drawing.Image CreateBarcode(string barcode)
            {
                #region Barcode Table
                Dictionary<string, string> barcodeTable = new Dictionary<string, string>();
                barcodeTable.Add("0", "101001101101");
                barcodeTable.Add("1", "110100101011");
                barcodeTable.Add("2", "101100101011");
                barcodeTable.Add("3", "110110010101");
                barcodeTable.Add("4", "101001101011");
                barcodeTable.Add("5", "110100110101");
                barcodeTable.Add("6", "101100110101");
                barcodeTable.Add("7", "101001011011");
                barcodeTable.Add("8", "110100101101");
                barcodeTable.Add("9", "101100101101");
                barcodeTable.Add("A", "110101001011");
                barcodeTable.Add("B", "101101001011");
                barcodeTable.Add("C", "110110100101");
                barcodeTable.Add("D", "101011001011");
                barcodeTable.Add("E", "110101100101");
                barcodeTable.Add("F", "101101100101");
                barcodeTable.Add("G", "101010011011");
                barcodeTable.Add("H", "110101001101");
                barcodeTable.Add("I", "101101001101");
                barcodeTable.Add("J", "101011001101");
                barcodeTable.Add("K", "110101010011");
                barcodeTable.Add("L", "101101010011");
                barcodeTable.Add("M", "110110101001");
                barcodeTable.Add("N", "101011010011");
                barcodeTable.Add("O", "110101101001");
                barcodeTable.Add("P", "101101101001");
                barcodeTable.Add("Q", "101010110011");
                barcodeTable.Add("R", "110101011001");
                barcodeTable.Add("S", "101101011001");
                barcodeTable.Add("T", "101011011001");
                barcodeTable.Add("U", "110010101011");
                barcodeTable.Add("V", "100110101011");
                barcodeTable.Add("W", "110011010101");
                barcodeTable.Add("X", "100101101011");
                barcodeTable.Add("Y", "110010110101");
                barcodeTable.Add("Z", "100110110101");
                barcodeTable.Add("-", "100101011011");
                barcodeTable.Add(".", "110010101101");
                barcodeTable.Add(" ", "100110101101");
                barcodeTable.Add("$", "100100100101");
                barcodeTable.Add("/", "100100101001");
                barcodeTable.Add("+", "100101001001");
                barcodeTable.Add("%", "101001001001");
                barcodeTable.Add("*", "100101101101");
                #endregion

                int size = 12;
                int oneBarLengt = 13;
                int imageLengt = (oneBarLengt * barcode.Length * size) - size - (size / 2);

                var img = new System.Drawing.Bitmap(imageLengt, 45 * size);
                var blackPen = new System.Drawing.Pen(System.Drawing.Color.Black, size);
                var whitePen = new System.Drawing.Pen(System.Drawing.Color.White, size);
                var drawing = System.Drawing.Graphics.FromImage(img);

                //sæt start position
                int positionX = size /2;

                for (int i = 0; i < barcode.Length; i++)
                {
                    string oneLetter = barcode.Substring(i, 1);

                    for (int a = 0; a < barcodeTable[oneLetter].Length; a++)
                    {
                        int codeId = int.Parse(barcodeTable[oneLetter].Substring(a, 1));
                        var writePen = codeId == 1 ? blackPen : whitePen;

                        drawing.DrawLine(writePen, positionX, 0, positionX, 30 * size);
                        positionX += size;
                    }

                    //afslut barcode for et tal/bogstav
                    if (i != barcode.Length -1)
                    {
                        drawing.DrawLine(whitePen, positionX, 0, positionX, 30 * size);
                        positionX += size;
                    }
                }

                //tilføj et mellemrum til barcoden
                string textBarcodeWithSpace = "";
                for (int i = 0; i < barcode.Length; i++)
                {
                    textBarcodeWithSpace += barcode.Substring(i,1);

                    if(i != barcode.Length -1)
                        textBarcodeWithSpace += " ";
                }
                
                var textFont = new System.Drawing.Font(System.Drawing.FontFamily.Families[72], 8 * size, System.Drawing.FontStyle.Bold);
                var textColor = System.Drawing.Brushes.Black;
                var textPoint = new System.Drawing.PointF(imageLengt / 2, 38 * size); //find midten

                //gør så teksten vil være i midten
                var stringFormat = new System.Drawing.StringFormat();
                stringFormat.Alignment = System.Drawing.StringAlignment.Center;
                stringFormat.LineAlignment = System.Drawing.StringAlignment.Center;

                //tilføj teksten
                drawing.DrawString(textBarcodeWithSpace, textFont, textColor, textPoint, stringFormat);

                drawing.Dispose();
                //img.Save(@"G:\Simon\Documents\img.png", System.Drawing.Imaging.ImageFormat.Png);                
                return img as System.Drawing.Image;
            }

        #endregion PDF Fragtbrev

            private bool useBold()
            {
                //om der skal bruges fed skrift på det man skriver i pdf
                FileStream readSettingsFile = new FileStream(Models.ImportantData.g_FolderDB + "Settings.xml", FileMode.Open, FileAccess.Read);

                System.Data.DataSet readSettings = new System.Data.DataSet();
                readSettings.ReadXml(readSettingsFile);

                if (bool.Parse(readSettings.Tables["Settings"].Rows[0][0].ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    public class ITextEvents2 : PdfPageEventHelper
    {

        public string FooterText = "";

        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;


        #region Fields
        private string _header;
        #endregion

        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {

            }
            catch (System.IO.IOException ioe)
            {

            }
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {

            //System.Windows.MessageBox.Show("3");
            base.OnEndPage(writer, document);

            Font baseFontNormal = new Font(Font.FontFamily.HELVETICA, 10f, Font.NORMAL, BaseColor.BLACK);

            Font baseFontBig = new Font(Font.FontFamily.HELVETICA, 10f, Font.BOLD, BaseColor.BLACK);

            //Create PdfTable object

            String text = writer.PageNumber + "/";

            //Add paging to footer
            {

                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.SetTextMatrix(document.PageSize.GetRight(40), document.PageSize.GetBottom(20));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 10);
                cb.AddTemplate(footerTemplate, document.PageSize.GetRight(40) + len, document.PageSize.GetBottom(20));


                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                len = bf.GetWidthPoint(FooterText, 10);
                float bredde = ((document.PageSize.GetRight(0) + document.PageSize.GetLeft(0)) / 2) - len;
                cb.MoveText(bredde, 20);
                cb.ShowText(FooterText);
                cb.EndText();
                //cb.AddTemplate(footerTemplate, document.PageSize.GetRight(100), document.PageSize.GetBottom(20));

            }
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 10);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            footerTemplate.EndText();


        }
    }

}