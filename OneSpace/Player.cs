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
        public static Texture2D _ESPIndicator;

        public List<Ship> Ships = new List<Ship>();

        public float DamageModifier = 1.0f;
        public float SpeedModifier = 1.0f;
        public int EMChargeLevel = 50;

        public int MotherShipHP = 1000;
        public float MotherShipPos = 0.0f;
        public float MotherShipAng = 0.0f;
        public Vector2 MotherShipVec;
        public Texture2D MotherShipTex;

        public bool Alive = true;

        public float rotationSpeed = 0.0003f;

        public int myIndex;
        public Keys myKey;
        public Color Colour;
        public bool Human;
        public int Rank = 0;

        public Timer KeyPressTimer = new Timer();

        public Level myLevel;

        public ProgressBar HealthBar;
        public ProgressBar ChargeBar;
        public IconList IconBar;
        public ProgressBar SpeedBar;
        public ProgressBar PowerBar;
        public ProgressBar EMBar;

        public EMPulse EMPulse;

        public bool AI_Charging = false;
        public int AI_HoldTime = 0;

        public float[] TimePeriods = { 500f, 2000f, 4500f, 7000f, 10000f, 13000f };

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

            EMPulse = new EMPulse(this);

            MotherShipVec = new Vector2(0, -280);
            MotherShipVec = Vector2.Transform(MotherShipVec, Matrix.CreateRotationZ(MotherShipPos));
            MotherShipVec.X += OneSpace.theGame.GraphicsDevice.Viewport.Width / 2; 
            MotherShipVec.Y += OneSpace.theGame.GraphicsDevice.Viewport.Height / 2;

            HealthBar = new ProgressBar(Vector2.Zero, 100, 10, MotherShipAng, MotherShipHP, Color.Red);
            HealthBar.EndMarks = false;
            HealthBar.Position = new Vector2(0, -316);
            HealthBar.Position = Vector2.Transform(HealthBar.Position, Matrix.CreateRotationZ(MotherShipPos));
            HealthBar.Position.X += OneSpace.theGame.GraphicsDevice.Viewport.Width / 2;
            HealthBar.Position.Y += OneSpace.theGame.GraphicsDevice.Viewport.Height / 2;

            ChargeBar = new ProgressBar(Vector2.Zero, 100, 10, MotherShipAng, 13000, Color.White);
            ChargeBar.MidMarkInterval = 13000 / 6;
            ChargeBar.Position = new Vector2(0, -345);
            ChargeBar.Position = Vector2.Transform(ChargeBar.Position, Matrix.CreateRotationZ(MotherShipPos));
            ChargeBar.Position.X += OneSpace.theGame.GraphicsDevice.Viewport.Width / 2;
            ChargeBar.Position.Y += OneSpace.theGame.GraphicsDevice.Viewport.Height / 2;

            EMBar = new ProgressBar(Vector2.Zero, 100, 4, MotherShipAng, 100, Color.Purple);
            EMBar.DrawBorder = false;
            EMBar.Position = new Vector2(0, -325);
            EMBar.Position = Vector2.Transform(EMBar.Position, Matrix.CreateRotationZ(MotherShipPos));
            EMBar.Position.X += OneSpace.theGame.GraphicsDevice.Viewport.Width / 2;
            EMBar.Position.Y += OneSpace.theGame.GraphicsDevice.Viewport.Height / 2;

            SpeedBar = new ProgressBar(Vector2.Zero, 100, 4, MotherShipAng, 5, Color.LightBlue);
            SpeedBar.DrawBorder = false;
            SpeedBar.Position = new Vector2(0, -330);
            SpeedBar.Position = Vector2.Transform(SpeedBar.Position, Matrix.CreateRotationZ(MotherShipPos));
            SpeedBar.Position.X += OneSpace.theGame.GraphicsDevice.Viewport.Width / 2;
            SpeedBar.Position.Y += OneSpace.theGame.GraphicsDevice.Viewport.Height / 2;

            PowerBar = new ProgressBar(Vector2.Zero, 100, 4, MotherShipAng, 5, Color.Red);
            PowerBar.DrawBorder = false;
            PowerBar.Position = new Vector2(0, -335);
            PowerBar.Position = Vector2.Transform(PowerBar.Position, Matrix.CreateRotationZ(MotherShipPos));
            PowerBar.Position.X += OneSpace.theGame.GraphicsDevice.Viewport.Width / 2;
            PowerBar.Position.Y += OneSpace.theGame.GraphicsDevice.Viewport.Height / 2;

            IconBar = new IconList(Vector2.Zero, 100, 10, MotherShipAng, 6, Colour);
            IconBar.MidMarkInterval = 1;
            IconBar.Position = new Vector2(0, -360);
            IconBar.Position = Vector2.Transform(IconBar.Position, Matrix.CreateRotationZ(MotherShipPos));
            IconBar.Position.X += OneSpace.theGame.GraphicsDevice.Viewport.Width / 2;
            IconBar.Position.Y += OneSpace.theGame.GraphicsDevice.Viewport.Height / 2;
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
                    for (int j = 0; j < OneSpace.Random.Next(8, 15); j++)
                        myLevel.Particles.Add(new Particle(Colour, Ships[i].Position, 2, gameTime));

                    OneSpace.theGame.AllShips.Remove(Ships[i]);
                    Ships.RemoveAt(i);
                    i--;

                    Audio.PlayCue("explosion");
                }
            }

            #region Rotate Mothership & Bars

            MotherShipVec = RotateStuff(MotherShipVec);
            MotherShipAng = GetFacingAngle(MotherShipVec);
            HealthBar.Position = RotateStuff(HealthBar.Position);
            ChargeBar.Position = RotateStuff(ChargeBar.Position);
            IconBar.Position = RotateStuff(IconBar.Position);
            SpeedBar.Position = RotateStuff(SpeedBar.Position);
            PowerBar.Position = RotateStuff(PowerBar.Position);
            EMBar.Position = RotateStuff(EMBar.Position);


            #endregion

            int TimeHeld = 0;

            if (MotherShipHP > 0)
            {
                //AI Stuff
                if (!Human && !AI_Charging)
                {
                    AI_HoldTime = 4;
                    while(AI_HoldTime == 4 || (AI_HoldTime == 5 && DamageModifier > 1.95f) || (AI_HoldTime == 6 && SpeedModifier > 1.95f))
                        AI_HoldTime = OneSpace.Random.Next(0, 6);

                    KeyPressTimer.Reset(gameTime);
                    AI_Charging = true;
                }

                if (!Human && AI_Charging)
                {
                    TimeHeld = (int)(KeyPressTimer.TimeElapsed(gameTime) * SpeedModifier);

                    if (AI_HoldTime == 0)
                    {
                        ProduceSomething(TimeHeld, gameTime);
                        AI_Charging = false;
                    }
                    else if (KeyPressTimer.TimeElapsed(gameTime) > TimePeriods[AI_HoldTime] / SpeedModifier)
                    {
                        ProduceSomething(TimeHeld, gameTime);
                        AI_Charging = false;
                    }
                }

                //Button Press Down
                if (Human && ((OneSpace.theGame.NewGamePadState[myIndex].Buttons.A == ButtonState.Pressed &&
                    OneSpace.theGame.OldGamePadState[myIndex].Buttons.A == ButtonState.Released) ||
                    (OneSpace.theGame.NewKeyboardState.IsKeyDown(myKey) &&
                    OneSpace.theGame.OldKeyboardState.IsKeyUp(myKey))))
                {
                    KeyPressTimer.Reset(gameTime);
                }

                
                if (Human && (OneSpace.theGame.NewGamePadState[myIndex].Buttons.A == ButtonState.Pressed ||
                    OneSpace.theGame.NewKeyboardState.IsKeyDown(myKey)))
                    TimeHeld = (int)(KeyPressTimer.TimeElapsed(gameTime) * SpeedModifier);

                //Button Press Release
                if (Human && ((OneSpace.theGame.NewGamePadState[myIndex].Buttons.A == ButtonState.Released &&
                    OneSpace.theGame.OldGamePadState[myIndex].Buttons.A == ButtonState.Pressed) ||
                    (OneSpace.theGame.NewKeyboardState.IsKeyUp(myKey) &&
                    OneSpace.theGame.OldKeyboardState.IsKeyDown(myKey))))
                {
                    TimeHeld = (int)(KeyPressTimer.TimeElapsed(gameTime) * SpeedModifier);

                    ProduceSomething(TimeHeld, gameTime);
                }
            }

            if (TimeHeld > 13000) TimeHeld = 13000;

                //Ship s = new Ship(MotherShipVec, MotherShipAng, ShipType.Large, myLevel, this);
                //Ships.Add(s);
                //OneSpace.theGame.AllShips.Add(s);

            MyMath.ClampAngleToRange(MotherShipAng);

            //Update Bar Positions
            HealthBar.UpdateAngle(MathHelper.ToDegrees(MotherShipAng));
            HealthBar.CurrentValue = MotherShipHP;
            HealthBar.ChangeColour(Color.Lerp(Color.Red, Color.Green, MotherShipHP / 1000.0f));

            ChargeBar.UpdateAngle(MathHelper.ToDegrees(MotherShipAng));
            IconBar.UpdateAngle(MathHelper.ToDegrees(MotherShipAng));
            SpeedBar.UpdateAngle(MathHelper.ToDegrees(MotherShipAng));
            PowerBar.UpdateAngle(MathHelper.ToDegrees(MotherShipAng));
            EMBar.UpdateAngle(MathHelper.ToDegrees(MotherShipAng));

            //Set charge bar based on time held
            if (TimeHeld == 0)
            {
                ChargeBar.CurrentValue = 0;
                IconBar.Highlighted = -1;
            }
            else if (TimeHeld < TimePeriods[0])
            {
                ChargeBar.CurrentValue = (int)(TimeHeld / TimePeriods[0] * ChargeBar.MidMarkInterval);
                IconBar.Highlighted = 0;
            }
            else if (TimeHeld < TimePeriods[1])
            {
                ChargeBar.CurrentValue = (int)(ChargeBar.MidMarkInterval + (((TimeHeld - TimePeriods[0]) / (TimePeriods[1] - TimePeriods[0])) * ChargeBar.MidMarkInterval));
                IconBar.Highlighted = 1;
            }
            else if (TimeHeld < TimePeriods[2])
            {
                ChargeBar.CurrentValue = (int)(ChargeBar.MidMarkInterval * 2 + (((TimeHeld - TimePeriods[1]) / (TimePeriods[2] - TimePeriods[1])) * ChargeBar.MidMarkInterval));
                IconBar.Highlighted = 2;
            }
            else if (TimeHeld < TimePeriods[3])
            {
                ChargeBar.CurrentValue = (int)(ChargeBar.MidMarkInterval * 3 + (((TimeHeld - TimePeriods[2]) / (TimePeriods[3] - TimePeriods[2])) * ChargeBar.MidMarkInterval));
                IconBar.Highlighted = 3;
            }
            else if (TimeHeld < TimePeriods[4])
            {
                ChargeBar.CurrentValue = (int)(ChargeBar.MidMarkInterval * 4 + (((TimeHeld - TimePeriods[3]) / (TimePeriods[4] - TimePeriods[3])) * ChargeBar.MidMarkInterval));
                IconBar.Highlighted = 4;
            }
            else if (TimeHeld < TimePeriods[5])
            {
                ChargeBar.CurrentValue = (int)(ChargeBar.MidMarkInterval * 5 + (((TimeHeld - TimePeriods[4]) / (TimePeriods[5] - TimePeriods[4])) * ChargeBar.MidMarkInterval));
                IconBar.Highlighted = 5;
            }
            else
            {
                ChargeBar.CurrentValue = (int)TimePeriods[5];
                IconBar.Highlighted = 6;
            }

            EMPulse.Update(gameTime);
        }

        private void ProduceSomething(int TimeHeld, GameTime gameTime)
        {
            if (TimeHeld < TimePeriods[0])
            {
                if (EMChargeLevel < 150)
                {
                    EMChargeLevel += 3;
                    EMBar.CurrentValue += 3;
                }
            }
            else if (TimeHeld < TimePeriods[1])
            {
                Ship s = new Ship(MotherShipVec, MotherShipAng, ShipType.Small, myLevel, this);
                Ships.Add(s);
                OneSpace.theGame.AllShips.Add(s);
            }
            else if (TimeHeld < TimePeriods[2])
            {
                Ship s = new Ship(MotherShipVec, MotherShipAng, ShipType.Medium, myLevel, this);
                Ships.Add(s);
                OneSpace.theGame.AllShips.Add(s);
            }
            else if (TimeHeld < TimePeriods[3])
            {
                Ship s = new Ship(MotherShipVec, MotherShipAng, ShipType.Large, myLevel, this);
                Ships.Add(s);
                OneSpace.theGame.AllShips.Add(s);
            }
            else if (TimeHeld < TimePeriods[4])
            {
                EMPulse.Blast(EMChargeLevel, gameTime);
                EMChargeLevel = 50;
                EMBar.CurrentValue = 0;
            }
            else if (TimeHeld < TimePeriods[5])
            {
                if (DamageModifier < 1.95f)
                {
                    DamageModifier += 0.2f;
                    PowerBar.CurrentValue++;
                }
            }
            else
            {
                if (SpeedModifier < 1.95f)
                {
                    SpeedModifier += 0.2f;
                    SpeedBar.CurrentValue++;
                }
            }
        }

        public Vector2 RotateStuff(Vector2 Pos)
        {
            Pos.X -= OneSpace.theGame.GraphicsDevice.Viewport.Width / 2;
            Pos.Y -= OneSpace.theGame.GraphicsDevice.Viewport.Height / 2;

            Pos = Vector2.Transform(Pos, Matrix.CreateRotationZ(rotationSpeed));

            Pos.X += OneSpace.theGame.GraphicsDevice.Viewport.Width / 2;
            Pos.Y += OneSpace.theGame.GraphicsDevice.Viewport.Height / 2;

            return Pos;
        }

        public float GetFacingAngle(Vector2 Pos)
        {
            Pos.X -= OneSpace.theGame.GraphicsDevice.Viewport.Width / 2;
            Pos.Y -= OneSpace.theGame.GraphicsDevice.Viewport.Height / 2;

            float Ang = (float)Math.Atan2(Pos.Y, Pos.X) - MathHelper.PiOver2;

            Pos.X += OneSpace.theGame.GraphicsDevice.Viewport.Width / 2;
            Pos.Y += OneSpace.theGame.GraphicsDevice.Viewport.Height / 2;

            return Ang;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Alive) return;

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
