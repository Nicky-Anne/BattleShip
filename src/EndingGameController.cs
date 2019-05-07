
using Microsoft.VisualBasic;
using System.Collections;
using System.Diagnostics;
using SwinGameSDK;
using System;
using System.Collections.Generic;

/// <summary>

/// ''' The EndingGameController is responsible for managing the interactions at the end

/// ''' of a game.

/// ''' </summary>

static class EndingGameController
{

	/// <summary>
	///     ''' Draw the end of the game screen, shows the win/lose state
	///     ''' </summary>
	public static void DrawEndOfGame ()
	{
		UtilityFunctions.DrawField (GameController.ComputerPlayer.PlayerGrid, GameController.ComputerPlayer, true);
		UtilityFunctions.DrawSmallField (GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
		GameLogic.GameLogic._time.Stop ();

		if (GameController.HumanPlayer.IsDestroyed) {
			SwinGame.DrawTextLines ("YOU LOSE!", Color.White, Color.Transparent, GameResources.GameFont ("ArialLarge"), FontAlignment.AlignCenter, 0, 250, SwinGame.ScreenWidth (), SwinGame.ScreenHeight ());
		} else if (GameLogic.GameLogic._time.ElapsedMilliseconds >= 5000) 
		{
		//	SwinGame.DrawTextLines ("YOU LOSE!", Color.White, Color.Transparent, GameResources.GameFont ("ArialLarge"), FontAlignment.AlignCenter, 0, 250, SwinGame.ScreenWidth (), SwinGame.ScreenHeight ());
			//		SwinGame.Delay (10);
			/*if (GameLogic.GameLogic._time.IsRunning) {
				GameLogic.GameLogic._time.Stop ();
			}*/
			//SwinGame.CloseAudio ();
			//	GameController.EndCurrentState ();
			/*	SwinGame.RefreshScreen ();
				GameController.CurrentState=GameState.ViewingMainMenu;
*/
			GameLogic.GameLogic._time.Reset();
			GameController.CurrentState=GameState.ViewingMainMenu;
		
		} 
		else {
			SwinGame.DrawTextLines ("-- WINNER --", Color.White, Color.Transparent, GameResources.GameFont ("ArialLarge"), FontAlignment.AlignCenter, 0, 250, SwinGame.ScreenWidth (), SwinGame.ScreenHeight ());
		}

	}
    /// <summary>
    ///     ''' Handle the input during the end of the game. Any interaction
    ///     ''' will result in it reading in the highsSwinGame.
    ///     ''' </summary>
    public static void HandleEndOfGameInput()
    {
        if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.vk_RETURN) || SwinGame.KeyTyped(KeyCode.vk_ESCAPE))
        {
            HighScoreController.ReadHighScore(GameController.HumanPlayer.Score);
            GameController.EndCurrentState();
        }
    }

}
