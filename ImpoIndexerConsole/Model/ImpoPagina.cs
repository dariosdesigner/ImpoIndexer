namespace ImpoIndexerConsole.Model;

public class ImpoPagina
{
    public ImpoPagina(float largura, float altura, Sangria sangria)
    {
        FormatoPagina = new Dimensao(largura, altura, 0, 0);
        Sangria = sangria;
    }
    public TipoOrientacao Orientacao { get; set; }
    public Sangria? Sangria { get; set; }
    public Dimensao? FormatoPagina { get; set; }
    public int Sequencia { get; set; }
    public Pagina Pagina { get; set; }
}
