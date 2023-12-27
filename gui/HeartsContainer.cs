using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class HeartsContainer : HBoxContainer
{
    private PackedScene heartGuiClass;
    public override void _Ready()
    {
        heartGuiClass = GD.Load<PackedScene>("res://gui/heart_gui.tscn");
    }

    public void SetMaxHearts(int max)
    {
        for (int i = 0; i < max; i++) {
            var heart = heartGuiClass.Instantiate();
            AddChild(heart);
        }
    }
}
