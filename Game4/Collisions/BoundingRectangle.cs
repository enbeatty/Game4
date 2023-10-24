﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4.Collisions
{
    /// <summary>
    /// A bounding rectangle for collision detection
    /// </summary>
    public struct BoundingRectangle
    {
        public float X; 

        public float Y; 
        
        public float Width;
        
        public float Height;

        public float Left => X;

        public float Right => X + Height;

        public float Top => Y;

        public float Bottom => Y + Height;

        public BoundingRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public BoundingRectangle(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        //public bool CollidesWith(BoundingDip other)
        //{
        //    return CollisionHelper.Collides(this, other);
        //}
    }
}
