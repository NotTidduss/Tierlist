using System.Linq;
using Godot;

public partial class TierlistPanel : PanelContainer
{
    [Signal] public delegate void OnTitleChanged_EventHandler(string newTitle);
    [Export] public LineEdit title;
    [Export] public Tierlist tierlist;
    [Export] public TierableContainer pool;
    public TierlistData Data
    {
        get => new() { Title = title.Text, TierData = tierlist.Data, PoolData = pool.Data };
        set { SetTitle(value.Title); tierlist.Data = value.TierData; pool.Data = value.PoolData; }
    }
    protected Tierable[] Tierables
    {
        get
        {
            Tier[] tiersInTierlist = [.. tierlist.Tiers.Select(tc => tc.Tier).Where(t => t != null)];
            Tierable[] tierablesInTiers = [.. tiersInTierlist.SelectMany(t => t.Tierables)];
            Tierable[] tierablesInPool = [.. pool.GetChildren().Cast<Tierable>()];
            return [.. tierablesInTiers, .. tierablesInPool];
        }
    }

    public override void _Ready()
    {
        if (title == null) LogMaster.LogError("Title is not assigned!", "TierlistPanel", "Ready");
        if (tierlist == null) LogMaster.LogError("Tierlist is not assigned!", "TierlistPanel", "Ready");
        if (pool == null) LogMaster.LogError("TierlistPool is not assigned!", "TierlistPanel", "Ready");
    }


    public void SetTitle(string newTitle) { title.Text = newTitle; OnTextSubmitted_Title(newTitle); }
    public void OnTextSubmitted_Title(string text)
    {
        EmitSignal(SignalName.OnTitleChanged_, text);
        foreach (Tierable tierable in Tierables) tierable.SetOrigin(text);
    }
    public void OnMouseExited_Title() { SetTitle(title.Text); title.ReleaseFocus(); }
}
