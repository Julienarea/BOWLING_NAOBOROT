using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KeglyaAimer;

public class ButtonView
{
    private ButtonModel _buttonModel;
    private Color _color;
    private string _text;
    private SpriteFont _font;
    private Texture2D _texture;
    private GraphicsDevice _graphicsDevice;

    public ButtonView(ButtonModel buttonModel, GraphicsDevice graphicsDevice, Color color)
    {
        _buttonModel = buttonModel;
        _color = color;

        _graphicsDevice = graphicsDevice;
        _texture = CreateButtonTexture(buttonModel.Width, buttonModel.Height);
        _text = "Start Game";
        _font = null;
        _color = color;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _buttonModel.Rectangle, _color);
        Vector2 textSize = _font.MeasureString(_text);
        Vector2 textPosition = new Vector2(
            _buttonModel.Rectangle.X + (_buttonModel.Rectangle.Width - textSize.X) / 2,
            _buttonModel.Rectangle.Y + (_buttonModel.Rectangle.Height - textSize.Y) / 2
        );
        spriteBatch.DrawString(_font, _text, textPosition, Color.White);
    }

    private Texture2D CreateButtonTexture(int width, int height)
    {
        Texture2D texture = new Texture2D(_graphicsDevice, width, height);
        Color[] colorData = new Color[width * height];

        for (int i = 0; i < colorData.Length; i++)
        {
            colorData[i] = _color;
        }

        texture.SetData(colorData);
        return texture;
    }

    public void LoadContent(ContentManager content)
    {
        _font = content.Load<SpriteFont>("fonts/ARIAL");
    }
}
