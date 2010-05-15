using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OneSpace
{
    public class Laser
    {
        public Ship myShip;
        public Timer AliveTimer = new Timer();
        public bool DrawMe = false;
        public Vector2 Target;
        public static Texture2D Pixel;

        public Laser(Ship S)
        {
            myShip = S;
        }

        public void Update(GameTime gameTime)
        {
            if (AliveTimer.HasTimeElapsed(gameTime, 200))
            {
                DrawMe = false;
            }
        }

        public void Reset(GameTime gameTime)
        {
            AliveTimer.Reset(gameTime);
            DrawMe = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!DrawMe) return;
            if (Vector2.Distance(Target, myShip.Position) > myShip.Range) return;

            spriteBatch.Draw(
                Pixel,
                new Rectangle((int)myShip.Position.X, (int)myShip.Position.Y, (int)Vector2.Distance(myShip.Position, Target), 1),
                new Rectangle(0, 0, 1, 1),
                myShip.myPlayer.Colour,
                MyMath.ToAngle(Target - myShip.Position),
                Vector2.Zero,
                SpriteEffects.None,
                0.9f);
        }
    }
}
