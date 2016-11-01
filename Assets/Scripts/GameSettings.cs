﻿/// <summary>
/// Contains a list of settings for the game Macabre, belings in GameManager
/// </summary>
public struct GameSettings {

    // Debug Stats
    public static bool createNewGame = false;
    public static bool useSavedXMLConfiguration = false;
    public static bool createNewXMLConfiguration = true;
    public static bool enableSaving = true;
	public static bool enableGameClock = false;
    public static bool reAssertUIScreen = true;
    public static bool autoSerializeGame = true;

    // Environment Stats
    public static float cameraSpeed = 1.0f;

    // Character Stats
    public static int characterInventorySize = 20;
    public static int conversationCharacterLimit = 500;
    public static int conversationChoiceOptions = 4;
    public static float characterWalkingSpeed = 1.0f;
    public static float characterRunningSpeed = 2.0f;
    public static float inspectRadius = 0.5f;

    // Controls
    public static bool useKeyboardMovement = true;
    public static bool useMouseMovement = false;
}