using ImpoIndexerConsole.Model;

namespace ImpoIndexerConsole.Indexers
{
    public interface IPaginacao
    {
        IEnumerable<Dobra> Calcular(int totalPaginas, Template templatedobras);
    }
}