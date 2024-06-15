namespace DonkeyLive.Front.Helpers;

public class ColorHelper
{
    static string[] _colors = {"info", "success", "warning", "danger"};
    public static string GetColor(int index)
    {
        return _colors[index % _colors.Length];
    }
}
