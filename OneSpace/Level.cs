using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace OneSpace
{
    public class Level
    {
        public static Texture2D BackgroundTex;

        public const int ShipRadius = 200;
        
        public Player[] Players = new Player[4];

        public Level(bool[] Humans)
        {
            Players[0] = new Player(Humans[0], Color.Red, 0f, this, 0); ;
            Players[1] = new Player(Humans[1], Color.Blue, MathHelper.PiOver2, this, 1);
            Players[2] = new Player(Humans[2], Color.Green, MathHelper.Pi, this, 2);
            Players[3] = new Player(Humans[3], Color.Yellow, -MathHelper.PiOver2, this, 3);
        }

        public void Update(GameTime gameTime)
        {
            //Update level stuff

            foreach (Player p in Players)
                p.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(
                BackgroundTex,
                new Rectangle(0, 0, OneSpace.theGame.GraphicsDevice.Viewport.Width, OneSpace.theGame.GraphicsDevice.Viewport.Height),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                1.0f);

            foreach (Player p in Players)
                p.Draw(spriteBatch);
        }
    }
}
