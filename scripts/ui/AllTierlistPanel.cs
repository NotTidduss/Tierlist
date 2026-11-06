using System.Linq;
using Godot;

public partial class AllTierlistPanel : TierlistPanel
{
    [Export] private Control invalidFallback;

    public void Clear() => tierlist.Data = [];
    public void Set()
    {
        invalidFallback.Visible = true;

        TierData[][] anyCurrentTiers = [.. Tree.Tabs.Data.Select(tierlistData => tierlistData.TierData)];
        if (!AreTiersValid(anyCurrentTiers)) return;

        invalidFallback.Visible = false;
        BuildTiers(anyCurrentTiers);
    }


    private bool AreTiersValid(TierData[][] tiers)
    {
        if (tiers.Length < 2) return false;

        TierData[] matchTier = tiers[0];
        for (int i = 1; i < tiers.Length; i++)
        {
            TierData[] compareTier = tiers[i];
            if (matchTier.Length != compareTier.Length) return false;

            for (int j = 0; j < compareTier.Length; j++)
                if (matchTier[j].RankText != compareTier[j].RankText)
                    return false;
        }

        return true;
    }

    private void BuildTiers(TierData[][] allData)
    {
        TierData[] filledTiers = GetTemplate(allData[0]);

        foreach (TierData[] tierlist in allData)
            for (int i = 0; i < tierlist.Length; i++)
                filledTiers[i].Tierables = [.. filledTiers[i].Tierables.Concat(tierlist[i].Tierables)];

        tierlist.Data = filledTiers;
        foreach (Tierable tierable in Tierables) tierable.ShowOrigin();
    }
    private TierData[] GetTemplate(TierData[] sample) => [..
        sample.Select(t => new TierData() {
            RankText = t.RankText, RankBackgroundColor = t.RankBackgroundColor, Tierables = []
        })];
}
