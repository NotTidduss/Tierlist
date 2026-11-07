using System.Linq;
using Godot;

public partial class Tier : Control
{
    [Export] private TierHandle rankHandle;
    [Export] private ColorPickerButton rankColorPicker;
    [Export] private TextureRect rankBackground;
    [Export] private LineEdit rankText;
    [Export] private TierableContainer tierableContainer;

    public TierContainer Parent { get; private set; }
    public Tierable[] Tierables => [.. tierableContainer.GetChildren().Cast<Tierable>()];
    public TierData Data
    {
        get => new(rankText.Text, rankBackground.Modulate.ToHtml(), [.. Tierables.Select(t => t.data)]);
        set
        {
            SetRank(value?.RankText, value?.RankBackgroundColor);
            tierableContainer.Data = [];
            foreach (TierableData tierableData in value?.Tierables ?? [])
                tierableContainer.AddTierable(tierableData);
        }
    }

    public void Init(TierData data, TierContainer parent)
    {
        Parent = parent;
        Data = data;
    }
    public void SetRank(string text, string color)
    {
        rankText.Text = text;
        rankColorPicker.Color = Color.FromHtml(color);
        rankBackground.Modulate = Color.FromHtml(color);
    }
    public void Clear() { Data = null; Parent = null; Visible = false; }

    private TierableData AsData(Node n) => ((Tierable)n).data;

    private void ToggleColorPicker()
    {
        if (rankHandle.Visible)
        {
            rankHandle.Visible = false;
            rankColorPicker.Visible = true;
        }
        else
        {
            rankHandle.Visible = true;
            rankColorPicker.Visible = false;
        }
    }

    public void OnMouseEnter_Handle() => rankHandle.FadeIn();
    public void OnMouseExited_Handle() => rankHandle.FadeOut();
    public void OnGuiInput_Handle(InputEvent e) { if (Utils.IsLeftClick(e)) Tree.UI.GrabTier(this); }

    public void OnGuiInput_Rank(InputEvent e) { if (Utils.IsRightClick(e)) ToggleColorPicker(); }

    public void OnColorChanged_ColorPicker(Color c) => rankBackground.Modulate = c;
    public void OnPopupClosed_ColorPicker() { rankHandle.Visible = true; rankColorPicker.Visible = false; }

    public void OnMouseExited_RankText() => rankText.ReleaseFocus();
    public void OnGuiInput_RankText(InputEvent e) { if (Utils.IsRightClick(e)) ToggleColorPicker(); }
}
