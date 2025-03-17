using ImpoIndexerConsole.Indexers;

namespace ImpoIndexerConsole.Model;

public class Template
{
    public string Nome { get; set; }
    public List<Dobra> Dobras { get; set; }

    public Dobra? ObterDobra(int restante)
    {
        return Dobras.FirstOrDefault(x=> x.TotalPagina<=restante);
    }
}
