using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace KeglyaAimer;

public abstract class Weapon
{
    public abstract float RecoilForce { get; }
    public abstract void Shoot(
        Vector2 position,
        Vector2 direction,
        List<BulletModel> bullets,
        PlayerModel playerModel
    );
}
