using Game4.Screens;
using Game4.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Squared.Tiled;
using System;
using System.IO;

namespace Game4
{
    public class Game4 : Game
    {
        private GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;

        /// <summary>
        /// Sound
        /// </summary>
        private Song _backgroundMusic;

        public Game4()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.PreferredBackBufferWidth = Constants.GAME_WIDTH;
            _graphics.PreferredBackBufferHeight = 896;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.ApplyChanges();

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            _screenManager.AddScreen(new BackgroundScreen(), null);
            _screenManager.AddScreen(new MainMenuScreen(this), null);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            _backgroundMusic = Content.Load<Song>("cosmic-glow-6703");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_backgroundMusic);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}