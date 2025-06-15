using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KeglyaAimer;

public class ButtonModel
{
    private int _width;
    public int Width
    {
        get => _width;
        set
        {
            _width = value;
            _rectangle.Width = value;
        }
    }
    public int _height;
    public int Height
    {
        get => _height;
        set
        {
            _height = value;
            _rectangle.Height = value;
        }
    }
    private Rectangle _rectangle;
    public Rectangle Rectangle
    {
        get => _rectangle;
        set => _rectangle = value;
    }
    private GraphicsDevice _graphicsDevice;

    public Vector2 Position { get; set; }

    public ButtonModel(GraphicsDevice graphicsDevice, Vector2 position, int width, int height)
    {
        _graphicsDevice = graphicsDevice;
        Position = position;
        _width = width;
        _height = height;
        _rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
    }

    public bool IsMouseOnButton(Vector2 mousePosition)
    {
        return _rectangle.Contains(mousePosition.ToPoint());
    }
}
