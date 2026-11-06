using Godot;

public partial class TierHandle : TextureRect
{
    public float NewAlpha => Utils.Limit(Modulate.A + (desiredAlpha == 1 ? 0.1f : -0.1f), 0, 1);
    private float desiredAlpha = 0;

    public override void _Process(double delta)
    {
        if (Modulate.A == desiredAlpha) return;

        Modulate = new(Modulate.R, Modulate.G, Modulate.B, NewAlpha);
    }

    public void FadeIn() => desiredAlpha = 1;
    public void FadeOut() => desiredAlpha = 0;
}
