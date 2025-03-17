using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoIndexerConsole.Model;

public record class PdfInfo
{
    public string Name { get; }
    readonly string _arquivo;
    public Box? MediaBox { get; private set; }
    public Box? CropBox { get; private set; }
    public Box? TrimBox { get; private set; }
    public float Altura { get; private set; }
    public float Largura { get; private set; }
    public int QtdPaginas { get; private set; }
    public PdfInfo(string arquivo)
    {
        if (Path.GetExtension(arquivo).ToLower() == ".pdf")
        {
            _arquivo = arquivo;
            Name = Path.GetFileName(arquivo);
            return;
        }
        throw new Exception("o Arquivo não é um pdf valido");

    }
    public void ObterInfo()
    {
        using (PdfDocument pdfdoc = new PdfDocument(new PdfReader(_arquivo)))
        {
            var paginaInicial = pdfdoc.GetFirstPage();
            Altura = paginaInicial.GetPageSize().GetHeight();
            Largura = paginaInicial.GetPageSize().GetWidth();
            QtdPaginas = pdfdoc.GetNumberOfPages();
            var trim = paginaInicial.GetTrimBox();
            TrimBox = new Box(trim.GetWidth, trim.GetWidth, trim.GetX, trim.GetY);
            var crop = paginaInicial.GetCropBox();
            CropBox = new Box(crop.GetWidth, crop.GetWidth, crop.GetX, crop.GetY);
            var media = paginaInicial.GetMediaBox();
            MediaBox = new Box(media.GetWidth, media.GetWidth, media.GetX, media.GetY);
            paginaInicial = null;
        }


    }
    public static PdfInfo ObterInfo(string arquivo)
    {
        PdfInfo info = new PdfInfo(arquivo);
        info.ObterInfo();
        return info;
    }
    public FileInfo Pdf()
    {
        return new FileInfo(_arquivo);
    }
}

