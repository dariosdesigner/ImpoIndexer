using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Xobject;

namespace ImpoIndexerConsole.Model;

class ClaudeApplication
{
    public Task Run()
    {

        Console.WriteLine("Programa de Imposição de Páginas PDF");
        Console.WriteLine("====================================");
        Console.WriteLine("1. Perfect-Bound (Encadernação perfeita)");
        Console.WriteLine("2. Cut-Stack (Corte e empilhamento)");
        Console.Write("Escolha o método de imposição (1 ou 2): ");

        int escolha = int.Parse(Console.ReadLine());

        Console.Write("Digite o caminho do arquivo PDF de entrada: ");
        string inputPath = Console.ReadLine();
        inputPath = @"D:\Programacao\ArquivosTeste\Moderna_AI_OB2_URB branco\00021-Branco\PAL\001_PAL_00021_0179P23020001_000000001090507_0001-0248.pdf";

        Console.Write("Digite o caminho do arquivo PDF de saída: ");
        string outputPath = Console.ReadLine();
        outputPath = @"D:\Programacao\ArquivosTeste\Moderna_AI_OB2_URB branco\00021-Branco\PAL\001_PAL_00021_0179P23020001_000000001090507_0001-0248_montado.pdf";

        try
        {
            switch (escolha)
            {
                case 1:
                    PerfectBoundImposition(inputPath, outputPath);
                    break;
                case 2:
                    CutStackImposition(inputPath, outputPath);
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }

            Console.WriteLine("Imposição concluída com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }

        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();
        return Task.CompletedTask;
    }

    static void PerfectBoundImposition(string inputPath, string outputPath)
    {
        // Abre o documento de entrada
        PdfDocument pdfDoc = new PdfDocument(new PdfReader(inputPath));
        int totalPages = pdfDoc.GetNumberOfPages();

        // Ajusta o número de páginas para ser múltiplo de 4
        int pageCount = totalPages;
        if (pageCount % 4 != 0)
        {
            pageCount = ((pageCount / 4) + 1) * 4;
        }

        // Cria o documento de saída
        PdfDocument outputDoc = new PdfDocument(new PdfWriter(outputPath));

        // Determina o número de folhas necessárias (cada folha terá 4 páginas)
        int sheets = pageCount / 4;

        // Largura e altura da página original
        PdfPage firstPage = pdfDoc.GetPage(1);
        Rectangle pageSize = firstPage.GetPageSize();
        float pageWidth = pageSize.GetWidth();
        float pageHeight = pageSize.GetHeight();

        // Tamanho da folha de saída (2x o tamanho original)
        Rectangle outputPageSize = new Rectangle(pageWidth * 2, pageHeight * 2);

        // Cria as páginas de saída
        for (int sheet = 1; sheet <= sheets; sheet++)
        {
            // Cria uma página para a frente da folha
            PdfPage outputPage = outputDoc.AddNewPage(new PageSize(outputPageSize));
            PdfCanvas canvas = new PdfCanvas(outputPage);

            // Calcula as páginas a serem colocadas na frente
            int p1 = pageCount - (sheet - 1) * 2;
            int p2 = (sheet - 1) * 2 + 1;
            int p3 = (sheet - 1) * 2 + 2;
            int p4 = pageCount - (sheet - 1) * 2 - 1;

            // Impõe as páginas na frente
            ImposePage(pdfDoc, canvas, p1, pageWidth, pageHeight, 0, 0);
            ImposePage(pdfDoc, canvas, p2, pageWidth, pageHeight, pageWidth, 0);
            ImposePage(pdfDoc, canvas, p3, pageWidth, pageHeight, 0, pageHeight);
            ImposePage(pdfDoc, canvas, p4, pageWidth, pageHeight, pageWidth, pageHeight);
        }

        // Fecha os documentos
        pdfDoc.Close();
        outputDoc.Close();
    }

    static void CutStackImposition(string inputPath, string outputPath)
    {
        try
        {
            using (PdfDocument outputDoc = new PdfDocument(new PdfWriter(outputPath)))
            {
                using (PdfDocument pdfDoc = new PdfDocument(new PdfReader(inputPath)))
                {

                    int totalPages = pdfDoc.GetNumberOfPages();

                int pageCount = totalPages;
                if (pageCount % 4 != 0)
                {
                    pageCount = ((pageCount / 4) + 1) * 4;
                }

                // Cria o documento de saída


                // Determina o número de folhas necessárias (cada folha terá 4 páginas)
                int sheets = pageCount / 4;

                // Largura e altura da página original
                PdfPage firstPage = pdfDoc.GetPage(1);
                Rectangle pageSize = firstPage.GetPageSize();
                float pageWidth = pageSize.GetWidth();
                float pageHeight = pageSize.GetHeight();

                // Tamanho da folha de saída (2x o tamanho original)
                Rectangle outputPageSize = new Rectangle(pageWidth * 2, pageHeight * 2);

                // Cria as páginas de saída
                for (int sheet = 0; sheet < sheets; sheet++)
                {
                    // Cria uma página para a folha
                    PdfPage outputPage = outputDoc.AddNewPage(new PageSize(outputPageSize));
                    PdfCanvas canvas = new PdfCanvas(outputPage);

                    // Calcula as páginas a serem colocadas na folha
                    int p1 = sheet * 4 + 1;
                    int p2 = sheet * 4 + 2;
                    int p3 = sheet * 4 + 3;
                    int p4 = sheet * 4 + 4;

                    try
                    {
                        // Impõe as páginas na folha
                        ImposePageSafe(pdfDoc, canvas, p1, totalPages, pageWidth, pageHeight, 0, 0);
                        ImposePageSafe(pdfDoc, canvas, p2, totalPages, pageWidth, pageHeight, pageWidth, 0);
                        ImposePageSafe(pdfDoc, canvas, p3, totalPages, pageWidth, pageHeight, 0, pageHeight);
                        ImposePageSafe(pdfDoc, canvas, p4, totalPages, pageWidth, pageHeight, pageWidth, pageHeight);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao impor páginas para a folha {sheet + 1}: {ex.Message}");
                        // Continua com a próxima folha
                    }
                }

                // Fecha os documentos
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro geral no método CutStackImposition: {ex.Message}");
            throw;
        }
    }

    static void ImposePage(PdfDocument pdfDoc, PdfCanvas canvas, int pageNumber, float width, float height, float x, float y)
    {
        // Verifica se a página existe
        if (pageNumber <= pdfDoc.GetNumberOfPages() && pageNumber > 0)
        {
            try
            {
                // Obtém a página
                PdfPage page = pdfDoc.GetPage(pageNumber);

                // Cria uma nova transformação de matriz
                canvas.SaveState();
                canvas.ConcatMatrix(AffineTransform.GetTranslateInstance(x, y));

                // Copia o conteúdo da página para a posição desejada
                PdfFormXObject pageCopy = page.CopyAsFormXObject(canvas.GetDocument());
                canvas.AddXObjectAt(pageCopy, 0, 0);

                // Restaura o estado
                canvas.RestoreState();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar a página {pageNumber}: {ex.Message}");
                // Adiciona uma página em branco em vez da página com erro
                canvas.SaveState();
                canvas.ConcatMatrix(AffineTransform.GetTranslateInstance(x, y));
                canvas.Rectangle(0, 0, width, height);
                canvas.Stroke();
                canvas.RestoreState();
            }
        }
        else
        {
            // Adiciona uma página em branco para as páginas inexistentes
            canvas.SaveState();
            canvas.ConcatMatrix(AffineTransform.GetTranslateInstance(x, y));
            canvas.Rectangle(0, 0, width, height);
            canvas.Stroke();
            canvas.RestoreState();
        }
    }

    // Método alternativo mais seguro para impor páginas
    static void ImposePageSafe(PdfDocument pdfDoc, PdfCanvas canvas, int pageNumber, int totalPages, float width, float height, float x, float y)
    {
        try
        {
            // Verifica se a página existe
            if (pageNumber <= totalPages && pageNumber > 0)
            {
                // Obtém a página
                PdfPage page = pdfDoc.GetPage(pageNumber);

                // Salva o estado atual do canvas
                canvas.SaveState();

                // Aplica a transformação para posicionar a página no local correto
                canvas.ConcatMatrix(AffineTransform.GetTranslateInstance(x, y));

                // Copia o conteúdo da página como um XObject
                PdfFormXObject pageCopy = page.CopyAsFormXObject(canvas.GetDocument());

                // Adiciona o XObject ao canvas na posição (0,0)
                canvas.AddXObject(pageCopy);

                // Restaura o estado do canvas
                canvas.RestoreState();
            }
            else
            {
                // Desenha uma borda vazia para páginas que não existem
                canvas.SaveState();
                canvas.SetLineWidth(0.5f);
                canvas.Rectangle(x, y, width, height);
                canvas.Stroke();
                canvas.RestoreState();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro no método ImposePageSafe para página {pageNumber}: {ex.Message}");

            // Em caso de erro, pelo menos desenha uma borda para indicar onde a página deveria estar
            try
            {
                canvas.SaveState();
                canvas.SetLineWidth(0.5f);
                canvas.Rectangle(x, y, width, height);
                canvas.Stroke();
                canvas.RestoreState();
            }
            catch
            {
                // Ignora erros ao desenhar a borda
            }
        }
    }
}
