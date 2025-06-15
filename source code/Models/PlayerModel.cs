using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KeglyaAimer;

public class PlayerModel
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Mass { get; set; } = 1f;
    private float _speed;
    private float _friction = 0.85f; // Коэффициент трения
    public Weapon CurrentWeapon { get; set; } = new Pistol(); // Текущая оружие игрока

    private float _weaponShakeTime = 0f;
    private float _weaponShakeDuration = 0.1f;
    private float _weaponShakeStrength = 5f;
    private float _weaponShakeDecay = 0.5f; // Затухание (0.9 — быстро, 0.99 — медленно)
    private Random _shakeRandom = new Random();
    public Vector2 WeaponShakeOffset { get; private set; }

    public PlayerModel(Vector2 position, float speed)
    {
        Position = position;
        _speed = speed;
        Velocity = Vector2.Zero;
    }

    public void Move(Vector2 direction)
    {
        // Теперь движение учитывает скорость через Velocity
        if (direction != Vector2.Zero)
        {
            direction.Normalize();
            Velocity += direction * _speed;
        }
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
    }

    public void ProcessRecoil(Vector2 mousePosition)
    {
        Vector2 directionToMouse = mousePosition - Position;
        if (directionToMouse != Vector2.Zero)
        {
            directionToMouse.Normalize();
            ApplyRecoil(directionToMouse);
        }
    }

    public void ApplyRecoil(Vector2 direction)
    {
        Vector2 recoilImpulse = -direction * CurrentWeapon.RecoilForce;
        Velocity += recoilImpulse / Mass;
    }

    public void ApplyWeaponShake()
    {
        _weaponShakeTime = _weaponShakeDuration;
        // Начальное смещение в случайном направлении
        WeaponShakeOffset =
            new Vector2(
                (float)(_shakeRandom.NextDouble() * 2 - 1),
                (float)(_shakeRandom.NextDouble() * 2 - 1)
            ) * _weaponShakeStrength;
    }

    public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
    {
        // Обновляем позицию на основе скорости
        Position += Velocity;

        // Применяем трение
        Velocity *= _friction;

        if (Velocity.LengthSquared() < 0.01f)
            Velocity = Vector2.Zero;

        // Ограничиваем позицию игрока в пределах экрана
        Position = new Vector2(
            MathHelper.Clamp(Position.X, 0, graphicsDevice.Viewport.Width),
            MathHelper.Clamp(Position.Y, 0, graphicsDevice.Viewport.Height)
        );

        // Обновление тряски оружия
        if (_weaponShakeTime > 0)
        {
            _weaponShakeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Генерируем новое случайное смещение
            var randomShake =
                new Vector2(
                    (float)(_shakeRandom.NextDouble() * 2 - 1),
                    (float)(_shakeRandom.NextDouble() * 2 - 1)
                ) * _weaponShakeStrength;

            // Смешиваем с предыдущим смещением для затухания
            WeaponShakeOffset =
                WeaponShakeOffset * _weaponShakeDecay + randomShake * (1 - _weaponShakeDecay);

            if (_weaponShakeTime <= 0)
            {
                _weaponShakeTime = 0;
                WeaponShakeOffset = Vector2.Zero;
            }
        }
    }

    public Vector2 GetGunMuzzlePosition(float gunLength = 40f)
    {
        // Направление на мышь
        Vector2 mousePos = Mouse.GetState().Position.ToVector2();
        Vector2 direction = mousePos - Position;
        if (direction != Vector2.Zero)
            direction.Normalize();

        // Смещение от центра игрока к дулу
        return Position + direction * gunLength;
    }
}
