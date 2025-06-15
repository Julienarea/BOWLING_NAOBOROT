using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace KeglyaAimer;

public class MachineGun : Weapon
{
    public override float RecoilForce => 1f;

    private float _fireRate = 0.05f; // очень высокая скорострельность (секунд между выстрелами)
    private float _fireDelayTimer = 0f;
    private bool _canShoot = true;

    public override void Shoot(
        Vector2 position,
        Vector2 direction,
        List<BulletModel> bullets,
        PlayerModel playerModel
    )
    {
        if (_canShoot)
        {
            float bulletSpeed = 25f;
            float spread = 1.6f; // радианы, максимальный угол отклонения
            var random = new System.Random();
            float angleOffset = (float)(random.NextDouble() * spread - spread / 2);

            // Поворачиваем направление на случайный угол
            Vector2 dir = Vector2.Transform(direction, Matrix.CreateRotationZ(angleOffset));
            bullets.Add(new BulletModel(position, dir * bulletSpeed));
            _canShoot = false;
            _fireDelayTimer = 0f;
            playerModel.ApplyWeaponShake(); // Добавляем тряску при выстреле
            playerModel.ApplyRecoil(direction * RecoilForce); // Применяем отдачу
        }
    }

    public void ApplyFireDelay(GameTime gameTime)
    {
        if (!_canShoot)
        {
            _fireDelayTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_fireDelayTimer >= _fireRate)
            {
                _canShoot = true;
                _fireDelayTimer = 0f;
            }
        }
    }

    public void ResetFireDelay()
    {
        _canShoot = true;
        _fireDelayTimer = 0f;
    }
}
