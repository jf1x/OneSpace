using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OneSpace
{
    public enum ShipType
    {
        Small,
        Medium,
        Large
    }

    public class Ship
    {
        public static Texture2D _smallShipTex;
        public static Texture2D _mediumShipTex;
        public static Texture2D _largeShipTex;

        public Vector2 Position;
        public Vector2 TargetPosition = Vector2.Zero;
        public Vector2 Velocity = Vector2.Zero;
        public float MaxVelocity;
        public float AccelerationForce;

        public Timer FiringTimer = new Timer();
        public int FiringRate;
        public int AttackPower;
        public int HitPoints;
        public int Range;

        public Texture2D shipTex;

        public ShipType Type;
        public Laser Laser;

        public Level myLevel;
        public Player myPlayer;

        public Player targetedMothership = null;
        public Ship targetedShip = null;

        public Ship(Vector2 initPos, float initRot, ShipType T, Level L, Player P)
        {
            Position = initPos;            

            Type = T;
            myLevel = L;
            myPlayer = P;

            Laser = new Laser(this);

            switch (Type)
            {
                case ShipType.Small:
                    MaxVelocity = 3.50f;
                    AccelerationForce = 0.05f;
                    FiringRate = 750;
                    AttackPower = 1;
                    HitPoints = 5;
                    Range = 100;
                    shipTex = _smallShipTex;
                    break;

                case ShipType.Medium:
                    MaxVelocity = 1.50f;
                    AccelerationForce = 0.03f;
                    FiringRate = 1250;
                    AttackPower = 3;
                    HitPoints = 15;
                    Range = 100;
                    shipTex = _mediumShipTex;
                    break;

                case ShipType.Large:
                    MaxVelocity = 1.00f;
                    AccelerationForce = 0.02f;
                    FiringRate = 2000;
                    AttackPower = 8;
                    HitPoints = 30;
                    Range = 100;
                    shipTex = _largeShipTex;
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            //Update Laser
            Laser.Update(gameTime);

            //Get Target
            TargetPosition = new Vector2(99999, 99999);

            Ship oldS = targetedShip;
            Player oldP = targetedMothership;

            foreach (Player p in myLevel.Players)
            {
                if (p.myIndex == myPlayer.myIndex) continue;

                if (Vector2.Distance(Position, p.MotherShipVec) < Vector2.Distance(Position, TargetPosition) && p.MotherShipHP > 0)
                {
                    TargetPosition = p.MotherShipVec;
                    targetedMothership = p;
                    targetedShip = null;
                }

                foreach (Ship s in p.Ships)
                {
                    if (Vector2.Distance(Position, s.Position) < Vector2.Distance(Position, TargetPosition))
                    {
                        TargetPosition = s.Position;
                        targetedMothership = null;
                        targetedShip = s;
                    }
                }
            }

            //Laser Position is always updated, even if it's not being drawn
            Laser.Target = TargetPosition;

            //Accelerate in its direction
            Vector2 AccDir = Vector2.Normalize(TargetPosition - Position) * AccelerationForce;

            //If in range, fire!
            if (Vector2.Distance(Position, TargetPosition) < Range)
            {
                AccDir *= -0.5f;

                if (FiringTimer.HasTimeElapsed(gameTime, FiringRate))
                {
                    if (targetedMothership != null) targetedMothership.MotherShipHP -= AttackPower;
                    if (targetedShip != null) targetedShip.HitPoints -= AttackPower;
                    Laser.Reset(gameTime);
                    FiringTimer.Reset(gameTime);
                }
            }

            //Move ship
            Velocity += AccDir;
            Position += Velocity;

            //Cap Velocity
            if (Vector2.Distance(Vector2.Zero, Velocity) > MaxVelocity)
                Velocity = Vector2.Normalize(Velocity) * MaxVelocity;

        }

        public void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                shipTex,
                new Rectangle((int)Position.X, (int)Position.Y, shipTex.Width, shipTex.Height),
                null,
                myPlayer.Colour,
                0f,
                new Vector2(shipTex.Width / 2, shipTex.Height / 2),
                SpriteEffects.None,
                0.9f);

        }
    }
}
