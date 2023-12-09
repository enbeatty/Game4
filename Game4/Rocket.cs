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
    public class Rocket
    {
        private Texture2D _rocket;

        private BoundingRectangle _bounds;

        private Vector2 _boundsOffset = new Vector2(0, 0);

        private Vector2 _position;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

        /// <summary>
        /// The color to blend with the ghost
        /// </summary>
        public Color Color { get; set; } = Color.White;

        public Vector2 Position { get; set; } 

        public Vector2 Velocity { get; set; }

        public int Texture { get; set; }

        public Rocket(Vector2 position)
        {
            Position = position;
            _bounds = new BoundingRectangle(new Vector2(position.X + _boundsOffset.X, position.Y + _boundsOffset.Y), (float)(24) - _boundsOffset.X, (float)(48) - _boundsOffset.Y);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            _rocket = content.Load<Texture2D>("rocket");
        }

        /// <summary>
        /// Updates the sprite's _position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            //_position += new Vector2(Direction, 1);
            Position -= new Vector2(0, 5);
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
            spriteBatch.Draw(_rocket, Position, null, Color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
        }
    }
}
