using Godot;

public partial class HeartGui : Panel
{
    private Sprite2D sprite;

    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite");
    }
    public void Update(bool whole) {
        if (whole) {
            sprite.Frame = 0;
        } else {
            sprite.Frame = 4;
        }
    }
}
