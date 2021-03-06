using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using SwinGameSDK;

/// <summary>

/// ''' The menu controller handles the drawing and user interactions

/// ''' from the menus in the game. These include the main menu, game

/// ''' menu and the settings m,enu.

/// ''' </summary>

static class MenuController
{

    /// <summary>
    ///     ''' The menu structure for the game.
    ///     ''' </summary>
    ///     ''' <remarks>
    ///     ''' These are the text captions for the menu items.
    ///     ''' </remarks>
	private readonly static string[][] _menuStructure = new[] { new string[] { "PLAY", "SETUP","SCREEN","MUSIC","SCORES", "MUTE", "QUIT" }, new string[] { "RETURN", "SURRENDER", "QUIT" }, new string[] { "EASY", "MEDIUM", "HARD" }, new string [] {"FULLSCREEN" , " BORDERLESS" } ,new string [] { "MUSIC 1", "MUSIC 2", "MUSIC 3" } };

    private const int MENU_TOP = 575;
    private const int MENU_LEFT = 30;
    private const int MENU_GAP = 0;
    private const int BUTTON_WIDTH = 83;
    private const int BUTTON_HEIGHT = 15;
    private const int BUTTON_SEP = BUTTON_WIDTH + MENU_GAP;
    private const int TEXT_OFFSET = 0;

    private const int MAIN_MENU = 0;
    private const int GAME_MENU = 1;
    private const int SETUP_MENU = 2;
	private const int SCREEN_MENU = 3; //recently added
	private const int MUSIC_MENU = 4;


    private const int MAIN_MENU_PLAY_BUTTON = 0;
    private const int MAIN_MENU_SETUP_BUTTON = 1;
	private const int MAIN_MENU_SCREEN_BUTTON = 2; //recently added
	private const int MAIN_MUSIC_SETUP_BUTTON = 3;
    private const int MAIN_MENU_TOP_SCORES_BUTTON = 4;
	private const int MAIN_MENU_MUTE_BUTTON = 5;
	private const int MAIN_MENU_QUIT_BUTTON = 6;

    private const int SETUP_MENU_EASY_BUTTON = 0;
    private const int SETUP_MENU_MEDIUM_BUTTON = 1;
    private const int SETUP_MENU_HARD_BUTTON = 2;
    private const int SETUP_MENU_EXIT_BUTTON = 3;

    private const int GAME_MENU_RETURN_BUTTON = 0;
    private const int GAME_MENU_SURRENDER_BUTTON = 1;
    private const int GAME_MENU_QUIT_BUTTON = 2;

	private const int SCREEN_MENU_FULLSCREEN_BUTTON = 0; //recently added
	private const int SCREEN_MENU_BORDERLESS_BUTTON = 1; //recently added

	private const int MUSIC1 = 0;
	private const int MUSIC2 = 1;
	private const int MUSIC3 = 2;

    private readonly static Color MENU_COLOR = SwinGame.RGBAColor(255, 255, 255, 255);
    private readonly static Color HIGHLIGHT_COLOR = SwinGame.RGBAColor(1, 57, 86, 255);

    /// <summary>
    ///     ''' Handles the processing of user input when the main menu is showing
    ///     ''' </summary>
    public static void HandleMainMenuInput()
    {
        HandleMenuInput(MAIN_MENU, 0, 0);
    }

    /// <summary>
    ///     ''' Handles the processing of user input when the main menu is showing
    ///     ''' </summary>
    public static void HandleSetupMenuInput()
    {
        bool handled;
        handled = HandleMenuInput(SETUP_MENU, 1, 1);

        if (!handled)
        {
            HandleMenuInput(MAIN_MENU, 0, 0);
        }
    }

    /// <summary>
    ///     ''' Handle input in the game menu.
    ///     ''' </summary>
    ///     ''' <remarks>
    ///     ''' Player can return to the game, surrender, or quit entirely
    ///     ''' </remarks>
    public static void HandleGameMenuInput()
    {
        HandleMenuInput(GAME_MENU, 0, 0);
    }

	/// <summary>
	/// Handles the processing of user input when selecting screen option
	/// </summary>
	public static void HandleOptionMenuInput ()
	{
		bool handled = false;
		handled = HandleMenuInput (SCREEN_MENU, 1, 2); //RECENTLY ADDED

		if (!handled) {
			HandleMenuInput (MAIN_MENU, 0, 0);
		}
	}
	/// <summary>
	/// Handles the music menu input.
	/// </summary>
	public static void HandleMusicMenuInput ()
	{
		bool handled = false;
		handled = HandleMenuInput (MUSIC_MENU, 1, 3);

		if (!handled) {
			HandleMenuInput (MAIN_MENU, 0, 0);
		}
	}

    /// <summary>
    ///     ''' Handles input for the specified menu.
    ///     ''' </summary>
    ///     ''' <param name="menu">the identifier of the menu being processed</param>
    ///     ''' <param name="level">the vertical level of the menu</param>
    ///     ''' <param name="xOffset">the xoffset of the menu</param>
    ///     ''' <returns>false if a clicked missed the buttons. This can be used to check prior menus.</returns>
    private static bool HandleMenuInput(int menu, int level, int xOffset)
    {
        if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE))
        {
            GameController.EndCurrentState();
            return true;
        }

        if (SwinGame.MouseClicked(MouseButton.LeftButton))
        {
            int i = 0;
            for (i = 0; i <= _menuStructure[menu].Length - 1; i++)
            {
                // IsMouseOver the i'th button of the menu
                if (IsMouseOverMenu(i, level, xOffset))
                {
                    PerformMenuAction(menu, i);
                    return true;
                }
            }

            if (level > 0)
            {
                // none clicked - so end this sub menu
                GameController.EndCurrentState();
            }
        }

        return false;
    }

    /// <summary>
    ///     ''' Draws the main menu to the screen.
    ///     ''' </summary>
    public static void DrawMainMenu()
    {
        // Clears the Screen to Black
        // SwinGame.DrawText("Main Menu", Color.White, GameFont("ArialLarge"), 50, 50)
		SwinGame.DrawTextLines ("MUSIC : " + GameController.MusicOption, Color.White, Color.Transparent, GameResources.GameFont ("Menu"), FontAlignment.AlignCenter, 310, 493, SwinGame.ScreenWidth (), SwinGame.ScreenHeight ());
        //Recently Added
        DrawButtons(MAIN_MENU);
    }

    /// <summary>
    ///     ''' Draws the Game menu to the screen
    ///     ''' </summary>
    public static void DrawGameMenu()
    {
        // Clears the Screen to Black
        // SwinGame.DrawText("Paused", Color.White, GameFont("ArialLarge"), 50, 50)

        DrawButtons(GAME_MENU);
    }

    /// <summary>
    ///     ''' Draws the settings menu to the screen.
    ///     ''' </summary>
    ///     ''' <remarks>
    ///     ''' Also shows the main menu
    ///     ''' </remarks>
    public static void DrawSettings()
    {
        // Clears the Screen to Black
        // SwinGame.DrawText("Settings", Color.White, GameFont("ArialLarge"), 50, 50)

        DrawButtons(MAIN_MENU);
        DrawButtons(SETUP_MENU, 1, 1);
    }

    /// <summary>
    ///     ''' Draw the buttons associated with a top level menu.
    ///     ''' </summary>
    ///     ''' <param name="menu">the index of the menu to draw</param>
    private static void DrawButtons(int menu)
    {
        DrawButtons(menu, 0, 0);
    }

	/// <summary>
	/// Draws the music menu.
	/// </summary>
	public static void DrawMusicMenu ()     //Recently Added
	{
		//Clears the Screen to Black
		//SwinGame.DrawText("Settings", Color.White, GameFont("ArialLarge"), 50, 50)

		DrawButtons (MAIN_MENU);
		DrawButtons (MUSIC_MENU, 1, 3);
	}

	/// <summary>
	/// Draws the screen option menu to the screen.
	/// </summary>
	/// <remarks>
	/// Also shows the main menu
	/// </remarks>
	public static void DrawOption ()
	{
		DrawButtons (MAIN_MENU);
		DrawButtons (SCREEN_MENU, 1, 2); //RECENTLY ADDED
	}
    /// <summary>
    ///     ''' Draws the menu at the indicated level.
    ///     ''' </summary>
    ///     ''' <param name="menu">the menu to draw</param>
    ///     ''' <param name="level">the level (height) of the menu</param>
    ///     ''' <param name="xOffset">the offset of the menu</param>
    ///     ''' <remarks>
    ///     ''' The menu text comes from the _menuStructure field. The level indicates the height
    ///     ''' of the menu, to enable sub menus. The xOffset repositions the menu horizontally
    ///     ''' to allow the submenus to be positioned correctly.
    ///     ''' </remarks>
    private static void DrawButtons(int menu, int level, int xOffset)
    {
        int btnTop = 0;

        btnTop = MENU_TOP - (MENU_GAP + BUTTON_HEIGHT) * level;
        int i = 0;
        for (i = 0; i <= _menuStructure[menu].Length - 1; i++)
        {
			string bton = _menuStructure [menu] [i];
            int btnLeft = 0;
            btnLeft = MENU_LEFT + BUTTON_SEP * (i + xOffset);
            // SwinGame.FillRectangle(Color.White, btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT)
			SwinGame.DrawTextLines(_menuStructure[menu][i], MENU_COLOR, Color.Black, GameResources.GameFont("Menu"), FontAlignment.AlignCenter, btnLeft + TEXT_OFFSET, btnTop + TEXT_OFFSET, BUTTON_WIDTH, BUTTON_HEIGHT);

	    if (GameResources.Muted && i == MAIN_MENU_MUTE_BUTTON)
		   bton = "UNMUTE";
		
            if (SwinGame.MouseDown(MouseButton.LeftButton) & IsMouseOverMenu(i, level, xOffset))
                SwinGame.DrawRectangle(HIGHLIGHT_COLOR, btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
			
        }
		SwinGame.FillRectangle (SwinGame.RGBAColor (0, 0, 0, 255), 23, 420, 299, 140);

		SwinGame.DrawText ("Instructions", Color.White, 27, 430);
		SwinGame.DrawText ("You own and control 5 ships. All you", Color.White, 27, 445);
		SwinGame.DrawText ("have to do is hide your ships, find", Color.White, 27, 460);
		SwinGame.DrawText ("your enemy's ships and destroy them", Color.White, 27, 475);
		SwinGame.DrawText ("by guessing their coordinates on the", Color.White, 27, 490);
		SwinGame.DrawText ("sea grid. If the whole ship is hit it", Color.White, 27, 505);
		SwinGame.DrawText ("sinks and the first one to sink", Color.White, 27, 520);
		SwinGame.DrawText ("opponent's fleet wins. GoodLuck!", Color.White, 27, 535);
    }

    /// <summary>
    ///     ''' Determined if the mouse is over one of the button in the main menu.
    ///     ''' </summary>
    ///     ''' <param name="button">the index of the button to check</param>
    ///     ''' <returns>true if the mouse is over that button</returns>
    private static bool IsMouseOverButton(int button)
    {
        return IsMouseOverMenu(button, 0, 0);
    }

    /// <summary>
    ///     ''' Checks if the mouse is over one of the buttons in a menu.
    ///     ''' </summary>
    ///     ''' <param name="button">the index of the button to check</param>
    ///     ''' <param name="level">the level of the menu</param>
    ///     ''' <param name="xOffset">the xOffset of the menu</param>
    ///     ''' <returns>true if the mouse is over the button</returns>
    private static bool IsMouseOverMenu(int button, int level, int xOffset)
    {
        int btnTop = MENU_TOP - (MENU_GAP + BUTTON_HEIGHT) * level;
        int btnLeft = MENU_LEFT + BUTTON_SEP * (button + xOffset);

        return UtilityFunctions.IsMouseInRectangle(btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
    }

    /// <summary>
    ///     ''' A button has been clicked, perform the associated action.
    ///     ''' </summary>
    ///     ''' <param name="menu">the menu that has been clicked</param>
    ///     ''' <param name="button">the index of the button that was clicked</param>
    private static void PerformMenuAction(int menu, int button)
    {
		switch (menu) {
		case MAIN_MENU: {
				PerformMainMenuAction (button);
				break;
			}

		case SETUP_MENU: {
				PerformSetupMenuAction (button);
				break;
			}

		case GAME_MENU: {
				PerformGameMenuAction (button);
				break;
			}
		case SCREEN_MENU: {
				PerformScreenMenuAction (button); //RECENTLY ADDED
				break;
			}
		case MUSIC_MENU: {
				PerformMusicMenuAction (button);  //Recently Added
				break;
			}
		}
    }

    /// <summary>
    ///     ''' The main menu was clicked, perform the button's action.
    ///     ''' </summary>
    ///     ''' <param name="button">the button pressed</param>
    private static void PerformMainMenuAction(int button)
    {
        switch (button)
        {
            case MAIN_MENU_PLAY_BUTTON:
                {
                    GameController.StartGame();
                    break;
                }

            case MAIN_MENU_SETUP_BUTTON:
                {
                    GameController.AddNewState(GameState.AlteringSettings);
                    break;
                }

			case MAIN_MENU_SCREEN_BUTTON:
				{
					GameController.AddNewState (GameState.AlteringOption); //RECENTLY ADDED
					break;
				}
			case MAIN_MUSIC_SETUP_BUTTON: {
				GameController.AddNewState (GameState.ChangingMusic);
				break;
			}
            case MAIN_MENU_TOP_SCORES_BUTTON:
                {
                    GameController.AddNewState(GameState.ViewingHighScores);
                    break;
                }

			
	    case MAIN_MENU_MUTE_BUTTON:
		{
		    GameResources.MuteButtonPressed ();
		    break;
		}

            case MAIN_MENU_QUIT_BUTTON:
                {
                    GameController.EndCurrentState();
                    break;
                }
        }
    }

    /// <summary>
    ///     ''' The setup menu was clicked, perform the button's action.
    ///     ''' </summary>
    ///     ''' <param name="button">the button pressed</param>
    private static void PerformSetupMenuAction(int button)
    {
        switch (button)
        {
            case SETUP_MENU_EASY_BUTTON:
                {
					GameController.SetDifficulty(AIOption.Easy);
                    break;
                }

            case SETUP_MENU_MEDIUM_BUTTON:
                {
                    GameController.SetDifficulty(AIOption.Medium);
                    break;
                }

            case SETUP_MENU_HARD_BUTTON:
                {
                    GameController.SetDifficulty(AIOption.Hard);
                    break;
                }
        }
        // Always end state - handles exit button as well
        GameController.EndCurrentState();
    }

	/// <summary>
	/// Performs the music menu action.
	/// </summary>
	/// <param name="button">Button.</param>
	private static void PerformMusicMenuAction (int button)
	{

		switch (button) {
		case MUSIC1: {
				GameController.SetMusic ("BGM1");
				break;
			}
		case MUSIC2: {
				GameController.SetMusic ("BGM2");
				break;
			}
		case MUSIC3: {
				GameController.SetMusic ("BGM3");
				break;
			}
			//case TURN_OFF:
			//	GameController.SetMusic ("Off");
			//	break;
			//case TURN_ON:
			//	GameController.SetMusic ("Background");
			//	break;
		}

		GameController.EndCurrentState ();
	}

    /// <summary>
    ///     ''' The game menu was clicked, perform the button's action.
    ///     ''' </summary>
    ///     ''' <param name="button">the button pressed</param>
    private static void PerformGameMenuAction(int button)
    {
        switch (button)
        {
            case GAME_MENU_RETURN_BUTTON:
                {
                    GameController.EndCurrentState();
                    break;
                }

            case GAME_MENU_SURRENDER_BUTTON:
                {
				SwinGame.PlayMusic (GameResources.GameMusic ("BGM1"));
                    GameController.EndCurrentState(); // end game menu
                    GameController.EndCurrentState(); // end game
                    break;
                }

            case GAME_MENU_QUIT_BUTTON:
                {
                    GameController.AddNewState(GameState.Quitting);
                    break;
                }
        }
    }


	//<summary>
	// The screen option menu was clicked, perform the button's action.
	// </summary>
	// <param name="button">the button pressed</param>
	public static void PerformScreenMenuAction (int button) //RECENTLY ADDED FUNCTION
	{
		switch (button)
		{
			case SCREEN_MENU_FULLSCREEN_BUTTON: {
				SwinGame.ToggleFullScreen ();

				break;}
			case SCREEN_MENU_BORDERLESS_BUTTON: {
				SwinGame.ToggleWindowBorder ();
				break;}
		}
		//Always end state - handles exit button as well
		GameController.EndCurrentState ();
	}
}
