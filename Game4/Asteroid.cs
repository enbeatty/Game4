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

        private BoundingRectangle _bounds; //TODO

        private Vector2 _boundsOffset = new Vector2(16,22);

        private Vector2 _position;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

        /// <summary>
        /// The color to blend with the ghost
        /// </summary>
        public Color Color { get; set; } = Color.White;

        public Vector2 Position => _position;

        public Vector2 Velocity { get; set; }

        public int Texture { get; set; }

        public bool Under { get; set; } = false;

        public Asteroid(Vector2 position, int texture)
        {
            _position = position;
            Texture = texture;
            _bounds = new BoundingRectangle(new Vector2(position.X + _boundsOffset.X, position.Y + _boundsOffset.Y), 128 - _boundsOffset.X, 64 - _boundsOffset.Y);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            string filename = "";
            switch (Texture)
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
            //_position += new Vector2(Direction, 1);
            _position += new Vector2(0, 5);
            _bounds.X = Position.X + _boundsOffset.X;
            _bounds.Y = Position.Y + _boundsOffset.Y;
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_asteroid, Position, null, Color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
        }
    }
}
