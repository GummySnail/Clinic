﻿using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Documents.Core.Interfaces.Logic;
using Documents.Core.Interfaces.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Microsoft.Extensions.Configuration;
using StreamReader = System.IO.StreamReader;


namespace Documents.Core.Logic;

public class PdfGenerator : IPdfGenerator
{
    private readonly string _storageConnectionString;
    private readonly string _storageContainerName;

    public PdfGenerator(IConfiguration config)
    {
        _storageConnectionString = config.GetValue<string>("BlobConnectionString");
        _storageContainerName = config.GetValue<string>("BlobContainerName");
    }

    public async Task<byte[]> CreatePdfAsync(string complaints, string conclusion, string recommendations)
    {
        byte[] bytes = null;

        using (MemoryStream ms = new MemoryStream())
        {
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            Chunk linebreak = new Chunk(new LineSeparator(4f, 100f, default, Element.ALIGN_CENTER, -1));
            
            document.Open();

            var header = new Paragraph("Appointment Result\n\n", new Font(Font.FontFamily.TIMES_ROMAN, 24))
            {
                Alignment = Element.ALIGN_CENTER
            };
            
            document.Add(header);
            document.Add(linebreak);

            var complaintsParagraph = new Paragraph("Complaints\n", new Font(Font.FontFamily.TIMES_ROMAN, 20))
            {
                Alignment = Element.ALIGN_CENTER
            };
            
            document.Add(complaintsParagraph);

            var complaintsText = new Paragraph(complaints, new Font(Font.FontFamily.TIMES_ROMAN, 14))
            {
                Alignment = Element.ALIGN_LEFT
            };
            
            document.Add(complaintsText);
            document.Add(linebreak);


            var conclusionParagraph = new Paragraph("Conclusion\n", new Font(Font.FontFamily.TIMES_ROMAN, 20))
            {
                Alignment = Element.ALIGN_CENTER
            };
            
            document.Add(conclusionParagraph);

            var conclusionText = new Paragraph(conclusion, new Font(Font.FontFamily.TIMES_ROMAN, 14))
            {
                Alignment = Element.ALIGN_LEFT
            };
            
            document.Add(conclusionText);
            document.Add(linebreak);


            var recommendationsParagraph = new Paragraph("Recommendations\n", new Font(Font.FontFamily.TIMES_ROMAN, 20))
            {
                Alignment = Element.ALIGN_CENTER
            };
            
            document.Add(recommendationsParagraph);

            var recommendationsText = new Paragraph(recommendations, new Font(Font.FontFamily.TIMES_ROMAN, 14))
            {
                Alignment = Element.ALIGN_LEFT
            };
            
            document.Add(recommendationsText);
            document.Add(linebreak);
            document.Close();
            writer.Close();

            bytes = ms.ToArray();
        }
        return bytes;
    }
}