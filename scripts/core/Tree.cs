using Godot;

public partial class Tree : Node
{
    [Export] private Packs packs;
    [Export] private UI ui;
    [Export] private Tabs tabs;
    [Export] private Menu menu;
    [Export] private Info info;
    [Export] private Filer filer;
    [Export] private Tierable grabbedTierable;
    [Export] private Tier grabbedTier;

    public static Packs Packs;
    public static UI UI;
    public static Tabs Tabs;
    public static Menu Menu;
    public static Info Info;
    public static Filer Filer;
    public static Tierable GrabbedTierable;
    public static Tier GrabbedTier;

    public static RandomNumberGenerator RNG;
    public static Tierlist Tierlist => Tabs.ActiveTierlist;
    public static TierableContainer TierlistPool => Tabs.ActivePool;

    public override void _Ready()
    {
        OS.LowProcessorUsageMode = true;
        RNG = new();

        if (packs == null) LogMaster.LogError("Packs is not assigned!", "Tree", "Ready");
        if (ui == null) LogMaster.LogError("UI is not assigned!", "Tree", "Ready");
        if (tabs == null) LogMaster.LogError("Tabs is not assigned!", "Tree", "Ready");
        if (menu == null) LogMaster.LogError("Menu is not assigned!", "Tree", "Ready");
        if (info == null) LogMaster.LogError("Info is not assigned!", "Tree", "Ready");
        if (filer == null) LogMaster.LogError("Filer is not assigned!", "Tree", "Ready");
        if (grabbedTierable == null) LogMaster.LogError("GrabbedTierable is not assigned!", "Tree", "Ready");
        if (grabbedTier == null) LogMaster.LogError("GrabbedTier is not assigned!", "Tree", "Ready");

        Packs = packs;
        UI = ui;
        Tabs = tabs;
        Menu = menu;
        Info = info;
        Filer = filer;
        GrabbedTierable = grabbedTierable;
        GrabbedTier = grabbedTier;
    }
}
