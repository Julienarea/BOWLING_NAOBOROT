using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KeglyaAimer;

public class MenuModel
{
    private GraphicsDevice _graphicsDevice;
    #region Start Button
    private ButtonModel _startButton;
    public ButtonModel StartButton
    {
        get => _startButton;
        set => _startButton = value;
    }
    private ButtonView _startButtonView;
    public ButtonView StartButtonView
    {
        get => _startButtonView;
        set => _startButtonView = value;
    }
    const int START_BUTTON_WIDTH = 500;
    const int START_BUTTON_HEIGHT = 100;
    private int START_BUTTON_X => _graphicsDevice.Viewport.Width / 2 - START_BUTTON_WIDTH / 2;
    private int START_BUTTON_Y => _graphicsDevice.Viewport.Height / 2 - START_BUTTON_HEIGHT / 2;
    #endregion

    public MenuModel(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
        //  Установить по центру экрана
        _startButton = new ButtonModel(
            graphicsDevice,
            new Vector2(START_BUTTON_X, START_BUTTON_Y),
            START_BUTTON_WIDTH,
            START_BUTTON_HEIGHT
        );
        _startButtonView = new ButtonView(_startButton, graphicsDevice, Color.Blue);
    }

    public void Update(Game1 game)
    {
        if (
            _startButton.IsMouseOnButton(new Vector2(Mouse.GetState().X, Mouse.GetState().Y))
            && Mouse.GetState().LeftButton == ButtonState.Pressed
        )
        {
            game.StartGame();
        }
    }
}
