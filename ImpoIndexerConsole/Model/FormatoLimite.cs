
namespace ImpoIndexerConsole.Model;

public class FormatoLimite
{
    public Formato FormatoMaximo { get; }
    public Formato FormatoMinimo { get; }

    public FormatoLimite(Formato formatoMaximo, Formato formatoMinimo)
    {
        FormatoMaximo = formatoMaximo;
        FormatoMinimo = formatoMinimo;
    }
    public FormatoLimite(float larguraMaxima, float alturaMaxima, float larguraMinima, float alturaMinima)
    {
        FormatoMaximo = new Formato(alturaMaxima, larguraMaxima);
        FormatoMinimo = new Formato(alturaMinima, larguraMinima);
    }

    public bool VerificarEspecificacaoFormato(Formato formato)
    {
        if (formato == null)
            return false;
        if (formato.Largura > FormatoMaximo.Largura)
            return false;
        if (formato.Altura > FormatoMaximo.Altura)
            return false;
        if (formato.Largura < FormatoMinimo.Largura)
            return false;
        if (formato.Altura < FormatoMinimo.Altura)
            return false;
        return true;
    }
}
