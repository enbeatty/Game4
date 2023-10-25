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
using SharpDX.MediaFoundation;

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

        private SpaceShip _spaceShip;

        private Game _game;

        public SpaceLevelScreen(Game game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _game = game;
        }

        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _map = Map.Load(Path.Combine(_content.RootDirectory, "Space.tmx"), _content);

            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(ScreenManager.GraphicsDevice);

            _spaceShip = new SpaceShip();
            _spaceShip.LoadContent(_content);

            PixieParticleSystem pixie = new PixieParticleSystem(_game, _spaceShip);
            _game.Components.Add(pixie);

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

            _spaceShip.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //Calculate our offset vector
            float playerY = MathHelper.Clamp(_spaceShip.Position.Y, 50, 450);
            float offsetY = 450 - playerY;

            Matrix transform;
            transform = Matrix.CreateTranslation(0, offsetY, 0);

            _spriteBatch.Begin(transformMatrix: transform);
            _map.Draw(_spriteBatch, new Rectangle(0, 0, Constants.GAME_WIDTH, Constants.GAME_HEIGHT), _viewportPosition); //TODO maybe paralax it a bit and move in different sprite batch call
            _spaceShip.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();
        }
    }
}
