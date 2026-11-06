using System;
using Godot;

[Serializable]
public class ImageData
{
    public int Width { get; set; }
    public int Height { get; set; }
    public bool UseMipmaps { get; set; }
    public long Format { get; set; }
    public byte[] Data { get; set; }

    public ImageData()
    {
        Width = 0;
        Height = 0;
        UseMipmaps = false;
        Format = 0L;
        Data = [];
    }
    public ImageData(int width, int height, bool useMipmaps, long format, byte[] data)
    {
        Width = width;
        Height = height;
        UseMipmaps = useMipmaps;
        Format = format;
        Data = data;
    }
    public ImageData(Image image)
    {
        Width = image.GetWidth();
        Height = image.GetHeight();
        UseMipmaps = image.HasMipmaps();
        Format = (long)image.GetFormat();
        Data = image.GetData();
    }
}
