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

    public void Attack(string direction = "up", int range = 10)
    {
        GD.Print("Sword attack triggered in direction, ", direction);

        if (direction == "right") {
			Position = new Vector2(range, 0);
		}
		if (direction == "left") {
			Position = new Vector2(-range, 0);
		}
		if (direction == "down") {
			Position = new Vector2(0, range);
		}
		if (direction == "up") {
			Position = new Vector2(0, -range);
		}

        animations.Play("attack_" + direction);
        //Dispose();
    }
}
