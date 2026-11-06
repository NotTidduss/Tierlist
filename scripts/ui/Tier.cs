using System.Linq;
using Godot;

public partial class Tier : Control
{
    [Export] private TierHandle rankHandle;
    [Export] private ColorPickerButton rankColorPicker;
    [Export] private TextureRect rankBackground;
    [Export] private LineEdit rankText;
    [Export] private TierableContainer tierableContainer;

    public TierData Data { get; private set; }
    public TierContainer Parent { get; private set; }
    public Tierable[] Tierables => [.. tierableContainer.GetChildren().Cast<Tierable>()];

    public void Init(TierData data, TierContainer parent)
    {
        Data = data;
        Parent = parent;

        SetRank(Data.RankText, Data.RankBackgroundColor);

        foreach (TierableData tierableData in Data.Tierables)
            tierableContainer.AddTierable(tierableData);
    }
    public void SetRank(string text, string color)
    {
        Data.RankText = text;
        rankText.Text = text;
        Data.RankBackgroundColor = color;
        rankColorPicker.Color = Color.FromHtml(color);
        rankBackground.Modulate = Color.FromHtml(color);
    }
    public void Clear() { Data = null; Parent = null; Visible = false; }

    public void Validate() => Data.Tierables = [.. tierableContainer.GetChildren().Select(AsData)];
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
    public void OnGuiInput_Handle(InputEvent e)
    {
        if (Utils.IsLeftClick(e))
        {
            Validate();
            Tree.UI.GrabTier(this);
        }
    }

    public void OnGuiInput_Rank(InputEvent e) { if (Utils.IsRightClick(e)) ToggleColorPicker(); }

    public void OnColorChanged_ColorPicker(Color c)
    {
        rankBackground.Modulate = c;
        Data.RankBackgroundColor = c.ToHtml();
    }
    public void OnPopupClosed_ColorPicker()
    {
        rankHandle.Visible = true;
        rankColorPicker.Visible = false;
    }

    public void OnTextChanged_RankText(string t) => Data.RankText = t;
    public void OnMouseExited_RankText() => rankText.ReleaseFocus();
    public void OnGuiInput_RankText(InputEvent e) { if (Utils.IsRightClick(e)) ToggleColorPicker(); }
}
