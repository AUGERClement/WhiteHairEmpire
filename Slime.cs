using System;
using Godot;
public partial class Slime : CharacterBody2D
{
    [Export]
    private int speed { get; set; } = 200;

    [Export]
    private double limit {get; set; } = 0.5;

    [Export]
    private Marker2D endPoint {get; set; }
    private AnimationPlayer animations;

    private Vector2 startPosition;
    private Vector2 endPosition;

	public override void _Ready()
	{
        animations = GetNode<AnimationPlayer>("AnimationPlayer");

		startPosition = Position;
        endPosition = endPoint.Position;

        GD.Print("Start", startPosition);
        GD.Print("End", endPosition);
	}

    private void ChangeDirection() {
       Vector2 tmpEnd = endPosition;

       endPosition = startPosition;
       startPosition = tmpEnd;
    }

    private void UpdateVelocity()
    {
        Vector2 moveDirection = endPosition - Position;

        if (moveDirection.Length() < limit) {
            ChangeDirection();
        }
        Velocity = moveDirection.Normalized() * speed;
    }
    private void UpdateAnimation()
    {
        String animationString = "walk_up";

        if (Velocity.X < 0) {
			animationString = "walk_left";
		} else if (Velocity.X > 0) {
			animationString = "walk_right";
		} else if (Velocity.Y < 0) {
			animationString = "walk_up";
		} else if (Velocity.Y > 0) {
			animationString = "walk_down";
		}
		
		if (Velocity.Length() > 0)
		{
			animations.Play(animationString);
		}
		else
		{
			animations.Stop();
		}
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateVelocity();
        MoveAndSlide();
        UpdateAnimation();
    }
}
