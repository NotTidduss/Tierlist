using Godot;

public partial class Tierable : Control
{
    [Export] public TextureRect textureRect;
    [Export] public Label originLabel;

    public TierableContainer Parent { get; private set; }
    public TierableData data;

    public override void _Ready()
    {
        if (textureRect == null) LogMaster.LogError("TextureRect is not assigned!", "Tierable", "Ready");
        if (originLabel == null) LogMaster.LogError("OriginPanel is not assigned!", "Tierable", "Ready");
    }
    public void Init(TierableData tierableData, TierableContainer parent)
    {
        data = tierableData;
        Parent = parent;

        textureRect.Texture = Utils.GetTexture(data.MainImage);
        originLabel.Visible = false;
    }
    public void Clear() { data = null; Parent = null; Visible = false; }
    public void SetOrigin(string origin) => data.Origin = origin;
    public void ShowOrigin() { originLabel.Text = Utils.Abbriviate(data.Origin); originLabel.Visible = true; }


    public void OnGuiInput_Tierable(InputEvent e)
    {
        if (Utils.IsLeftClick(e)) Tree.UI.GrabTierable(this);
        if (Utils.IsRightClick(e)) Tree.UI.SelectTierable(this);
    }

    public void SendTextureRequest(string url)
    {
        HttpRequest request = new();
        AddChild(request);
        request.RequestCompleted += OnTextureRequestCompleted;

        Error error = request.Request(url);
        if (error != Error.Ok)
            LogMaster.LogError($"Tierable {data.Title}: An error occurred in the HTTP request: {error}", "Tierable", "SendTextureRequest");
    }
    private void OnTextureRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        if (result != (long)HttpRequest.Result.Success)
        {
            LogMaster.LogError($"Tierable {data.Title}: Image couldn't be downloaded. Try a different image.", "Tierable", "OnTextureRequestCompleted");
            return;
        }

        ImageData loadedImageData = Utils.LoadImageData(body);
        data.MainImage = loadedImageData;
        Tree.Info.SetMainImage(loadedImageData);
        textureRect.Texture = Utils.GetTexture(loadedImageData);
    }
}
