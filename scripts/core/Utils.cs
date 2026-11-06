using System;
using System.Linq;
using Godot;

public static class Utils
{
    public static bool IsLeftClick(InputEvent e)
        => e is InputEventMouseButton mouse
        && mouse.ButtonIndex == MouseButton.Left
        && mouse.IsPressed();
    public static bool IsRightClick(InputEvent e)
        => e is InputEventMouseButton mouse
        && mouse.ButtonIndex == MouseButton.Right
        && mouse.IsPressed();

    public static float Limit(float value, float min, float max)
        => value < min ? min
        : value > max ? max
        : value;

    public static string Abbriviate(string s)
        => string.Concat(s.Replace("#", "").Capitalize().Split(" ").Select(AsFirstLetterAndNumbers));
    private static string AsFirstLetterAndNumbers(string s)
        => $"{s[0]}{string.Concat(s[1..].Where(IsNumber))}";
    private static bool IsNumber(char c) => "0123456789".Contains(c);

    public static TierData[] TierlistTemplate => [
        new TierData() {  RankText = "S", RankBackgroundColor = "#e93535", Tierables = []},
        new TierData() {  RankText = "A", RankBackgroundColor = "#dda707", Tierables = []},
        new TierData() {  RankText = "B", RankBackgroundColor = "#e2e233", Tierables = []},
        new TierData() {  RankText = "C", RankBackgroundColor = "#44aa66", Tierables = []},
        new TierData() {  RankText = "D", RankBackgroundColor = "#4790c0", Tierables = []},
        new TierData() {  RankText = "E", RankBackgroundColor = "#887eaa", Tierables = []},
    ];

    public static TierData[] NumbersTemplate => [
        new TierData() {  RankText = "5", RankBackgroundColor = "#ebf7fa", Tierables = []},
        new TierData() {  RankText = "4", RankBackgroundColor = "#dae4f0", Tierables = []},
        new TierData() {  RankText = "3", RankBackgroundColor = "#c3c5cc", Tierables = []},
        new TierData() {  RankText = "2", RankBackgroundColor = "#9899aa", Tierables = []},
        new TierData() {  RankText = "1", RankBackgroundColor = "#616271", Tierables = []},
        new TierData() {  RankText = "0", RankBackgroundColor = "#49484d", Tierables = []},
    ];


    #region Image Loading

    public static ImageTexture GetTexture(ImageData d)
    {
        Image img = new();
        img.SetData(d.Width, d.Height, d.UseMipmaps, (Image.Format)d.Format, d.Data);
        return ImageTexture.CreateFromImage(img);
    }

    public static ImageData LoadImageDataByFilePath(string path)
    {
        if (!FileAccess.FileExists(path))
            return GetErrorImage($"Image file does not exist: {path}");

        using FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        if (file == null)
            return GetErrorImage($"Cannot open file for reading: {path}");

        long imageLength = (long)file.GetLength();
        byte[] imageBytes = file.GetBuffer(imageLength);

        return LoadImageData(imageBytes);
    }
    public static ImageData GetErrorImage(string errorMessage)
    {
        LogMaster.LogError(errorMessage, "Utils", "GetErrorTexture");
        Image errorImage = GD.Load<CompressedTexture2D>(Res.ErrorTexturePath).GetImage();
        return new(errorImage);
    }
    public static ImageData GetDefaultImage()
    {
        Image defaultImage = GD.Load<CompressedTexture2D>(string.Format(Res.DefaultTexturePath, Tree.RNG.RandiRange(0, 9))).GetImage();
        return new(defaultImage);
    }
    public static ImageData LoadImageData(byte[] imageBytes)
    {
        if (imageBytes == null || imageBytes == Array.Empty<byte>())
            return GetErrorImage("Empty image data.");

        Image image = new();
        Error error = GetFormatError(imageBytes, FindFormat(imageBytes), ref image);

        if (error != Error.Ok)
            return GetErrorImage("Invalid image data.");

        return new ImageData(image);
    }
    private static ImageFormat FindFormat(byte[] imageBytes, int formatIndex = 0)
    {
        if (formatIndex >= (int)ImageFormat.INVALID) return (ImageFormat)formatIndex;

        Image image = new();
        Error error = GetFormatError(imageBytes, (ImageFormat)formatIndex, ref image);

        return error == Error.Ok ? (ImageFormat)formatIndex : FindFormat(imageBytes, formatIndex + 1);
    }
    // loading in the wrong format throws errors, but that's expected here and therefore fine.
    private static Error GetFormatError(byte[] buffer, ImageFormat format, ref Image img)
        => format switch
        {
            ImageFormat.PNG => img.LoadPngFromBuffer(buffer),
            ImageFormat.JPG => img.LoadJpgFromBuffer(buffer),
            ImageFormat.WEB => img.LoadWebpFromBuffer(buffer),
            ImageFormat.TGA => img.LoadTgaFromBuffer(buffer),
            ImageFormat.BMP => img.LoadBmpFromBuffer(buffer),
            ImageFormat.KTX => img.LoadKtxFromBuffer(buffer),
            ImageFormat.DDS => img.LoadDdsFromBuffer(buffer),
            ImageFormat.EXR => img.LoadExrFromBuffer(buffer),
            ImageFormat.SVG => img.LoadSvgFromBuffer(buffer),
            _ => Error.InvalidData
        };

    #endregion
}