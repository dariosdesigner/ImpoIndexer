


namespace ImpoIndexerConsole.Model;

public class Dobra
{
    private List<int>? _indice;

    public TipoOrientacao OrientacaoInicial { get; set; }
    public TipoPaginacao LayoutPagina { get; set; }
    public int TotalPagina { get; set; }
    public List<Tracado> Tracados { get; set; } = [];
    public TipoTombo Tombo { get; set; }
    public IEnumerable<int>? Indice => _indice;

    internal void AddIndice(int v)
    {
        if (_indice is null)
        {
            _indice =[TotalPagina];
        }
        _indice.Add(v);
    }
    public void AddPageSet(Pagina[] pagina)
    {
        var pageset = Tracados.SelectMany(t => t.Frames).SelectMany(x => x.PageSet);
        foreach (var page in pageset)
        {
            page.Pagina=pagina[1];
        }
    }
}
