using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class Player : CharacterBody2D
{
	/*
	[Signal]
	public delegate void HitEventHandler(int dmg = 1);

	[Signal]
	public delegate void HealEventHandler(int heal = 1);
	*/
	[Signal]
	public delegate void HealthChangedEventHandler();

	[Export]
    public int Speed { get; set; } = 400; // How fast the player will move (pixels/sec).

	[Export]
	public int maxHealth = 3;

	[Export]
	public int knockBackPower = 25;

    public Vector2 ScreenSize; // Size of the game window.

	private Vector2 _targetVelocity;
	private AnimationPlayer animations;
	private AnimationPlayer effects;
	private Timer hurtTimer;
	private int currentHealth;
	private bool isHurt = false;
	private List<Area2D> enemyCollisions;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		animations = GetNode<AnimationPlayer>("AnimationPlayer");
		effects = GetNode<AnimationPlayer>("Effects");
		hurtTimer = GetNode<Timer>("hurtTimer");

		enemyCollisions = new List<Area2D>();
		currentHealth = maxHealth;
		effects.Play("RESET");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		HandleInput();
		HandleCollision();
		MoveAndSlide();
		UpdateAnimation();
		if (!isHurt) {
			foreach (Area2D enemyArea in enemyCollisions) {
				HurtByEnemy(enemyArea);
			}
		}
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
		
		if (Velocity.Length() > 0) {
			animations.Play(animationString);
		} else {
			animations.Stop();
		}
    }

	public int GetCurrentHealth()
	{
		return currentHealth;
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

	private async void HurtByEnemy(Area2D area)
	{
		computeDamage(1);
		GD.Print("I'm HIT by ", area.GetParent().Name, " and now have ", currentHealth, " hp");
		EmitSignal(SignalName.HealthChanged);
		isHurt = true;
		KnockBack(area.GetParent<Slime>().Velocity);
		effects.Play("hurt_blink");
		hurtTimer.Start();
		await ToSignal(hurtTimer, "timeout");
		effects.Play("RESET");
		isHurt = false;
	}

	private void OnHurtBoxAreaEntered(Area2D area) {
		if (area.Name == "HitBox") { // Touched by hostile
			enemyCollisions = enemyCollisions.Append<Area2D>(area).ToList();
		}
	}

	private void OnHurtBoxAreaExited(Area2D area)
	{
		enemyCollisions.Remove(area);
	}

	private void KnockBack(Vector2 enemyVelocity)
	{
		Vector2 knockBackDirection = (enemyVelocity -Velocity).Normalized() * knockBackPower * 100; //Adjusting factor

		Velocity = knockBackDirection;
		MoveAndSlide();
	}
}
