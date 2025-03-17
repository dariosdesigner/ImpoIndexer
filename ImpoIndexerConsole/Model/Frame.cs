using System;
namespace ImpoIndexerConsole.Model;

public class Frame
{
    private readonly List<ImpoPagina> pageSet;
    public IEnumerable<ImpoPagina> PageSet => pageSet;
    public Frame(int imposicao)
    {
        pageSet = new List<ImpoPagina>(imposicao);
    }
    public void AddPage(ImpoPagina page)
    {
        if(pageSet.Count == pageSet.Capacity)
        {
            throw new InvalidOperationException("Frame is full");
        }
        pageSet.Add(page);
    }
}
