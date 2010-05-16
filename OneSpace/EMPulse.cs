using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Thanks to http://profile.xna.com/profile.aspx?crn=MessiahAndrw
//for help with the circle drawing code.
//Original post stating his permission for anyone to use: http://forums.xna.com/forums/p/7414/39338.aspx#39338

namespace OneSpace
{
    public class EMPulse
    {
        public int Range;
        public Player myPlayer;
        public Timer timeToLive = new Timer();
        public List<Vector2> CircleBits = new List<Vector2>();
        public Color Colour;
        public bool DrawMe = false;

        public EMPulse(Player P)
        {
            myPlayer = P;
        }

        public void Update(GameTime gameTime)
        {
            if (!timeToLive.HasTimeElapsed(gameTime, 2000))
            {
                Colour = Color.Lerp(new Color(128, 0, 128, 255), new Color(128, 0, 128, 0), timeToLive.TimeElapsed(gameTime) / 2000f);
            }
            else
                DrawMe = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (CircleBits.Count < 2 || !DrawMe)
                return;

            for (int i = 1; i < CircleBits.Count; i++)
            {
                Vector2 vector1 = (Vector2)CircleBits[i - 1];
                Vector2 vector2 = (Vector2)CircleBits[i];

                // calculate the distance between the two vectors
                float distance = Vector2.Distance(vector1, vector2);

                // calculate the angle between the two vectors
                float angle = (float)Math.Atan2((double)(vector2.Y - vector1.Y),
                    (double)(vector2.X - vector1.X));

                // stretch the pixel between the two vectors
                spriteBatch.Draw(
                    Laser.Pixel,
                    myPlayer.MotherShipVec + vector1,
                    null,
                    Colour,
                    angle,
                    Vector2.Zero,
                    new Vector2(distance, 1),
                    SpriteEffects.None,
                    0);
            }
        }

        public void Blast(int R, GameTime gameTime)
        {
            Range = R;

            timeToLive.Reset(gameTime);
            DrawMe = true;

            CircleBits.Clear();

            float max = 2 * (float)Math.PI;
            float step = max / (float)64;

            for (float theta = 0; theta < max; theta += step)
            {
                CircleBits.Add(new Vector2(Range * (float)Math.Cos((double)theta),
                    Range * (float)Math.Sin((double)theta)));
            }

            // then add the first vector again so it's a complete loop
            CircleBits.Add(new Vector2(Range * (float)Math.Cos(0),
                    Range * (float)Math.Sin(0)));

            //Kill ships within radius
            foreach (Ship s in OneSpace.theGame.AllShips)
            {
                if (Vector2.Distance(s.Position, myPlayer.MotherShipVec) < Range)
                {
                    s.HitPoints -= 50;
                }
            }
        }
    }
}
