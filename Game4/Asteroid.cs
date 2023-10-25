using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game4.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game4
{
    public class Asteroid
    {
        private Texture2D _asteroid;

        private BoundingRectangle _bounds = new BoundingRectangle(new Vector2(-24, -24), 24, 48); //TODO

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

        /// <summary>
        /// The color to blend with the ghost
        /// </summary>
        public Color Color { get; set; } = Color.White;

        public Vector2 Position { get; set; } = new Vector2(650, 450);

        public Vector2 Velocity { get; set; }

        public float Direction { get; set; }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            string filename = "";
            int rand = RandomHelper.Next(4);
            switch (rand)
            { 
                case 0:
                    filename = "lightAsteroid1";
                    break;
                case 1:
                    filename = "lightAsteroid2";
                    break;
                case 2:
                    filename = "darkAsteroid1";
                    break;
                case 3:
                    filename = "darkAsteroid2";
                    break;
                default:
                    break;
            }

            _asteroid = content.Load<Texture2D>(filename);
        }

        /// <summary>
        /// Updates the sprite's _position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            //Position.X += Direction;
            _bounds.X = Position.X;
            _bounds.Y = Position.Y;
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_asteroid, Position, null, Color.White, 0f, new Vector2(64, 64), 1f, SpriteEffects.None, 0);
        }
    }
}
