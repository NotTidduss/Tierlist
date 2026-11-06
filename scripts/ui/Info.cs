using System.Linq;
using Godot;

public partial class Info : Control
{
    [Export] private LineEdit title;
    [Export] private TextureRect mainImage;
    [Export] private InfoImageInput mainImageInput;
    [Export] private TextEdit description;
    [Export] private Control extraImageContainer;
    [Export] private InfoImageInput extraImageInput;

    public TierableData CurrentData => new()
    {
        Title = title.Text,
        Blurb = description.Text,
        MainImage = new(mainImage.Texture.GetImage()),
        ExtraImages = [.. extraImageContainer.GetChildren().Cast<TextureRect>().Select(t => new ImageData(t.Texture.GetImage()))]
    };
    public bool IsExtraFull => extraImageContainer.GetChildCount() >= 3;

    public override void _Ready()
    {
        if (title == null) LogMaster.LogError("Title is not assigned!", "Info", "Ready");
        if (mainImage == null) LogMaster.LogError("Main Image is not assigned!", "Info", "Ready");
        if (mainImageInput == null) LogMaster.LogError("Main Image Input is not assigned!", "Info", "Ready");
        if (description == null) LogMaster.LogError("Description is not assigned!", "Info", "Ready");
        if (extraImageContainer == null) LogMaster.LogError("Extra Image Container is not assigned!", "Info", "Ready");
        if (extraImageInput == null) LogMaster.LogError("Extra Image Input is not assigned!", "Info", "Ready");
    }


    public void Set(TierableData data)
    {
        Visible = data != null;
        Tree.Menu.Visible = data == null;
        if (data == null) return;

        SetTitle(data.Title ?? "");
        SetDescription(data.Blurb ?? "");
        SetMainImagePath(data.MainImagePath ?? "");
        SetMainImage(data.MainImage);
        SetExtraImages(data.ExtraImages ?? []);
    }

    public void SetTitle(string text) => title.Text = text;
    public void SetDescription(string text) => description.Text = text;
    public void SetMainImagePath(string text) => mainImageInput.Path = text;
    public void SetMainImage(ImageData imageData)
    {
        mainImage.Visible = imageData != null;
        mainImageInput.Visible = imageData == null;
        if (imageData == null) return;

        mainImage.Texture = Utils.GetTexture(imageData);
    }
    public void SetExtraImages(ImageData[] extraImages)
    {
        EnableExtraInput();
        extraImageContainer.GetChildren().ToList().ForEach(n => n.QueueFree());
        extraImages?.ToList().ForEach(AddExtraImage);
    }
    public void EnableExtraInput() => extraImageInput.Visible = true;


    private void AddExtraImage(ImageData extraImageData)
    {
        if (extraImageData == null) return;

        InfoExtra extra = Tree.Packs.GetInfoExtra();
        extra.Texture = Utils.GetTexture(extraImageData);
        extraImageContainer.AddChild(extra);
        extraImageInput.Visible = !IsExtraFull;
    }
    private void SendExtraTextureRequest(string url)
    {
        HttpRequest request = new();
        AddChild(request);
        request.RequestCompleted += OnTextureRequestCompleted;

        Error error = request.Request(url);
        if (error != Error.Ok)
            LogMaster.LogError($"Extra request failed: {error}", "Info", "SendExtraTextureRequest");
    }
    private void OnTextureRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        if (result != (long)HttpRequest.Result.Success)
        {
            LogMaster.LogError($"Image couldn't be downloaded: {responseCode}", "Info", "OnTextureRequestCompleted");
            return;
        }

        AddExtraImage(Utils.LoadImageData(body));
    }


    public void OnPressed_Save() => Tree.Filer.Open(FileAction.SaveTierable);
    public void OnPressed_Close() => Tree.UI.DeselectTierable();
    public void OnMouseExited_Title() => title.ReleaseFocus();
    public void OnMouseExited_Description() => description.ReleaseFocus();
    public void OnGuiInput_MainImage(InputEvent e) { if (Utils.IsRightClick(e)) SetMainImage(null); }
    public void OnInputPressed_MainImageInput(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return;

        path = path.ToLower().Trim(' ', '"');
        if (path.StartsWith("http"))
        {
            Tree.UI.selection.SendTextureRequest(path);
        }
        else
        {
            ImageData loadedImage = Utils.LoadImageDataByFilePath(path);
            SetMainImage(loadedImage);
            Tree.UI.selection.textureRect.Texture = Utils.GetTexture(loadedImage);
        }
    }
    public void OnInputPressed_ExtraImageInput(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return;
        path = path.ToLower().Trim(' ', '"');
        if (path.StartsWith("http")) SendExtraTextureRequest(path);
        else AddExtraImage(Utils.LoadImageDataByFilePath(path));
    }
}
