using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KeglyaAimer;

public class TargetCreater
{
    private GraphicsDevice _graphicsDevice;
    private float _radius;
    private Vector2 _position;
    private Vector2 _playerPosition;
    private Color _color;
    private Texture2D _texture;

    private Random _random = new Random();

    public TargetCreater(
        GraphicsDevice graphicsDevice,
        float radius,
        Color color,
        Vector2 playerPosition
    )
    {
        _graphicsDevice = graphicsDevice;
        _radius = radius;
        _position = GetRandomPosBehindMainScreen();
        _color = color;
        _playerPosition = playerPosition;
    }

    public List<TargetModel> CreateTarget(int count, Vector2 playerPosition)
    {
        var targets = new List<TargetModel>();
        var rand = new Random();

        for (int i = 0; i < count; i++)
        {
            Vector2 position = GetRandomPosBehindMainScreen();
            // 50% шанс на Splitting, 50% на Normal
            TargetType type = rand.NextDouble() < 0.5 ? TargetType.Normal : TargetType.Splitting;
            var target = new TargetModel(_radius, position, playerPosition, _texture, type);
            targets.Add(target);
        }

        return targets;
    }

    public void UpdateParams(float radius, Color color, Vector2 playerPosition)
    {
        _radius = radius;
        _color = color;
        _playerPosition = playerPosition;
    }

    private Vector2 GetRandomPosBehindMainScreen()
    {
        float x = _random.Next(0, _graphicsDevice.Viewport.Width);
        float y = -_radius;
        Vector2 posUp = new Vector2(x, y);

        x = _random.Next(0, _graphicsDevice.Viewport.Width);
        y = _graphicsDevice.Viewport.Height + _radius;
        Vector2 posDown = new Vector2(x, y);

        x = -_radius;
        y = _random.Next(0, _graphicsDevice.Viewport.Height);
        Vector2 posLeft = new Vector2(x, y);

        x = _graphicsDevice.Viewport.Width + _radius;
        y = _random.Next(0, _graphicsDevice.Viewport.Height);
        Vector2 posRight = new Vector2(x, y);

        return _random.Next(0, 4) switch
        {
            0 => posUp,
            1 => posDown,
            2 => posLeft,
            _ => posRight,
        };
    }

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("images/target");
    }
}
