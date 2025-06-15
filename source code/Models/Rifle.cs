using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace KeglyaAimer;

public class Rifle : Weapon
{
    public override float RecoilForce => 2f;

    private float _fireRate = 0.4f; // время между выстрелами в секундах
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
            float bulletSpeed = 33f;
            Vector2 velocity = direction * bulletSpeed;
            bullets.Add(new BulletModel(position, velocity));
            playerModel.ApplyWeaponShake();
            playerModel.ApplyRecoil(direction * RecoilForce);
            _canShoot = false;
            _fireDelayTimer = 0f;
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
