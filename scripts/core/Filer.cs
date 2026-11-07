using System;
using System.Text.Json;
using Godot;

public partial class Filer : FileDialog
{
    private readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true };
    private string _lastFullPath;
    private string _lastTierablePath;
    private FileAction _pendingFileAction = FileAction.None;


    public void Open(FileAction action)
    {
        _pendingFileAction = action;
        SetFilterByAction(action);
        FileMode = GetFileModeByAction(action);
        CurrentPath = _lastFullPath;
        PopupCenteredRatio(0.7f);
    }

    public void QuicksaveFull()
    {
        if (string.IsNullOrWhiteSpace(_lastFullPath)
        || !FileAccess.FileExists(_lastFullPath))
            return;

        SaveFull(_lastFullPath);
    }
    public void QuicksaveTierable()
    {
        if (string.IsNullOrWhiteSpace(_lastTierablePath)
        || !FileAccess.FileExists(_lastTierablePath))
            return;

        SaveTierable(_lastTierablePath);
    }

    private void SaveFull(string path) => Save(Tree.Tabs.Data, path);
    private void SaveTierlist(string path) => Save(Tree.Tabs.ActiveTierlistPanel.Data, path);
    private void SaveTierable(string path) => Save(Tree.UI.SelectedData, path);
    private void Save<T>(T data, string path)
    {
        using FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
        if (file == null)
        {
            LogMaster.LogError($"Cannot open project file for writing: {path}", "Filer", "Save");
            return;
        }

        string json = JsonSerializer.Serialize(data, jsonOptions);
        file.StoreString(json);
    }
    private void LoadFull(string path) => Tree.Tabs.Build(Load<TierlistData[]>(path));
    private void LoadTierlist(string path) => Tree.Tabs.ActiveTierlistPanel.Data = Load<TierlistData>(path);
    private void LoadTierable(string path) => Tree.TierlistPool.AddTierable(Load<TierableData>(path));
    private T Load<T>(string path)
    {
        if (!FileAccess.FileExists(path))
        {
            LogMaster.LogError($"Save file does not exist: {path}", "Filer", "Load");
            return default;
        }

        using FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        if (file == null)
        {
            LogMaster.LogError($"Cannot open file for reading: {path}", "Filer", "Load");
            return default;
        }

        string json = file.GetAsText();
        T loaded;
        try
        {
            loaded = JsonSerializer.Deserialize<T>(json, jsonOptions);
        }
        catch (Exception e)
        {
            LogMaster.LogError($"Failed to deserialize project: {e.Message}", "Filer", "Load");
            return default;
        }

        if (loaded is null)
        {
            LogMaster.LogError("Loaded project data is null.", "Filer", "Load");
            return default;
        }

        return loaded;
    }

    private void SetFilterByAction(FileAction action)
    {
        Filters = [];
        switch (action)
        {
            case FileAction.SaveFull:
            case FileAction.LoadFull:
                AddFilter("*.save ; Tierlist Project Files");
                break;
            case FileAction.SaveTierlist:
            case FileAction.LoadTierlist:
                AddFilter("*.tierlist ; Tierlist Files");
                break;
            case FileAction.SaveTierable:
            case FileAction.LoadTierable:
                AddFilter("*.tierdata ; Tierlist Data Files");
                break;
        }
    }
    private FileModeEnum GetFileModeByAction(FileAction action) => action switch
    {
        FileAction.SaveFull => FileModeEnum.SaveFile,
        FileAction.SaveTierlist => FileModeEnum.SaveFile,
        FileAction.SaveTierable => FileModeEnum.SaveFile,
        FileAction.LoadFull => FileModeEnum.OpenFile,
        FileAction.LoadTierlist => FileModeEnum.OpenFile,
        FileAction.LoadTierable => FileModeEnum.OpenFile,
        _ => FileModeEnum.OpenAny,
    };


    public void OnCanceled() => _pendingFileAction = FileAction.None;
    public void OnFileSelected(string path)
    {
        switch (_pendingFileAction)
        {
            case FileAction.SaveFull: _lastFullPath = path; SaveFull(path); break;
            case FileAction.LoadFull: _lastFullPath = path; LoadFull(path); break;
            case FileAction.SaveTierlist: SaveTierlist(path); break;
            case FileAction.LoadTierlist: LoadTierlist(path); break;
            case FileAction.SaveTierable: _lastTierablePath = path; SaveTierable(path); break;
            case FileAction.LoadTierable: _lastTierablePath = path; LoadTierable(path); break;
        }
        _pendingFileAction = FileAction.None;
    }
}