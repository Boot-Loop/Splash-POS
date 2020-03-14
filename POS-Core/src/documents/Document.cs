using iTextSharp.text;
using iTextSharp.text.pdf;

using System;
using System.Data;
using System.IO;

namespace Core.Documents
{
    public abstract class Doc
    {
        public void exportDataTableToPdf(DataTable data_table, String pdf_path, string header) {
            FileStream file_stream = new FileStream(pdf_path, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document();
            document.SetPageSize(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, file_stream);
            document.Open();

            //Report Header
            BaseFont base_font_header = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font_head = new Font(base_font_header, 16, 1, Color.GRAY);
            Paragraph paragraph_heading = new Paragraph();
            paragraph_heading.Alignment = Element.ALIGN_CENTER;
            paragraph_heading.Add(new Chunk(header.ToUpper(), font_head));
            document.Add(paragraph_heading);

            //Author
            Paragraph paragraph_author = new Paragraph();
            BaseFont base_font_author = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font_author = new Font(base_font_author, 8, 2, Color.GRAY);
            paragraph_author.Alignment = Element.ALIGN_RIGHT;
            paragraph_author.Add(new Chunk("Splash Shoe", font_author));
            paragraph_author.Add(new Chunk("\nCreation Date : " + DateTime.Now.ToShortDateString(), font_author));
            document.Add(paragraph_author);

            //Add a line seperation
            Paragraph paragraph = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(paragraph);

            //Add line break
            document.Add(new Chunk("\n", font_head));

            //Write the table
            PdfPTable table = new PdfPTable(data_table.Columns.Count);
            //Table header
            BaseFont base_font_coulum_header = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntColumnHeader = new Font(base_font_coulum_header, 10, 1, Color.WHITE);
            for (int i = 0; i < data_table.Columns.Count; i++) {
                PdfPCell cell = new PdfPCell();
                cell.BackgroundColor = Color.GRAY;
                cell.AddElement(new Chunk(data_table.Columns[i].ColumnName.ToUpper(), fntColumnHeader));
                table.AddCell(cell);
            }
            //table Data
            for (int i = 0; i < data_table.Rows.Count; i++) {
                for (int j = 0; j < data_table.Columns.Count; j++) {
                    table.AddCell(data_table.Rows[i][j].ToString());
                }
            }

            document.Add(table);
            document.Close();
            writer.Close();
            file_stream.Close();
        }
    }
}