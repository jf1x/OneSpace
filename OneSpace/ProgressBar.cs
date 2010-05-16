using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OneSpace
{
    public class ProgressBar
    {
        public Rectangle Size;
        public Vector2 Position;
        public float Angle;
        public int Value;
        public int CurrentValue;
        public bool EndMarks = true;
        public int MidMarkInterval = -1;
        public Color Colour;

        public bool DrawBorder = true;

        public Vector2 TopLeft;
        public Vector2 TopRight;
        public Vector2 LowerLeft;
        public Vector2 LowerRight;

        public ProgressBar(Vector2 Pos, int Width, int Height, float Rot, int Value, Color C)
        {
            Size = new Rectangle(0, 0, Width, Height);
            Position = Pos;
            UpdateAngle(Rot);
            this.Value = Value;
            ChangeColour(C);
        }

        public void UpdateAngle(float a)
        {
            Angle = a;

            Vector2 origTopLeft = new Vector2(-Size.Width / 2, -Size.Height /2);
            Vector2 origTopRight = new Vector2(Size.Width / 2, -Size.Height / 2);
            Vector2 origLowerLeft = new Vector2(-Size.Width / 2, Size.Height / 2);
            Vector2 origLowerRight = new Vector2(Size.Width / 2, Size.Height / 2);

            TopLeft = Vector2.Transform(origTopLeft, Matrix.CreateRotationZ(MathHelper.ToRadians(Angle)));
            TopRight = Vector2.Transform(origTopRight, Matrix.CreateRotationZ(MathHelper.ToRadians(Angle)));
            LowerLeft = Vector2.Transform(origLowerLeft, Matrix.CreateRotationZ(MathHelper.ToRadians(Angle)));
            LowerRight = Vector2.Transform(origLowerRight, Matrix.CreateRotationZ(MathHelper.ToRadians(Angle)));

            TopLeft += Position;
            TopRight += Position;
            LowerLeft += Position;
            LowerRight += Position;
        }

        public void ChangeColour(Color C)
        {
            Colour = C;
            Colour.A = 150;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Color borderColour = Color.White;
            Color borderColour = Colour;
            borderColour.A = 255;

            spriteBatch.Draw(
                Laser.Pixel,
                new Rectangle((int)(TopLeft.X), (int)(TopLeft.Y), (int)((float)Size.Width * ((float)CurrentValue / (float)Value)), Size.Height),
                new Rectangle(0, 0, 1, 1),
                Colour,
                MyMath.ToAngle(TopRight - TopLeft),
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);

            if (!DrawBorder) return;

            spriteBatch.Draw(
                Laser.Pixel,
                new Rectangle((int)(TopLeft.X), (int)(TopLeft.Y), Size.Width, 1),
                new Rectangle(0, 0, 1, 1),
                borderColour,
                MyMath.ToAngle(TopRight - TopLeft),
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);

            spriteBatch.Draw(
                Laser.Pixel,
                new Rectangle((int)(TopRight.X), (int)(TopRight.Y), Size.Height, 1),
                new Rectangle(0, 0, 1, 1),
                borderColour,
                MyMath.ToAngle(LowerRight - TopRight),
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);

            spriteBatch.Draw(
                Laser.Pixel,
                new Rectangle((int)(LowerRight.X), (int)(LowerRight.Y), Size.Width, 1),
                new Rectangle(0, 0, 1, 1),
                borderColour,
                MyMath.ToAngle(LowerLeft - LowerRight),
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);

            spriteBatch.Draw(
                Laser.Pixel,
                new Rectangle((int)(LowerLeft.X), (int)(LowerLeft.Y), Size.Height, 1),
                new Rectangle(0, 0, 1, 1),
                borderColour,
                MyMath.ToAngle(TopLeft - LowerLeft),
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);

            if (!EndMarks) return;

            spriteBatch.Draw(
                Laser.Pixel,
                new Rectangle((int)(LowerLeft.X), (int)(LowerLeft.Y), 3, 1),
                new Rectangle(0, 0, 1, 1),
                borderColour,
                MyMath.ToAngle(LowerLeft - TopLeft),
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);

            spriteBatch.Draw(
                Laser.Pixel,
                new Rectangle((int)(LowerRight.X), (int)(LowerRight.Y), 3, 1),
                new Rectangle(0, 0, 1, 1),
                borderColour,
                MyMath.ToAngle(LowerRight - TopRight),
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);

            if (MidMarkInterval == -1) return;

            for (int i = MidMarkInterval; i < Value; i += MidMarkInterval)
            {
                spriteBatch.Draw(
                    Laser.Pixel,
                    new Rectangle((int)(MathHelper.Lerp(LowerLeft.X, LowerRight.X, i / (float)Value)), (int)(MathHelper.Lerp(LowerLeft.Y, LowerRight.Y, i / (float)Value)), 3, 1),
                    new Rectangle(0, 0, 1, 1),
                    borderColour,
                    MyMath.ToAngle(LowerLeft - TopLeft),
                    Vector2.Zero,
                    SpriteEffects.None,
                    0.0f);
            }

        }
    }
}
