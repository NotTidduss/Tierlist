using Godot;

public partial class InfoImageInput : Control
{
    [Signal] public delegate void OnInputPressed_EventHandler(string input);
    [Export] private LineEdit input;
    public string Path { get => input.Text; set => input.Text = value; }


    public void OnPressed() { EmitSignal(SignalName.OnInputPressed_, input.Text); input.Clear(); }
    public void OnMouseExited_Input() => input.ReleaseFocus();
}
