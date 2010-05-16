using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace OneSpace
{
    public class Particle
    {
        public Color Colour;
        public Vector2 Position;
        public Vector2 Velocity;
        public int Size;
        public Timer timeToLive = new Timer();

        public Particle(Color C, Vector2 P, int S, GameTime gameTime)
        {
            Colour = C;
            Position = P;
            Size = S;

            while(Velocity.X == 0 && Velocity.Y == 0)
                Velocity = new Vector2((float)OneSpace.Random.NextDouble() * 4f - 2f, (float)OneSpace.Random.NextDouble() * 4f - 2f);

            timeToLive.Reset(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity;
            Colour.A = (byte)MathHelper.Lerp(255, 0, timeToLive.TimeElapsed(gameTime) / 2000f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Laser.Pixel,
                new Rectangle((int)Position.X, (int)Position.Y, Size, Size),
                new Rectangle(0, 0, 1, 1),
                Colour);
        }
    }
}
