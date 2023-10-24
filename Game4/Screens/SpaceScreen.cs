using Game4.StateManagement;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Squared.Tiled;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace Game4.Screens
{
    public class SpaceScreen : GameScreen
    {
        private ContentManager _content;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
        private Random _random = new Random();

        private Map _map;
        private Vector2 _viewportPosition;


        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _map = Map.Load(Path.Combine(_content.RootDirectory, "Space.tmx"), _content);

            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(ScreenManager.GraphicsDevice);
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Begin();
            _map.Draw(_spriteBatch, new Rectangle(0, 0, Constants.GAME_WIDTH, Constants.GAME_HEIGHT), _viewportPosition);
            _spriteBatch.End();
        }
    }
}
