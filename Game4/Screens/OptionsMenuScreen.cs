using System;
using System.Collections.Generic;
using Game4.StateManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.IO;
using System.Text.Json;

namespace Game4.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    public class OptionsMenuScreen : MenuScreen
    {

        private Random _random;

        private GameSave _gameSave;

        private Game _game;

        private enum Ungulate
        {
            BactrianCamel,
            Dromedary,
            Llama,
        }

        private readonly MenuEntry _deleteSave;


        public OptionsMenuScreen(Game game) : base("Delete Save", game)
        {
            _deleteSave = new MenuEntry(string.Empty);
            _game = game;
            SetMenuEntryText();

            var back = new MenuEntry("Back");

            _deleteSave.Selected += DeleteMenuEntrySelected;
            back.Selected += OnCancel;

            MenuEntries.Add(_deleteSave);
            MenuEntries.Add(back);
        }

        // Fills in the latest values for the options screen menu text.
        private void SetMenuEntryText()
        {
            string fileName = Path.Combine(Path.GetFullPath("."), "SaveGame.json");

            if (!File.Exists(fileName))
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

            _deleteSave.Text = $"Delete Save. Your current level: {_gameSave.Level}";
        }

        private void DeleteMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to delete your Save?";

            var confirmExitMessageBox = new DeleteLevelMessageBoxScreen(message);
            confirmExitMessageBox.Accepted += DeleteMessageBoxAccepted;
            confirmExitMessageBox.Cancelled += DeleteMessageBoxCancelled;

            ScreenManager.AddScreen(confirmExitMessageBox, 0);
        }

        private void DeleteMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {

                _random = new Random();
                _gameSave = new GameSave(1, 0, 100, _random.Next(10000, 50000));
                string fileName = "SaveGame.json";
                string jsonString = JsonSerializer.Serialize(_gameSave);
                File.WriteAllText(fileName, jsonString);

            OnCancel(0);
        }

        private void DeleteMessageBoxCancelled(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(0);
        }
    }
}
