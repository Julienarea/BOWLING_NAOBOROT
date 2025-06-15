using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KeglyaAimer;

public class TargetView
{
    private TargetModel _targetModel;
    private Color _color;
    private Texture2D _texture;
    private GraphicsDevice _graphicsDevice;

    public TargetView(
        TargetModel targetModel,
        Color color,
        Texture2D texture,
        GraphicsDevice graphicsDevice
    )
    {
        _targetModel = targetModel;
        _color = color;
        _texture = texture;
        _graphicsDevice = graphicsDevice;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var model = _targetModel;

        Rectangle[] frames = SpriteSheetHelper.GetAnimationFrames(
            model.FrameWidth,
            model.FrameHeight,
            model.AnimationRow,
            model.AnimationColumn,
            model.FramesCount,
            _texture.Width
        );

        float scale = (model.Radius * 2) / model.FrameWidth + 1.25f;

        spriteBatch.Draw(
            _texture,
            model.Position,
            frames[model.CurrentFrame],
            Color.White,
            model.Rotation,
            new Vector2(model.FrameWidth / 2f, model.FrameHeight / 2f),
            scale,
            SpriteEffects.None,
            0f
        );
    }
}
