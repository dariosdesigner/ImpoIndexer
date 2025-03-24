using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Properties;

namespace ImpoLib;

public static class ImpositionEngine
{
    /// <summary>
    /// Executa a imposição lendo do stream de entrada (ou gera um booklet numerado se nulo)
    /// e gravando o PDF resultante no stream de saída.
    /// </summary>
    public static void RunImposition(ImpositionSettings settings, Stream inputStream, Stream outputStream)
    {
        PdfDocument pdfDoc;
        int totalPages = 0;

        // Se não houver stream de entrada, gera um booklet numerado
        if (inputStream == null)
        {
            totalPages = settings.TotalPages;
            // Tamanho padrão (A4)
            PageSize pageSize = PageSize.A4;
            using (var msTemp = new MemoryStream())
            {
                // Cria PDF temporário
                PdfDocument tempDoc = new PdfDocument(new PdfWriter(msTemp));
                Document doc = new Document(tempDoc, pageSize);

                for (int i = 1; i <= totalPages; i++)
                {
                    PdfPage page = tempDoc.AddNewPage(pageSize);
                    PdfCanvas canvas = new PdfCanvas(page);
                    Canvas textCanvas = new Canvas(canvas, page.GetPageSize());
                    textCanvas.ShowTextAligned($"Página {i}",
                        page.GetPageSize().GetWidth() / 2,
                        page.GetPageSize().GetHeight() / 2,

                        TextAlignment.CENTER,
                        VerticalAlignment.MIDDLE, 0);
                    textCanvas.Close();
                }
                doc.Close();

                // Reabre o PDF gerado a partir do MemoryStream temporário
                pdfDoc = new PdfDocument(new PdfReader(new MemoryStream(msTemp.ToArray())));
            }
        }
        else
        {
            pdfDoc = new PdfDocument(new PdfReader(inputStream));
            totalPages = pdfDoc.GetNumberOfPages();
        }

        // Calcula número de páginas por folha (considerando frente e verso)
        int pagesPerSheet = settings.DoubleSided ? settings.PagesPerSide * 2 : settings.PagesPerSide;
        int pageCount = totalPages;
        if (totalPages % pagesPerSheet != 0)
        {
            pageCount = ((totalPages / pagesPerSheet) + 1) * pagesPerSheet;
        }

        PdfDocument outputDoc = new PdfDocument(new PdfWriter(outputStream));

        // Obtém dimensões da primeira página (originais)
        PdfPage firstPage = pdfDoc.GetPage(1);
        Rectangle pageSizeOriginal = firstPage.GetPageSize();
        float originalWidth = pageSizeOriginal.GetWidth();
        float originalHeight = pageSizeOriginal.GetHeight();

        // Calcula dimensões de saída com base no layout escolhido
        LayoutCalculator.CalculateOutputDimensions(settings.PagesPerSide, originalWidth, originalHeight,
            settings.GapBetweenPages, out float outputWidth, out float outputHeight, out int cols, out int rows);
        PageSize outputPageSize = new PageSize(outputWidth, outputHeight);

        // Determina o total de folhas necessárias
        int totalSheets = (int)Math.Ceiling((double)pageCount / pagesPerSheet);

        // Executa o método de imposição selecionado
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

        pdfDoc.Close();
        outputDoc.Close();
    }
}
