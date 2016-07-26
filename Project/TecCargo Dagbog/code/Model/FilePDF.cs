using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Windows.Controls;

namespace TecCargo_Dagbog.Model
{
    class FilePDF
    {

        //Hvor dagbogen gemmes og hvad den hedder
        public static string printName = "General\\TecCargoDagbog.pdf";

        /// <summary>
        /// Opretter en dagbog som pdf 
        /// så man senere kan printe den
        /// </summary>
        public void CreatePDF() 
        {
            FileStream documentCreate = new FileStream(printName, FileMode.Create, FileAccess.Write);
            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, documentCreate);

            //Tilføj indhold
            PdfPTable document = makePdfTable(); 

            //lav document
            doc.Open();
            doc.Add(document);
            doc.Close();
            writer.Close();
            documentCreate.Close();
        }

        /// <summary>
        /// Opretter dagbog layout
        /// </summary>
        private PdfPTable makePdfTable() 
        {
            Model.FileClass.fileInput input = Inc.Settings.fileInput;
            Model.FileClass.function funcFile = new FileClass.function();
            Model.FileClass.function.sortDateArray sortData = funcFile.DateSort(input);

            PdfPCell whiteSpace = new PdfPCell();
            whiteSpace.Border = 0;
            whiteSpace.FixedHeight = 10;

            PdfPTable PersonTable = new PdfPTable(2);
            PersonTable.DefaultCell.Border = 0;
            PersonTable.SetWidths(new int[] { 1, 5 });

            PersonTable.AddCell(InsertText("Navn:", true));
            PersonTable.AddCell(InsertText(input.name));
            PersonTable.AddCell(InsertText("CPR:", true));
            PersonTable.AddCell(InsertText(input.cpr));

            PdfPTable logTable = new PdfPTable(1);
            for (int i = 0; i < sortData.dato.Count; i++)
			{
                logTable.AddCell(whiteSpace);
                PdfPTable newLog = new PdfPTable(1);
                newLog.DefaultCell.Border = 0;

                newLog.AddCell(InsertText(sortData.dato[i].ToShortDateString(), true, new int[] {5,5,5,5}));
                newLog.AddCell(InsertText(sortData.text[i], false, new int[] { 15, 5, 5, 5 }));

                logTable.AddCell(new PdfPCell(newLog));
                
			}


            PdfPTable document = new PdfPTable(1);
            document.DefaultCell.Border = 0;

            document.AddCell(ConvertTableToCell(PersonTable));
            document.AddCell(ConvertTableToCell(logTable));

            return document;
        }


        /// <summary>
        /// Laver en table om til en cell uden border
        /// </summary>
        private PdfPCell ConvertTableToCell(PdfPTable table)
        {

            PdfPCell cellTable = new PdfPCell(table);
            cellTable.Border = 0;

            return cellTable;
        }

        /// <summary>
        /// Tilføj noget tekst hurtigt
        /// </summary>
        private PdfPCell InsertText(string text, bool bold = false, int[] padding = null)
        {
            Font textStyle = new Font(Font.FontFamily.HELVETICA, 10f);

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

    }
}
