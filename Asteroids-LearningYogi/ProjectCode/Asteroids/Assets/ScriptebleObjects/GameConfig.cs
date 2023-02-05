using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="GameConfiguration", menuName = "Create Config/Game Configuration")]
public class GameConfig : ScriptableObject
{
    [Header("SpaceShip Configuration")]
    [Space(1)]
    
    public SpaceShipconfig playerConfig;
   
    [Header("Gameplay Configuration")]
    [Space(1)]
    public GameplayConfig gameplayConfig;

    [Header("Astroid Configuration")]
    [Space(1)]
    public AstroidConfig astroids;

    [Header("PowerUp Configuration")]
    public PowerUpConfig powerUpConfig;
}
[System.Serializable]
public class SpaceShipconfig
{
    [Space(1)]
    [Header("Acceleration")]
    [Range(1f, 5f)]
    public float acceleration = 1.3f;
    [Space(1)]
    [Header("Rotation speed")]
    [Range(0.05f, 0.5f)]
    public float rotationSpeed = 0.12f;
    [Space(1)]
    [Header("Amount of Health")]
    [Range(0, 200)]
    public int amountOfHealth = 100;
    [Header("Amount of health to be reduced by dameage")]
    [Range(0, 50)]
    public int reduceHealthbyDamage = 20;
}
[System.Serializable]
public class GameplayConfig
{
    [Space(1)]
    [Header("Gameplay Difficulty Overtime")]
    [Range(10, 30)]
    public int difficultyOverTime = 30;


}
[System.Serializable]
public class AstroidConfig
{

    [Space(1)]
    [Header("Steps to be destroid")]
    [Range(1, 3)]
    public int steps = 2;
}
[System.Serializable]
public class PowerUpConfig
{
    [Space(1)]
    [Header("How often power will appear")]
    [Range(1, 5)]
    public int powerUpOftenRate = 1;

    [Space(1)]
    [Header("PowerUps add here")]
    public List<PowerUPType> totalPowerUps;

}

