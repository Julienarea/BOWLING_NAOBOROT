using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace KeglyaAimer;

public class Pistol : Weapon
{
    public override float RecoilForce => 6f;

    public override void Shoot(
        Vector2 position,
        Vector2 direction,
        List<BulletModel> bullets,
        PlayerModel playerModel
    )
    {
        float bulletSpeed = 15f;
        Vector2 velocity = direction * bulletSpeed;
        bullets.Add(new BulletModel(position, velocity));
        playerModel.ApplyWeaponShake();
    }
}
