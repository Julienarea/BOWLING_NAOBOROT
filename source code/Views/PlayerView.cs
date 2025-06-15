using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KeglyaAimer;

public class PlayerView
{
    private PlayerModel _playerModel;
    private Texture2D _texture;
    private GraphicsDevice _graphicsDevice;

    private Texture2D _pistolTexture;
    private Texture2D _shotgunTexture;
    private Texture2D _rifleTexture;
    private Texture2D _mashineGunTexture;

    public PlayerView(PlayerModel playerModel, GraphicsDevice graphicsDevice)
    {
        _playerModel = playerModel;
        _graphicsDevice = graphicsDevice;
        _texture = new Texture2D(graphicsDevice, 1, 1);
    }

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("images/keglya");
        _pistolTexture = content.Load<Texture2D>("images/M92");
        _shotgunTexture = content.Load<Texture2D>("images/SawedOffShotgun");
        _rifleTexture = content.Load<Texture2D>("images/AK47");
        _mashineGunTexture = content.Load<Texture2D>("images/MP5"); // Добавляем текстуру для пулемёта
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Отрисовка игрока
        spriteBatch.Draw(
            _texture,
            _playerModel.Position,
            null,
            Color.White,
            0f,
            new Vector2(_texture.Width / 2, _texture.Height / 2),
            0.25f,
            SpriteEffects.None,
            0f
        );

        // Определяем текстуру оружия
        Texture2D weaponTexture = null;
        if (_playerModel.CurrentWeapon is Pistol)
            weaponTexture = _pistolTexture;
        else if (_playerModel.CurrentWeapon is Shotgun)
            weaponTexture = _shotgunTexture;
        else if (_playerModel.CurrentWeapon is Rifle)
            weaponTexture = _rifleTexture;
        else if (_playerModel.CurrentWeapon is MachineGun)
            weaponTexture = _mashineGunTexture;

        if (weaponTexture != null)
        {
            // Получаем позицию курсора мыши в экранных координатах
            var mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

            // Вычисляем угол между игроком и мышью
            Vector2 toMouse = mousePosition - _playerModel.Position;
            float rotation = (float)Math.Atan2(toMouse.Y, toMouse.X);

            // Нормализуем направление и задаём смещение (например, 20 пикселей)
            Vector2 offset = Vector2.Zero;
            if (toMouse != Vector2.Zero)
                offset = Vector2.Normalize(toMouse) * 20f; // 20 — длина выноса оружия вперёд

            // Определяем, нужно ли переворачивать спрайт по вертикали
            SpriteEffects effects = SpriteEffects.None;
            if (toMouse.X < 0)
                effects = SpriteEffects.FlipVertically;

            // Отрисовка оружия с поворотом и смещением
            spriteBatch.Draw(
                weaponTexture,
                _playerModel.Position + offset + _playerModel.WeaponShakeOffset, // Используем WeaponShakeOffset из модели
                null,
                Color.White,
                rotation,
                new Vector2(weaponTexture.Width / 2, weaponTexture.Height / 2),
                3f,
                effects,
                0f
            );
        }
    }
}
