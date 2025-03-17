using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.Pdf;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Colors;
namespace ImpoClaude
{
    public static class PageImposition
    {
        public static void ImposePageSafe(PdfDocument pdfDoc, PdfDocument outputDoc, PdfCanvas canvas,
            int pageNumber, int totalPages, float width, float height, float x, float y)
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
                    PdfFormXObject pageCopy = page.CopyAsFormXObject(outputDoc);

                    // Adiciona o XObject ao canvas na posição (0,0)
                    canvas.AddXObject(pageCopy);

                    // Adiciona o número da página para fins de depuração (opcional)
                    canvas.BeginText();
                    canvas.SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 8);
                    canvas.MoveText(5, 5);
                    canvas.ShowText(pageNumber.ToString());
                    canvas.EndText();

                    // Restaura o estado do canvas
                    canvas.RestoreState();
                }
                else
                {
                    // Desenha uma borda vazia para páginas que não existem
                    canvas.SaveState();
                    canvas.ConcatMatrix(AffineTransform.GetTranslateInstance(x, y));
                    canvas.Rectangle(0, 0, width, height);
                    canvas.Stroke();

                    // Adiciona um texto indicando que a página está vazia
                    canvas.BeginText();
                    canvas.SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 12);
                    canvas.MoveText(width / 2 - 20, height / 2);
                    canvas.ShowText("Página vazia");
                    canvas.EndText();

                    canvas.RestoreState();
                }
            }
            catch (Exception ex)
            {
                // Registra o erro e continua
                Console.WriteLine($"Erro ao impor página {pageNumber}: {ex.Message}");

                // Desenha um retângulo vermelho para indicar erro
                canvas.SaveState();
                canvas.ConcatMatrix(AffineTransform.GetTranslateInstance(x, y));
                canvas.SetStrokeColor(ColorConstants.RED);
                canvas.Rectangle(0, 0, width, height);
                canvas.Stroke();
                canvas.RestoreState();
            }
        }
    }
}