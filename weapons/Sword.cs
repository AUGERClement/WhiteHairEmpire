using Godot;

public partial class Sword : Area2D, IWeapon
{
    private AnimationPlayer animations;
    public override void _Ready()
    {
        GD.Print("Initializer called");
        animations = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void Attack()
    {
        GD.Print("Sword attack triggered");
        GD.Print(animations);
        animations.Play("attack_up");
        Dispose();
    }
}
