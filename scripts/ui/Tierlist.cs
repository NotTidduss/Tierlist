using System.Linq;
using Godot;

public partial class Tierlist : VBoxContainer
{
    public TierContainer[] Tiers => [.. GetChildren().Cast<TierContainer>()];
    public TierData[] Data
    {
        get => [.. Tiers.Select(tier => tier.Data).Where(data => data != null)];
        set { Clean(); Build(value); }
    }
    private static TierData NewTierData => new() { Tierables = [] };
    private long nextTierContainerID = 0;

    public void ShowAll() { foreach (TierContainer tc in Tiers) tc.Visible = true; }
    public void ShowNonEmpty() { foreach (TierContainer tc in Tiers) tc.Visible = tc.Tier != null; }

    public TierData[] CleanupTierlist()
    {
        TierData[] dataBackup = [.. Data];
        Clean();
        return dataBackup;
    }
    public void Rebuild()
    {
        TierData[] cleanTiers = CleanupTierlist();
        Build(cleanTiers);
    }

    private void Clean()
    {
        foreach (TierContainer tc in Tiers)
        {
            RemoveChild(tc);
            tc.QueueFree();
        }
        nextTierContainerID = 0;
    }
    private void Build(TierData[] tiers)
    {
        foreach (TierData data in tiers)
        {
            CreateTierContainer();
            CreateTier(data);
        }
        CreateTierContainer();
        ShowNonEmpty();
    }


    #region Tier Placement

    private TierContainer CreateTierContainer()
    {
        TierContainer tierContainer = Tree.Packs.GetTierContainer();
        tierContainer.ID = nextTierContainerID++;
        AddChild(tierContainer);
        return tierContainer;
    }
    private void CreateTier() => CreateTier(NewTierData);
    protected void CreateTier(TierData data) => CreateTierContainer().AddTier(data);

    private void CreateFirstTier() { CreateTierContainer(); CreateTier(); CreateTierContainer(); }

    public void AddTierAtTop()
    {
        if (Data.Length >= Res.TierLimit) return;

        TierData[] data = CleanupTierlist();
        CreateFirstTier();
        foreach (TierData d in data) { CreateTier(d); CreateTierContainer(); }

        ShowNonEmpty();
    }
    public void AddTierAtBottom()
    {
        if (Data.Length >= Res.TierLimit) return;

        CreateTier();
        CreateTierContainer();

        ShowNonEmpty();
    }

    public void TakeFrom(TierContainer oldParent)
    {
        TierContainer predecessor = Tiers.FirstOrDefault(tc => tc.ID == oldParent.ID - 1);
        predecessor?.QueueFree();

        TierContainer successor = Tiers.FirstOrDefault(tc => tc.ID == oldParent.ID + 1);
        successor?.QueueFree();

        oldParent.Clear();
    }

    #endregion

    #region Theme

    public void ApplyThemeSABCDE() => ApplyTheme(Utils.TierlistTemplate);
    public void ApplyTheme012345() => ApplyTheme(Utils.NumbersTemplate);
    private void ApplyTheme(TierData[] theme)
    {
        Tier[] currentData = [.. Tiers.Where(tc => tc.Tier != null).Select(tc => tc.Tier)];
        if (currentData.Length != theme.Length) return;

        for (int i = 0; i < theme.Length; i++)
            currentData[i].SetRank(theme[i].RankText, theme[i].RankBackgroundColor);
    }

    #endregion
}
