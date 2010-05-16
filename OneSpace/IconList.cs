using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace OneSpace
{
    public class IconList : ProgressBar
    {
        public static Texture2D[] Icons = new Texture2D[7];
        public int Highlighted = -1;

        public Color Faded = new Color(255, 255, 255, 150);
        public Color PlayerC;

        public IconList(Vector2 Pos, int Width, int Height, float Rot, int Value, Color C)
            : base(Pos, Width, Height, Rot, Value, C)
        {
            PlayerC = C;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < 7; i++)
            {
                spriteBatch.Draw(
                    Icons[i],
                    new Rectangle((int)(MathHelper.Lerp(LowerLeft.X, LowerRight.X, i / (float)Value)), (int)(MathHelper.Lerp(LowerLeft.Y, LowerRight.Y, i / (float)Value)), Icons[i].Width, Icons[i].Height),
                    null,
                    (i == Highlighted ? PlayerC : Faded),
                    MathHelper.ToRadians(Angle),
                    new Vector2(Icons[i].Width / 2, Icons[i].Height / 2),
                    SpriteEffects.None,
                    0);
            }
        }
    }
}
