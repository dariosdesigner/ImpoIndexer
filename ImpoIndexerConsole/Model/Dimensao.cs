
namespace ImpoIndexerConsole.Model;
public class Dimensao : Formato
{
    public float PosX { get; init; }
    public float PosY { get; init; }
    public Dimensao(float altura, float largura, float posX, float posY) : base(altura, largura)
    {
        PosX = posX;
        PosY = posY;
    }
}
