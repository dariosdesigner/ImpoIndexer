using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Layout;

namespace ImpoClaude;


public static class ImpositionMethods
{
    public static void PerfectBoundImposition(PdfDocument pdfDoc, PdfDocument outputDoc,
        int totalPages, int pageCount, int totalSheets,
        float originalWidth, float originalHeight, PageSize outputPageSize,
        int pagesPerSide, bool doubleSided, float gap, int cols, int rows)
    {
        for (int sheet = 0; sheet < totalSheets; sheet++)
        {
            // Cria uma nova página para a folha atual
            PdfPage outputPage = outputDoc.AddNewPage(outputPageSize);
            PdfCanvas canvas = new PdfCanvas(outputPage);

            // Calcula e impõe as páginas de acordo com o padrão perfect-bound
            int[] pageNumbers = LayoutCalculator.CalculatePerfectBoundPages(sheet, pageCount, pagesPerSide, doubleSided);

            // Impõe as páginas na folha
            for (int i = 0; i < pageNumbers.Length; i++)
            {
                int pageNumber = pageNumbers[i];
                int row = i / cols;
                int col = i % cols;

                float x = col * (originalWidth + gap);
                float y = row * (originalHeight + gap);

                PageImposition.ImposePageSafe(pdfDoc, outputDoc, canvas, pageNumber, totalPages,
                    originalWidth, originalHeight, x, y);
            }

            // Se for frente e verso, cria a página do verso
            if (doubleSided && sheet < totalSheets - 1)
            {
                // Cria uma nova página para o verso da folha
                outputPage = outputDoc.AddNewPage(outputPageSize);
                canvas = new PdfCanvas(outputPage);

                // Calcula e impõe as páginas do verso
                pageNumbers = LayoutCalculator.CalculatePerfectBoundPages(sheet, pageCount, pagesPerSide, doubleSided, true);

                for (int i = 0; i < pageNumbers.Length; i++)
                {
                    int pageNumber = pageNumbers[i];
                    int row = i / cols;
                    int col = i % cols;

                    float x = col * (originalWidth + gap);
                    float y = row * (originalHeight + gap);

                    PageImposition.ImposePageSafe(pdfDoc, outputDoc, canvas, pageNumber, totalPages,
                        originalWidth, originalHeight, x, y);
                }

                // Incrementa o contador de folhas
                sheet++;
            }
        }
    }

    public static void CutStackImposition(PdfDocument pdfDoc, PdfDocument outputDoc,
            int totalPages, int pageCount, int totalSheets,
            float originalWidth, float originalHeight, PageSize outputPageSize,
            int pagesPerSide, bool doubleSided, float gap, int cols, int rows)
    {
        // Caso específico: 2 páginas por lado e impressão duplex
        if (pagesPerSide == 2 && doubleSided)
        {
            // Assume que totalPages é múltiplo de 4 (ex.: 16 páginas)
            int half = totalPages / 2;
            int sheets = half / 2; // número de folhas (blocos)
            for (int i = 0; i < sheets; i++)
            {
                // FACE DA FRENTE:
                // Coluna esquerda: elemento ímpar da primeira metade
                int frontLeft = 2 * i + 1;
                // Coluna direita: elemento correspondente da segunda metade
                int frontRight = half + 2 * i + 1;

                // FACE DO VERSO:
                // Coluna esquerda: elemento da segunda metade que vem imediatamente após o par da frente
                int backLeft = half + 2 * i + 2;
                // Coluna direita: elemento par da primeira metade
                int backRight = 2 * i + 2;

                // Impressão da FACE DA FRENTE
                PdfPage frontPage = outputDoc.AddNewPage(outputPageSize);
                PdfCanvas frontCanvas = new PdfCanvas(frontPage);
                float y = 0;
                float x = 0; // coluna esquerda
                PageImposition.ImposePageSafe(pdfDoc, outputDoc, frontCanvas, frontLeft, totalPages,
                    originalWidth, originalHeight, x, y);
                x = originalWidth + gap; // coluna direita
                PageImposition.ImposePageSafe(pdfDoc, outputDoc, frontCanvas, frontRight, totalPages,
                    originalWidth, originalHeight, x, y);

                // Impressão da FACE DO VERSO com a ordem invertida
                PdfPage backPage = outputDoc.AddNewPage(outputPageSize);
                PdfCanvas backCanvas = new PdfCanvas(backPage);
                // Para o verso, a ordem é: coluna esquerda recebe backLeft e coluna direita recebe backRight
                x = 0;
                PageImposition.ImposePageSafe(pdfDoc, outputDoc, backCanvas, backLeft, totalPages,
                    originalWidth, originalHeight, x, y);
                x = originalWidth + gap;
                PageImposition.ImposePageSafe(pdfDoc, outputDoc, backCanvas, backRight, totalPages,
                    originalWidth, originalHeight, x, y);
            }
        }
        else
        {
            // Caso genérico ou quando pagesPerSide != 2, utiliza a lógica anterior
            for (int sheet = 0; sheet < totalSheets; sheet++)
            {
                // Cria uma nova página para a folha atual (frente)
                PdfPage outputPage = outputDoc.AddNewPage(outputPageSize);
                PdfCanvas canvas = new PdfCanvas(outputPage);

                // Calcula o número de páginas a serem impostas nesta folha (frente)
                int startPage = sheet * pagesPerSide + 1;

                // Impõe as páginas na folha (frente)
                for (int i = 0; i < pagesPerSide; i++)
                {
                    int pageNumber = startPage + i;
                    int row = i / cols;
                    int col = i % cols;

                    float x = col * (originalWidth + gap);
                    float y = row * (originalHeight + gap);

                    PageImposition.ImposePageSafe(pdfDoc, outputDoc, canvas, pageNumber, totalPages,
                        originalWidth, originalHeight, x, y);
                }

                // Se for frente e verso, cria a página do verso
                if (doubleSided && sheet < totalSheets - 1)
                {
                    // Cria uma nova página para o verso da folha
                    outputPage = outputDoc.AddNewPage(outputPageSize);
                    canvas = new PdfCanvas(outputPage);

                    // Calcula o número de páginas para o verso
                    startPage = (sheet + 1) * pagesPerSide + 1;

                    for (int i = 0; i < pagesPerSide; i++)
                    {
                        int pageNumber = startPage + i;
                        int row = i / cols;
                        int col = i % cols;

                        // Para o verso, inverte a ordem das colunas para alinhamento
                        float x = (cols - 1 - col) * (originalWidth + gap);
                        float y = row * (originalHeight + gap);

                        PageImposition.ImposePageSafe(pdfDoc, outputDoc, canvas, pageNumber, totalPages,
                            originalWidth, originalHeight, x, y);
                    }
                }
            }
        }
    }
}





