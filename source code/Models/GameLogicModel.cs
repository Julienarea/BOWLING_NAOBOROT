using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KeglyaAimer;

public class GameLogicModel
{
    const int INITIAL_TARGET_COUNT = 2;
    private const float PLAYER_SPEED = 0.7f;
    private const int TARGET_SIZE = 30;
    private const float TARGET_SPEED_INCREMENT = 0.01f;
    private const float INITIAL_TARGET_SPEED = 50f;
    private const float SPAWN_INTERVAL = 1f;
    private const int TARGET_COUNT_INCREMENT = 2;
    private List<TargetModel> _targets;
    public List<TargetModel> Targets
    {
        get => _targets;
        set => _targets = value;
    }
    private List<BulletModel> _bullets = new List<BulletModel>();
    public List<BulletModel> Bullets => _bullets;
    private PlayerModel _playerModel;
    public PlayerModel PlayerModel
    {
        get => _playerModel;
        set => _playerModel = value;
    }
    private PlayerController _playerController;
    public PlayerController PlayerController
    {
        get => _playerController;
        set => _playerController = value;
    }

    private int _clickCount;

    //TODO реализовать подсчет попаданий и промахов
    private int _missCount;
    private int _hitCount = 0;
    public int HitCount => _hitCount;

    private float _survivalTime = 0f;
    public float SurvivalTime => _survivalTime;

    private Random _random;
    private MouseState _previousMouseState;
    public MouseState PreviousModelState
    {
        get => _previousMouseState;
        set => _previousMouseState = value;
    }
    private GraphicsDevice _graphicsDevice;
    private Color _targetColor;
    public Color TargetColor
    {
        get => _targetColor;
        set => _targetColor = value;
    }
    private int _targetSize;
    public int TargetSize
    {
        get => _targetSize;
        set => _targetSize = value;
    }
    private float _targetSpeedIncrement;
    private float _initialTargetSpeed;
    private float _targetSpeed; //Изменяется
    private float _elapsedTime;
    private float _spawnInterval;
    private int _targetCount; //Изменяется
    private int _targetCountIncrement;
    private TargetCreater _targetCreater;
    public TargetCreater TargetCreater
    {
        get => _targetCreater;
        set => _targetCreater = value;
    }
    private float _playerSpeed;

    private List<WeaponDropModel> _weaponDrops = new List<WeaponDropModel>();
    public List<WeaponDropModel> WeaponDrops => _weaponDrops;

    private float _bestSurvivalTime = 0f;
    private float _lastSurvivalTime = 0f;
    public float BestSurvivalTime => _bestSurvivalTime;
    public float LastSurvivalTime => _lastSurvivalTime;

    public GameLogicModel(GraphicsDevice graphicsDevice)
    {
        _random = new Random();
        _targets = new List<TargetModel>();
        _clickCount = 0;
        _missCount = 0;
        _hitCount = 0;
        _graphicsDevice = graphicsDevice;

        //!fix
        var center = new Vector2(
            graphicsDevice.Viewport.Width / 2,
            graphicsDevice.Viewport.Height / 2
        );

        _playerModel = new PlayerModel(center, PLAYER_SPEED);
        _targetCreater = new TargetCreater(graphicsDevice, TARGET_SIZE, Color.White, center);

        _targetColor = Color.White;
        _targetSize = TARGET_SIZE;
        _targetSpeedIncrement = TARGET_SPEED_INCREMENT;
        _initialTargetSpeed = INITIAL_TARGET_SPEED;
        _spawnInterval = SPAWN_INTERVAL;
        _targetCountIncrement = TARGET_COUNT_INCREMENT;
        _playerSpeed = PLAYER_SPEED;
    }

    public void Start()
    {
        _targetCreater.UpdateParams(_targetSize, _targetColor, _playerModel.Position);

        _targets.Clear();
        _weaponDrops.Clear();
        _playerModel.SetSpeed(_playerSpeed);
        _playerModel.CurrentWeapon = new Pistol(); // Начальное оружие
        _playerController = new PlayerController(_playerModel);
        _elapsedTime = 0;
        SetDefaultValues();

        _playerModel.SetPosition(
            new Vector2(_graphicsDevice.Viewport.Width / 2, _graphicsDevice.Viewport.Height / 2) //Центр экрана
        );

        List<TargetModel> target = _targetCreater.CreateTarget(
            INITIAL_TARGET_COUNT,
            _playerModel.Position
        );
        _targetCount = INITIAL_TARGET_COUNT;
        _targets.AddRange(target);
        _bullets.Clear();

        _survivalTime = 0f;
        _hitCount = 0;
    }

    public void Update(GameTime gameTime, Game1 game)
    {
        // Сначала обновляем мышь!
        _playerController.UpdateMouse();

        MouseState currentMouseState = _playerController.CurrentMouseState;
        Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
        Vector2 playerPosition = _playerModel.Position;

        #region Обработка мыши и обновление мишеней
        for (int i = 0; i < _targets.Count; i++)
        {
            var target = _targets[i];

            if (
                _playerController.IsClicked(currentMouseState, _playerController.PreviousMouseState)
            )
            {
                _clickCount++;
            }
            if (target.HasReachedPlayer(playerPosition))
            {
                game.EndGame();
            }

            var trueTargetSpeed = (int)Math.Round(Math.Sqrt(_targetSpeed));
            target.SetTargetSpeed(trueTargetSpeed);
            target.Update(gameTime);
        }
        _targetSpeed += _targetSpeedIncrement;
        #endregion

        // Обработка движения игрока
        _playerController.UpdateKeyboard();
        _playerModel.Update(gameTime, _graphicsDevice);

        // Стрельба
        if (_playerModel.CurrentWeapon is Rifle or MachineGun)
        {
            if (_playerController.MouseLeftIsPressed())
            {
                Vector2 direction = mousePosition - _playerModel.Position;
                if (direction != Vector2.Zero)
                {
                    direction.Normalize();
                    Vector2 muzzlePos = _playerModel.GetGunMuzzlePosition();
                    _playerModel.CurrentWeapon.Shoot(muzzlePos, direction, _bullets, PlayerModel);
                }
            }
        }
        else if (
            _playerController.IsClicked(currentMouseState, _playerController.PreviousMouseState)
        )
        {
            Vector2 direction = mousePosition - _playerModel.Position;
            if (direction != Vector2.Zero)
                direction.Normalize();

            // Замените здесь:
            Vector2 muzzlePos = _playerModel.GetGunMuzzlePosition();
            _playerModel.CurrentWeapon.Shoot(muzzlePos, direction, _bullets, PlayerModel);
        }
        if (currentMouseState.LeftButton == ButtonState.Released)
        {
            if (PlayerModel.CurrentWeapon is Rifle rifle)
            {
                rifle.ResetFireDelay();
            }
            else if (PlayerModel.CurrentWeapon is MachineGun machineGun)
            {
                machineGun.ResetFireDelay();
            }
        }

        // Обновляем таймер
        _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        _survivalTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_elapsedTime >= _spawnInterval)
        {
            _elapsedTime = 0;
            _targetCount += _targetCountIncrement;
            //TODO Переименновать
            int targetCountTrue = (int)Math.Round(Math.Sqrt(_targetCount));
            List<TargetModel> target = _targetCreater.CreateTarget(
                targetCountTrue,
                _playerModel.Position
            );
            _targets.AddRange(target);
        }

        for (int i = 0; i < _targets.Count; i++)
        {
            for (int j = i + 1; j < _targets.Count; j++)
            {
                if (_targets[i].CheckCollision(_targets[j]))
                {
                    // _targets[i].HandleCollision(_targets[j]);
                    // _targets.RemoveAt(j);ws
                }
            }
        }
        Console.WriteLine($"{currentMouseState.LeftButton}, {_previousMouseState.LeftButton}");
        if (PlayerModel.CurrentWeapon is Rifle or MachineGun)
        {
            if (_playerController.MouseLeftIsPressed())
            {
                // PlayerModel.ProcessRecoil(mousePosition);
                if (PlayerModel.CurrentWeapon is Rifle rifle)
                {
                    rifle.ApplyFireDelay(gameTime);
                }
                else if (PlayerModel.CurrentWeapon is MachineGun machineGun)
                {
                    machineGun.ApplyFireDelay(gameTime);
                }
            }
        }
        else if (
            _playerController.IsClicked(
                PlayerController.CurrentMouseState,
                PlayerController.PreviousMouseState
            )
        )
        {
            PlayerModel.ProcessRecoil(mousePosition);
        }

        // Обновление пуль
        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            var bullet = _bullets[i];
            bullet.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            // Удаляем пулю, если она вышла за пределы экрана
            if (
                bullet.Position.X < 0
                || bullet.Position.X > _graphicsDevice.Viewport.Width
                || bullet.Position.Y < 0
                || bullet.Position.Y > _graphicsDevice.Viewport.Height
            )
            {
                _bullets.RemoveAt(i);
                continue;
            }

            // Проверка столкновения с мишенями
            for (int j = _targets.Count - 1; j >= 0; j--)
            {
                var target = _targets[j];
                float dist = Vector2.Distance(bullet.Position, target.Position);
                if (dist < bullet.Radius + target.Radius)
                {
                    if (target.Type == TargetType.Splitting && target.Radius > 12f) // не делить слишком маленькие
                    {
                        float newRadius = target.Radius * 0.7f;
                        // Создаем две новых мишени чуть смещённых
                        var t1 = new TargetModel(
                            newRadius,
                            target.Position + new Vector2(newRadius, 0),
                            _playerModel.Position,
                            target.Texture,
                            TargetType.Splitting
                        );
                        var t2 = new TargetModel(
                            newRadius,
                            target.Position - new Vector2(newRadius, 0),
                            _playerModel.Position,
                            target.Texture,
                            TargetType.Splitting
                        );
                        _targets.Add(t1);
                        _targets.Add(t2);
                    }
                    _targets.RemoveAt(j);
                    _bullets.RemoveAt(i);
                    RegisterHit();
                    // После удаления мишени
                    if (_random.NextDouble() < 0.2) // 20% шанс дропа
                    {
                        Weapon randomWeapon = GetRandomWeapon();
                        _weaponDrops.Add(new WeaponDropModel(target.Position, randomWeapon));
                    }
                    break;
                }
            }
        }

        for (int i = _weaponDrops.Count - 1; i >= 0; i--)
        {
            var drop = _weaponDrops[i];
            if (!drop.IsPicked && Vector2.Distance(_playerModel.Position, drop.Position) < 40f)
            {
                _playerModel.CurrentWeapon = drop.Weapon;
                drop.IsPicked = true;
                _weaponDrops.RemoveAt(i);
            }
        }
    }

    public void RegisterHit()
    {
        _hitCount++;
    }

    private void SetDefaultValues()
    {
        _targetSpeed = _initialTargetSpeed;
        _targetCount = INITIAL_TARGET_COUNT;
    }

    private Weapon GetRandomWeapon()
    {
        int type = _random.Next(0, 4);
        return type switch
        {
            0 => new Rifle(),
            1 => new Shotgun(),
            2 => new MachineGun(),
            _ => new Pistol(),
        };
    }
}
