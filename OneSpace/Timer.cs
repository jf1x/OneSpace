using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OneSpace
{
    public class Timer
    {
        double currentMilliseconds;
        double startMilliseconds = -1;

        public Timer()
        {
        }

        public void Update(GameTime gameTime)
        {
            currentMilliseconds = gameTime.TotalGameTime.TotalMilliseconds;
        }

        public void Reset(GameTime gameTime)
        {
            startMilliseconds = gameTime.TotalGameTime.TotalMilliseconds;
        }

        public bool HasTimeElapsed(GameTime gameTime, int ms)
        {
            Update(gameTime);
            return (currentMilliseconds - startMilliseconds >= ms);
        }

        public int TimeElapsed(GameTime gameTime)
        {
            Update(gameTime);
            return (int)(currentMilliseconds - startMilliseconds);
        }
    }
}
