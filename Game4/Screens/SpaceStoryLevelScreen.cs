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
using Microsoft.Xna.Framework.Input;
using System.Net.Sockets;

namespace Game4.Screens
{
    public class SpaceStoryLevelScreen : GameScreen
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

        private ExplosionParticleSystem _explosion;

        private const string _message = "You Lost, want to restart?";

        private bool _collided = false;

        private OOTilemap _ooMap;

        private Planet _planet;

        private KeyboardState _keyboardState;
        private bool _showRocket = false;
        private Rocket _rocket;
        private int _rocketCount = 0;
        private bool _rocketCollide = false;


        public SpaceStoryLevelScreen(Game game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _game = game;
        }

        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _ooMap = _content.Load<OOTilemap>("SpaceMap");

            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(ScreenManager.GraphicsDevice);

            _spaceShip = new SpaceShip();
            _spaceShip.LoadContent(_content);

            _pixie = new PixieParticleSystem(_game, _spaceShip);
            _explosion = new ExplosionParticleSystem(_game, 20);
            _game.Components.Add(_pixie);
            _game.Components.Add(_explosion);

            string fileName = Path.Combine(Path.GetFullPath("."), "StorySaveGame.json");

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
            _rocket = new Rocket(_spaceShip.Position + new Vector2(20, 5));
            _rocket.LoadContent(_content);

            _planet = new Planet();
            _planet.LoadContent(_content);
            _planet.Position = new Vector2(100, -10450);

        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();

            string fileName = "StorySaveGame.json";
            string jsonString = JsonSerializer.Serialize(_gameSave);
            File.WriteAllText(fileName, jsonString);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            _keyboardState = Keyboard.GetState();

            if (!_collided)
            {
                _spaceShip.Update(gameTime);

                if(_gameSave.Level == 3)
                {
                    _planet.Update(gameTime);
                }

                if (_keyboardState.IsKeyDown(Keys.LeftShift) && _rocketCount < 3 && !_showRocket)
                {
                    _rocket.Position = _spaceShip.Position + new Vector2(20, 5);
                    _showRocket = true;
                    
                    _rocketCount++;
                }

                if (_showRocket)
                {
                    if(_rocket.Position.Y < 0 || _rocketCollide)
                    {
                        _showRocket = false;
                        _rocketCollide = false;
                    }
                    _rocket.Update(gameTime);
                }

                if (_numAsteroidsLeft == 0)
                {
                    _gameSave.Asteroids += 30;
                    _gameSave.Level++;
                    _gameSave.Seed++;
                    _game.Components.Remove(_pixie);
                    _game.Components.Remove(_explosion);

                    if( _gameSave.Level > 4 )
                    {
                        //LOAD An Ending Screen
                        _gameSave = new GameSave(1, 0, 100, _random.Next(10000, 50000));
                        string fileName = "StorySaveGame.json";
                        string jsonString = JsonSerializer.Serialize(_gameSave);
                        File.WriteAllText(fileName, jsonString);
                        LoadingScreen.Load(ScreenManager, true, 0, new BackgroundScreen(), new MainMenuScreen(_game));
                    }
                    else
                    {
                        LoadingScreen.Load(ScreenManager, true, 0, new SpaceStoryLevelScreen(_game));
                    }

                    
                }

                foreach (var a in _asteroids)
                {
                    a.Update(gameTime);
                    if(a.Bounds.CollidesWith(_rocket.Bounds) && _showRocket)
                    {
                        a.Shot = true;
                        _numAsteroidsLeft--;
                        _rocketCollide = true;
                        _explosion.PlaceExplosion(_rocket.Position);
                    }
                    
                    if (a.Bounds.CollidesWith(_spaceShip.Bounds) && !a.Shot)
                    {
                        _game.Components.Remove(_pixie);
                        _game.Components.Remove(_explosion);
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
            }

            Matrix transform;
            transform = Matrix.CreateTranslation(0, _height, 0);

            _spriteBatch.Begin(transformMatrix: transform);
            _ooMap.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin();

            foreach (var a in _asteroids)
            {
                if(!a.Under && !a.Shot)
                {
                    a.Draw(gameTime, _spriteBatch);
                }
            }

            _spaceShip.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(font, $"Current Level: {_gameSave.Level}", new Vector2(0, 0), Color.LightGoldenrodYellow);
            _spriteBatch.DrawString(font, $"Shots Left: {3 - _rocketCount}", new Vector2(Constants.GAME_WIDTH - 195, 0), Color.LightGoldenrodYellow);

            if (_showRocket)
            {
                _rocket.Draw(gameTime, _spriteBatch);
            }
            if(_gameSave.Level == 3)
            {
                _planet.Draw(gameTime, _spriteBatch);
            }

            _spriteBatch.End();
        }


        private void LostMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, 0, new SpaceStoryLevelScreen(_game));
        }

        private void LostMessageBoxCancelled(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, 0, new BackgroundScreen(), new MainMenuScreen(_game));
        }

        private void LostMessageBoxReset(object sender, PlayerIndexEventArgs e)
        {
            _gameSave.Seed = _gameSave.Seed + 23;
            LoadingScreen.Load(ScreenManager, true, 0, new SpaceStoryLevelScreen(_game));
        }
    }
}
