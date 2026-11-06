using Godot;

public partial class InfoExtra : TextureRect
{
    public void OnGuiInput(InputEvent e)
    {
        if (Utils.IsRightClick(e))
        {
            if (Tree.Info.IsExtraFull) Tree.Info.EnableExtraInput();
            QueueFree();
        }
    }
}
