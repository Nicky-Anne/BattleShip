
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SwinGameSDK;

namespace GameLogic
{
    public class GameLogic
    {

        public static void Main()
        {
            // Opens a new Graphics Window
            SwinGame.OpenGraphicsWindow("Battle Ships", 800, 600);

            // Load Resources
            GameResources.LoadResources();

            SwinGame.PlayMusic(GameResources.GameMusic("Background"));

            // Game Loop
            while (!(true == SwinGame.WindowCloseRequested() || GameController.CurrentState == GameState.Quitting))
            {

                GameController.HandleUserInput();
                GameController.DrawScreen();
            }


            SwinGame.StopMusic();

            // Free Resources and Close Audio, to end the program.
            GameResources.FreeResources();
        }
    }
}
