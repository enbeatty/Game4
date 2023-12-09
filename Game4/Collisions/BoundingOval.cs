using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game4.Collisions
{
    public struct BoundingOval
    {
        /// <summary>
        /// The center of the BoundingOval
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// The Ovals a position
        /// </summary>
        public float SemiMajor;

        /// <summary>
        /// The Ovals b position
        /// </summary>
        public float SemiMinor;

        /// <summary>
        /// Constructs a new BoundingOval
        /// </summary>
        /// <param name="center">The Center</param>
        /// <param name="semiMajor">The a of oval</param>
        /// <param name="semiMinor">The b of oval</param>
        public BoundingOval(Vector2 center, float semiMajor, float semiMinor)
        {
            Center = center;
            SemiMajor = semiMajor;
            SemiMinor = semiMinor;

        }

        /// <summary>
        /// Tests for a collision between this and another bounding Oval
        /// </summary>
        /// <param name="other">The other bounding circle</param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingOval other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

    }
}
