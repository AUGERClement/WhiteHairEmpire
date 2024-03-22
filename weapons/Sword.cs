using System;
using Godot;

public partial class Sword : Area2D, IWeapon
{
    private AnimationPlayer animations;
    public override void _Ready()
    {
        GD.Print("Initializer called");
        animations = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void Attack(string direction = "up")
    {
        GD.Print("Sword attack triggered in direction, ", direction);
        //GD.Print(animations);
        animations.Play("attack_" + direction);
        //Dispose();
    }
}
