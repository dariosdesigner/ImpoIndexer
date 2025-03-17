using ImpoIndexerConsole.Model;

namespace ImpoIndexerConsole.Indexers;

public class CutStack : IPaginacao
{
    public IEnumerable<Dobra> Calcular(int totalPaginas, Template templatedobras)
    {
        List<Dobra> Sequenciamento = [];

        while (Sequenciamento.Sum(x => x.TotalPagina) < totalPaginas)
        {
            var restante = totalPaginas - Sequenciamento.Sum(x => x.TotalPagina);
            var dobra = templatedobras.ObterDobra(restante);

            while (dobra is null)
            {
                totalPaginas++;
                dobra= templatedobras.ObterDobra(restante);
                if(dobra is not null)
                {
                    Sequenciamento.Insert(Sequenciamento.Count-1, dobra);
                    break;
                }
            }

            Sequenciamento.Add(dobra);
        }


        var indice = 0;
        foreach (var dobra in Sequenciamento)
        { 
   
            dobra.AddIndice(indice++);
            var fator= dobra.Tombo== TipoTombo.SingleSided? dobra.TotalPagina: dobra.TotalPagina/2;
            for (int p = 1; p < fator; p++)
            {
                var result = indice + p * Sequenciamento.Count;
                    dobra.AddIndice(result);

            }

        }
        return Enumerable.Empty<Dobra>();
    }

    public void CalcularArrayPoolStruct(Dictionary<string, IEnumerable<int>> arquivos, int pagsImpo)
    {
        //var ind = 0;
        //var listaPaginas = arquivos.SelectMany(x => x.Value.Select(v => new PageSetStruct(ind++, x.Key, v)!)).ToArray();
        //var totalpaginas = listaPaginas.Count();
        //var listaPaginasOrdenada = ArrayPool<PageSetStruct>.Shared.Rent(totalpaginas);

        //while (totalpaginas % pagsImpo != 0)
        //{
        //    totalpaginas++;
        //}
        //var frames = totalpaginas / pagsImpo;
        ////var listaIndexer = new List<int>(totalpaginas);
        //var offset = 0;
        //for (int i = 0; i < frames; i++)
        //{
        //    listaPaginasOrdenada[offset++] = listaPaginas[i];
        //    //listaIndexer.Add(i);
        //    for (int p = 1; p < pagsImpo; p++)
        //    {
        //        var indice = i + p * frames;
        //        if (indice < listaPaginas.Length)
        //        {
        //            listaPaginasOrdenada[offset++] = listaPaginas[indice];
        //        }
        //        //listaIndexer.Add(i+p*frames);
        //    }
        //}
        //for (int i = 0; i < totalpaginas; i++)
        //{
        //    listaPaginas[i] = listaPaginasOrdenada[i];
        //}

        //ArrayPool<PageSetStruct>.Shared.Return(listaPaginasOrdenada);
    }
}


