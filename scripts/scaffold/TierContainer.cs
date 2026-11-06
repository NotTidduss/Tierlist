using Godot;

public partial class TierContainer : Control
{
    public long ID { get; set; }
    public Tier Tier { get; private set; }
    public TierData Data => Tier?.Data;

    public void AddTier(TierData data)
    {
        if (Tier != null || data == null) return;

        Tier t = Tree.Packs.GetTier();
        t.Init(data, this);
        AddChild(t);
        Tier = t;
    }

    public void InsertGrabbedTier()
    {
        if (Tree.GrabbedTier.Data != null) AddTier(Tree.GrabbedTier.Data);
        Tree.GrabbedTier.Clear();
        Tree.Tierlist.Rebuild();
        Tree.Tierlist.ShowNonEmpty();
    }

    public void Clear()
    {
        Tier.QueueFree();
        Tier = null;
    }

    public void OnGuiInput_TierContainer(InputEvent e)
    {
        if (Utils.IsLeftClick(e) && Tree.GrabbedTier.Visible) InsertGrabbedTier();
    }
}
