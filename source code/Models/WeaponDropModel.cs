using Microsoft.Xna.Framework;

namespace KeglyaAimer;

public class WeaponDropModel
{
    public Vector2 Position { get; set; }
    public Weapon Weapon { get; set; }
    public bool IsPicked { get; set; } = false;

    public WeaponDropModel(Vector2 position, Weapon weapon)
    {
        Position = position;
        Weapon = weapon;
    }
}
