using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeglyaAimer;

public enum TargetType
{
    Normal,
    Splitting,
}

public class TargetModel
{
    public Vector2 Position { get; private set; }
    public float Radius { get; private set; }
    private float _targetSpeed;

    public Vector2 Direction { get; set; }
    private Vector2 _startPlayerPosition;
    private Vector2 _startTargetdirection;
    public Texture2D Texture { get; set; }
    public TargetType Type { get; set; } = TargetType.Normal;

    public int CurrentFrame = 0;
    public float FrameTimer = 0f;
    public int FramesCount = 8; // сколько кадров в анимации
    public float FrameSpeed = 0.12f; // секунд на кадр
    public int AnimationRow
    {
        get
        {
            switch (Type)
            {
                case TargetType.Splitting:
                    return 5;
                case TargetType.Normal:
                default:
                    return 6;
            }
        }
    } // строка на спрайт-листе
    public int AnimationColumn = 0; // столбец первого кадра
    public float Rotation
    {
        get => (float)Math.Atan2(Direction.Y, Direction.X) + MathF.PI;
    }
    public int FrameWidth = 16;
    public int FrameHeight = 16;

    public TargetModel(
        float radius,
        Vector2 position,
        Vector2 startPlayerPosition,
        Texture2D texture,
        TargetType type = TargetType.Normal
    )
    {
        Radius = radius;
        Position = position;
        _startPlayerPosition = startPlayerPosition;
        _startTargetdirection = GetVectorToPlayer(startPlayerPosition);
        Texture = texture;
        Type = type;
    }

    private Vector2 GetVectorToPlayer(Vector2 playerPosition)
    {
        Vector2 vectorToPlayer = playerPosition - Position;
        vectorToPlayer.Normalize();
        return vectorToPlayer;
    }

    public bool HasReachedPlayer(Vector2 playerPosition)
    {
        return Vector2.Distance(Position, playerPosition) <= Radius;
    }

    public bool CheckCollision(TargetModel otherTarget)
    {
        float distance = Vector2.Distance(this.Position, otherTarget.Position);
        return distance <= this.Radius + otherTarget.Radius;
    }

    public void Update(GameTime gameTime)
    {
        MoveOneStepToPlayerWithInertion(_startPlayerPosition);
        Position += Direction * _targetSpeed;
        UpdateAnimation((float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    public void SetTargetSpeed(float speed)
    {
        _targetSpeed = speed;
    }

    //TODO реализовать метод движения с инерцией
    private void MoveOneStepToPlayerWithInertion(Vector2 playerPosition)
    {
        var currentVectorToPlayre = GetVectorToPlayer(playerPosition);
        var startPlayerPosition = _startPlayerPosition;
        var angle = GetAngleBetweenDirectionAndPlayer(playerPosition);

        Direction = _startTargetdirection;
    }

    private float GetAngleBetweenDirectionAndPlayer(Vector2 playerPosition)
    {
        float angle = (float)Math.Acos(Vector2.Dot(Direction, playerPosition));
        if (float.IsNaN(angle))
        {
            angle = 0f;
        }
        else if (angle > Math.PI)
        {
            angle = MathHelper.TwoPi - angle;
        }
        return angle;
    }

    public void UpdateAnimation(float elapsed)
    {
        FrameTimer += elapsed;
        if (FrameTimer >= FrameSpeed)
        {
            FrameTimer = 0f;
            CurrentFrame = (CurrentFrame + 1) % FramesCount;
        }
    }
}
