using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OneSpace
{
    public static class MyMath
    {
        public static float ClampAngleToRange(float angle)
        {
            if (angle < -MathHelper.Pi)
                angle = MathHelper.Pi - (-MathHelper.Pi - angle);

            if (angle > MathHelper.Pi)
                angle = -MathHelper.Pi + (angle - MathHelper.Pi);

            return angle;
        }

        public static Vector2 ToVector2(float angle)
        {
            Vector2 direction = Vector2.Zero;
            direction.X = (float)Math.Cos(angle);
            direction.Y = (float)Math.Sin(angle);
            return direction;
        }

        public static float ToAngle(Vector2 direction)
        {
            direction.Normalize();
            return (float)Math.Atan2(direction.X, direction.Y);
        }

        public static float ToRelAngle(Vector2 To, Vector2 From)
        {
            return (float)(Math.Atan2(To.X, To.Y) - Math.Atan2(From.X, From.Y));
        }

        public static float AngleBetweenVectors(Vector2 One, Vector2 Two)
        {
            return (float)(Math.Acos(Vector2.Dot(Vector2.Normalize(One), Vector2.Normalize(Two))));
        }

        public static float AngleBetweenAngles(float One, float Two)
        {
            return (float)(Math.Acos(Vector2.Dot(Vector2.Normalize(ToVector2(One)), Vector2.Normalize(ToVector2(Two)))));
        }
    }
}
