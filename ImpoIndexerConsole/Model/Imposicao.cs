using ImpoIndexerConsole.Indexers;
namespace ImpoIndexerConsole.Model;
public class Imposicao
{
    public string Nome { get; set; }
    public Papel Papel { get; set; }
    public Template Template { get; set; }
    public List<Dobra> Cadernos = [];
    public List<Pagina> PageSets = [];
    public IPaginacao Paginacao { get; set; }

    
}
