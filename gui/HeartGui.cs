using Godot;

public partial class HeartGui : Panel
{
    private Sprite2D sprite;

    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite");
    }
    public void Update(int hp) {
        sprite.Frame = 4 - hp; // If 1 hp, frame 3 is needed.
    }
}
