namespace ImpoLib;

public class ImpositionSettings
{
    public int ImpositionMethod { get; set; } = 2; // 1 = Perfect-Bound, 2 = Cut-Stack
    public string InputPath { get; set; }
    public string OutputPath { get; set; }
    public bool DoubleSided { get; set; } = true;
    public int PagesPerSide { get; set; } = 2;
    public float GapBetweenPages { get; set; } = 6; // Fresa (em pontos)
    public int TotalPages { get; set; } = 64;
}
