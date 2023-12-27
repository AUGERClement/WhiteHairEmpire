using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

    public void UpdateHearts(int currentHealth)
    {
        //My first LINQ in C#, banger
        //https://www.reddit.com/r/godot/comments/ca73t1/get_all_children_nodes_of_type_in_c/
        var hearts = GetChildren()
                        .Where(child => child is HeartGui)
                        .Select(heart => heart)
                        .Cast<HeartGui>()
                        .ToArray();

        for (int i = 0; i < currentHealth; i++) {
            hearts[i].Update(true);
        }
        
        for (int i = currentHealth; i < hearts.Length; i++) {
            hearts[i].Update(false);
        }

        /*
        for (int i = hearts.Length -1; i > 0; i--) {
            hearts[i].Update(false);
        }
        */
    }
}
