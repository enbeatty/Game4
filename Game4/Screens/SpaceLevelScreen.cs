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
using System.Reflection.Metadata;
using SharpDX.Direct2D1;

namespace Game4.Screens
{
    public class SpaceLevelScreen : GameScreen
    {
        private ContentManager _content;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
        private Random _random;

        private Map _map;

        //private Vector2 _viewportPosition = new Vector2(0, Constants.GAME_HEIGHT - 896);

        private float _height = -Constants.GAME_HEIGHT + 900; //we almost there

        private GameSave _gameSave;

        private SpaceShip _spaceShip;

        private Game _game;

        private Asteroid[] _asteroids;

        private int _numAsteroidsLeft;

        private PixieParticleSystem _pixie;

        private Texture2D ball;

        private const string _message = "You Lost, want to restart?";

        private bool _collided = false;

        private OOTilemap _ooMap;

        private BasicTilemap _basicMap;



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

            //_map = Map.Load(Path.Combine(_content.RootDirectory, "SpaceMap.tmx"), _content);

            _ooMap = _content.Load<OOTilemap>("SpaceMap");

            //_basicMap = _content.Load<BasicTilemap>("test");

            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(ScreenManager.GraphicsDevice);

            _spaceShip = new SpaceShip();
            _spaceShip.LoadContent(_content);

            _pixie = new PixieParticleSystem(_game, _spaceShip);
            _game.Components.Add(_pixie);

            string fileName = Path.Combine(Path.GetFullPath("."), "SaveGame.json");

            if( !File.Exists(fileName) )
            {
                _random = new Random();
                _gameSave = new GameSave(1, 0, 100, _random.Next(10000, 50000));
            }
            else
            {
                string jsonString = File.ReadAllText(fileName);
                GameSave game = JsonSerializer.Deserialize<GameSave>(jsonString)!;
                _gameSave = game;
            }
            _random = new Random(_gameSave.Seed);
            _asteroids = new Asteroid[_gameSave.Asteroids];
            _numAsteroidsLeft = _gameSave.Asteroids;
            for(int i = 0; i < _gameSave.Asteroids; i++)
            {
                _asteroids[i] = new Asteroid(new Vector2(_random.Next(-32, Constants.GAME_WIDTH-100), _random.Next(-9000, 500)), _random.Next(0,4));
                _asteroids[i].LoadContent(_content);
            }


            ball = _content.Load<Texture2D>("rectangle");

        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();

            string fileName = "SaveGame.json";
            string jsonString = JsonSerializer.Serialize(_gameSave);
            File.WriteAllText(fileName, jsonString);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (!_collided)
            {
                _spaceShip.Update(gameTime);

                if (_numAsteroidsLeft == 0)
                {
                    _gameSave.Asteroids += 30;
                    _gameSave.Level++;  //TODO Make it change restart the map with a harder difficulty and save the level
                    _gameSave.Seed++;
                    _game.Components.Remove(_pixie);

                    LoadingScreen.Load(ScreenManager, true, 0, new SpaceLevelScreen(_game));
                }

                foreach (var a in _asteroids)
                {
                    a.Update(gameTime);
                    if (/*!a.Under &&*/ a.Bounds.CollidesWith(_spaceShip.Bounds))
                    {
                        _game.Components.Remove(_pixie);
                        var LostMessageBox = new MessageBoxScreen(_message);
                        LostMessageBox.Accepted += LostMessageBoxAccepted;
                        LostMessageBox.Cancelled += LostMessageBoxCancelled;
                        LostMessageBox.Reset += LostMessageBoxReset;
                    
                        ScreenManager.AddScreen(LostMessageBox, 0);
                        _collided = true;
                    
                    }
                    if (a.Position.Y > _spaceShip.Position.Y + 900 && !a.Under)
                    {
                        a.Under = true;
                        _numAsteroidsLeft--;
                    }
                }
            }
        }

        private void LostMessageBox_Reset(object sender, PlayerIndexEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            var font = ScreenManager.Font;

            if (!_collided)
            {
                _height += 3;
            //_viewportPosition += new Vector2(0, -3);
            }

            Matrix transform;
            transform = Matrix.CreateTranslation(0, _height, 0);

            _spriteBatch.Begin(transformMatrix: transform);
            _ooMap.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin(); //Can do the transform
            //_map.Draw(_spriteBatch, new Rectangle(0, 0, Constants.GAME_WIDTH, Constants.GAME_HEIGHT), _viewportPosition); //TODO maybe paralax it a bit and move in different sprite batch call
            
            //_basicMap.Draw(gameTime, _spriteBatch);

            foreach (var a in _asteroids)
            {
                if(!a.Under)
                {
                    a.Draw(gameTime, _spriteBatch);
                    //var rect = new Rectangle((int)a.Bounds.X, (int)a.Bounds.Y, (int)a.Bounds.Width, (int)a.Bounds.Height);
                    //_spriteBatch.Draw(ball, rect, Color.White);
                }
            }

            _spaceShip.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(font, $"Current Level: {_gameSave.Level}", new Vector2(0, 0), Color.LightGoldenrodYellow);
            //var newrect = new Rectangle((int)_spaceShip.Bounds.X, (int)_spaceShip.Bounds.Y, (int)_spaceShip.Bounds.Width, (int)_spaceShip.Bounds.Height);
            //var newrect = new Rectangle(450, 750, 64, 64);
            //_spriteBatch.Draw(ball, newrect, Color.White);

            _spriteBatch.End();
        }


        private void LostMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, 0, new SpaceLevelScreen(_game));
        }

        private void LostMessageBoxCancelled(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, 0, new BackgroundScreen(), new MainMenuScreen(_game));
        }

        private void LostMessageBoxReset(object sender, PlayerIndexEventArgs e)
        {
            _gameSave.Seed = _gameSave.Seed + 23;
            LoadingScreen.Load(ScreenManager, true, 0, new SpaceLevelScreen(_game));
        }
    }
}
