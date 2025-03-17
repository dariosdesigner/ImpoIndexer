namespace ImpoIndexerConsole.Model;

public struct PageSetStruct(string source, int pagina, PdfInfo pdfInfo)
{
    public string Source { get; } = source;
    public int Pagina { get; } = pagina;
    public PdfInfo PdfInfo { get; } = pdfInfo;

    public override string ToString()
    {
        return $"{Source}-{Pagina}";
    }
}

