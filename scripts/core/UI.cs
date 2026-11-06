using Godot;

public partial class UI : Control
{
    public TierableData SelectedData
    {
        get => Tree.Info.CurrentData;
        set => Tree.Info.Set(value);
    }
    public Tierable selection;
    private Vector2 grabOffset = new(5, 5);

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("toggle_controls")) Tree.Menu.ToggleControls();
        if (Input.IsActionJustPressed("apply_template_sabcde")) Tree.Tierlist.ApplyThemeSABCDE();
        if (Input.IsActionJustPressed("apply_template_012345")) Tree.Tierlist.ApplyTheme012345();
        if (Input.IsActionJustPressed("quicksave") && !Tree.Info.Visible) Tree.Filer.QuicksaveFull();
        if (Input.IsActionJustPressed("quicksave") && Tree.Info.Visible) Tree.Filer.QuicksaveTierable();
        if (Input.IsActionJustPressed("add_top") && !Tree.GrabbedTier.Visible) Tree.Tierlist.AddTierAtTop();
        if (Input.IsActionJustPressed("add_bottom") && !Tree.GrabbedTier.Visible) Tree.Tierlist.AddTierAtBottom();
        if (Input.IsActionJustPressed("undo_grab") && Tree.GrabbedTierable.Visible) UndoGrabTierable();
        if (Input.IsActionJustPressed("undo_grab") && Tree.GrabbedTier.Visible) UndoGrabTier();
        if (Input.IsActionJustPressed("remove") && Tree.GrabbedTier.Visible) RemoveGrabbedTier();
        if (Input.IsActionJustPressed("remove") && Tree.GrabbedTierable.Visible) RemoveGrabbedTierable();
        if (Input.IsActionJustPressed("deselect") && Tree.Info.Visible) DeselectTierable();

        if (Tree.GrabbedTierable.Visible)
            Tree.GrabbedTierable.GlobalPosition = GetGlobalMousePosition() + grabOffset;

        if (Tree.GrabbedTier.Visible)
            Tree.GrabbedTier.GlobalPosition = GetGlobalMousePosition() + grabOffset;
    }

    public void GrabTierable(Tierable tierable)
    {
        if (tierable == null || Tree.GrabbedTierable.data != null) return;

        Tree.GrabbedTierable.Init(tierable.data, tierable.Parent);
        Tree.GrabbedTierable.GlobalPosition = GetGlobalMousePosition() + grabOffset;
        Tree.GrabbedTierable.Visible = true;
        tierable.QueueFree();
    }
    public void UndoGrabTierable() => Tree.GrabbedTierable.Parent.InsertGrabbedTierable();
    public void SelectTierable(Tierable tierable)
    {
        if (tierable == null || Tree.GrabbedTierable.data != null) return;

        if (selection != null && selection != tierable) selection.data = SelectedData;

        selection = tierable;
        SelectedData = tierable.data;
    }
    public void DeselectTierable() { selection.data = SelectedData; Tree.Info.Set(null); }

    public void GrabTier(Tier tier)
    {
        if (tier == null || Tree.GrabbedTier.Data != null) return;

        Tree.GrabbedTier.Init(tier.Data, tier.Parent);
        Tree.GrabbedTier.GlobalPosition = GetGlobalMousePosition() + grabOffset;
        Tree.GrabbedTier.Visible = true;
        Tree.Tierlist.TakeFrom(tier.Parent);
        Tree.Tierlist.ShowAll();
    }
    public void UndoGrabTier() => Tree.GrabbedTier.Parent.InsertGrabbedTier();
    public void RemoveGrabbedTier()
    {
        if (Tree.Tierlist.Tiers.Length < 3) return;

        foreach (TierableData d in Tree.GrabbedTier.Data.Tierables) Tree.TierlistPool.AddTierable(d);
        Tree.GrabbedTier.Clear();
        Tree.Tierlist.Rebuild();
        Tree.Tierlist.ShowNonEmpty();
    }
    public void RemoveGrabbedTierable()
    {
        Tree.GrabbedTierable.Clear();
    }
}
