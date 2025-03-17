namespace ImpoIndexerConsole.Model;

public class Formato
{
    public float Altura { get; }
    public float Largura { get; }
    public bool Retrato { get; }
    public Formato(float altura, float largura)
    {
        Altura = altura;
        Largura = largura;
        if (altura >= largura)
            Retrato = true;
    }
}
