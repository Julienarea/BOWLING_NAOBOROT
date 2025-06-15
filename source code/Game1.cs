using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KeglyaAimer;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Random _random = new Random();
    private GameState _gameState;
    private MenuModel _menuModel;
    private MenuView _menuView;
    private GameLogicModel _gameLogic;
    private GameLogicView _gameLogicView;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.IsFullScreen = true;
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        _menuModel = new MenuModel(GraphicsDevice);
        _menuView = new MenuView(_menuModel, GraphicsDevice);
        _gameLogic = new GameLogicModel(GraphicsDevice);
        _gameLogicView = new GameLogicView(_gameLogic, GraphicsDevice);
        _gameState = GameState.Menu;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _menuView.LoadContent(Content);
        _gameLogicView.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        switch (_gameState)
        {
            case GameState.Menu:
                _menuModel.Update(this);
                break;
            case GameState.Game:
                _gameLogic.Update(gameTime, this);
                break;
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Gray); // Утсановка фона
        _spriteBatch.Begin();

        switch (_gameState)
        {
            case GameState.Menu:
                _menuView.Draw(_spriteBatch);
                break;
            case GameState.Game:
                _gameLogicView.Draw(_spriteBatch);
                break;
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public void StartGame()
    {
        _gameState = GameState.Game;
        _gameLogic.Start();
    }

    public void EndGame()
    {
        UpdateMaxValues();
        _gameState = GameState.Menu;
    }

    private void UpdateMaxValues()
    {
        if (_gameLogic.HitCount > _menuModel.MaxHits)
            _menuModel.MaxHits = _gameLogic.HitCount;
        if (_gameLogic.SurvivalTime > _menuModel.MaxLifetime)
            _menuModel.MaxLifetime = _gameLogic.SurvivalTime;
    }
}
