using System;
using Godot;

public partial class Player : CharacterBody2D
{
	//[Signal]
	//public delegate void HitEventHandler(int dmg = 1);

	[Export]
    public int Speed { get; set; } = 400; // How fast the player will move (pixels/sec).

	[Export]
	public int maxHealth = 3;

    public Vector2 ScreenSize; // Size of the game window.

	private Vector2 _targetVelocity;
	private AnimationPlayer animations;
	private int currentHealth;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		animations = GetNode<AnimationPlayer>("AnimationPlayer");
		currentHealth = maxHealth;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		HandleInput();
		HandleCollision();
		MoveAndSlide();
		UpdateAnimation();
	}

	private void HandleInput()
	{
		var direction = Vector2.Zero; // The player's movement vector.

		if (Input.IsActionPressed("move_right")) {
			direction.X += 1;
		}
		if (Input.IsActionPressed("move_left")) {
			direction.X -= 1;
		}
		if (Input.IsActionPressed("move_down")) {
			direction.Y += 1;
		}
		if (Input.IsActionPressed("move_up")) {
			direction.Y -= 1;
		}

		direction = direction.Normalized() * Speed;

		_targetVelocity.X = direction.X;// * (float)delta;
		_targetVelocity.Y = direction.Y;// * (float)delta;

		Velocity = _targetVelocity;
	}

	private void HandleCollision()
	{
		for (int i = 0; i < GetSlideCollisionCount(); i++) {
			KinematicCollision2D collision = GetSlideCollision(i);
			GD.Print("I collided with ", ((Node)collision.GetCollider()).Name);
		}
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

	private void computeHeal(int heal) {
		currentHealth += heal;
		if (currentHealth >= maxHealth) {
			currentHealth = maxHealth;
		}
	}

	private void computeDamage(int damage) {
		currentHealth -= damage;
		if (currentHealth <= 0) {
			currentHealth = 0;
			//GameOver;
		}
	}

	private void OnHurtBoxAreaEntered(Area2D area) {
		GD.Print("current health = ", currentHealth);
		if (area.Name == "HitBox") {
			computeDamage(1);
			GD.Print("I'm HIT by ", area.GetParent().Name, " and now have ", currentHealth, " hp");
		}
	}
}
