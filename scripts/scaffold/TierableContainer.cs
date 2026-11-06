using System.Linq;
using Godot;

public partial class TierableContainer : GridContainer
{
    public TierableData[] Data
    {
        get => [.. GetChildren()
            .Cast<Tierable>()
            .Where(tierable => tierable != null)
            .Select(tierable => tierable.data)];
        set
        {
            foreach (Node node in GetChildren()) node.QueueFree();
            foreach (TierableData tierable in value) AddTierable(tierable);
        }
    }

    public void AddTierable(TierableData tierableData)
    {
        if (tierableData == null) return;

        Tierable t = Tree.Packs.GetTierable();
        t.Init(tierableData, this);
        AddChild(t);
    }

    public void InsertGrabbedTierable()
    {
        if (Tree.GrabbedTierable.data != null) AddTierable(Tree.GrabbedTierable.data);
        Tree.GrabbedTierable.Clear();
    }

    public void OnGuiInput_TierableContainer(InputEvent e)
    {
        if (Utils.IsLeftClick(e) && Tree.GrabbedTierable.Visible) InsertGrabbedTierable();
    }
}
