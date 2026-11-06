using Godot;

public partial class MenuPanel : Control
{
    [Export] private Button deleteTierlistButton;
    [Export] private Button saveTierlistButton;
    [Export] private Button loadTierlistButton;
    [Export] private Button createDataButton;
    [Export] private Button loadDataButton;

    public override void _Ready()
    {
        if (deleteTierlistButton == null) LogMaster.LogError("Delete TL is not assigned!", "MenuPanel", "Ready");
        if (saveTierlistButton == null) LogMaster.LogError("Save TL is not assigned!", "MenuPanel", "Ready");
        if (loadTierlistButton == null) LogMaster.LogError("Load TL is not assigned!", "MenuPanel", "Ready");
        if (createDataButton == null) LogMaster.LogError("Create Data is not assigned!", "MenuPanel", "Ready");
        if (loadDataButton == null) LogMaster.LogError("Load Data is not assigned!", "MenuPanel", "Ready");
    }

    public void EnableDeleteTierlistButton() => deleteTierlistButton.Disabled = false;
    public void EnableSaveTierlistButton() => saveTierlistButton.Disabled = false;
    public void EnableLoadTierlistButton() => loadTierlistButton.Disabled = false;
    public void EnableCreateDataButton() => createDataButton.Disabled = false;
    public void EnableLoadDataButton() => loadDataButton.Disabled = false;
    public void DisableDeleteTierlistButton() => deleteTierlistButton.Disabled = true;
    public void DisableSaveTierlistButton() => saveTierlistButton.Disabled = true;
    public void DisableLoadTierlistButton() => loadTierlistButton.Disabled = true;
    public void DisableCreateDataButton() => createDataButton.Disabled = true;
    public void DisableLoadDataButton() => loadDataButton.Disabled = true;

    public void OnPressed_SaveFile() => Tree.Filer.Open(FileAction.SaveFull);
    public void OnPressed_LoadFile() => Tree.Filer.Open(FileAction.LoadFull);
    public void OnPressed_DeleteTierlist() => Tree.Tabs.DeleteCurrentTierlist();
    public void OnPressed_SaveTierlist() => Tree.Filer.Open(FileAction.SaveTierlist);
    public void OnPressed_LoadTierlist() => Tree.Filer.Open(FileAction.LoadTierlist);
    public void OnPressed_CreateData() => Tree.Tabs.ActivePool.AddTierable(new());
    public void OnPressed_LoadData() => Tree.Filer.Open(FileAction.LoadTierable);
}
