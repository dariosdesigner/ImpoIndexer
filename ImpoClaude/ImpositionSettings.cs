

namespace ImpoClaude;

public class ImpositionSettings
{
    public int ImpositionMethod { get; set; } = 1; // 1 = Perfect-Bound, 2 = Cut-Stack
    public string InputPath { get; set; }
    public string OutputPath { get; set; }
    public bool DoubleSided { get; set; } = true;
    public int PagesPerSide { get; set; } = 4;
    public float GapBetweenPages { get; set; } = 0; // Fresa (em pontos)
    public int TotalPages { get; set; } = 16;
}
