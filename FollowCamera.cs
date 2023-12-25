using System;
using Godot;

public partial class FollowCamera : Camera2D
{
	[Export]
	private TileMap tileMap;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		Rect2I mapRect = tileMap.GetUsedRect();
		int tileSize = tileMap.RenderingQuadrantSize;
		//GD.Print(tileSize);
		Vector2 worldSizePixel = mapRect.Size * tileSize;
		//GD.Print(worldSizePixel);
		LimitRight = (int)worldSizePixel.X;
		LimitBottom = (int)worldSizePixel.Y;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
