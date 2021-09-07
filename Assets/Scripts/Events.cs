﻿public class Events
{
    public const string LEVEL_START = "LevelStart";
    public const string LEVEL_FINISHED = "LevelFinished";
    public const string LEVEL_FAILED = "LevelFailed";
    
    public const string PLAYER_DIED = "PlayerDied";
    public const string PLAYER_SHOOT = "PlayerShoot";
    public const string PLAYER_UNDER_ATTACK = "PlayerUnderAttack";
    public const string PLAYER_JUMP = "PlayerJump";

    public const string ENEMY_UNDER_ATTACK = "EnemyUnderAttack";
    public const string ENEMY_DIED = "EnemyDied";
    public const string ENEMY_SPAWNED = "EnemySpawned";

    public const string TRANSITION_OPEN = "TransitionOpen";
    public const string TRANSITION_CLOSE = "TransitionClose";
    public const string TRANSITION_OPEN_FINISHED = "TransitionOpenFinished";
    public const string TRANSITION_CLOSE_FINISHED = "TransitionCloseFinished";
    
    // camera events 
    public const string BOUNDARIES_TOP_RIGHT = "BoundariesTopRight";
    public const string BOUNDARIES_BOTTOM_LEFT = "BoundariesBottomLeft";
    public const string CAMERA_START_FOLLOWING = "CameraStartFollowing";
    public const string CAMERA_STOP_FOLLOWING = "CameraStopFollowing";
    
    // settings events
    public const string MUSIC_SETTINGS_CHANGED = "MusicSettingsChanged";
    
    // custom events
    public const string TAKE_STAKE = "TakeStake";
    public const string TIME_OUT = "TimeOut";
}
