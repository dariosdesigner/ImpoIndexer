
namespace ImpoIndexerConsole.Model;

public class Box
{
    private Func<float> getWidth1;
    private Func<float> getWidth2;
    private Func<float> getX;
    private Func<float> getY;

    public Box(Func<float> getWidth1, Func<float> getWidth2, Func<float> getX, Func<float> getY)
    {
        this.getWidth1 = getWidth1;
        this.getWidth2 = getWidth2;
        this.getX = getX;
        this.getY = getY;
    }
}
