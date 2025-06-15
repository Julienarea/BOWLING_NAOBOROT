using KeglyaAimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KeglyaAimer;

public class MenuView
{
    private MenuModel _menuModel;
    private GraphicsDevice _graphicsDevice;

    public MenuView(MenuModel menuModel, GraphicsDevice graphicsDevice)
    {
        _menuModel = menuModel;
        _graphicsDevice = graphicsDevice;
    }

    public void LoadContent(ContentManager content)
    {
        _menuModel.StartButtonView.LoadContent(content);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _menuModel.StartButtonView.Draw(spriteBatch);
    }
}
