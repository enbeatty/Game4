﻿using System;
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

        private BoundingRectangle _bounds = new BoundingRectangle(new Vector2(397, 0), 24, 24);

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

        /// <summary>
        /// The color to blend with the ghost
        /// </summary>
        public Color Color { get; set; } = Color.White;

        public Vector2 Position { get; set; } = new Vector2(450, 750);

        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            _spaceShip = content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip\\PNGs\\Main Ship - Base - Full health");
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
                Position += new Vector2(-5f, 0);

                // Update the _bounds
                _bounds.X = Position.X - 52; //TODO
            }
            if (_keyboardState.IsKeyDown(Keys.Right) || _keyboardState.IsKeyDown(Keys.D))
            {
                Position += new Vector2(5f, 0);

                // Update the _bounds
                _bounds.X = Position.X - 52; //TODO
            }
            if (_keyboardState.IsKeyDown(Keys.Up) || _keyboardState.IsKeyDown(Keys.A))
            {
                Position += new Vector2(0, -5f);

                // Update the _bounds
                _bounds.X = Position.X - 52; //TODO
            }

            _bounds.Y = Position.Y - 50;
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
        }
    }
}
