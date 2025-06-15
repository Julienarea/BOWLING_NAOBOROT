using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KeglyaAimer;

public class PlayerController
{
    private PlayerModel _player;

    private MouseState _previousMouseState;
    public MouseState PreviousMouseState
    {
        get => _previousMouseState;
        set => _previousMouseState = value;
    }
    private MouseState _currentMouseState;
    public MouseState CurrentMouseState
    {
        get => _currentMouseState;
        set => _currentMouseState = value;
    }

    public PlayerController(PlayerModel player)
    {
        _player = player;
    }

    public void UpdateKeyboard()
    {
        // Обработка движения игрока через switch case
        KeyboardState keyboardState = Keyboard.GetState();
        foreach (var key in keyboardState.GetPressedKeys())
        {
            switch (key)
            {
                case Keys.W:
                    _player.Move(new Vector2(0, -1));
                    break;
                case Keys.S:
                    _player.Move(new Vector2(0, 1));
                    break;
                case Keys.A:
                    _player.Move(new Vector2(-1, 0));
                    break;
                case Keys.D:
                    _player.Move(new Vector2(1, 0));
                    break;
            }
        }

        // if (Keyboard.GetState().IsKeyDown(Keys.D1))
        //     _player.CurrentWeapon = new Pistol();
        // if (Keyboard.GetState().IsKeyDown(Keys.D2))
        //     _player.CurrentWeapon = new Shotgun();
        // if (Keyboard.GetState().IsKeyDown(Keys.D3))
        //     _player.CurrentWeapon = new Rifle();
        // if (Keyboard.GetState().IsKeyDown(Keys.D4))
        //     _player.CurrentWeapon = new MachineGun();
    }

    public bool IsMouseOnTarget(Vector2 mousePosition, TargetModel target)
    {
        float distance = Vector2.Distance(target.Position, mousePosition);
        return distance <= target.Radius;
    }

    public void UpdateMouse()
    {
        _previousMouseState = _currentMouseState;
        _currentMouseState = Mouse.GetState();
    }

    public Vector2 GetMousePosition()
    {
        return new Vector2(_currentMouseState.X, _currentMouseState.Y);
    }

    public bool IsClicked(MouseState currentMouseState, MouseState previousMouseState)
    {
        if (
            currentMouseState.LeftButton == ButtonState.Pressed
            && previousMouseState.LeftButton == ButtonState.Released
        )
            return true;
        return false;
    }

    public bool MouseLeftIsPressed()
    {
        return _currentMouseState.LeftButton == ButtonState.Pressed;
    }
}
