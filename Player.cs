using Godot;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void HitEventHandler(int dmg = 1);

	[Export]
    public int Speed { get; set; } = 400; // How fast the player will move (pixels/sec).

    public Vector2 ScreenSize; // Size of the game window.

	private Vector2 _targetVelocity;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		//Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
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

    	var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (direction.Length() > 0)
		{
			direction = direction.Normalized() * Speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}

		/*
		Position += direction * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
		*/
		_targetVelocity.X = direction.X;// * (float)delta;
		_targetVelocity.Y = direction.Y;// * (float)delta;

		Velocity = _targetVelocity;
		MoveAndSlide();

		if (direction.X != 0) {
			animatedSprite2D.Animation = "walk_left";
			animatedSprite2D.FlipV = false;
			animatedSprite2D.FlipH = direction.X > 0;
		} else if (direction.Y < 0) {
			animatedSprite2D.Animation = "walk_up";
		} else if (direction.Y > 0) {
			animatedSprite2D.Animation = "walk_down";
		}
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	private void OnBodyEntered(Node2D body)
	{
		Hide(); // Player disappears after being hit.
		EmitSignal(SignalName.Hit);
		// Must be deferred as we can't change physics properties on a physics callback.
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
}
