using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Gameplay Configuration Props")]
    [Space(5)]
    [SerializeField] private GameConfig gameConfig;

    [SerializeField]
    private Player player;
    [SerializeField]
    private ParticleSystem explosion;

    [SerializeField]
    private int lives = 3;
    [SerializeField]
    private int maxLives = 10;
    private int score = 0;
    [SerializeField]
    private float respawnInvulnerabilityTime = 3f;
    [SerializeField]
    private float respawnTime = 3f;
    private bool gameOver = false;
    public GameObject startText;
    [SerializeField]
    private Text scoreText;
    [SerializeField] private Text finalScoreText;
    [SerializeField]
    private Text livescount;
    [SerializeField]
    private GameObject gameOverPopup;

    private int extraLifeCount = 10000;
    private int extraLifeScore = 10000;

    public AudioManager am;

    [Header("Player health bar properties")]
    public HealthBar healthBar;
    [Header("Astreoid steps to be destroyed")]
    public int stepsToBeDestroyed;
    [Header("Asteroid Spawner")]
    public AsteroidSpawner spawner;
    [Header("PowerUp Bar Properties")]
    public PowerUpHealth sheildPowerUpHelath;
    public PowerUpHealth normalPowerUpHelath;
    [HideInInspector] public Powerup activePowerUp;
    public List<PowerUPType> totalPowerups;
    public int powerupsOftenRate = 1;
    public int powerupsOftenCount = 0;
    public bool isStarted = true;

    private void Awake()
    {
        isStarted = true;
        if (instance == null) instance = this;
        totalPowerups = new List<PowerUPType>();
        SetGameConfigValues();

    }

    private void Start()
    {
        UpdateScoreText();
        UpdateLifeIcons();
        gameOverPopup.SetActive(false);

    }

    public void SwitchStartText(bool _switch)
    {
        startText.SetActive(_switch);
    }

    private void Update()
    {
        if (gameOver == true && Input.GetKeyDown(KeyCode.R))
        {
            gameOver = false;
            gameOverPopup.SetActive(false);
            Respawn();
            spawner.StartSpawning();
        }
    }

    #region Player Properties

    public void StopPowerUp()
    {
        if (player.isPowerUPActive == true)
        {
            player.isPowerUPActive = false;
            normalPowerUpHelath.StopPowerUp();
            normalPowerUpHelath.gameObject.SetActive(false);
            player.ResetPowerUps();
        }


    }

    public void StopsheildPower()
    {

        player.isSheildOn = false;
        sheildPowerUpHelath.StopPowerUp();
        sheildPowerUpHelath.gameObject.SetActive(false);
    }


    public void StartPowerUp(Powerup _powerUp)
    {
        //  normalPowerUpHelath.gameObject.SetActive(false);
        activePowerUp = _powerUp;
        if (activePowerUp.type == PowerUPType.Sheild)
        {
            sheildPowerUpHelath.gameObject.SetActive(true);
            sheildPowerUpHelath.StartPowerUp(activePowerUp.type);
        }
        else
        {
            normalPowerUpHelath.gameObject.SetActive(true);
            normalPowerUpHelath.StartPowerUp(activePowerUp.type);
        }
    }


    public void ResetPlayerHealthProperties()
    {
        player.reduceHealthByDamage = gameConfig.playerConfig.reduceHealthbyDamage;
        player.playerHealth = gameConfig.playerConfig.amountOfHealth;
        healthBar.SetMaxHealth(player.playerHealth);
    }

    #endregion


    public void AsteroidDestroyed(Asteroid asteroid, Transform trans, float speed)
    {
        explosion.transform.position = asteroid.transform.position;
        explosion.Play();

        if (asteroid.size <= asteroid.minSize)
        {
            score += 100;
            am.PlaySmallExplosionSound();
        }
        //else if (asteroid.size < 1.2)
        //{
        //    score += 50;
        //    am.PlayMediumExplosionSound();
        //}
        //else
        //{
        //    score += 25;
        //    am.PlayLargeExplosionSound();
        //}

        if (score >= extraLifeCount)
        {
            am.PlayExtraShipSound();
            extraLifeCount += extraLifeScore;
            lives++;
            UpdateLifeIcons();
        }
        int powerUpPropebility = Random.Range(1, 5);

        if (powerUpPropebility == 3)
        {
            powerupsOftenCount++;
            if (powerupsOftenCount > powerupsOftenRate)
            {
                powerupsOftenCount = 0;
                spawner.SpawnPowerUP(trans, speed);
            }
        }
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        int remainder = score % 10000;
        if (remainder < 1000)
        {
            am.beatRate = 1f;
        }
        else if (remainder < 2000)
        {
            am.beatRate = 0.8f;
        }
        else if (remainder < 3000)
        {
            am.beatRate = 0.7f;
        }
        else if (remainder < 4000)
        {
            am.beatRate = 0.6f;
        }
        else if (remainder < 5000)
        {
            am.beatRate = 0.5f;
        }
        else if (remainder < 8000)
        {
            am.beatRate = 0.4f;
        }
        else
        {
            am.beatRate = 0.3f;
        }

        scoreText.text = score.ToString();
    }

    public void PlayerDied()
    {
        explosion.transform.position = player.transform.position;
        explosion.Play();
        am.PlayMediumExplosionSound();

        player.ResetPowerUps();
        player.isSheildOn = false;
        player.isPowerUPActive = false;
        sheildPowerUpHelath.gameObject.SetActive(false);
        normalPowerUpHelath.gameObject.SetActive(false);

        lives--;

        if (lives <= 0)
        {
            UpdateLifeIcons();
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), respawnTime);
        }
    }

    private void UpdateLifeIcons()
    {

        int val = Mathf.Min(lives, maxLives);

        livescount.text = val.ToString();

    }


    private void Respawn()
    {
        UpdateLifeIcons();
        UpdateScoreText();
        //Reset Maximum health on respan
        ResetPlayerHealthProperties();

        player.transform.position = Vector3.zero;
        player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        player.gameObject.SetActive(true);
        Invoke(nameof(TurnOnCollisions), respawnInvulnerabilityTime);

    }

    private void TurnOnCollisions()
    {
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver()
    {
        lives = 3;
        finalScoreText.text = "Score - " + score;
        score = 0;
        extraLifeCount = extraLifeScore;

        gameOverPopup.SetActive(true);
        gameOver = true;
        spawner.StopSpawnAndDestroy();
        // Invoke(nameof(Respawn), respawnTime);
    }


    public void SetGameConfigValues()
    {
        //Assigh Game Config values 
        player.playerHealth = gameConfig.playerConfig.amountOfHealth;
        player.reduceHealthByDamage = gameConfig.playerConfig.reduceHealthbyDamage;
        player.thrustSpeed = gameConfig.playerConfig.acceleration;
        player.turnSpeed = gameConfig.playerConfig.rotationSpeed;
        spawner.difficultyTimer = gameConfig.gameplayConfig.difficultyOverTime;

        //Assign Asteroids Config values
        stepsToBeDestroyed = gameConfig.astroids.steps;

        //Assign Powerup properties
        powerupsOftenRate = gameConfig.powerUpConfig.powerUpOftenRate;
        totalPowerups = ValidatePowerUps(gameConfig.powerUpConfig.totalPowerUps);


        ResetPlayerHealthProperties();
    }

    List<PowerUPType> ValidatePowerUps(List<PowerUPType> _list)
    {
        List<PowerUPType> _powerups = new List<PowerUPType>();
        foreach (var item in _list)
        {
            if (!_powerups.Contains(item) && item != PowerUPType.None) _powerups.Add(item);
        }

        return _powerups;
    }


}
[System.Serializable]
public enum PowerUPType
{
    None = 0,
    Sheild,
    Spread8Way,
    LinearPaint,
    Circle

}