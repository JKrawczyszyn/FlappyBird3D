using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public int frameRate = -1;
    public float gravity = 40;
    public float acceleration = 2f;

    public BirdConfig birdConfig;
    public WallsConfig wallsConfig;
    public ObstaclesConfig obstaclesConfig;

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
public class ObstaclesConfig
{
    public int spawnDistance = 200;
    public int freeDistance = 30;
    public int intervalDistance = 20;
    public float startSpeed = 10f;
}
