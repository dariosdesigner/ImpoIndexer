using ImpoIndexerConsole.Model;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.Pdf;
using System.Diagnostics;

using Path = System.IO.Path;
using ImpoIndexerConsole.Extensions;
using ImpoIndexerConsole.Indexers;

namespace ImpoIndexerConsole
{
    class Application
    {
        public Task Run(string[] ags)
        {
            var w = new Stopwatch();
            w.Start();

            var arquivosImpo = Directory.GetFiles(@"D:\Programacao\ArquivosTeste\Moderna_AI_OB2_URB branco\00005-Branco\ENC_Parte1", "*.pdf");
            var pdfs = ObterPdfs(arquivosImpo);
            var pageset = ObterPageset(pdfs);
            CriarImposicao(pageset, @"D:\Programacao\ArquivosTeste\Moderna_AI_OB2_URB branco\00005-Branco\ENC_Parte1\Montado.pdf", 310, 510, 210, 297, 5, 90, 1f);

            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds);
            return Task.CompletedTask;
        }

        private IEnumerable<PdfInfo> ObterPdfs(string[] arquivos)
        {
            foreach (var arquivo in arquivos)
            {
                yield return PdfInfo.ObterInfo(arquivo);
            }
        }
        private IEnumerable<PageSetStruct> ObterPageset(IEnumerable<PdfInfo> pdfs)
        {
            return pdfs.SelectMany(x => Enumerable.Range(1, x.QtdPaginas).Select(p => new PageSetStruct(x.Name, p, x)));
        }

        private void CriarImposicao(IEnumerable<PageSetStruct> pageset, string destino, float papelLargura, float papelAltura, float largura, float altura, float sangria, double angulo, float escala, float deslocamento = 0)
        {
            var paginacao = new CutStack();
            var sequenciamento = paginacao.Calcular(pageset.Count(),new Template()).Index().ToList();
            ImpoPagina PaginaPadrao = new(largura, altura, new Sangria(5));
            var fresa = 6f.PT();
            deslocamento = deslocamento.PT();
            papelAltura = papelAltura.PT();
            papelLargura = papelLargura.PT();
            float l;
            float a;
            largura = largura.PT();
            altura = altura.PT();
            if (angulo == 0)
            {
                a = altura;
                l = largura;
            }
            else
            {
                l = altura;
                a = largura;
            }

            sangria = sangria.PT();
            float posicaoX = (papelLargura - l) / 2;
            float posicaoYP1 = (papelAltura - (a * 2 + fresa)) / 2;
            float posicaoYP2 = posicaoYP1 + fresa + a;
            using (var pdfDocWriter = new PdfDocument(new PdfWriter(destino)))
            {
                //for (var i = 0; i < sequenciamento.Count(); i++)
                //{
                //    pdfDocWriter.AddNewPage(new PageSize(new Rectangle(papelLargura, papelAltura)));
                //}
                //var ordenacao = sequenciamento.SelectMany(x => x.Item.PageSet.Index().Select(p=> new {x.Index,Sequencia= p.Index,PageSet=p.Item}) ).ToList();

                //foreach (var pdfinfos in ordenacao.GroupBy(x => x.PageSet.PdfInfo))
                //{
                //    using (var pdfDocReader = new PdfDocument(new PdfReader(pdfinfos.Key.Pdf())))
                //    {
                //        foreach (var page in pdfinfos)
                //        {
                //            var YPos= page.Sequencia % 2 == 0 ? posicaoYP1 : posicaoYP2;
                //            PdfCanvas canvas = new PdfCanvas(pdfDocWriter.GetPage(page.Index+1));
                //                PosicionarPagina(canvas, largura, altura, sangria, angulo, escala, posicaoX, YPos, deslocamento, pdfDocWriter, pdfDocReader, page.PageSet.Pagina);
                //        }
                //    }
                //}
            }
        }

        private void PosicionarPagina(PdfCanvas canvas, float largura, float altura, float sangria, double angulo, float escala, float posicaoX, float posicaoY, float deslocamento, PdfDocument pdfDocWriter, PdfDocument pdfDocReader, int i)
        {

            PdfPage paginaOriginal = pdfDocReader.GetPage(i);
            Rectangle formatoOriginal = paginaOriginal.GetPageSize();
            var rotacao = paginaOriginal.GetRotation();
            AffineTransform tranformRotacao = new AffineTransform();
            float[] matrixRot = new float[6];
            var trimrot = paginaOriginal.GetTrimBox();
            var trimbox = paginaOriginal.GetTrimBox();
            tranformRotacao.Rotate(AnguloToRadianos(-paginaOriginal.GetRotation()));
            PdfFormXObject rotpage = new PdfFormXObject(paginaOriginal.GetPageSizeWithRotation());
            var canvasRot = new PdfCanvas(rotpage, pdfDocWriter);
            if (rotacao == 90)
            {
                tranformRotacao.Concatenate(AffineTransform.GetTranslateInstance(-rotpage.GetHeight(), 0));
                trimbox = new Rectangle(trimrot.GetY(), trimrot.GetX(), trimrot.GetHeight(), trimrot.GetWidth());
            }

            if (rotacao == 180)
            {
                tranformRotacao.Concatenate(AffineTransform.GetTranslateInstance(-rotpage.GetWidth(), -rotpage.GetHeight()));

            }
            if (rotacao == 270)
            {
                tranformRotacao.Concatenate(AffineTransform.GetTranslateInstance(0, -rotpage.GetWidth()));
                trimbox = new Rectangle(trimrot.GetY(), trimrot.GetX(), trimrot.GetHeight(), trimrot.GetWidth());
            }


            tranformRotacao.GetMatrix(matrixRot);
            canvasRot.AddXObjectWithTransformationMatrix(paginaOriginal.CopyAsFormXObject(pdfDocWriter), matrixRot[0], matrixRot[1], matrixRot[2], matrixRot[3], matrixRot[4], matrixRot[5]);
            var formatoCropado = new Rectangle(largura + (sangria * 2), (altura + sangria * 2));
            var offsetx = (trimbox.GetWidth() - largura) / 2;
            var offsety = (trimbox.GetHeight() - largura) / 2;

            var larguraPaginaEscalonado = rotpage.GetWidth() * escala;
            var alturaPaginaEscalonado = rotpage.GetHeight() * escala;
            var offsetxpagina = ((formatoCropado.GetWidth() - (trimbox.GetWidth() * escala)) / 2) - (trimbox.GetX() * escala) + deslocamento;
            var offsetypagina = ((formatoCropado.GetHeight() - (trimbox.GetHeight() * escala)) / 2) - (trimbox.GetY() * escala);

            PdfFormXObject shirinkpage = new PdfFormXObject(formatoCropado);
            Rectangle tamanhoEscalonado = new Rectangle(offsetxpagina, offsetypagina, larguraPaginaEscalonado, alturaPaginaEscalonado);

            var canvasShirink = new PdfCanvas(shirinkpage, pdfDocWriter);
            canvasShirink.AddXObjectFittedIntoRectangle(rotpage, tamanhoEscalonado);
            canvasShirink.SaveState();

            var xpos = posicaoX;
            var ypos = posicaoY;


            AffineTransform affineTransform = new AffineTransform();
            float[] matrix = new float[6];
            Rectangle AreaCropMark;
            affineTransform.Rotate(AnguloToRadianos(angulo));
            if (angulo == 90)
            {
                AreaCropMark = new Rectangle(xpos, ypos, altura, largura);
                affineTransform.Translate(ypos, -(xpos + altura));
            }
            else if (angulo == 180)
            {
                AreaCropMark = new Rectangle(xpos, ypos, largura, altura);
                affineTransform.Translate(-(xpos + largura), -(ypos + altura));
            }
            else if (angulo == 270)
            {
                AreaCropMark = new Rectangle(xpos, ypos, altura, largura);
                affineTransform.Translate(-(ypos + largura), xpos);
            }
            else
            {
                AreaCropMark = new Rectangle(xpos, ypos, largura, altura);
                affineTransform.Translate(xpos, ypos);
            }

            var Transformoffsetx = -sangria;
            var transformoffsety = -sangria;
            affineTransform.Concatenate(AffineTransform.GetTranslateInstance(Transformoffsetx, transformoffsety));
            affineTransform.GetMatrix(matrix);
            canvas.AddXObjectWithTransformationMatrix(shirinkpage, matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5]);
            canvas.Fill();

            AddCropMark(AreaCropMark, 5f.PT(), 3f.PT(), canvas);
        }

        private void AddCropMark(Rectangle rectangle, float tamanho, float offset, PdfCanvas gabarito)
        {
            //tamanho = tamanho.PT();
            //offset= offset.PT();

            var xl = rectangle.GetLeft();
            var yb = rectangle.GetBottom();
            var xr = rectangle.GetRight();
            var yt = rectangle.GetTop();


            AddLinha(xl - offset, yb, xl - tamanho - offset, yb, gabarito);
            AddLinha(xl, yb - offset, xl, yb - offset - tamanho, gabarito);

            AddLinha(xr + offset, yb, xr + tamanho + offset, yb, gabarito);
            AddLinha(xr, yb - offset, xr, yb - offset - tamanho, gabarito);

            AddLinha(xl - offset, yt, xl - tamanho - offset, yt, gabarito);
            AddLinha(xl, yt + offset, xl, yt + offset + tamanho, gabarito);

            AddLinha(xr + offset, yt, xr + tamanho + offset, yt, gabarito);
            AddLinha(xr, yt + offset, xr, yt + offset + tamanho, gabarito);

        }
        private void AddLinha(float inicialX, float inicialY, float finalX, float finalY, PdfCanvas canvas)
        {


            canvas.MoveTo(inicialX, inicialY).SetLineWidth(1);
            canvas.LineTo(finalX, finalY).SetLineWidth(1);

            canvas.SetStrokeColor(DeviceCmyk.BLACK);
            canvas.Stroke();
        }

        private double AnguloToRadianos(double angulo)
        {
            return 2.0 * Math.PI * (angulo / 360.0);
        }
    }
}
