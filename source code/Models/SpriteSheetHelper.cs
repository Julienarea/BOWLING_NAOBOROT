using Microsoft.Xna.Framework;

namespace KeglyaAimer;

public static class SpriteSheetHelper
{
    public static Rectangle[] GetAnimationFrames(
        int frameWidth,
        int frameHeight,
        int startRow,
        int startColumn,
        int frameCount,
        int sheetWidth // в пикселях
    )
    {
        Rectangle[] frames = new Rectangle[frameCount];
        int framesPerRow = sheetWidth / frameWidth;
        for (int i = 0; i < frameCount; i++)
        {
            int col = (startColumn + i) % framesPerRow;
            int row = startRow + (startColumn + i) / framesPerRow;
            frames[i] = new Rectangle(col * frameWidth, row * frameHeight, frameWidth, frameHeight);
        }
        return frames;
    }
}
