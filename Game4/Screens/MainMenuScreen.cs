using Game4.StateManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4.Screens
{
    public class MainMenuScreen : MenuScreen
    {
        private Game _game;

        public MainMenuScreen(Game game) : base("Main Menu", game)
        {
            var playStoryMenuEntry = new MenuEntry("Play Story Mode");
            var playGameMenuEntry = new MenuEntry("Play Endless Mode");
            var exitMenuEntry = new MenuEntry("Exit");
            var optionsMenuEntry = new MenuEntry("Delete Save");
            var controlsMenuEntry = new MenuEntry("Controls");

            playStoryMenuEntry.Selected += PlayStoryMenuEntrySelected;
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            controlsMenuEntry.Selected += ControlsMenuEntrySelected;

            MenuEntries.Add(playStoryMenuEntry);
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(controlsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
            _game = game;
        }
        private void PlayStoryMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new SpaceStoryLevelScreen(_game));
        }

        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new SpaceLevelScreen(_game));
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?";
            var confirmExitMessageBox = new MessageBoxExitScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(_game), e.PlayerIndex);
        }

        private void ControlsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Contorls";
            var controlsMessageBox = new MessageBoxControlsScreen(message);

            ScreenManager.AddScreen(controlsMessageBox , e.PlayerIndex);
        }
    }

}
