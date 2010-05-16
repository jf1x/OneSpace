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

        public List<Particle> Particles = new List<Particle>();

        public int Winner = -1;

        public Level(bool[] Humans)
        {
            Players[0] = new Player(Humans[0], Color.Red, 0f, this, 0); ;
            Players[1] = new Player(Humans[1], new Color(64, 64, 255, 255), MathHelper.PiOver2, this, 1);
            Players[2] = new Player(Humans[2], Color.Green, MathHelper.Pi, this, 2);
            Players[3] = new Player(Humans[3], Color.Yellow, -MathHelper.PiOver2, this, 3);
        }

        public void Update(GameTime gameTime)
        {
            //Update level stuff

            foreach (Player p in Players)
            {
                p.Update(gameTime);
                if (p.MotherShipHP <= 0 && p.Alive == true)
                {
                    for(int i=0; i < 20; i++)
                        Particles.Add(new Particle(p.Colour, p.MotherShipVec, 3, gameTime));

                    Audio.PlayCue("mothershipExplosion");
                    p.Alive = false;
                }

                int AliveCount = 0;
                for (int i = 0; i < 4; i++)
                    if (Players[i].Alive) AliveCount++;

                Winner = -1;
                if (AliveCount == 1)
                    for (int i = 0; i < 4; i++)
                        if (Players[i].Alive) Winner = i;

                if (Winner > -1)
                {

                }
            }

            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i].Update(gameTime);

                if (Particles[i].timeToLive.HasTimeElapsed(gameTime, 2000))
                    Particles.RemoveAt(i--);
            }
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
