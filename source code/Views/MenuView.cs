using KeglyaAimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KeglyaAimer;

public class MenuView
{
    private MenuModel _menuModel;
    private GraphicsDevice _graphicsDevice;
    private SpriteFont font;
    public int maxHits = 0;
    public float maxLifetime = 0f;

    public MenuView(MenuModel menuModel, GraphicsDevice graphicsDevice)
    {
        _menuModel = menuModel;
        _graphicsDevice = graphicsDevice;
    }

    public void LoadContent(ContentManager content)
    {
        _menuModel.StartButtonView.LoadContent(content);
        font = content.Load<SpriteFont>("fonts/ARIAL");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        maxHits = _menuModel.MaxHits;
        maxLifetime = _menuModel.MaxLifetime;
        _menuModel.StartButtonView.Draw(spriteBatch);
        spriteBatch.DrawString(
            font,
            $"Максимум попаданий: {maxHits}",
            new Vector2(700, 400),
            Color.White
        );
        spriteBatch.DrawString(
            font,
            $"Максимальное время жизни: {maxLifetime:0.0} сек",
            new Vector2(700, 430),
            Color.White
        );
    }
}
