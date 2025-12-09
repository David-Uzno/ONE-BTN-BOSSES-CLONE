using System;
using System.Globalization;
using System.Text;

public static class DataTruncateHelper
{
    // Trunca los valores de puntos por nivel en gameData al número de decimales especificado
    public static void TruncatePointsPerLevel(DataGame gameData, int decimals)
    {
        if (gameData == null || gameData._pointsPerLevel == null) return;
        for (int i = 0; i < gameData._pointsPerLevel.Count; i++)
            gameData._pointsPerLevel[i] = TruncateValue(gameData._pointsPerLevel[i], decimals);
    }

    // Trunca un valor flotante al número de decimales especificado
    public static float TruncateValue(float value, int decimals)
    {
        decimals = Math.Max(0, decimals);
        decimal factor = GetDecimalFactor(decimals);
        decimal truncated = Math.Truncate((decimal)value * factor) / factor;
        return (float)truncated;
    }

    // Construye un JSON con los valores truncados de puntos por nivel
    public static string BuildTruncatedJson(DataGame gameData, int decimals)
    {
        if (gameData == null || gameData._pointsPerLevel == null)
            return "{\"_pointsPerLevel\":[]}";

        var jsonStringBuilder = new StringBuilder();
        decimals = Math.Max(0, decimals);

        jsonStringBuilder.Append("{\"_pointsPerLevel\":[");
        for (int i = 0; i < gameData._pointsPerLevel.Count; i++)
        {
            // Truncar el valor antes de agregarlo al JSON
            decimal truncated = Math.Truncate((decimal)gameData._pointsPerLevel[i] * GetDecimalFactor(decimals)) / GetDecimalFactor(decimals);
            jsonStringBuilder.Append(truncated.ToString($"F{decimals}", CultureInfo.InvariantCulture));
            if (i < gameData._pointsPerLevel.Count - 1)
                jsonStringBuilder.Append(',');
        }
        jsonStringBuilder.Append("]}");

        return jsonStringBuilder.ToString();
    }

    // Calcula el factor de truncamiento según la cantidad de decimales
    private static decimal GetDecimalFactor(int decimals)
    {
        decimal factor = 1m;
        for (int i = 0; i < decimals; i++)
            factor *= 10m;
        return factor;
    }
}
