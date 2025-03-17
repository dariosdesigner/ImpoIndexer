namespace ImpoIndexerConsole.Model;

public class Pagina
{
    public bool AlinhamentoTrim { get; set; }
    public TipoAncora Ancora { get; set; }
    public int Numero { get; set; }
    public Dimensao? FormatoArquivo { get; private set; }
    public float EscalaLargura { get; set; }
    public float EscalaAltura { get; set; }
    public float DeslocamentoX { get; set; }
    public float DeslocamentoY { get; set; }
    public PdfInfo PdfInfo { get; }
    public Pagina(string source, int pagina, PdfInfo pdfInfo)
    {
        PdfInfo = pdfInfo;
        EscalaAltura = 1;
        EscalaLargura = 1;
        DeslocamentoX = 0;
        DeslocamentoY = 0;
        Numero = pagina;
        Ancora = TipoAncora.Central;
        AlinhamentoTrim = true;
    }

}
