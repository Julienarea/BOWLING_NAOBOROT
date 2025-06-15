using System;
using Microsoft.Xna.Framework;

namespace KeglyaAimer;

public class BulletModel
{
    public Vector2 Position;
    public Vector2 Velocity;
    public float Radius = 8f;

    public int CurrentFrame = 0;
    public float FrameTimer = 0f;
    public int FramesCount = 4; // количество кадров в анимации
    public float FrameSpeed = 0.08f; // секунд на кадр
    public int AnimationRow = 6; // строка в спрайт-листе (если нужно разные анимации)
    public int AnimationColumn = 21; // столбец первого кадра
    public int FrameWidth = 16; // ширина кадра
    public int FrameHeight = 16; // высота кадра
    public float Rotation
    {
        get => (float)Math.Atan2(Velocity.Y, Velocity.X);
    }

    public BulletModel(Vector2 position, Vector2 velocity)
    {
        Position = position;
        Velocity = velocity;
    }

    public void Update(float elapsed)
    {
        Position += Velocity;
        FrameTimer += elapsed;
        if (FrameTimer >= FrameSpeed)
        {
            FrameTimer = 0f;
            CurrentFrame = (CurrentFrame + 1) % FramesCount;
        }
    }
}
