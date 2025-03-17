namespace ImpoIndexerConsole.Extensions;

public static class UnitConversion
{
    public static float PT(this float valor)
    {
        return valor * 2.834645669f;
    }
    public static float MM(this float valor)
    {
        return Convert.ToSingle(Math.Round(valor * 0.352777778, 0));
    }
}
