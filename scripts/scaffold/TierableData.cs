using System;

[Serializable]
public class TierableData
{
    public string Title { get; set; }
    public string MainImagePath { get; set; }
    public string Blurb { get; set; }
    public ImageData MainImage { get; set; }
    public ImageData[] ExtraImages { get; set; }
    public string Origin { get; set; }

    public TierableData(string title, string path, string blurb, ImageData main, ImageData[] extra, string origin)
    {
        Title = title;
        MainImagePath = path;
        Blurb = blurb;
        MainImage = main;
        ExtraImages = extra;
        Origin = origin;
    }
    public TierableData()
    {
        Title = "";
        MainImagePath = "";
        Blurb = "";
        MainImage = Utils.GetDefaultImage();
        ExtraImages = [];
        Origin = Tree.Tabs.ActiveTierlistPanel.title.Text;
    }
}
