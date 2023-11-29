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
    public class SpaceShip : IParticleEmitter
    {
        private KeyboardState _keyboardState;

        private double _animationTimer;

        private Texture2D _spaceShip;

        private BoundingRectangle _bounds;

        private Vector2 _boundsOffset = new Vector2(11, 11);

        private Rocket _rocket;

        private bool _showRocket = false;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

        /// <summary>
        /// The color to blend with the ghost
        /// </summary>
        public Color Color { get; set; } = Color.White;

        public Vector2 Position { get; set; } = new Vector2(350, 750);

        public Vector2 Velocity { get; set; }

        public SpaceShip()
        {
            _bounds = new BoundingRectangle(new Vector2(Position.X + _boundsOffset.X, Position.Y + _boundsOffset.Y), 48 - _boundsOffset.X, 48 - _boundsOffset.Y);
            _rocket = new Rocket(Position + new Vector2(0, 5));
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            _spaceShip = content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip\\PNGs\\Main Ship - Base - Full health");
            _rocket.LoadContent(content);
        }

        /// <summary>
        /// Updates the sprite's _position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();

            Vector2 shipPosition = new Vector2(Position.X, Position.Y);

            // Apply keyboard movement
            if (_keyboardState.IsKeyDown(Keys.Left) || _keyboardState.IsKeyDown(Keys.A))
            {
                if(Position.X > -10)
                {
                    Position += new Vector2(-5f, 0);
                }                
            }
            if (_keyboardState.IsKeyDown(Keys.Right) || _keyboardState.IsKeyDown(Keys.D))
            {
                if(Position.X < Constants.GAME_WIDTH - 40)
                {
                    Position += new Vector2(5f, 0);
                }                
            }
            if (_keyboardState.IsKeyDown(Keys.Up) || _keyboardState.IsKeyDown(Keys.W))
            {                
                if (Position.Y > 0)
                {
                    Position += new Vector2(0, -5f);
                }
            }
            if (_keyboardState.IsKeyDown(Keys.Down) || _keyboardState.IsKeyDown(Keys.S))
            {                
                if (Position.Y < 860)
                {
                    Position += new Vector2(0, 5f);
                }
            }

            if (_keyboardState.IsKeyDown(Keys.LeftShift))
            {
                _rocket.Position = Position + new Vector2(0, 5);
                _showRocket = true;
            }

            if(_showRocket)
            {
                _rocket.Update(gameTime);
            }

            _bounds.X = Position.X + _boundsOffset.X;
            _bounds.Y = Position.Y + _boundsOffset.Y;
            Velocity = shipPosition - Position;
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spaceShip, Position, null, Color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            if(_showRocket)
            {
                _rocket.Draw(gameTime, spriteBatch);
            }
        }
    }
}
