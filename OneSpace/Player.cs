using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace OneSpace
{
    public class Player
    {
        public static Texture2D _motherShipTex;

        public List<Ship> Ships = new List<Ship>();

        public float DamageModifier = 1.0f;
        public float SpeedModifier = 1.0f;
        public float EMChargeLevel = 0.0f;

        public int MotherShipHP = 1000;
        public float MotherShipPos = 0.0f;
        public float MotherShipAng = 0.0f;
        public Vector2 MotherShipVec;
        public Texture2D MotherShipTex;

        public int myIndex;
        public Keys myKey;
        public Color Colour;
        public bool Human;
        public int Rank = 0;

        public Timer KeyPressTimer = new Timer();

        public Level myLevel;

        public Player(bool IsHuman, Color C, float start, Level L, int index)
        {
            Human = IsHuman;
            Colour = C;
            MotherShipPos = start;
            MotherShipTex = _motherShipTex;
            myIndex = index;

            if (myIndex == 0) myKey = Keys.A;
            if (myIndex == 1) myKey = Keys.F;
            if (myIndex == 2) myKey = Keys.J;
            if (myIndex == 3) myKey = Keys.Enter;

            myLevel = L;

            MotherShipVec = new Vector2(0, -280);
            MotherShipVec = Vector2.Transform(MotherShipVec, Matrix.CreateRotationZ(MotherShipPos));
            MotherShipVec.X += OneSpace.theGame.GraphicsDevice.Viewport.Width / 2; 
            MotherShipVec.Y += OneSpace.theGame.GraphicsDevice.Viewport.Height / 2;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Ship s in Ships)
            {
                s.Update(gameTime);
            }

            //Check for dead ships
            for (int i = 0; i < Ships.Count; i++)
            {
                if (Ships[i].HitPoints <= 0)
                {
                    OneSpace.theGame.AllShips.Remove(Ships[i]);
                    Ships.RemoveAt(i);
                    i--;
                }
            }

            #region Rotate Mothership
            MotherShipVec.X -= OneSpace.theGame.GraphicsDevice.Viewport.Width / 2;
            MotherShipVec.Y -= OneSpace.theGame.GraphicsDevice.Viewport.Height / 2;

            MotherShipVec = Vector2.Transform(MotherShipVec, Matrix.CreateRotationZ(0.0003f));
            MotherShipAng = (float)Math.Atan2(MotherShipVec.Y, MotherShipVec.X) - MathHelper.PiOver2;

            MotherShipVec.X += OneSpace.theGame.GraphicsDevice.Viewport.Width / 2;
            MotherShipVec.Y += OneSpace.theGame.GraphicsDevice.Viewport.Height / 2;
            #endregion

            //Button Press Down
            if ((OneSpace.theGame.NewGamePadState[myIndex].Buttons.A == ButtonState.Pressed &&
                OneSpace.theGame.OldGamePadState[myIndex].Buttons.A == ButtonState.Released) ||
                (OneSpace.theGame.NewKeyboardState.IsKeyDown(myKey) &&
                OneSpace.theGame.OldKeyboardState.IsKeyUp(myKey)))
            {
                KeyPressTimer.Reset(gameTime);
            }

            //Button Press Release
            if ((OneSpace.theGame.NewGamePadState[myIndex].Buttons.A == ButtonState.Released &&
                OneSpace.theGame.OldGamePadState[myIndex].Buttons.A == ButtonState.Pressed) ||
                (OneSpace.theGame.NewKeyboardState.IsKeyUp(myKey) &&
                OneSpace.theGame.OldKeyboardState.IsKeyDown(myKey)))
            {
                int TimeHeld = KeyPressTimer.TimeElapsed(gameTime);

                if (TimeHeld < 500)
                {

                }
                else if (TimeHeld < 3000)
                {
                    Ship s = new Ship(MotherShipVec, MotherShipAng, ShipType.Small, myLevel, this);
                    Ships.Add(s);
                    OneSpace.theGame.AllShips.Add(s);
                }
                else if (TimeHeld < 6000)
                {
                    Ship s = new Ship(MotherShipVec, MotherShipAng, ShipType.Medium, myLevel, this);
                    Ships.Add(s);
                    OneSpace.theGame.AllShips.Add(s);
                }
                else if (TimeHeld < 10000)
                {
                    Ship s = new Ship(MotherShipVec, MotherShipAng, ShipType.Large, myLevel, this);
                    Ships.Add(s);
                    OneSpace.theGame.AllShips.Add(s);
                }
            }

                //Ship s = new Ship(MotherShipVec, MotherShipAng, ShipType.Large, myLevel, this);
                //Ships.Add(s);
                //OneSpace.theGame.AllShips.Add(s);

            MyMath.ClampAngleToRange(MotherShipAng);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                MotherShipTex,
                new Rectangle((int)MotherShipVec.X, (int)MotherShipVec.Y, MotherShipTex.Width, MotherShipTex.Height),
                null,
                Colour,
                MotherShipAng,
                new Vector2(MotherShipTex.Width / 2, MotherShipTex.Height / 2),
                SpriteEffects.None,
                0.8f);
        }
    }
}
