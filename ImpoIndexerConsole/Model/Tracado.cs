
namespace ImpoIndexerConsole.Model;

public class Tracado
{
    public TipoTombo Tombo { get; set; }
    public Dimensao? Folha { get; set; }  
    public List<Frame>? Frames { get; set; }
}