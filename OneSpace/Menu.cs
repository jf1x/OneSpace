using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace OneSpace
{
    public class Menu
    {
        public static Texture2D _logos;

        public bool MenuOn = true;

        public Timer startTimer = new Timer();
        public ProgressBar timerBar = new ProgressBar(new Vector2(512, 605), 430, 20, 0, 10000, Color.White);


        public bool[] Humans = { false, false, false, false };
        public Color TextColour;

        public Menu()
        {
            timerBar.EndMarks = false;
        }

        public void Update(GameTime gameTime)
        {
            if ((OneSpace.theGame.NewGamePadState[0].Buttons.A == ButtonState.Pressed &&
                OneSpace.theGame.OldGamePadState[0].Buttons.A == ButtonState.Released) ||
                (OneSpace.theGame.NewKeyboardState.IsKeyDown(Keys.A) &&
                OneSpace.theGame.OldKeyboardState.IsKeyUp(Keys.A)))
            {
                Humans[0] = !Humans[0];
                Audio.PlayCue("blip");
            }

            if ((OneSpace.theGame.NewGamePadState[1].Buttons.A == ButtonState.Pressed &&
                OneSpace.theGame.OldGamePadState[1].Buttons.A == ButtonState.Released) ||
                (OneSpace.theGame.NewKeyboardState.IsKeyDown(Keys.F) &&
                OneSpace.theGame.OldKeyboardState.IsKeyUp(Keys.F)))
            {
                Humans[1] = !Humans[1];
                Audio.PlayCue("blip");
            }

            if ((OneSpace.theGame.NewGamePadState[2].Buttons.A == ButtonState.Pressed &&
                OneSpace.theGame.OldGamePadState[2].Buttons.A == ButtonState.Released) ||
                (OneSpace.theGame.NewKeyboardState.IsKeyDown(Keys.J) &&
                OneSpace.theGame.OldKeyboardState.IsKeyUp(Keys.J)))
            {
                Humans[2] = !Humans[2];
                Audio.PlayCue("blip");
            }

            if ((OneSpace.theGame.NewGamePadState[3].Buttons.A == ButtonState.Pressed &&
                OneSpace.theGame.OldGamePadState[3].Buttons.A == ButtonState.Released) ||
                (OneSpace.theGame.NewKeyboardState.IsKeyDown(Keys.Enter) &&
                OneSpace.theGame.OldKeyboardState.IsKeyUp(Keys.Enter)))
            {
                Humans[3] = !Humans[3];
                Audio.PlayCue("blip");
            }

            int timePassed = startTimer.TimeElapsed(gameTime);
            timerBar.CurrentValue = 10000 - timePassed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Level.BackgroundTex,
                new Rectangle(0, 0, OneSpace.theGame.GraphicsDevice.Viewport.Width, OneSpace.theGame.GraphicsDevice.Viewport.Height),
                Color.White);

            spriteBatch.Draw(
                _logos,
                new Rectangle(0, 0, OneSpace.theGame.GraphicsDevice.Viewport.Width, OneSpace.theGame.GraphicsDevice.Viewport.Height),
                Color.White);

            timerBar.Draw(spriteBatch);

            for (int i = 0; i < 4; i++)
            {
                TextColour = Color.White;
                if (Humans[i])
                {
                    if (i == 0) TextColour = Color.Red;
                    if (i == 1) TextColour = new Color(64, 64, 255, 255);
                    if (i == 2) TextColour = Color.Green;
                    if (i == 3) TextColour = Color.Yellow;
                }

                spriteBatch.DrawString(
                    OneSpace.theGame.spriteFont,
                    (Humans[i] ? "P" + (i+1).ToString() : "AI"),
                    new Vector2(260 + (i * 158), 500),
                    TextColour);
            }
        }
    }
}
