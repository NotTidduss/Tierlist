using System;

[Serializable]
public class TierlistData
{
    public string Title { get; set; }
    public TierData[] TierData { get; set; }
    public TierableData[] PoolData { get; set; }

    public TierlistData(string title, TierData[] tierData, TierableData[] poolData)
    {
        Title = title;
        TierData = tierData;
        PoolData = poolData;
    }
    public TierlistData()
    {
        Title = "";
        TierData = [];
        PoolData = [];
    }
}
