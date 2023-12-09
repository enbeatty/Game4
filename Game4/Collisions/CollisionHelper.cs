using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Game4.Collisions
{
    public static class CollisionHelper
    {
        /// <summary>
        /// Detects a collision between two BoundingCircles
        /// </summary>
        /// <param name="a">The first bounding circle</param>
        /// <param name="b">The second bounding circle</param>
        /// <returns>true for collision, false otherwise</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >= Math.Pow(a.Center.X - b.Center.X, 2) + Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Detects a collision between two BoundingRectangles
        /// </summary>
        /// <param name="a">The first rectangle</param>
        /// <param name="b">The second rectangle</param>
        /// <returns>true for collision, false otherwise</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right || a.Top > b.Bottom || a.Bottom < b.Top);
        }

        public static bool Collides(BoundingDip a, BoundingDip b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >= Math.Pow(a.Center.X - b.Center.X, 2) + Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Detects a collision between a rectangle and a circle
        /// </summary>
        /// <param name="c">the BoundingCircle</param>
        /// <param name="r">the BoundingRectangle</param>
        /// <returns>true for collision, flase otherwise</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);

            return Math.Pow(c.Radius, 2) >= Math.Pow(c.Center.X - nearestX, 2) + Math.Pow(c.Center.Y - nearestY, 2);
        }

        public static bool Collides(BoundingRectangle r, BoundingCircle c)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);

            return Math.Pow(c.Radius, 2) >= Math.Pow(c.Center.X - nearestX, 2) + Math.Pow(c.Center.Y - nearestY, 2);
        }

        public static bool Collides(BoundingDip d, BoundingCircle c)
        {
            return c.Center.Y >= (d.Center.Y) + (float)Math.Sqrt(65536f - (float)Math.Pow(c.Center.X - (d.Center.X + 128), 2));
            //return Math.Pow(d.Radius + c.Radius - 32, 2) >= Math.Pow(d.Center.X - c.Center.X, 2) + Math.Pow(d.Center.Y - c.Center.Y, 2);
            //return Math.Pow(d.Radius + c.Radius, 2) >= Math.Pow(a.Center.X - b.Center.X, 2) + Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        public static bool Collides(BoundingCircle c, BoundingDip d)
        {
            double part = 16384 - Math.Pow(c.Center.X - (d.Center.X), 2);
            var test = (d.Center.Y) + (float)Math.Sqrt(part);
            return c.Center.Y + 32 >= (d.Center.Y) + (float)Math.Sqrt(65536f - (float)Math.Pow(c.Center.X - (d.Center.X), 2));
        }

        public static bool Collides(BoundingOval a, BoundingOval b)
        {
            float distanceX = b.Center.X - a.Center.X;
            float distanceY = b.Center.Y - a.Center.Y;

            double sumRadiiSquared = Math.Pow(a.SemiMajor + b.SemiMajor, 2) + Math.Pow(a.SemiMinor + b.SemiMinor, 2);
            double distanceSquared = (Math.Pow(distanceX, 2) / Math.Pow(a.SemiMajor + b.SemiMajor, 2)) + (Math.Pow(distanceY, 2) / Math.Pow(a.SemiMinor + b.SemiMinor, 2));

            return distanceSquared <= sumRadiiSquared;
        }

        public static bool Collides(BoundingOval o, BoundingRectangle r)
        {
            double ovalRadius = Math.Sqrt(Math.Pow(o.SemiMajor, 2) + Math.Pow(o.SemiMinor, 2)) / 2;

            double circleDistanceX = Math.Abs(o.Center.X - r.X - r.Width / 2);
            double circleDistanceY = Math.Abs(o.Center.Y - r.Y - r.Height / 2);

            if (circleDistanceX > (r.Width / 2 + ovalRadius)) return false;
            if (circleDistanceY > (r.Height / 2 + ovalRadius)) return false;

            if (circleDistanceX <= (r.Width / 2)) return true;
            if (circleDistanceY <= (r.Height / 2)) return true;

            double cornerDistanceSquared = Math.Pow(circleDistanceX - r.Width / 2, 2) + Math.Pow(circleDistanceY - r.Height / 2, 2);
            return cornerDistanceSquared <= Math.Pow(ovalRadius, 2);
        }
    }
}
