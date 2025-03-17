using ImpoIndexerConsole.Model;

namespace ImpoIndexerConsole.Extensions
{
    public static class ImposicaoExtensionPDF
    {
        public static FileInfo GerarPDF(this Imposicao imposicao, string caminho)
        {
            //var result=  imposicao.Paginacao.Calcular(imposicao.PageSets.ToArray(), imposicao.Template.Dobras);
            return new FileInfo(caminho);
        }
    }
}
