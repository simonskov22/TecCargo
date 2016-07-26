using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace TecCargo_Faktura.Class
{
    class MakePDF
    {
        private Font DefaultNormalText;

        const int c_Transport_Kurer = 0;
        const int c_Transport_Pakke = 1;
        const int c_Transport_Gods = 2;

        private class FontFragtbrevPDF
        {
            public static Font Big = new Font(Font.FontFamily.HELVETICA, 11f, Font.NORMAL, BaseColor.BLACK);
            public static Font BigBoldRed = new Font(Font.FontFamily.HELVETICA, 11f, Font.BOLD, BaseColor.RED);
            public static Font Normal = new Font(Font.FontFamily.HELVETICA, 7.3f, Font.NORMAL, BaseColor.BLACK);
            public static Font NormalUnderline = new Font(Font.FontFamily.HELVETICA, 7.3f, Font.UNDERLINE, BaseColor.BLACK);
            public static Font NormalBold = new Font(Font.FontFamily.HELVETICA, 7.3f, Font.BOLD, BaseColor.BLACK);
            public static Font MediumBigBold = new Font(Font.FontFamily.HELVETICA, 9f, Font.BOLD, BaseColor.BLACK);
            public static Font NormalRed = new Font(Font.FontFamily.HELVETICA, 7.3f, Font.NORMAL, BaseColor.RED);
            public static Font Small = new Font(Font.FontFamily.HELVETICA, 5.7f, Font.NORMAL, BaseColor.BLACK);
        }
        private class FontFakturaPDF
        {
            public static Font Normal = new Font(Font.FontFamily.HELVETICA, 10f);
            public static Font NormalBold = new Font(Font.FontFamily.HELVETICA, 10f, Font.BOLD);
            public static Font BigWhiteBold = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.WHITE);
            public static Font CustomCargo = new Font(Font.FontFamily.HELVETICA, 15f);
            public static Font CustomInvoicBlue = new Font(Font.FontFamily.HELVETICA, 24f, Font.BOLD, BaseColor.BLUE);
        }
        
        private class BorderPDF
        {
            public static float[] Left = { 0.1f, 0, 0, 0 };
            public static float[] Top = { 0, 0.1f, 0, 0 };
            public static float[] Right = { 0, 0, 0.1f, 0 };
            public static float[] Bottom = { 0, 0, 0, 0.1f };

            public static float[] LeftTop = { 0.1f, 0.1f, 0, 0 };
            public static float[] LeftTopRight = { 0.1f, 0.1f, 0.1f, 0 };
            public static float[] LeftTopBottom = { 0.1f, 0.1f, 0, 0.1f };
            public static float[] LeftRight = { 0.1f, 0, 0.1f, 0 };
            public static float[] LeftRightBottom = { 0.1f, 0, 0.1f, 0.1f };
            public static float[] LeftBottom = { 0.1f, 0, 0, 0.1f };
            public static float[] TopBottom = { 0 , 0.1f, 0, 0.1f };

            public static float[] Box = { 0.1f, 0.1f, 0.1f, 0.1f };
            public static float[] None = { 0, 0, 0, 0 };


        }

        private class PacketFaktura
        {
            public int Count = 1;
            public string Description = "";
            public string Price = "";

        }
        
        public void CreateFaktura(XML_Files.DocData.Layout.DocumentData FileInfo) {
            this.DefaultNormalText = FontFakturaPDF.Normal;
            string pdfFolder = Models.ImportantData.g_FolderPdf;//PDF mappe placering
            string imageFolder = Directory.GetCurrentDirectory() + @"\Images\";//billede mappe placering

            //transport ids
            int idType1 = FileInfo.Transport.transportType[0];
            int idType2 = FileInfo.Transport.transportType[1];
            int idType3 = FileInfo.Transport.transportType[2] == -1 ? 0 : FileInfo.Transport.transportType[2];

            double totalWeight = 0;
            double totalPrices = 0;
            double nettoFragt = 0;
            int packetsCount = 0;

            #region Table Oprettelse

            PdfPTable tableTopLeft = new PdfPTable(1);
            PdfPTable tableTopRight = new PdfPTable(1);
            PdfPTable tableTop = new PdfPTable(3);
            PdfPTable tableMiddleAddress = new PdfPTable(2);
            PdfPTable tableMiddlePackets = new PdfPTable(2);
            PdfPTable tableMiddleNetto = new PdfPTable(2);
            PdfPTable tableMiddleTime = new PdfPTable(2);
            PdfPTable tableMiddleExtra = new PdfPTable(2);
            PdfPTable tableMiddleGebyr = new PdfPTable(2);
            PdfPTable tableMiddle = new PdfPTable(2);
            PdfPTable tableBottomLeft = new PdfPTable(1);
            PdfPTable tableBottomRight = new PdfPTable(3);
            PdfPTable tableBottom = new PdfPTable(3);

            tableTopLeft.DefaultCell.Border = Rectangle.NO_BORDER;
            tableTopRight.DefaultCell.Border = Rectangle.NO_BORDER;
            tableTop.DefaultCell.Border = Rectangle.NO_BORDER;
            tableMiddleAddress.DefaultCell.Border = Rectangle.NO_BORDER;
            tableMiddlePackets.DefaultCell.Border = Rectangle.NO_BORDER;
            tableMiddleNetto.DefaultCell.Border = Rectangle.NO_BORDER;
            tableMiddleTime.DefaultCell.Border = Rectangle.NO_BORDER;
            tableMiddleExtra.DefaultCell.Border = Rectangle.NO_BORDER;
            tableMiddleGebyr.DefaultCell.Border = Rectangle.NO_BORDER;
            tableMiddle.DefaultCell.Border = Rectangle.NO_BORDER;
            tableBottomLeft.DefaultCell.Border = Rectangle.NO_BORDER;
            tableBottom.DefaultCell.Border = Rectangle.NO_BORDER;

            tableMiddle.DefaultCell.Padding = 0;
            tableBottom.DefaultCell.Padding = 0;

            #endregion

            #region TopLeft (Bill to)

            XML_Files.DocData.Layout.AdresseInfo inputsBillTo;

            if (FileInfo.Afsender.isPayer)
                inputsBillTo = FileInfo.Afsender;
            else if (FileInfo.Modtager.isPayer)
                inputsBillTo = FileInfo.Modtager;
            else
                inputsBillTo = FileInfo.Modtager2;


            tableTopLeft.AddCell(AddTextCell("CARGO", TextFont: FontFakturaPDF.CustomCargo));
            tableTopLeft.AddCell(AddTextCell("TEC-Distribution Center",TextFont: FontFakturaPDF.NormalBold));
            tableTopLeft.AddCell(AddTextCell(""));
            tableTopLeft.AddCell(AddTextCell("Stamholmen 201, 2650 Hvidovre"));
            tableTopLeft.AddCell(AddTextCell("(Kontakt: +45 2545 3288)"));
            tableTopLeft.AddCell(AddTextCell("CVR: 12345678"));
            tableTopLeft.AddCell(AddTextCell(""));
            tableTopLeft.AddCell(AddTextCell("BILL TO:", TextFont: FontFakturaPDF.BigWhiteBold, Background: BaseColor.DARK_GRAY, VAlign: Rectangle.ALIGN_MIDDLE));
            tableTopLeft.AddCell(AddTextCell(inputsBillTo.firma));
            tableTopLeft.AddCell(AddTextCell(inputsBillTo.vej));
            tableTopLeft.AddCell(AddTextCell(inputsBillTo.zipCode + " " + inputsBillTo.city));
            tableTopLeft.AddCell(AddTextCell(string.Format("(Kontakt: +45 {0})", inputsBillTo.telefon)));

            #endregion

            #region TopRight (Invoice til logo)

            PdfPTable tableDIC = new PdfPTable(2);
            tableDIC.DefaultCell.Border = Rectangle.NO_BORDER;
            tableDIC.AddCell(AddTextCell("DATE:"));
            tableDIC.AddCell(AddTextCell(DateTime.Now.ToShortDateString(), HAlign: Rectangle.ALIGN_CENTER, 
                Background: BaseColor.LIGHT_GRAY,BorderWidth: BorderPDF.LeftTopRight)
            );
            tableDIC.AddCell(AddTextCell("INVOICE#"));
            tableDIC.AddCell(AddTextCell(FileInfo.Invoice.ToString(), HAlign: Rectangle.ALIGN_CENTER,
                BorderWidth: BorderPDF.LeftTopRight)
            );
            //fjern mellemrum fra telefon nr
            string inputCustomId = FileInfo.Modtager.isPayer ? FileInfo.Modtager.telefon.Replace(" ", "") : FileInfo.Afsender.telefon.Replace(" ", "");
            tableDIC.AddCell(AddTextCell("Customer ID"));
            tableDIC.AddCell(AddTextCell(inputCustomId, HAlign: Rectangle.ALIGN_CENTER,
                BorderWidth: BorderPDF.Box)
            );
            
            Image cargo_logo = Image.GetInstance(imageFolder + @"Pdf\logo.png");
            cargo_logo.ScaleToFit(100f, 100f);

            PdfPCell cellLogo = new PdfPCell(cargo_logo);
            cellLogo.Border = Rectangle.NO_BORDER;
            cellLogo.HorizontalAlignment = Rectangle.ALIGN_CENTER;
            cellLogo.PaddingTop = 8;
            cellLogo.PaddingBottom = 10;

            tableTopRight.AddCell(AddTextCell("INVOICE", 
                TextFont: FontFakturaPDF.CustomInvoicBlue, 
                HAlign: Rectangle.ALIGN_CENTER,
                Padding: new float[] { 2,2,2,5 })
            );
            tableTopRight.AddCell(tableDIC);
            tableTopRight.AddCell(cellLogo);
            #endregion

            #region Top (Sæt TopLeft og TopRigth sammen)

            //tableTop.SetWidths(new float[] { 3f, 1f });
            tableTop.AddCell(tableTopLeft);
            tableTop.AddCell("");
            tableTop.AddCell(tableTopRight);

            #endregion

            #region MiddleAddress (Afsender/Modtager info)

            float[] paddingIndryk = { 5, 2, 2, 2 };
            tableMiddleAddress.AddCell(AddTextCell(FileInfo.Afsender.firma,
                TextFont: FontFakturaPDF.NormalBold,
                BorderWidth: BorderPDF.Left,
                Padding: new float[] { 2, 10, 2, 2 })
            );
            tableMiddleAddress.AddCell(AddTextCell(FileInfo.Modtager.firma,
                TextFont: FontFakturaPDF.NormalBold,
                Padding: new float[] { 2, 10, 2, 2 })
            );
            tableMiddleAddress.AddCell(AddTextCell(FileInfo.Afsender.vej, Padding: paddingIndryk, BorderWidth: BorderPDF.Left));
            tableMiddleAddress.AddCell(AddTextCell(FileInfo.Modtager.vej, Padding: paddingIndryk));
            tableMiddleAddress.AddCell(AddTextCell(FileInfo.Afsender.zipCode + " " + FileInfo.Afsender.city, Padding: paddingIndryk, BorderWidth: BorderPDF.Left));
            tableMiddleAddress.AddCell(AddTextCell(FileInfo.Modtager.zipCode + " " + FileInfo.Modtager.city, Padding: paddingIndryk));
            #endregion

            #region MiddlePackets

            tableMiddlePackets.SetWidths(new float[] { 5f, 1f });
            float[] paddingHeader = { 2, 10f, 2, 2 };
            float[] paddingItems = { 10f, 2, 2, 2 };

            List<PacketFaktura> packetsContainer = new List<PacketFaktura>();

            for (int i = 0; i < FileInfo.Transport.pakker.Count; i++)
            {
                Controls.PakkeControlItem packet = FileInfo.Transport.pakker[i];
                totalWeight += packet.weightD;

                for (int a = 0; a < packet.countI; a++)
                {
                    PacketFaktura newPacket = new PacketFaktura();
                    packetsCount++;

                    switch (idType1)
                    {
                        case c_Transport_Kurer:
                            newPacket.Description = string.Format("{0} {1} KG", packet.contains, packet.weightS);
                            newPacket.Price = "-";
                            break;
                        case c_Transport_Pakke:
                            //Pakke str.M 14 KG
                            newPacket.Description = string.Format("Pakke str.{0} {1} KG", packet.transportTypeName, packet.weightS);
                            newPacket.Price = "-";
                            break;
                        case c_Transport_Gods:
                            //Takstzone:-1, L:12, B:21, H:13, Beregningstype: None Fragtpligtigvægt 11 KG
                            newPacket.Description = string.Format("Takstzone:{0}, L:{1}, B:{2}, H:{3}, Beregningstype: {4}, Fragtpligtigvægt: {5}KG",
                                packet.takstName[a], packet.volumeL, packet.volumeB, packet.volumeH,
                                packet.beregningstypeName[a], packet.weightS);
                            newPacket.Price = "-";
                            break;
                    }

                    int index = packetsContainer.FindIndex(item => item.Description == newPacket.Description && item.Price == newPacket.Price);

                    if (index != -1)
                        packetsContainer[index].Count++;
                    else
                        packetsContainer.Add(newPacket);
                }
            }

            if(packetsContainer.Count > 0)
            {
                tableMiddlePackets.AddCell(AddTextCell("Pakker", BorderWidth: BorderPDF.Left, TextFont: FontFakturaPDF.NormalBold, Padding: paddingHeader));
                tableMiddlePackets.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftRight));

                foreach (var packet in packetsContainer)
                {

                    if (false)
                    {
                        if (idType1 != c_Transport_Kurer)
                        {
                            double packetPrice = 0;
                            double.TryParse(packet.Price, out packetPrice);
                            packet.Price = packetPrice.ToString("N2");

                        }

                        tableMiddlePackets.AddCell(AddTextCell(string.Format("{0}x {1}", packet.Count, packet.Description), BorderWidth: BorderPDF.Left, Padding: paddingItems));
                        tableMiddlePackets.AddCell(AddTextCell(packet.Price, BorderWidth: BorderPDF.LeftRight, HAlign: Rectangle.ALIGN_RIGHT));
                    }
                    else {
                        for (int i = 0; i < packet.Count; i++)
                        {
                            tableMiddlePackets.AddCell(AddTextCell(packet.Description, BorderWidth: BorderPDF.Left, Padding: paddingItems));
                            tableMiddlePackets.AddCell(AddTextCell(packet.Price, BorderWidth: BorderPDF.LeftRight, HAlign: Rectangle.ALIGN_RIGHT));
                        }
                    }
                }
            }
            
            PdfPCell cellMiddlePackets = new PdfPCell(tableMiddlePackets);
            cellMiddlePackets.Colspan = 2;
            cellMiddlePackets.Border = Rectangle.NO_BORDER;

            #endregion

            #region MiddleNetto (Nettofragt kurertransport)

            tableMiddleNetto.SetWidths(new float[] { 5f, 1f });

            if (idType1 == c_Transport_Kurer)
            {
                tableMiddleNetto.AddCell(AddTextCell("Nettofragt", BorderWidth: BorderPDF.Left, TextFont: FontFakturaPDF.NormalBold, Padding: paddingHeader));
                tableMiddleNetto.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftRight));

                double kilometerCount = 0;
                double.TryParse(FileInfo.LukFragtbrev.kilometer, out kilometerCount);
                double startPrice = Models.FakturaPrisliste.kurerPriser.startgebyr[idType2][idType3];
                double kilometerPrice = kilometerCount * Models.FakturaPrisliste.kurerPriser.kilometer[idType2][idType3];
                nettoFragt += startPrice + kilometerPrice;
                double minimumPrice = Models.FakturaPrisliste.kurerPriser.minimun[idType2][idType3] - nettoFragt;


                tableMiddleNetto.AddCell(AddTextCell("Startgebyr (inkl. 20min. læssetid)", BorderWidth: BorderPDF.Left, Padding: paddingItems));
                tableMiddleNetto.AddCell(AddTextCell(startPrice.ToString("N2"), BorderWidth: BorderPDF.LeftRight, HAlign: Rectangle.ALIGN_RIGHT));

                if (kilometerPrice > 0)
                {
                    tableMiddleNetto.AddCell(AddTextCell(string.Format("Kilometertakst. ({0}km)", kilometerCount), BorderWidth: BorderPDF.Left, Padding: paddingItems));
                    tableMiddleNetto.AddCell(AddTextCell(kilometerPrice.ToString("N2"), BorderWidth: BorderPDF.LeftRight, HAlign: Rectangle.ALIGN_RIGHT));
                }
                if (minimumPrice > 0)
                {
                    nettoFragt += minimumPrice;
                    tableMiddleNetto.AddCell(AddTextCell("Minimun pris pr. tur", BorderWidth: BorderPDF.Left, Padding: paddingItems));
                    tableMiddleNetto.AddCell(AddTextCell(minimumPrice.ToString("N2"), BorderWidth: BorderPDF.LeftRight, HAlign: Rectangle.ALIGN_RIGHT));
                }
            }

            PdfPCell cellMiddleNetto = new PdfPCell(tableMiddleNetto);
            cellMiddleNetto.Colspan = 2;
            cellMiddleNetto.Border = Rectangle.NO_BORDER;

            #endregion

            #region MiddleTime (Tid / Minut)

            tableMiddleTime.SetWidths(new float[] { 5f, 1f });

            if (idType1 == c_Transport_Kurer || idType1 == c_Transport_Gods)
            {
                int idGebyr1 = idType1 == c_Transport_Kurer && idType2 != 2 ? idType2 : 0;
                int idGebyr2 = idType1 == c_Transport_Kurer ? idType3 : 3;

                tableMiddleTime.AddCell(AddTextCell("Tid / Minut", BorderWidth: BorderPDF.Left, TextFont: FontFakturaPDF.NormalBold, Padding: paddingHeader));
                tableMiddleTime.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftRight));

                int timeL = 0;
                int timeW = 0;
                int timeU = 0;
                int timeC = 0;
                int.TryParse(FileInfo.LukFragtbrev.tidsforbrug1, out timeL);
                int.TryParse(FileInfo.LukFragtbrev.tidsforbrug2, out timeW);
                int.TryParse(FileInfo.LukFragtbrev.tidsforbrug3, out timeL);
                int.TryParse(FileInfo.LukFragtbrev.tidsforbrug4, out timeL);


                int timeInclude = 20;
                int timeUsedTotal = (timeL + timeW + timeU) - timeInclude;
                double timeCRoundUp = Math.Ceiling(timeC / 30f);
                int timeCCount = 0;
                int.TryParse(FileInfo.EkstraGebyr.textboxsValues[0], out timeCCount);

                double timePrice = timeUsedTotal * Models.FakturaPrisliste.kurerPriser.ekstraTidforbrug[idGebyr1][idGebyr2];
                double timePriceC = timeCCount * (timeCRoundUp * Models.FakturaPrisliste.EkstraGebyrPrice.prices[0][idGebyr1][idGebyr2]);

                if (timePrice > 0)
                {
                    tableMiddleTime.AddCell(AddTextCell(string.Format("Tidsforbrug ({0} min)", timeUsedTotal), BorderWidth: BorderPDF.Left, Padding: paddingItems));
                    tableMiddleTime.AddCell(AddTextCell(timePrice.ToString("N2"), BorderWidth: BorderPDF.LeftRight, HAlign: Rectangle.ALIGN_RIGHT));
                }

                if (timePriceC > 0 && FileInfo.EkstraGebyr.buttonsIsCheck[0])
                {
                    tableMiddleTime.AddCell(AddTextCell(string.Format("Medhjælper ({0} min)", timeCRoundUp), BorderWidth: BorderPDF.Left, Padding: paddingItems));
                    tableMiddleTime.AddCell(AddTextCell(timePriceC.ToString("N2"), BorderWidth: BorderPDF.LeftRight, HAlign: Rectangle.ALIGN_RIGHT));
                }
            }

            PdfPCell cellMiddleTime = new PdfPCell(tableMiddleTime);
            cellMiddleTime.Colspan = 2;
            cellMiddleTime.Border = Rectangle.NO_BORDER;

            #endregion

            #region MiddleExtra (Tillæg for særlige ydelser)

            tableMiddleExtra.SetWidths(new float[] { 5f, 1f });

            if (idType1 == c_Transport_Kurer || idType1 == c_Transport_Gods) {

                int idGebyr1 = idType1 == c_Transport_Kurer && idType2 != 2 ? idType2 : 0;
                int idGebyr2 = idType1 == c_Transport_Kurer ? idType3 : 3;

                bool categoryAdded = false;

                string[] inputExtraText = {
                    "Flytte tilæg", "ADR-tilæg", "Aften- og nattillæg (18:00-06:00)",
                    "Weekendtillæg (lørdag-søndag)", "Yderzonetillæg", "Byttepalletillæg",
                    "SMS servicetillæg", "Adresse korrektion", "Bro afgift", "Vej afgift",
                    "Færge afgift"
                };

                int idDuplicate = 0;
                int[][] priceDuplicate = {
                    new int[] { 1,0},
                    new int[] { 6,0},
                    new int[] { 7,0}
                };
                int.TryParse(FileInfo.EkstraGebyr.textboxsValues[1], out priceDuplicate[0][1]);
                int.TryParse(FileInfo.EkstraGebyr.textboxsValues[2], out priceDuplicate[1][1]);
                int.TryParse(FileInfo.EkstraGebyr.textboxsValues[3], out priceDuplicate[2][1]);
                

                for (int i = 1; i < FileInfo.EkstraGebyr.buttonsIsCheck.Length; i++)
                {
                    if (FileInfo.EkstraGebyr.buttonsIsCheck[i])
                    {
                        double inputExtraPrice = 0;

                        //hent prisen
                        if (i < 9)
                            inputExtraPrice = Models.FakturaPrisliste.EkstraGebyrPrice.prices[i][idGebyr1][idGebyr2];
                        else
                            double.TryParse(FileInfo.EkstraGebyr.textboxsValues[i -5], out inputExtraPrice);
                        
                        //om man skal gange prisen
                        if (priceDuplicate[idDuplicate][0] == i)
                        {
                            inputExtraPrice = priceDuplicate[idDuplicate][1] * inputExtraPrice;
                            idDuplicate++;
                        }

                        if (inputExtraPrice > 0)
                        {
                            //tjek om kategorien er blivet tilføjet
                            if (!categoryAdded)
                            {
                                categoryAdded = true;
                                tableMiddleExtra.AddCell(AddTextCell("Tillæg for særlige ydelser", BorderWidth: BorderPDF.Left, TextFont: FontFakturaPDF.NormalBold, Padding: paddingHeader));
                                tableMiddleExtra.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftRight));
                            }

                            //tilføj item
                            tableMiddleExtra.AddCell(AddTextCell(inputExtraText[i -1], BorderWidth: BorderPDF.Left, Padding: paddingItems));
                            tableMiddleExtra.AddCell(AddTextCell(inputExtraPrice.ToString("N2"), BorderWidth: BorderPDF.LeftRight, HAlign: Rectangle.ALIGN_RIGHT));
                        }
                    }
                }
            }


            PdfPCell cellMiddleExtra = new PdfPCell(tableMiddleExtra);
            cellMiddleExtra.Colspan = 2;
            cellMiddleExtra.Border = Rectangle.NO_BORDER;

            #endregion

            #region MiddleGebyr (Gebyr)

            tableMiddleGebyr.SetWidths(new float[] { 5f, 1f });

            tableMiddleGebyr.AddCell(AddTextCell("Gebyr", BorderWidth: BorderPDF.Left, TextFont: FontFakturaPDF.NormalBold, Padding: paddingHeader));
            tableMiddleGebyr.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftRight));


            if (idType1 == c_Transport_Pakke)
            {
                double inputPrice = packetsCount * Models.FakturaPrisliste.pakkePriser.Prices[7][idType2];

                tableMiddleGebyr.AddCell(AddTextCell("Servicegebyr pr. pakke", BorderWidth: BorderPDF.Left, Padding: paddingItems));
                tableMiddleGebyr.AddCell(AddTextCell(inputPrice.ToString("N2"), BorderWidth: BorderPDF.LeftRight, HAlign: Rectangle.ALIGN_RIGHT));
            }
            else
            {
                int idGebyr1 = idType1 == c_Transport_Kurer && idType2 != 2 ? idType2 : 0;
                int idGebyr2 = idType1 == c_Transport_Kurer ? idType3 : 3;

                double[] inputGebyrPrice = {
                    nettoFragt * Models.FakturaPrisliste.kurerPriser.braendstof[idGebyr1][idGebyr2],
                    nettoFragt * Models.FakturaPrisliste.kurerPriser.miljoegebyr[idGebyr1][idGebyr2],
                    Models.FakturaPrisliste.kurerPriser.Adminnistrationsgebyr[idGebyr1][idGebyr2]
                };
                string[] inputGebyrText =
                {
                    "Brændstofgebyr Beregnes af nettofragt",
                    "Miljøgebyr beregnes af nettofragt",
                    "Adminnistrationsgebyr pr. faktura"
                };

                for (int i = 0; i < 3; i++)
                {
                    tableMiddleGebyr.AddCell(AddTextCell(inputGebyrText[i], BorderWidth: BorderPDF.Left, Padding: paddingItems));
                    tableMiddleGebyr.AddCell(AddTextCell(inputGebyrPrice[i].ToString("N2"), BorderWidth: BorderPDF.LeftRight, HAlign: Rectangle.ALIGN_RIGHT));
                }

            }

            PdfPCell cellMiddleGebyr = new PdfPCell(tableMiddleGebyr);
            cellMiddleGebyr.Colspan = 2;
            cellMiddleGebyr.Border = Rectangle.NO_BORDER;
            #endregion

            #region Middle

            tableMiddle.SetWidths(new float[] { 5f,1f});
            tableMiddle.AddCell(AddTextCell("DESCRIPTION",
                TextFont: FontFakturaPDF.BigWhiteBold,
                HAlign: Rectangle.ALIGN_CENTER,
                BorderWidth: BorderPDF.LeftTopBottom,
                Background: BaseColor.DARK_GRAY)
            );
            tableMiddle.AddCell(AddTextCell("AMOUNT",
                TextFont: FontFakturaPDF.BigWhiteBold,
                HAlign: Rectangle.ALIGN_CENTER,
                BorderWidth: BorderPDF.Box,
                Background: BaseColor.DARK_GRAY)
            );

            
            string inputPakkeInfo = string.Format("Transport Leveret {0} ({1}) Transportation Control Number: {2}", FileInfo.LukFragtbrev.leveringsdato.ToLongDateString(), FileInfo.LukFragtbrev.leveringsdato.ToShortTimeString(), FileInfo.Invoice);
            tableMiddle.AddCell(AddTextCell(inputPakkeInfo,
                TextFont: FontFakturaPDF.NormalBold,
                BorderWidth: BorderPDF.Left,
                Padding: new float[] { 2,10,2,2})
            );
            tableMiddle.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftRight));
            
            tableMiddle.AddCell(tableMiddleAddress);
            tableMiddle.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftRight));
            

            string inputRef = "Reference: 578000000"+ FileInfo.Invoice;
            tableMiddle.AddCell(AddTextCell(inputRef, BorderWidth: BorderPDF.Left,
                Padding: new float[] { 2, 20, 2, 2 })
            );
            tableMiddle.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftRight));

            string[] optionsTransport1 = { "Kurertransport", "Pakketransport", "Godstransport" };
            string[][] optionsTransport2 = {
                new string[] {" GoRush", " GoFlex", " GoVIP" },
                new string[] {" GoGreen", " GoPlus" },
                new string[] {" GoFull", " GoPart" }
            };
            string[][] optionsTransport3 = {
                new string[] {" Grp 1", " Grp 2", " Grp 3", " Grp 4" },
                new string[] {"" },
                new string[] {"" }
            };

            string inputTransport1 = optionsTransport1[idType1];
            string inputTransport2 = optionsTransport2[idType1][idType2];
            string inputTransport3 = optionsTransport3[idType1][idType3];

            string inputAntalKilo = string.Format("* * * CARGO {0}{1}{2} * * * {3} stk * * * {4} KG * * *",
                inputTransport1, inputTransport2, inputTransport3, packetsCount, totalWeight.ToString("N2"));
            tableMiddle.AddCell(AddTextCell(inputAntalKilo, BorderWidth: BorderPDF.Left,
                TextFont: FontFakturaPDF.NormalBold));
            tableMiddle.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftRight));

            //tilføj færdige tabler
            //laver i andre "region"
            tableMiddle.AddCell(cellMiddlePackets);
            tableMiddle.AddCell(cellMiddleNetto);
            tableMiddle.AddCell(cellMiddleTime);
            tableMiddle.AddCell(cellMiddleExtra);
            tableMiddle.AddCell(cellMiddleGebyr);

            if (FileInfo.FragtbrevNotifications.Trim() != "")
            {
                tableMiddleGebyr.AddCell(AddTextCell("Etv. Bemærkninger", BorderWidth: BorderPDF.Left, TextFont: FontFakturaPDF.NormalBold, Padding: paddingHeader));
                tableMiddleGebyr.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftRight));

                tableMiddleGebyr.AddCell(AddTextCell(FileInfo.FragtbrevNotifications, BorderWidth: BorderPDF.Left, Padding: paddingItems));
                tableMiddleGebyr.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftRight, HAlign: Rectangle.ALIGN_RIGHT));
            }

            //lav mellemrum til sidst
            tableMiddle.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftBottom, Height: 15f));
            tableMiddle.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftRightBottom, Height: 15f));

            //for (int row = 0; row < 3; row++)
            //{
            //    if (row == 3 - 1)
            //    {
            //        tableMiddle.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftBottom));
            //        tableMiddle.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftRightBottom));
            //    }
            //    else {
            //        tableMiddle.AddCell(AddTextCell(BorderWidth: BorderPDF.Left));
            //        tableMiddle.AddCell(AddTextCell(BorderWidth: BorderPDF.LeftRight));
            //    }
            //}

            #endregion

            #region BottomLeft

            tableBottomLeft.AddCell(AddTextCell());
            tableBottomLeft.AddCell(AddTextCell("OTHER COMMENTS", BorderWidth: BorderPDF.Box, Background: BaseColor.DARK_GRAY, TextFont: FontFakturaPDF.BigWhiteBold));
            tableBottomLeft.AddCell(AddTextCell("1. Total payment due in 30 days", BorderWidth: BorderPDF.LeftRight));
            tableBottomLeft.AddCell(AddTextCell("2. Please include the invoice number on your check", BorderWidth: BorderPDF.LeftRight));
            tableBottomLeft.AddCell(AddTextCell("3. Danske Bank reg: 5211 konto 5211478956477", BorderWidth: BorderPDF.LeftRightBottom));
            #endregion

            #region BottomRight

            string[][] inputTotal = {
                new string[] { "SUBTOTAL", "DKR", "" },
                new string[] { "TAX RATE", "", "25%"},
                new string[] { "TAX", "DKR", ""},
                new string[] { "RABAT RATE", "", ""},
                new string[] { "RABAT", "DKR", ""},
            };

            for (int row = 0; row < inputTotal.Length; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    BaseColor columnBackground = row % 2 == 0 ? BaseColor.LIGHT_GRAY : BaseColor.WHITE;
                    int align = Rectangle.ALIGN_LEFT;

                    if (column == 1)
                        align = Rectangle.ALIGN_CENTER;
                    else if (column == 2)
                        align = Rectangle.ALIGN_RIGHT;


                    tableBottomRight.AddCell(AddTextCell(inputTotal[row][column], Background: columnBackground, HAlign: align));
                }
            }

            //add dobbelt linje
            tableBottomRight.AddCell(AddTextCell(Height: 2f, BorderWidth: BorderPDF.TopBottom));
            tableBottomRight.AddCell(AddTextCell(Height: 2f, BorderWidth: BorderPDF.TopBottom));
            tableBottomRight.AddCell(AddTextCell(Height: 2f, BorderWidth: BorderPDF.TopBottom));

            tableBottomRight.AddCell(AddTextCell("TOTAL", TextFont: FontFakturaPDF.NormalBold, Background: BaseColor.LIGHT_GRAY));
            tableBottomRight.AddCell(AddTextCell("DKR", TextFont: FontFakturaPDF.NormalBold, HAlign: Rectangle.ALIGN_CENTER, Background: BaseColor.LIGHT_GRAY));
            tableBottomRight.AddCell(AddTextCell("10", TextFont: FontFakturaPDF.NormalBold, HAlign: Rectangle.ALIGN_RIGHT, Background: BaseColor.LIGHT_GRAY));


            #endregion

            #region Bottom

            tableBottom.SetWidths(new float[] { 3.5f, 0.5f, 2f });
            tableBottom.AddCell(tableBottomLeft);
            tableBottom.AddCell("");
            tableBottom.AddCell(tableBottomRight);
            #endregion


            PdfPTable tableDocument = new PdfPTable(1);
            tableDocument.DefaultCell.Border = Rectangle.NO_BORDER;
            tableDocument.DefaultCell.Padding = 0;
            tableDocument.WidthPercentage = 100f;

            tableDocument.AddCell(tableTop);
            tableDocument.AddCell(tableMiddle);
            tableDocument.AddCell(tableBottom);

            
            //gør klar til at skrive i filen
            FileStream documentCreate = new FileStream(pdfFolder + "Faktura-" + FileInfo .Invoice + ".pdf", FileMode.Create, FileAccess.Write);
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, documentCreate);

            doc.Open();
            doc.Add(tableDocument);
            doc.Close();

            writer.Close();
            documentCreate.Close();
        }

        public void CreateFragtBrev(XML_Files.DocData.Layout.DocumentData FileInfo)
        {
            string imageFolder = Directory.GetCurrentDirectory();
            this.DefaultNormalText = FontFragtbrevPDF.Normal;

            #region table opretteselse

            PdfPTable tableTopLeft = new PdfPTable(1);
            PdfPTable tableTopRight = new PdfPTable(1);
            PdfPTable tableMiddleTop = new PdfPTable(4);
            PdfPTable tableMiddleMiddleTop = new PdfPTable(6);
            PdfPTable tableMiddleMiddleCheckbox = new PdfPTable(2);
            PdfPTable tableMiddleMiddlePacket = new PdfPTable(6);
            PdfPTable tableBottomTop = new PdfPTable(2);
            PdfPTable tableBottomMiddle = new PdfPTable(6);
            PdfPTable tableBottom = new PdfPTable(4);
            PdfPTable tableTime = new PdfPTable(2);
            PdfPTable tableNotifications = new PdfPTable(2);
            PdfPTable tableTimeNotify = new PdfPTable(3);

            tableTopLeft.DefaultCell.Border = Rectangle.NO_BORDER;
            tableTopRight.DefaultCell.Border = Rectangle.NO_BORDER;
            tableMiddleTop.DefaultCell.Border = Rectangle.NO_BORDER;
            tableMiddleMiddleTop.DefaultCell.Border = Rectangle.NO_BORDER;
            tableMiddleMiddleCheckbox.DefaultCell.Border = Rectangle.NO_BORDER;
            tableMiddleMiddlePacket.DefaultCell.Border = Rectangle.NO_BORDER;
            tableBottomTop.DefaultCell.Border = Rectangle.NO_BORDER;
            tableTime.DefaultCell.Border = Rectangle.NO_BORDER;
            tableNotifications.DefaultCell.Border = Rectangle.NO_BORDER;
            tableTimeNotify.DefaultCell.Border = Rectangle.NO_BORDER;

            //gør så der ikke kommer et ind ryk
            //hvis man tiføjer en table inde i en table
            tableTopLeft.DefaultCell.Padding = 0;
            tableTopRight.DefaultCell.Padding = 0;
            tableMiddleTop.DefaultCell.Padding = 0;
            tableMiddleMiddleCheckbox.DefaultCell.Padding = 0;
            tableMiddleMiddlePacket.DefaultCell.Padding = 0;
            tableBottomTop.DefaultCell.Padding = 0;
            tableBottomMiddle.DefaultCell.Padding = 0;
            tableTime.DefaultCell.Padding = 0;
            #endregion

            #region TopLeft - (Logo til Reference)

            //hent billederne
            Image imageFirmLogo = Image.GetInstance(imageFolder + "/Images/Pdf/logo logoOnly.png");
            Image imageFirmText = Image.GetInstance(imageFolder + "/Images/Pdf/logo textOnly.png");
            //sæt størrelse
            imageFirmLogo.ScaleToFit(70f, 130f);
            imageFirmText.ScaleToFit(140f, 70f);
            
            //convert til cell
            PdfPCell cellFirmLogo = new PdfPCell(imageFirmLogo);
            PdfPCell cellFirmText = new PdfPCell(imageFirmText);
            //fjern ramme
            cellFirmLogo.Border = Rectangle.NO_BORDER;
            cellFirmText.Border = cellFirmLogo.Border;
            //sæt det helt til venstre
            cellFirmLogo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
            cellFirmText.HorizontalAlignment = Rectangle.ALIGN_LEFT;
            cellFirmLogo.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
            cellFirmText.VerticalAlignment = Rectangle.ALIGN_MIDDLE;

            PdfPTable tableFirmLogo = new PdfPTable(2);
            tableFirmLogo.SetWidths(new float[] { 70f, 140f });
            //tilføj logo og navn
            tableFirmLogo.AddCell(cellFirmLogo);
            tableFirmLogo.AddCell(cellFirmText);
            tableTopLeft.AddCell(tableFirmLogo);//tilføj logo tabel

            //find kunde nummer
            string inputKundeNr = "";
            if (FileInfo.Afsender.isPayer || FileInfo.Modtager2.isPayer)
                inputKundeNr = FileInfo.Afsender.telefon.Replace(" ", "");
            else if(FileInfo.Modtager.isPayer)
                inputKundeNr = FileInfo.Modtager.telefon.Replace(" ", "");

            //tilføj tekst
            tableTopLeft.AddCell(
                AddTextCell(inputKundeNr, 
                    BorderWidth: BorderPDF.Bottom, 
                    HAlign: Rectangle.ALIGN_CENTER)
            );

            tableTopLeft.AddCell(
                AddTextCell("KUNDENUMMER", 
                    HAlign: Rectangle.ALIGN_CENTER,
                    TextFont: FontFragtbrevPDF.BigBoldRed,
                    Padding: new float[] { 5,5,5,5})
            );

            //Opret barcode
            Image imageBarcode = Image.GetInstance(CreateBarcode("*57800000"+ FileInfo.Invoice + "*"), BaseColor.WHITE);
            imageBarcode.ScaleToFit(300f, 64f);
            PdfPCell cellBarcode = new PdfPCell(imageBarcode);
            cellBarcode.Border = Rectangle.NO_BORDER;
            cellBarcode.HorizontalAlignment = Rectangle.ALIGN_CENTER;
            cellBarcode.PaddingBottom = 5;
            tableTopLeft.AddCell(cellBarcode);

            //tilføj reference
            PdfPTable tableReference = new PdfPTable(2);
            tableReference.AddCell(AddTextCell("REFERENCE",
                BorderWidth: BorderPDF.Top,
                Padding: new float[] { 2, 8, 2, 8 })
            );
            tableReference.AddCell(AddTextCell(FileInfo.Generelt.reference,
                BorderWidth: BorderPDF.Top,
                Padding: new float[] { 2,8,2,8 })
            );
            tableTopLeft.AddCell(tableReference);

            #endregion

            #region TopRight (Afsender)


            tableTopRight.AddCell(AddTextCell("AFSENDER, TELEFONNR.",
                BorderWidth: BorderPDF.LeftTopRight,
                HAlign: Rectangle.ALIGN_CENTER)
            );
            string[] inputsAfsender = { FileInfo.Afsender.firma, FileInfo.Afsender.vej, FileInfo.Afsender.telefon };
            for (int i = 0; i < inputsAfsender.Length; i++)
            {
                tableTopRight.AddCell(AddTextCell(inputsAfsender[i],
                    BorderWidth: BorderPDF.LeftRight,
                    HAlign: Rectangle.ALIGN_CENTER,
                    Padding: new float[] { 2, 3, 2, 3 })
                );
            }
            
            //lav table med postnummer
            PdfPTable tableAfsenderPost = new PdfPTable(2);
            tableAfsenderPost.SetWidths(new float[] { 0, 1});//gør så zipCode vil komme helt i center
            tableAfsenderPost.DefaultCell.Border = Rectangle.NO_BORDER;
            tableAfsenderPost.AddCell(AddTextCell("POSTNUMMER",
                BorderWidth: BorderPDF.Left)
            );
            tableAfsenderPost.AddCell(AddTextCell(FileInfo.Afsender.zipCode +" " + FileInfo.Afsender.city,
                BorderWidth: BorderPDF.Right,
                HAlign: Rectangle.ALIGN_CENTER)
            );
            tableTopRight.AddCell(tableAfsenderPost);

            //
            string afsenderInfoText = "VED EFTERKRAV MÅ FORSENDELSEN UDLEVERES MOD BETALING MED SÆDVANLIGE BE-\n" +
                    "TALINGSMIDLER, HERUNDER KONTANTER OG ALMINDELIG CHECK. HAR AFSENDEREN PÅ-\n" +
                    "FØRT SIN UNDERSKRIFT NEDENFOR, MÅ UDLEVERING DOG KUN SKE MED KONTANTER EL-\n" +
                    "LER BANKNOTERET CHECK.";

            tableTopRight.AddCell(AddTextCell(afsenderInfoText,
                BorderWidth: BorderPDF.LeftTopRight,
                TextFont: FontFragtbrevPDF.Small,
                Padding: new float[] { 2, 2, 2, 25 })
            );

            tableTopRight.AddCell(AddTextCell("AFSENDERS UNDERSKRIFT",
                BorderWidth: BorderPDF.LeftTopRight,
                HAlign: Rectangle.ALIGN_CENTER,
                Padding: new float[] { 2, 5, 2, 2 })
            );
            #endregion

            #region MiddleTop (Fragtmand til Rute(2))

            string inputsDato1 = FileInfo.Generelt.isSetDatoRute1 ? FileInfo.Generelt.datoRute1.ToShortDateString() :  "";
            string inputsDato2 = FileInfo.Generelt.isSetDatoRute2 ? FileInfo.Generelt.datoRute2.ToShortDateString() : "";
            string inputsRute1 = FileInfo.Generelt.isSetDatoRute1 ? FileInfo.Generelt.zipRute1 : "";
            string inputsRute2 = FileInfo.Generelt.isSetDatoRute2 ? FileInfo.Generelt.zipRute2 : "";
            bool useByttePalle1 = FileInfo.Byttepaller.useByttepalle;
            bool useByttePalle2 = !FileInfo.Byttepaller.useByttepalle;

            tableMiddleTop.SetWidths(new float[] { 60f, 70f, 50f, 30f });

            //Række 1
            tableMiddleTop.AddCell(AddTextCell("FRAGTMAND",
                BorderWidth: BorderPDF.LeftTop,
                Colspan: 2)
            );
            tableMiddleTop.AddCell(AddTextCell("DATO",
                BorderWidth: BorderPDF.LeftTop)
            );
            tableMiddleTop.AddCell(AddTextCell("RUTE",
                BorderWidth: BorderPDF.LeftTopRight)
            );

            //Række 2
            tableMiddleTop.AddCell(AddTextCell(FileInfo.Generelt.fragtmand,
                BorderWidth: BorderPDF.Left,
                Colspan: 2)
            );
            tableMiddleTop.AddCell(AddTextCell(inputsDato1,
                HAlign: Rectangle.ALIGN_CENTER,
                BorderWidth: BorderPDF.Left)
            );
            tableMiddleTop.AddCell(AddTextCell(inputsRute1,
                HAlign: Rectangle.ALIGN_CENTER,
                BorderWidth: BorderPDF.LeftRight)
            );

            //Række 3
            tableMiddleTop.AddCell(AddTextCell("EUR.PALLER(ANTAL)",
                BorderWidth: BorderPDF.LeftTop)
            );
            tableMiddleTop.AddCell(AddTextCell("BYTTEPALLE",
                BorderWidth: BorderPDF.LeftTop)
            );
            tableMiddleTop.AddCell(AddTextCell("DATO",
                BorderWidth: BorderPDF.LeftTop)
            );
            tableMiddleTop.AddCell(AddTextCell("RUTE",
                BorderWidth: BorderPDF.LeftTopRight)
            );

            string mtpSpace = "         ";
            tableMiddleTop.AddCell(AddTextCell(string.Format("1/1{0}{1}{2}1/2{3}{4}{5}1/4{6}{7}", 
                mtpSpace, FileInfo.Byttepaller.palle1_1, mtpSpace, 
                mtpSpace, FileInfo.Byttepaller.palle1_2, mtpSpace,
                mtpSpace, FileInfo.Byttepaller.palle1_4),
                BorderWidth: BorderPDF.Left)
            );

            PdfPTable tableByttePalle = new PdfPTable(4);
            //tableByttePalle.SetWidths(new float[] { 1f, 3f, 1f, 3f});
            tableByttePalle.AddCell(AddTextCell("JA", BorderWidth: BorderPDF.Left, HAlign: Rectangle.ALIGN_RIGHT));
            tableByttePalle.AddCell(AddTextCellCheckbox(Size: new float[] { 10f, 10f }, IsChecked: useByttePalle1));
            tableByttePalle.AddCell(AddTextCell("NEJ", HAlign: Rectangle.ALIGN_RIGHT));
            tableByttePalle.AddCell(AddTextCellCheckbox(Size: new float[] { 10f, 10f }, IsChecked: useByttePalle2));

            tableMiddleTop.AddCell(tableByttePalle);

            tableMiddleTop.AddCell(AddTextCell(inputsDato2,
                BorderWidth: BorderPDF.Left,
                HAlign: Rectangle.ALIGN_CENTER)
            );
            tableMiddleTop.AddCell(AddTextCell(inputsRute2,
                BorderWidth: BorderPDF.LeftRight,
                HAlign: Rectangle.ALIGN_CENTER)
            );

            #endregion

            #region MiddleMiddleTop (Transport Headers)

            tableMiddleMiddleTop.SetWidths(new float[] { 20f, 20f, 20f, 70f, 50f ,30f });
            tableMiddleMiddleTop.AddCell(AddTextCell("MRK./NR.", 
                HAlign: Rectangle.ALIGN_CENTER,
                VAlign: Rectangle.ALIGN_MIDDLE,
                BorderWidth: BorderPDF.LeftTopBottom)
            );
            tableMiddleMiddleTop.AddCell(AddTextCell("ANTAL", 
                HAlign: Rectangle.ALIGN_CENTER,
                VAlign: Rectangle.ALIGN_MIDDLE,
                BorderWidth: BorderPDF.LeftTopBottom)
            );
            tableMiddleMiddleTop.AddCell(AddTextCell("ART", 
                HAlign: Rectangle.ALIGN_CENTER,
                VAlign: Rectangle.ALIGN_MIDDLE,
                BorderWidth: BorderPDF.LeftTopBottom)
            );
            tableMiddleMiddleTop.AddCell(AddTextCell("INDHOLD", 
                HAlign: Rectangle.ALIGN_CENTER,
                VAlign: Rectangle.ALIGN_MIDDLE,
                BorderWidth: BorderPDF.LeftTopBottom)
            );
            tableMiddleMiddleTop.AddCell(AddTextCell("TRANSPORT TYPE", 
                HAlign: Rectangle.ALIGN_CENTER,
                VAlign: Rectangle.ALIGN_MIDDLE,
                BorderWidth: BorderPDF.LeftTopBottom)
            );
            tableMiddleMiddleTop.AddCell(AddTextCell("VÆGT/\nRUMFANG", 
                HAlign: Rectangle.ALIGN_CENTER,
                BorderWidth: BorderPDF.Box)
            );

            #endregion

            #region MiddleMiddle (Transport Checkboxs)

            //indstilinger til loop
            float[] tmmcCheckboxBig = new float[] { 13f, 13f };
            float[] tmmcCheckboxSmall = new float[] { 10f, 10f };
            float[] tmmcPaddingSmall = new float[] { 8f,2,2,2 };
            string[] tmmcText = {
                "Kurertransport", "GoRush", "GoFlex", "GoVIP", "Grp 1", "Grp 2", "Grp 3", "Grp 4",
                "Pakketransport", "GoGreen", "GoPlus", "XS", "S", "M", "L", "XL", "2XL", "3XL",
                "Godstransport", "GoFull", "GoPart" ,"LDM", "PLL", "M\u00b3"
            };

            //hvad id der skal have hvad
            int[] headerIds = { 0,8,18};
            int[] checkboxBigIds = {1,2,3,9,10,19,20 };
            int[] borderLeftIds = { 1,3,4,6,8,9,11,13,15,17,18,19,21,23};
            int[] useColspan = {3,17,23 };
            List<int> checkedIds = new List<int>();

            #region find ud hvad der skal hakkes af

            switch (FileInfo.Transport.transportType[0])
            {
                case c_Transport_Kurer:

                    checkedIds.Add(FileInfo.Transport.transportType[1] + 1);
                    checkedIds.Add(FileInfo.Transport.transportType[2] + 4);
                    break;
                case c_Transport_Pakke:
                    checkedIds.Add(FileInfo.Transport.transportType[1] + 9);

                    foreach (var item in FileInfo.Transport.pakker)
                    {
                        int idType = item.transportTypeId + 11;
                        if (item.transportTypeId >= 0 && item.transportTypeId <= 6 && !checkedIds.Contains(idType)) {
                            checkedIds.Add(idType);
                        }
                    }
                    break;
                case c_Transport_Gods:
                    checkedIds.Add(FileInfo.Transport.transportType[1] + 19);

                    foreach (var item in FileInfo.Transport.pakker)
                    {
                        int idType = item.transportTypeId + 21;
                        if (item.transportTypeId >= 0 && item.transportTypeId <= 2 && !checkedIds.Contains(idType))
                        {
                            checkedIds.Add(idType);
                        }
                    }
                    break;
            }

            #endregion

            
            for (int i = 0; i < tmmcText.Length; i++)
            {
                //om det er en header eller checkbox
                if (headerIds.Contains(i))
                {
                    //opret header
                    tableMiddleMiddleCheckbox.AddCell(AddTextCell(tmmcText[i],
                        Colspan: 2, BorderWidth: BorderPDF.Left,
                        HAlign: Rectangle.ALIGN_CENTER)
                    );
                }
                else
                {
                    float[] forBorder = borderLeftIds.Contains(i) ? BorderPDF.Left : BorderPDF.None;//lav ramme
                    float[] forSize = checkboxBigIds.Contains(i) ? tmmcCheckboxBig : tmmcCheckboxSmall; //om det er en stor eller lile checkbox
                    float[] forPadding = !checkboxBigIds.Contains(i) ? tmmcPaddingSmall : new float[] { 2, 2, 2, 2 };//lav indryk
                    int forColspan = useColspan.Contains(i) ? 2 : 1;
                    bool isChecked = checkedIds.Contains(i);

                    tableMiddleMiddleCheckbox.AddCell(AddTextCellCheckbox(
                        tmmcText[i], 
                        BorderWidth: forBorder, 
                        Size: forSize,
                        Padding: forPadding,
                        Colspan: forColspan,
                        IsChecked: isChecked)
                    );
                }
            }
            
            //lav den om til en cell så man senere kan sæt rowpan på den
            PdfPCell cellMiddleMiddleCheckbox = new PdfPCell(tableMiddleMiddleCheckbox);
            cellMiddleMiddleCheckbox.Border = Rectangle.NO_BORDER;
            #endregion

            #region MiddleMiddlePacket

            //brug samme width som i Headers
            tableMiddleMiddlePacket.SetWidths(new float[] { 20f, 20f, 20f, 70f, 50f, 30f });
            cellMiddleMiddleCheckbox.Rowspan = FileInfo.Transport.pakker.Count < 18 ? 20 : FileInfo.Transport.pakker.Count + 2;//plus 2 er for vægt ialt
            double totalWeight = 0; //vil blive brugt når vægt i alt bliver lavet
            bool transportCheckboxBeenAdded = false;

            foreach (var packet in FileInfo.Transport.pakker)
            {
                totalWeight += packet.weightD;
                string[] inputsPackets = { packet.mrkNumb, packet.countS, packet.artName,
                    packet.contains, "CHECKBOX", packet.weightS + "kg\n" + packet.volume};
                for (int i = 0; i < inputsPackets.Length; i++)
                {
                    //skal kun tilføjes en gang
                    if (i == 4 && !transportCheckboxBeenAdded)
                    {
                        transportCheckboxBeenAdded = true;
                        tableMiddleMiddlePacket.AddCell(cellMiddleMiddleCheckbox);
                    }
                    //skal hoppe til næste loop hvis det er checkbox
                    if (i == 4)
                        continue;

                    int hAlign = i != 1 ? Rectangle.ALIGN_CENTER : Rectangle.ALIGN_RIGHT;
                    float[] packBorder = i != 5 ? BorderPDF.Left : BorderPDF.LeftRight;
                    float[] packPadding = i != 1 ? new float[] { 2,2,2,2} : new float[] { 2, 2, 8, 2 };

                    tableMiddleMiddlePacket.AddCell(AddTextCell(inputsPackets[i],
                        BorderWidth: packBorder,
                        HAlign: hAlign,
                        VAlign: Rectangle.ALIGN_MIDDLE,
                        Padding: packPadding)
                    );
                }
            }
            #endregion

            #region MiddleMiddleWeight

            //check om volume er sat
            //da volume bruger 2 linjer
            int packetSingleLines = 0;
            int packetDoubleLines = 0;

            foreach (var packet in FileInfo.Transport.pakker)
            {
                if (packet.volume == "")
                    packetSingleLines++;
                else
                    packetDoubleLines++;
            }

            //linjer brugt ialt
            int packetLinesUsed = packetSingleLines + (packetDoubleLines * 2);

            //gør så at vægt ialt kommer i buden
            int emptyPaces = (packetLinesUsed +2) - 20;

            //System.Windows.MessageBox.Show(string.Format("Single: {0} Double: {1} Result: {2} Empty: {3}", packetSingleLines, packetDoubleLines, packetLinesUsed, emptyPaces));
            for (int row = 0; row < emptyPaces; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    if (column == 4 && !transportCheckboxBeenAdded)
                    {
                        transportCheckboxBeenAdded = true;
                        tableMiddleMiddlePacket.AddCell(cellMiddleMiddleCheckbox);
                    }

                    float[] emptyBorder = column == 4 ?  BorderPDF.LeftRight : BorderPDF.Left;
                    tableMiddleMiddlePacket.AddCell(AddTextCell(" ", BorderWidth: emptyBorder));
                }
            }

            //
            int columnCount = 5;//emptyPaces <= 0 ? 6 : 5;
            //tilføj vægt ialt celler
            for (int row = 0; row < 2; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    //tilføj en tom cell
                    if (column != columnCount -1)
                    {
                        tableMiddleMiddlePacket.AddCell(AddTextCell("", BorderWidth: BorderPDF.Left));
                    }
                    else {
                        string cellText = row == 0 ? "VÆGT I ALT" : totalWeight + "kg";
                        tableMiddleMiddlePacket.AddCell(AddTextCell(cellText, 
                            BorderWidth: BorderPDF.LeftTopRight,
                            HAlign: Rectangle.ALIGN_CENTER)
                        );
                    }
                }
            }

            #endregion

            #region BottomTopLeft (Modtager)
            
            PdfPTable tableBottomTopLeft = new PdfPTable(2);
            tableBottomTopLeft.AddCell(AddTextCell("MODTAGER",
                BorderWidth: BorderPDF.LeftTop,
                Padding: new float[] { 2, 5, 2, 3 })
            );
            tableBottomTopLeft.AddCell(AddTextCell("TELEFONNR.",
                BorderWidth: BorderPDF.Top,
                Padding: new float[] { 2, 5, 2, 3 })
            );

            tableBottomTopLeft.AddCell(AddTextCell(FileInfo.Modtager.firma,
                BorderWidth: BorderPDF.Left,
                Padding: new float[] { 5, 0, 0, 0 })
            );
            tableBottomTopLeft.AddCell(AddTextCell(FileInfo.Modtager.telefon,
                Padding: new float[] { 5, 0, 0, 0 }));
            tableBottomTopLeft.AddCell(AddTextCell(FileInfo.Modtager.vej,
                BorderWidth: BorderPDF.Left,
                Padding: new float[] { 5, 0, 0, 0 },
                Colspan: 2)
            );
            tableBottomTopLeft.AddCell(AddTextCell(FileInfo.Modtager.zipCode + " " + FileInfo.Modtager.city,
                BorderWidth: BorderPDF.Left,
                Padding: new float[] { 5, 0, 0, 0 },
                Colspan: 2)
            );
            tableBottomTopLeft.AddCell(AddTextCell("POSTNUMMER",
                BorderWidth: BorderPDF.Left,
                Padding: new float[] { 2, 5, 2, 3 })
            );
            tableBottomTopLeft.AddCell(AddTextCell(FileInfo.Modtager.zipCode,
                Padding: new float[] { 5, 5, 2, 3 })
            );

            #endregion

            #region BottomTopRight (Efterkrav)

            string inputEfterkrav0 = FileInfo.Efterkrav.useEfterkrav ? FileInfo.Efterkrav.efterkravGebyr : "";
            string inputEfterkrav1 = FileInfo.Efterkrav.useEfterkrav ? FileInfo.Efterkrav.forsikringssum : "";
            string inputEfterkrav2 = FileInfo.Efterkrav.useEfterkrav ? FileInfo.Efterkrav.premium : "";
            string inputEfterkrav3 = FileInfo.Efterkrav.useEfterkrav ? FileInfo.Efterkrav.total : "";

            PdfPTable tableBottomTopRight = new PdfPTable(2);
            tableBottomTopRight.AddCell(AddTextCell("EFTERKRAV\n(INKL. GEBYR)",
                BorderWidth: BorderPDF.LeftTop,
                Padding: new float[] { 2, 7f, 2, 7f },
                HAlign: Rectangle.ALIGN_CENTER
                ));
            tableBottomTopRight.AddCell(AddTextCell(inputEfterkrav0,
                BorderWidth: BorderPDF.LeftTopRight,
                Padding: new float[] { 2, 5, 2, 5 }
                ));
            tableBottomTopRight.AddCell(AddTextCell("",
                BorderWidth: BorderPDF.LeftTopRight,
                Padding: new float[] { 2, 5, 2, 5 },
                Colspan: 2
                ));

            #endregion

            #region BottomTop (sæt dem sammen)
            
            tableBottomTop.SetWidths(new float[] { 130f, 80f});
            tableBottomTopRight.SetWidths(new float[] { 50f, 30f });

            tableBottomTop.AddCell(tableBottomTopLeft);
            tableBottomTop.AddCell(tableBottomTopRight);

            #endregion

            #region BottomMiddle (Franko til Ialt kr.)

            bool inputIsFranko = FileInfo.Modtager.isPayer;
            bool inputIsUfranko = (FileInfo.Afsender.isPayer || FileInfo.Modtager2.isPayer);


            tableBottomMiddle.SetWidths(new float[] { 25f, 40f, 32.5f, 32.5f, 40f, 40f });

            tableBottomMiddle.AddCell(AddTextCell("DET UBENYTTEDE BEDES OVERSTREGET", 
                BorderWidth: BorderPDF.LeftTop,
                Colspan: 4,
                TextFont: FontFragtbrevPDF.NormalUnderline)
            );
            tableBottomMiddle.AddCell(AddTextCell("FRAGT",
                HAlign: Rectangle.ALIGN_CENTER,
                VAlign: Rectangle.ALIGN_MIDDLE,
                BorderWidth: BorderPDF.LeftTop,
                Padding: new float[] { 0, 5, 0, 5 },
                Rowspan: 2)
            );
            tableBottomMiddle.AddCell(AddTextCell("",
                 HAlign: Rectangle.ALIGN_CENTER,
                BorderWidth: BorderPDF.LeftTopRight,
                 Padding: new float[] { 0, 5, 0, 5 },
                 Rowspan: 2)
             );


            tableBottomMiddle.AddCell(AddTextCell("FRANKO",
                BorderWidth: BorderPDF.Left,
                HAlign: Rectangle.ALIGN_CENTER,
                Colspan: 2,
                TextFont: FontFragtbrevPDF.Big,
                IsStrikethru: inputIsFranko)
            );
            tableBottomMiddle.AddCell(AddTextCell("UFRANKO",
                HAlign: Rectangle.ALIGN_CENTER,
                Colspan: 2,
                TextFont: FontFragtbrevPDF.Big,
                IsStrikethru: inputIsUfranko)
            );

            //lav A B C D delen

            PdfPTable tableAbcdType = new PdfPTable(5);
            string[] abcdText = { "A", "", "C", "", "B", "", "D", "" };

            //sæt kryds
            int forsikringId = FileInfo.Generelt.forsikringstype;
            if (forsikringId >= 0 && forsikringId <= 3) {
                int abcdIndex = (FileInfo.Generelt.forsikringstype *2) + 1;
                abcdText[abcdIndex] = "X";
            }


            int abcdId = 0;
            for (int row = 0; row < 2; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    //der skal være afslut ramme for C og B svar box
                    float[] forBorder = column != 3 ? new float[] { 0.1f, 0.1f, 0, 0 } : new float[] { 0.1f, 0.1f, 0.1f, 0 };

                    //lav et tom flet
                    if (row == 0 && column == 4) {
                        tableAbcdType.AddCell(AddTextCell("", Rowspan: 2,
                        BorderWidth: new float[] { 0, 0.1f, 0, 0 })
                        );
                    }

                    //skal ikke tilføj ekstra cell
                    if (column == 4)
                        continue;
                    
                    tableAbcdType.AddCell(AddTextCell(abcdText[abcdId],
                        HAlign: Rectangle.ALIGN_CENTER,
                        BorderWidth: forBorder)
                    );

                    abcdId++;
                }
            }

            PdfPCell cellAbcdType = new PdfPCell(tableAbcdType);
            cellAbcdType.Border = Rectangle.NO_BORDER;
            cellAbcdType.Rowspan = 2;

            tableBottomMiddle.AddCell(cellAbcdType);
            tableBottomMiddle.AddCell(AddTextCell("FORSIKRINGSSUM KR.",
                BorderWidth: new float[] { 0.1f, 0.1f, 0, 0 })
            );
            tableBottomMiddle.AddCell(AddTextCell("PRÆMIE KR.",
                BorderWidth: new float[] { 0.1f, 0.1f, 0, 0 })
            );
            tableBottomMiddle.AddCell(AddTextCell("KVITTERING",
                BorderWidth: new float[] { 0.1f, 0.1f, 0, 0 })
            );

            tableBottomMiddle.AddCell(AddTextCell("I ALT. KR.\nINKL.MOMS",
                HAlign: Rectangle.ALIGN_CENTER,
                VAlign: Rectangle.ALIGN_MIDDLE,
                BorderWidth: new float[] { 0.1f, 0.1f, 0, 0 },
                Padding: new float[] { 0, 5, 0, 5 },
                Rowspan: 2)
            );
            tableBottomMiddle.AddCell(AddTextCell(inputEfterkrav3,
                 HAlign: Rectangle.ALIGN_CENTER,
                BorderWidth: new float[] { 0.1f, 0.1f, 0.1f, 0 },
                 Padding: new float[] { 0, 5, 0, 5 },
                 Rowspan: 2)
             );

            tableBottomMiddle.AddCell(AddTextCell(inputEfterkrav1,
                BorderWidth: new float[] { 0.1f, 0.1f, 0, 0 })
            );
            tableBottomMiddle.AddCell(AddTextCell(inputEfterkrav2,
                BorderWidth: new float[] { 0.1f, 0.1f, 0, 0 })
            );
            tableBottomMiddle.AddCell(AddTextCell("",
                BorderWidth: new float[] { 0.1f, 0.1f, 0, 0 })
            );

            #endregion

            #region Bottom (FORVALTER + rød teskt)

            tableBottom.AddCell(AddTextCell("FORVALTER/DISPONENT",
                BorderWidth: BorderPDF.LeftTop,
                Padding: new float[] { 2, 4, 2, 4 })
            );
            tableBottom.AddCell(AddTextCell("KVITTERING FOR MODTAGELSEN",
                BorderWidth: BorderPDF.LeftTop,
                Padding: new float[] { 2, 4, 2, 4 })
            );
            tableBottom.AddCell(AddTextCell("FRAGTMANDENS KVITTERING",
                BorderWidth: BorderPDF.LeftTop,
                Padding: new float[] { 2, 4, 2, 4 })
            );
            tableBottom.AddCell(AddTextCell("MODTAGERS KUNDENUMMER",
                BorderWidth: BorderPDF.LeftTopRight,
                Padding: new float[] { 2, 4, 2, 4 })
            );

            string inputsForvalter = "";
            List<string> usedInitialer = new List<string>();
            foreach (string item in FileInfo.Initialer)
            {
                if (!usedInitialer.Contains(item))
                    inputsForvalter += item+ ", ";
            }
            if (inputsForvalter != "")
                inputsForvalter = inputsForvalter.Substring(0, inputsForvalter.Length - 2);

            tableBottom.AddCell(AddTextCell(inputsForvalter,
                BorderWidth: BorderPDF.LeftTop,
                Padding: new float[] { 5, 8, 2, 8 })
            );
            tableBottom.AddCell(AddTextCell("",
                BorderWidth: BorderPDF.LeftTop,
                Padding: new float[] { 5, 8, 2, 8 })
            );
            tableBottom.AddCell(AddTextCell("",
                BorderWidth: BorderPDF.LeftTop,
                Padding: new float[] { 5, 8, 2, 8 })
            );
            tableBottom.AddCell(AddTextCell(FileInfo.Modtager.telefon.Replace(" ", ""),
                BorderWidth: BorderPDF.LeftTopRight,
                Padding: new float[] { 5, 8, 2, 8 })
            );

            //rød tekst
            tableBottom.AddCell(AddTextCell("TRANSPORTEN UDFØRES I HENHOLD TIL CMR-REGLERNE",
                BorderWidth: BorderPDF.Top,
                Padding: new float[] { 2, 2, 2, 5 },
                Colspan: 4,
                TextFont: FontFragtbrevPDF.NormalRed,
                HAlign: Rectangle.ALIGN_CENTER)
            );

            #endregion

            #region Time (Hos Afsender og Hos Modtager)

            tableTime.AddCell(AddTextCell("Hos Afsender",
                HAlign: Rectangle.ALIGN_CENTER,
                TextFont: FontFragtbrevPDF.MediumBigBold,
                BorderWidth: BorderPDF.LeftTopBottom,
                Padding: new float[] { 2, 5, 2, 2 })
            );
            tableTime.AddCell(AddTextCell("Hos Modtager",
                HAlign: Rectangle.ALIGN_CENTER,
                TextFont: FontFragtbrevPDF.MediumBigBold,
                BorderWidth: BorderPDF.Box,
                Padding: new float[] { 2,5,2,2 })
            );

            PdfPTable tableTimeInputs = new PdfPTable(2);
            float[] inputsPadding = new float[] { 2, 7, 2, 7 };
            string[] inputText = { "Ankomst", "Afgang", "Total" };

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 2; column++)
                {
                    if (column == 0)
                    {
                        float[] forBorder = row != 2 ? BorderPDF.Left : BorderPDF.LeftBottom;

                        tableTimeInputs.AddCell(AddTextCell(inputText[row],
                            HAlign: Rectangle.ALIGN_RIGHT,
                            TextFont: FontFragtbrevPDF.NormalBold,
                            BorderWidth: forBorder,
                            Padding: inputsPadding)
                        );
                    }
                    else
                    {
                        tableTimeInputs.AddCell(AddTextCell("",
                            BorderWidth: BorderPDF.LeftRightBottom,
                            Padding: inputsPadding)
                        );
                    }
                }
            }

            tableTime.AddCell(tableTimeInputs);
            tableTime.AddCell(tableTimeInputs);
            #endregion

            #region TimeNotify (Sæt Time og Evt. Bemærkninger sammen)

            PdfPTable tableNotify = new PdfPTable(1);
            tableNotify.AddCell(AddTextCell("Evt. Bemærkninger",
                BorderWidth: BorderPDF.LeftTopRight));
            tableNotify.AddCell(AddTextCell(FileInfo.FragtbrevNotifications,
                BorderWidth: BorderPDF.LeftRightBottom,
                Padding: new float[] { 5,2,2,2}));

            //sæt tabelerne sammen
            tableTimeNotify.SetWidths(new float[] {90f, 15f ,105f });
            tableTimeNotify.AddCell(tableTime);
            tableTimeNotify.AddCell(AddTextCell(""));
            tableTimeNotify.AddCell(tableNotify);

            #endregion


            #region Sammen Sæt Hele Fragtbrevet
            PdfPTable tableDocument = new PdfPTable(1);
            tableDocument.DefaultCell.Border = Rectangle.NO_BORDER;
            tableDocument.DefaultCell.Padding = 0;
            tableDocument.WidthPercentage = 100f;

            PdfPTable tableDocumentTop = new PdfPTable(2);
            tableDocumentTop.DefaultCell.Border = Rectangle.NO_BORDER;
            tableDocumentTop.DefaultCell.Padding = 0;

            tableDocumentTop.AddCell(tableTopLeft);
            tableDocumentTop.AddCell(tableTopRight);

            tableDocument.AddCell(tableDocumentTop);
            tableDocument.AddCell(tableMiddleTop);
            tableDocument.AddCell(tableMiddleMiddleTop);
            tableDocument.AddCell(tableMiddleMiddlePacket);
            tableDocument.AddCell(tableBottomTop);
            tableDocument.AddCell(tableBottomMiddle);
            tableDocument.AddCell(tableBottom);
            tableDocument.AddCell(tableTimeNotify);
            #endregion

            //PDF mappe placering
            string folder = Models.ImportantData.g_FolderPdf;

            //gør klar til at skrive i filen
            FileStream documentCreate = new FileStream(folder + "Fragtbrev-" + FileInfo.Invoice + ".pdf", FileMode.Create, FileAccess.Write);
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, documentCreate);

            ITextEvents footerTextEvent = new ITextEvents();
            writer.PageEvent = footerTextEvent;

            
            //lav footer tekst
            string[] footText = { "Afsenderens Kvittering", "Modtagerens Kvittering", "Chaufførens Kvittering" };
            doc.Open();
            for (int i = 0; i < footText.Length; i++)
            {
                doc.ResetPageCount();
                doc.PageCount = 1;
                footerTextEvent.FooterText = footText[i];
                doc.Add(tableDocument);
                doc.NewPage();
            }
            doc.Close();

            writer.Close();
            documentCreate.Close();

        }


        private PdfPCell AddTextCell(string Text = "", float[] BorderWidth = null, 
            int HAlign = Rectangle.ALIGN_LEFT, int VAlign = Rectangle.ALIGN_TOP,
            Font TextFont = null, float[] Padding = null, float Height = -1f,
            int Rowspan = 1, int Colspan = 1, bool IsStrikethru = false,
            BaseColor Background = null)
        {
            if (BorderWidth == null)
                BorderWidth = BorderPDF.None;
            if (TextFont == null)
                TextFont = this.DefaultNormalText;
            if (Padding == null)
                Padding = new float[] { 2, 2, 2, 2 };
            if (Text == "")
                Text = " ";
            
            Chunk textChunk = new Chunk(Text, TextFont);

            if (IsStrikethru) {
                float lineThick = TextFont.Size / 6;
                float lineMiddle = (TextFont.Size / 2) - lineThick;
                textChunk.SetUnderline(lineThick, lineMiddle);
            }
            PdfPCell cellText = new PdfPCell(new Paragraph(textChunk));
            cellText.NoWrap = true;//gør så den kan kan overflow

            if (Background != null)
                cellText.BackgroundColor = Background;

            if (Height != -1f)
                cellText.FixedHeight = Height;

            cellText.HorizontalAlignment = HAlign;
            cellText.VerticalAlignment = VAlign;
            cellText.Rowspan = Rowspan;
            cellText.Colspan = Colspan;

            cellText.PaddingLeft = Padding[0];
            cellText.PaddingTop = Padding[1];
            cellText.PaddingRight = Padding[2];
            cellText.PaddingBottom = Padding[3];

            cellText.BorderWidthLeft = BorderWidth[0];
            cellText.BorderWidthTop= BorderWidth[1];
            cellText.BorderWidthRight = BorderWidth[2];
            cellText.BorderWidthBottom = BorderWidth[3];


            return cellText;
        }

        private PdfPCell AddTextCellCheckbox(string Text = "", bool IsChecked = false, 
            float[] Padding = null, Font TextFont = null, int Colspan = 1, float[] Size = null, float[] BorderWidth = null)
        {
            //Checkbox billede
            string checkboxB = IsChecked ? "boxCheck.png" : "box.png";
            //sæt null values
            TextFont = TextFont == null ? this.DefaultNormalText : TextFont;
            Padding = Padding ?? new float[] { 2, 2, 2, 2 };
            Size = Size ?? new float[] { 13f, 13f };
            BorderWidth = BorderWidth ?? BorderPDF.None;

            Image CheckboxImage = Image.GetInstance(Directory.GetCurrentDirectory() + "/Images/Pdf/" + checkboxB);
            CheckboxImage.ScaleAbsolute(Size[0], Size[1]);

            PdfPCell CheckboxCell = new PdfPCell(CheckboxImage);
            CheckboxCell.Border = Rectangle.NO_BORDER;
            CheckboxCell.HorizontalAlignment = Rectangle.ALIGN_CENTER;
            CheckboxCell.NoWrap = true;
            CheckboxCell.PaddingLeft = 15f; //gør så den ikke er på stregen/ i en anden cell

            PdfPTable table = new PdfPTable(2);
            table.DefaultCell.Border = 0;
            
            table.SetWidths(new float[] { 0f,  1f});

            table.AddCell(CheckboxCell);
            table.AddCell(AddTextCell(Text, 
                TextFont: TextFont, Padding: new float[] { Size[0] +6f ,2,2,2}));


            PdfPCell cellDone = new PdfPCell(table);
            cellDone.Border = 0;
            cellDone.Colspan = Colspan;

            cellDone.PaddingLeft = Padding[0];
            cellDone.PaddingTop = Padding[1];
            cellDone.PaddingRight = Padding[2];
            cellDone.PaddingBottom = Padding[3];

            cellDone.BorderWidthLeft = BorderWidth[0];
            cellDone.BorderWidthTop = BorderWidth[1];
            cellDone.BorderWidthRight = BorderWidth[2];
            cellDone.BorderWidthBottom = BorderWidth[3];

            return cellDone;
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
            int positionX = size / 2;

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
                if (i != barcode.Length - 1) //skal ikke gælde den det sidte
                {
                    drawing.DrawLine(whitePen, positionX, 0, positionX, 30 * size);
                    positionX += size;
                }
            }

            //tilføj et mellemrum til barcoden
            string textBarcodeWithSpace = "";
            for (int i = 0; i < barcode.Length; i++)
            {
                textBarcodeWithSpace += barcode.Substring(i, 1);

                if (i != barcode.Length - 1)
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
    }

    public class ITextEvents : PdfPageEventHelper
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
                len = bf.GetWidthPoint(FooterText, 6f);
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
