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

    public void SetMaxHearts(int max) // Second version with quarters hearts;
    {
        int unfilledHeart = (max % 4) != 0 ? 1 : 0;
        int hearts = max / 4 + unfilledHeart;
        GD.Print("Max = ", max, " so Number hearts = ", hearts);

        for (int i = 0; i < hearts; i++) {
            var heart = heartGuiClass.Instantiate();
            AddChild(heart);
        }

    }

    public void UpdateHearts(int currentHealth)
    {
        int i = 0;
        //My first LINQ in C#, banger
        //https://www.reddit.com/r/godot/comments/ca73t1/get_all_children_nodes_of_type_in_c/
        var hearts = GetChildren()
                        .Where(child => child is HeartGui)
                        .Select(heart => heart)
                        .Cast<HeartGui>()
                        .ToArray();

        for (; i < currentHealth / 4; i++) { //Fill hearts
            hearts[i].Update(4);
        }

        
        if (currentHealth % 4 != 0) { // Last heart partly filled
            hearts[i].Update(currentHealth % 4);
        } else {
            hearts[i].Update(0);
        }
    }
    
}
