using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public int frameRate = -1;
    public float maxBirdSpeed = 10f;
    public float birdJumpForce = 10f;
    public float birdMass = 1f;
    public float gravity = 9.81f;
}
