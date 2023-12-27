using Godot;
using System;

public partial class World : Node2D
{
    private HeartsContainer heartsContainer;

    public override void _Ready()
    {
        heartsContainer = GetNode<HeartsContainer>("CanvasLayer/heartsContainer");

        heartsContainer.SetMaxHearts(3);
    }
}
