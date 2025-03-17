namespace ImpoIndexerConsole.Model;

public class Sangria
{
    public Sangria(float sangrias)
    {
        Pe = sangrias;
        Cabeca = sangrias;
        Lombo = sangrias;
        Corte = sangrias;
    }

    public float Pe { get; set; }
    public float Cabeca { get; set; }
    public float Lombo { get; set; }
    public float Corte { get; set; }
}
