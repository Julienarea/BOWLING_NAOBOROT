using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace KeglyaAimer;

public class Shotgun : Weapon
{
    public override float RecoilForce => 18f;

    public override void Shoot(
        Vector2 position,
        Vector2 direction,
        List<BulletModel> bullets,
        PlayerModel playerModel
    )
    {
        float bulletSpeed = 8f;
        int pellets = 6; // количество дробинок в выстреле
        float spread = 0.9f; // радианы

        for (int i = 0; i < pellets; i++)
        {
            float angle = -spread / 2 + spread * i / (pellets - 1);
            Vector2 dir = Vector2.Transform(direction, Matrix.CreateRotationZ(angle));
            bullets.Add(new BulletModel(position, dir * bulletSpeed));
            playerModel.ApplyWeaponShake(); // Добавляем тряску при выстреле
        }
    }
}
