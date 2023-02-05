using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField]
    private Asteroid asteroidPrefab;
    [SerializeField]
    private Powerup powerupPrefab;
    [SerializeField]
    private float trajectoryVariance = 15f;
    [SerializeField]
    private float spawnRate = 5f;
    [SerializeField]
    private float spawnDistance = 15f;
    [SerializeField]
    private int spawnAmount = 1;
    public int difficultyTimer;
    //private void Start()
    //{
    //    // InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);

    //}

    public void StartGame()
    {
        if (I_TimerCoroutine == null)
            I_TimerCoroutine = StartCoroutine(nameof(I_StartSpawning));
    }

    Coroutine I_TimerCoroutine;
    int spawntime = 0;
    public int timer = 0;
    IEnumerator I_StartSpawning()
    {
        timer = 0;
        spawnRate = 5;
        while(true)
        {
            spawntime++;
            timer++;
            if (timer >= difficultyTimer)
            {
                spawnRate = spawnRate > 2 ? --spawnRate : spawnRate; 
                timer = 0;
            }
            if (spawntime >= spawnRate)
            {
                Spawn();
                spawntime = 0;

            }

            yield return new WaitForSeconds(1f);
        }
    }

    public void StartSpawning()
    {
        //CancelInvoke(nameof(Spawn));
        //InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);

        if (I_TimerCoroutine != null)
            I_TimerCoroutine = StartCoroutine(nameof(I_StartSpawning));
    }

    List<GameObject> objs = new List<GameObject>();
    private void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
            Vector3 spawnPos = transform.position + spawnDirection;

            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

       

            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPos, rotation);
            asteroid.size = asteroid.maxSize;
            
            asteroid.SetTrajectory(rotation * -spawnDirection);

            objs.Add(asteroid.gameObject);
        }
    }

    public void CreateSplit(Transform _transform, float _speed)
    {
        Vector2 pos = _transform.position;
        pos += Random.insideUnitCircle * 0.5f;

        Asteroid half = Instantiate(asteroidPrefab, pos, _transform.rotation);

        // half.size = size * 0.5f;
        half.size = 1;

        half.SetTrajectory(Random.insideUnitCircle.normalized * _speed);
    }

    public void SpawnPowerUP(Transform _transform, float _speed)
    {
        Vector2 pos = _transform.position;
        pos += Random.insideUnitCircle * 0.5f;

        Powerup half = Instantiate(powerupPrefab, pos, _transform.rotation);
        half.size = Vector2.one * 2;
        half.SetTrajectory(Random.insideUnitCircle.normalized);
    }

    public void StopSpawnAndDestroy()
    {
        timer = 0;
        // CancelInvoke(nameof(Spawn));
        if (I_TimerCoroutine != null) StopCoroutine(I_TimerCoroutine);
        foreach (var item in objs)
        {
            if (item != null) Destroy(item);
        }
    }

}
