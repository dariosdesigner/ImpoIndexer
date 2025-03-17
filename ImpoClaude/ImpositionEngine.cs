using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoClaude
{
    public static class ImpositionEngine
    {
        public static void RunImposition(ImpositionSettings settings)
        {
            PdfDocument pdfDoc;
            int totalPages = 0;

            // Se não houver inputPath, gera um booklet com número de páginas
            if (string.IsNullOrWhiteSpace(settings.InputPath))
            {
                totalPages = settings.TotalPages;
                // Define tamanho padrão (A4)
                var pageSize = PageSize.A4;
                // Cria um PDF em memória
                MemoryStream ms = new MemoryStream();
                PdfDocument tempDoc = new PdfDocument(new PdfWriter(ms));

                Document doc = new Document(tempDoc,pageSize);

                for (int i = 1; i <= totalPages; i++)
                {
                    PdfPage page = tempDoc.AddNewPage(pageSize);
                    PdfCanvas canvas = new PdfCanvas(page);
                    // Cria um canvas de layout para centralizar o texto
                    Canvas textCanvas = new Canvas(canvas, page.GetPageSize());
                    textCanvas.ShowTextAligned($"Página {i}",
                        page.GetPageSize().GetWidth() / 2,
                        page.GetPageSize().GetHeight() / 2,
                        
                        TextAlignment.CENTER,
                        VerticalAlignment.MIDDLE,0);
                    textCanvas.Close();
                }
                doc.Close();

                // Reabre o PDF gerado em memória
                pdfDoc = new PdfDocument(new PdfReader(new MemoryStream(ms.ToArray())));
            }
            else
            {
                pdfDoc = new PdfDocument(new PdfReader(settings.InputPath));
                totalPages = pdfDoc.GetNumberOfPages();
            }

            // Calcula o número de páginas por folha (frente e verso)
            int pagesPerSheet = settings.DoubleSided ? settings.PagesPerSide * 2 : settings.PagesPerSide;

            // Ajusta o número de páginas para ser múltiplo do número de páginas por folha
            int pageCount = totalPages;
            if (totalPages % pagesPerSheet != 0)
            {
                pageCount = ((totalPages / pagesPerSheet) + 1) * pagesPerSheet;
            }

            // Cria o documento de saída
            PdfDocument outputDoc = new PdfDocument(new PdfWriter(settings.OutputPath));

            // Largura e altura da página original
            PdfPage firstPage = pdfDoc.GetPage(1);
            Rectangle pageSizeOriginal = firstPage.GetPageSize();
            float originalWidth = pageSizeOriginal.GetWidth();
            float originalHeight = pageSizeOriginal.GetHeight();

            // Calcula o tamanho da página de saída com base no número de páginas por lado
            float outputWidth = 0;
            float outputHeight = 0;
            int cols = 0;
            int rows = 0;

            LayoutCalculator.CalculateOutputDimensions(settings.PagesPerSide, originalWidth, originalHeight,
                settings.GapBetweenPages, out outputWidth, out outputHeight, out cols, out rows);

            // Tamanho da folha de saída
            PageSize outputPageSize = new PageSize(new Rectangle(outputWidth, outputHeight));

            // Determina o número de folhas necessárias
            int totalSheets = (int)Math.Ceiling((double)pageCount / pagesPerSheet);

            // Seleciona o método de imposição
            if (settings.ImpositionMethod == 1)
            {
                ImpositionMethods.PerfectBoundImposition(pdfDoc, outputDoc, totalPages, pageCount, totalSheets,
                    originalWidth, originalHeight, outputPageSize,
                    settings.PagesPerSide, settings.DoubleSided, settings.GapBetweenPages, cols, rows);
            }
            else
            {
                ImpositionMethods.CutStackImposition(pdfDoc, outputDoc, totalPages, pageCount, totalSheets,
                    originalWidth, originalHeight, outputPageSize,
                    settings.PagesPerSide, settings.DoubleSided, settings.GapBetweenPages, cols, rows);
            }

            // Fecha os documentos
            pdfDoc.Close();
            outputDoc.Close();
        }
    }
}
