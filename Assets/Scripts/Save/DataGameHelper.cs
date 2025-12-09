public static class DataGameHelper
{
    public static void UpdatePointsPerLevel(DataGame data, float bestTime, ushort level, int decimals)
    {
        float truncatedBestTime = DataTruncateHelper.TruncateValue(bestTime, decimals);

        if (data._pointsPerLevel.Count > level)
        {
            data._pointsPerLevel[level] = truncatedBestTime;
        }
        else
        {
            while (data._pointsPerLevel.Count < level)
                data._pointsPerLevel.Add(float.MaxValue);

            data._pointsPerLevel.Add(truncatedBestTime);
        }
    }
}
