using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Game4.StateManagement;
using Microsoft.Xna.Framework.Input;

namespace Game4.Screens
{
    public class StoryScreen : GameScreen
    {
        ContentManager _content;
        Texture2D _background;
        TimeSpan _displayTime;

        private KeyboardState _keyboardState;

        bool _show = false;

        Game _game;

        public StoryScreen(Game game)
        {
            _game = game;
        }

        public override void Activate()
        {
            base.Activate();

            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _background = _content.Load<Texture2D>("background");
            _displayTime = TimeSpan.FromSeconds(3);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            _displayTime -= gameTime.ElapsedGameTime;
            if(_displayTime <= TimeSpan.Zero) _show = true;
            if( _show )
            {
                _keyboardState = Keyboard.GetState();
                if(_keyboardState.IsKeyDown(Keys.Space))
                {
                    LoadingScreen.Load(ScreenManager, true, 0, new SpaceStoryLevelScreen(_game));
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var font = ScreenManager.Font;

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(_background, Vector2.Zero, Color.White);

            ScreenManager.SpriteBatch.DrawString(font, "<First Officer> Captian! \nScanners are showing a series of asteroid belts.\n" +
                "In order to make it to Spyranis3600\nwe have to pass through it!" +
                "\nAwaiting your comand Captian." +
                "\n\n<Captian> Everyone to your posts! \nBrace yourselves!", new Vector2(10, 200), Color.AntiqueWhite);
            if( _show )
            {
                ScreenManager.SpriteBatch.DrawString(font, "Press Space to Continue", new Vector2(225, 700), Color.AntiqueWhite);
            }
            ScreenManager.SpriteBatch.End();
        }
    }
}
