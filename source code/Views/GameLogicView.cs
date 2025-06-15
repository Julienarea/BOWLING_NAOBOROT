using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KeglyaAimer;

public class GameLogicView
{
    public const float ZOOM = 0.5f; // Сделайте публичным, если нужно менять снаружи
    private GameLogicModel _gameLogic;
    private PlayerView _playerView;
    private GraphicsDevice _graphicsDevice;
    private SpriteFont _font;
    private Texture2D _backgroundTexture;
    private Texture2D _bulletSpriteSheet;
    private Texture2D _targetSpriteSheet;

    // В классе GameLogicView добавьте поля для текстур оружия:
    private Texture2D _rifleTexture;
    private Texture2D _shotgunTexture;
    private Texture2D _machineGunTexture;
    private Texture2D _pistolTexture;

    public GameLogicView(GameLogicModel gameLogic, GraphicsDevice graphicsDevice)
    {
        _gameLogic = gameLogic;
        _graphicsDevice = graphicsDevice;
        _playerView = new PlayerView(_gameLogic.PlayerModel, graphicsDevice);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        float screenWidth = _graphicsDevice.Viewport.Width;
        float screenHeight = _graphicsDevice.Viewport.Height;

        float textureWidth = _backgroundTexture.Width * ZOOM; // Учитываем ZOOM при расчёте размера текстуры
        float textureHeight = _backgroundTexture.Height * ZOOM;

        int tilesX = (int)Math.Ceiling(screenWidth / textureWidth) + 1; // +1 для перекрытия границ
        int tilesY = (int)Math.Ceiling(screenHeight / textureHeight) + 1;

        for (int x = 0; x < tilesX; x++)
        {
            for (int y = 0; y < tilesY; y++)
            {
                Vector2 position = new Vector2(x * textureWidth, y * textureHeight);
                Rectangle destinationRect = new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    (int)(_backgroundTexture.Width * ZOOM),
                    (int)(_backgroundTexture.Height * ZOOM)
                );

                spriteBatch.Draw(
                    _backgroundTexture,
                    destinationRect,
                    null,
                    Color.Gray,
                    0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0f
                );
            }
        }

        // Отрисовка целей и игрока
        foreach (var target in _gameLogic.Targets)
        {
            var targetView = new TargetView(
                target,
                _gameLogic.TargetColor,
                _targetSpriteSheet,
                _graphicsDevice
            );
            targetView.Draw(spriteBatch);
        }

        _playerView.Draw(spriteBatch);

        // Отрисовка пуль
        foreach (var bullet in _gameLogic.Bullets)
        {
            // Получаем массив кадров для анимации пули
            var frames = SpriteSheetHelper.GetAnimationFrames(
                bullet.FrameWidth,
                bullet.FrameHeight,
                bullet.AnimationRow,
                bullet.AnimationColumn,
                bullet.FramesCount,
                _bulletSpriteSheet.Width
            );

            spriteBatch.Draw(
                _bulletSpriteSheet,
                bullet.Position,
                frames[bullet.CurrentFrame],
                Color.White,
                bullet.Rotation,
                new Vector2(bullet.FrameWidth / 2f, bullet.FrameHeight / 2f),
                bullet.Radius / 2f,
                SpriteEffects.None,
                0f
            );
        }

        // Отрисовка текста (если нужно)
        var text =
            $"IsClicked: {_gameLogic.PlayerController.IsClicked(_gameLogic.PlayerController.CurrentMouseState, _gameLogic.PlayerController.PreviousMouseState)}";
        var textSize = _font.MeasureString(text);
        var textPosition = new Vector2(_graphicsDevice.Viewport.Width - textSize.X - 10, 10);
        spriteBatch.DrawString(_font, text, textPosition, Color.White);

        // Время и количество попаданий
        string timeText = $"Время: {Math.Floor(_gameLogic.SurvivalTime)} сек";
        string hitText = $"Попаданий: {_gameLogic.HitCount}";
        spriteBatch.DrawString(_font, timeText, new Vector2(20, 20), Color.White);
        spriteBatch.DrawString(_font, hitText, new Vector2(20, 60), Color.White);

        // Отрисовка выпадающих оружий
        foreach (var drop in _gameLogic.WeaponDrops)
        {
            if (!drop.IsPicked)
            {
                Texture2D dropTexture = null;
                if (drop.Weapon is Rifle)
                    dropTexture = _rifleTexture;
                else if (drop.Weapon is Shotgun)
                    dropTexture = _shotgunTexture;
                else if (drop.Weapon is MachineGun)
                    dropTexture = _machineGunTexture;
                else if (drop.Weapon is Pistol)
                    dropTexture = _pistolTexture;

                if (dropTexture != null)
                    spriteBatch.Draw(
                        dropTexture,
                        drop.Position,
                        null,
                        Color.White,
                        -0.3f,
                        new Vector2(dropTexture.Width / 2f, dropTexture.Height / 2f),
                        3f,
                        SpriteEffects.None,
                        0f
                    );
            }
        }
    }

    // В методе LoadContent загрузите их:
    public void LoadContent(ContentManager content)
    {
        _playerView.LoadContent(content);
        _gameLogic.TargetCreater.LoadContent(content);
        _font = content.Load<SpriteFont>("fonts/ARIAL");
        _backgroundTexture = content.Load<Texture2D>("images/background");
        _bulletSpriteSheet = content.Load<Texture2D>("images/All_Bullet_Pixel_16x16");
        _targetSpriteSheet = content.Load<Texture2D>("images/spheres");

        _rifleTexture = content.Load<Texture2D>("images/AK47");
        _shotgunTexture = content.Load<Texture2D>("images/SawedOffShotgun");
        _machineGunTexture = content.Load<Texture2D>("images/MP5");
        _pistolTexture = content.Load<Texture2D>("images/M92");
    }
}
