using System;
using Godot;
public partial class Slime : CharacterBody2D
{
    [Export]
    private int speed { get; set; } = 200;

    [Export]
    private double limit {get; set; } = 0.5;
    private AnimatedSprite2D animations;

    private Vector2 startPosition;
    private Vector2 endPosition;
	public override void _Ready()
	{
        animations = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		startPosition = Position;
        endPosition = startPosition + new Vector2(0, 3*16);
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
            Position = endPosition;
            //moveDirection = new Vector2(0, 0);
            ChangeDirection();
        }
        Velocity = moveDirection.Normalized() * speed;
    }
    private void UpdateAnimation()
    {
        String animationString = "walk_up";
        if (Velocity.Y > 0) {
            animationString = "walk_down";
        }

        animations.Play(animationString);
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateVelocity();
        MoveAndSlide();
        UpdateAnimation();
    }
}
