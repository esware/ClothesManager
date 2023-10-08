using UnityEngine;

namespace EWGames.Dev.Scripts
{
    public enum ColorCode
    {
        Red,
        Green,
        Blue,
        Yellow,
    }

    public class ColorMapper
    {
        public static Color GetColorFromCode(ColorCode code)
        {
            switch (code)
            {
                case ColorCode.Red:
                    return Color.red;
                case ColorCode.Green:
                    return Color.green;
                case ColorCode.Blue:
                    return Color.blue;
                case ColorCode.Yellow:
                    return Color.yellow;
                default:
                    return Color.white;
            }
        }
    }

}