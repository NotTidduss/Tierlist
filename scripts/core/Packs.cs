using Godot;

public partial class Packs : Node
{
    [Export] private PackedScene packedTierable;
    [Export] private PackedScene packedTierContainer;
    [Export] private PackedScene packedTier;
    [Export] private PackedScene packedTierlistPanel;
    [Export] private PackedScene packedInfoExtra;

    public override void _Ready()
    {
        if (packedTierable == null) LogMaster.LogError("Tierable is not assigned!", "Packs", "Ready");
        if (packedTierContainer == null) LogMaster.LogError("TierContainer is not assigned!", "Packs", "Ready");
        if (packedTier == null) LogMaster.LogError("Tier is not assigned!", "Packs", "Ready");
        if (packedTierlistPanel == null) LogMaster.LogError("TierListPanel is not assigned!", "Packs", "Ready");
        if (packedInfoExtra == null) LogMaster.LogError("InfoExtra is not assigned!", "Packs", "Ready");
    }

    public Tierable GetTierable() => packedTierable.Instantiate<Tierable>();
    public TierContainer GetTierContainer() => packedTierContainer.Instantiate<TierContainer>();
    public Tier GetTier() => packedTier.Instantiate<Tier>();
    public TierlistPanel GetTierlistPanel() => packedTierlistPanel.Instantiate<TierlistPanel>();
    public InfoExtra GetInfoExtra() => packedInfoExtra.Instantiate<InfoExtra>();
}
