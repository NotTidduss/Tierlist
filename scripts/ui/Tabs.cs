using System.Linq;
using Godot;

public partial class Tabs : TabContainer
{
    [Export] private Control newTab;
    [Export] private AllTierlistPanel allTab;

    public TierlistPanel ActiveTierlistPanel => (TierlistPanel)GetCurrentTabControl();
    public Tierlist ActiveTierlist => ActiveTierlistPanel.tierlist;
    public TierableContainer ActivePool => ActiveTierlistPanel.pool;
    public int NewTabIndex => GetTabCount() - 2;
    public int AllTabIndex => GetTabCount() - 1;
    public Node[] TierlistNodes => [.. GetChildren().Where(node => node != newTab && node != allTab)];
    public TierlistData[] Data => [.. TierlistNodes
        .Cast<TierlistPanel>()
        .Where(panel => panel != null)
        .Select(panel => panel.Data)];

    public override void _Ready()
    {
        ActiveTierlistPanel.OnTitleChanged_ += OnTitleChanged_TierlistPanel;
        ActiveTierlistPanel.Data = new() { Title = "Title", TierData = Utils.NumbersTemplate, PoolData = [] };
    }


    private void ShowAllTab() => SetTabHidden(AllTabIndex, false);
    private void HideAllTab() => SetTabHidden(AllTabIndex, true);

    private void AddPanel(int index, TierlistData data)
    {
        if (GetTabCount() == 3) { ShowAllTab(); Tree.Menu.EnableDeleteTierlistButton(); }

        TierlistPanel newTierlistPanel = Tree.Packs.GetTierlistPanel();
        AddChild(newTierlistPanel);
        MoveChild(newTierlistPanel, index);
        newTierlistPanel.OnTitleChanged_ += OnTitleChanged_TierlistPanel;
        newTierlistPanel.Data = data;
    }
    private void RemovePanel(int index)
    {
        if (index == CurrentTab) CurrentTab--;
        if (GetTabCount() == 4) { HideAllTab(); Tree.Menu.DisableDeleteTierlistButton(); }

        GetChild(index).QueueFree();
    }
    public void DeleteCurrentTierlist() => RemovePanel(CurrentTab);

    public void Build(TierlistData[] tierlists)
    {
        if (tierlists == null || tierlists.Length == 0)
        {
            LogMaster.LogWarning("Given data was null or empty!", "Tabs", "Build");
            return;
        }

        foreach (Node node in TierlistNodes) node.QueueFree();
        for (int i = 0; i < tierlists.Length; i++)
        {
            AddPanel(i, tierlists[i]);
            SetTabTitle(i, tierlists[i].Title);
        }

        CurrentTab = 0;
        if (tierlists.Length == 1) { HideAllTab(); Tree.Menu.DisableDeleteTierlistButton(); }
    }


    public void OnTabChanged(int tabIndex)
    {
        if (tabIndex == AllTabIndex)
        {
            Tree.Menu.DisableDeleteTierlistButton();
            Tree.Menu.DisableSaveTierlistButton();
            Tree.Menu.DisableLoadTierlistButton();
            Tree.Menu.DisableCreateDataButton();
            Tree.Menu.DisableLoadDataButton();
        }
        else
        {
            Tree.Menu.EnableDeleteTierlistButton();
            Tree.Menu.EnableSaveTierlistButton();
            Tree.Menu.EnableLoadTierlistButton();
            Tree.Menu.EnableCreateDataButton();
            Tree.Menu.EnableLoadDataButton();
        }
    }

    public void OnVisibilityChanged_NewTab()
    {
        if (!newTab.Visible) return;

        int targetIndex = NewTabIndex;
        TierData initialTier = new() { RankText = "A", RankBackgroundColor = "#FAFAFA", Tierables = [] };
        TierlistData data = new() { Title = $"Tierlist #{targetIndex}", TierData = [initialTier], PoolData = [] };
        AddPanel(targetIndex, data);
        SelectPreviousAvailable();
    }
    public void OnVisibilityChanged_AllTab()
    {
        if (allTab.Visible)
        {
            foreach (TierlistPanel tierlistPanel in TierlistNodes.Cast<TierlistPanel>())
                tierlistPanel.tierlist.Validate();

            allTab.Set();
        }
        else allTab.Clear();
    }

    // To avoid setting a title on the NewTab when adding panels, CurrentTab is reduced if needed.
    private int SafeCurrentTab => CurrentTab == NewTabIndex ? CurrentTab - 1 : CurrentTab;
    public void OnTitleChanged_TierlistPanel(string newTitle) => SetTabTitle(SafeCurrentTab, newTitle);
}
