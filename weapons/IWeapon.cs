using Godot;

interface IWeapon
{
    public void Attack(string direction = "up", int range = 10);
}