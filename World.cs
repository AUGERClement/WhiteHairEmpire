using Godot;

public partial class World : Node2D
{
    private HeartsContainer heartsContainer;
    private Player player;

    public override void _Ready()
    {
        heartsContainer = GetNode<HeartsContainer>("CanvasLayer/heartsContainer");
        player = GetNode<Player>("TileMap/Player");

        heartsContainer.SetMaxHearts(player.maxHealth);
        heartsContainer.UpdateHearts(player.getCurrentHealth());
    }

    private void OnPlayerHealthChanged()
    {
        heartsContainer.UpdateHearts(player.getCurrentHealth());
    }
}
