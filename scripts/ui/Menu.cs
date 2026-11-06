using Godot;

public partial class Menu : Control
{
    [Export] private MenuPanel menuPanel;
    [Export] private ControlsPanel controlsPanel;

    public override void _Ready()
    {
        if (menuPanel == null) LogMaster.LogError("Menu Panel is not assigned!", "Menu", "Ready");
        if (controlsPanel == null) LogMaster.LogError("Controls Panel is not assigned!", "Menu", "Ready");
    }


    public void EnableDeleteTierlistButton() => menuPanel.EnableDeleteTierlistButton();
    public void EnableSaveTierlistButton() => menuPanel.EnableSaveTierlistButton();
    public void EnableLoadTierlistButton() => menuPanel.EnableLoadTierlistButton();
    public void EnableCreateDataButton() => menuPanel.EnableCreateDataButton();
    public void EnableLoadDataButton() => menuPanel.EnableLoadDataButton();
    public void DisableDeleteTierlistButton() => menuPanel.DisableDeleteTierlistButton();
    public void DisableSaveTierlistButton() => menuPanel.DisableSaveTierlistButton();
    public void DisableLoadTierlistButton() => menuPanel.DisableLoadTierlistButton();
    public void DisableCreateDataButton() => menuPanel.DisableCreateDataButton();
    public void DisableLoadDataButton() => menuPanel.DisableLoadDataButton();
    public void ToggleControls() => controlsPanel.ToggleVisibility();
}
