using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public int frameRate = -1;
    public float gravity = 40;
    public float acceleration = 2f;

    public GameplayConfig gameplayConfig;
    public BirdConfig birdConfig;
    public WallsConfig wallsConfig;
    public ObstaclesConfig obstaclesConfig;
    public CollectiblesConfig collectiblesConfig;
}

[Serializable]
public class GameplayConfig
{
    public float startSpeed = 10f;
    public float countdownTime = 3f;
    public float endGameInteractionDelay = 2f;
}

[Serializable]
public class BirdConfig
{
    public float maxSpeed = 5f;
    public float jumpForce = 3f;
    public float mass = 0.2f;
    public Vector3 startPosition = new(0f, 0f, 9f);
}

[Serializable]
public class WallsConfig
{
    public int interval = 100;
    public int loop = 2;
}

[Serializable]
public class MovingObjectsConfig
{
    public int maxSpawnDistance = 200;
    public int startSpawnDistance = 30;
    public int intervalDistance = 20;
}

[Serializable]
public class ObstaclesConfig : MovingObjectsConfig
{
    public float behindAlpha = 0.5f;
}

[Serializable]
public class CollectiblesConfig : MovingObjectsConfig
{
    public Vector2 spawnMin = new(-3f, -3f);
    public Vector2 spawnMax = new(3f, 3f);
}
