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
using System.Text.Json;

namespace Game4.Screens
{
    public class SpaceLevelScreen : GameScreen
    {
        private ContentManager _content;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
        private Random _random = new Random();

        private Map _map;
        private Vector2 _viewportPosition;

        private GameSave _gameSave; 


        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _map = Map.Load(Path.Combine(_content.RootDirectory, "Space.tmx"), _content);

            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(ScreenManager.GraphicsDevice);

            string fileName = Path.Combine(Path.GetFullPath("."), "SaveGame.json");
            string jsonString = File.ReadAllText(fileName);
            GameSave game = JsonSerializer.Deserialize<GameSave>(jsonString)!;

            if (game.Level != 0)
            {
                _gameSave = game;
            }
            else
            {
                _gameSave = new GameSave(0, 0, 5); //Need to change 2nd argument of Speed
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();

            string fileName = "SpaceSave.json";
            string jsonString = JsonSerializer.Serialize(_gameSave);
            File.WriteAllText(fileName, jsonString);
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
