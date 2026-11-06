using System;

[Serializable]
public class TierData
{
    public string RankText { get; set; }
    public string RankBackgroundColor { get; set; }
    public TierableData[] Tierables { get; set; }

    public TierData(string rankText, string rankColor, TierableData[] tierables)
    {
        RankText = rankText;
        RankBackgroundColor = rankColor;
        Tierables = tierables;
    }
    public TierData()
    {
        RankText = "";
        RankBackgroundColor = "#000";
        Tierables = [];
    }
}
