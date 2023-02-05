using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    public float size = 1f;
    public float minSize = 1f;
    public float maxSize = 1.7f;
    [SerializeField]
    private float speed = 50f;
    [SerializeField]
    private float lifetime = 30f;
    Transform currentTransform;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentTransform = this.transform;
    }

    private void Start()
    {
        sr.sprite = sprites[Random.Range(0, sprites.Length)];

        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360);
        transform.localScale = Vector3.one * size;

        rb.mass = size;
    }

    public void SetTrajectory(Vector2 direction)
    {
        rb.AddForce(direction * speed);

        Destroy(gameObject, lifetime);
    }

    bool wait = false;
    IEnumerator Wait()
    {
        wait = true;
        yield return new WaitForSeconds(0.5f);
        wait = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (wait) return;
        StartCoroutine(Wait());

        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(gameObject);
            if (/*(size * 0.5f)*/size >= maxSize && GameManager.instance.stepsToBeDestroyed == 3)
            {
                GameManager.instance.spawner.CreateSplit(currentTransform, speed);
                GameManager.instance.spawner.CreateSplit(currentTransform, speed);
                GameManager.instance.spawner.CreateSplit(currentTransform, speed);
            }
            else if (/*(size * 0.5f)*/size >= maxSize && GameManager.instance.stepsToBeDestroyed == 2)
            {
                GameManager.instance.spawner.CreateSplit(currentTransform, speed);
                GameManager.instance.spawner.CreateSplit(currentTransform, speed);
            }
            else if (/*(size * 0.5f)*/size >= maxSize && GameManager.instance.stepsToBeDestroyed == 1)
            {
                GameManager.instance.spawner.CreateSplit(currentTransform, speed);
            }
            GameManager.instance.AsteroidDestroyed(this, currentTransform, speed);
          //  GameManager.instance.spawner.SpawnPowerUP(currentTransform, speed);
        }
        //if (collision.gameObject.tag == "Asteroid")
        //{
        //     rb.AddForce((Random.insideUnitCircle.normalized * speed) * speed);
        //}
    }




}
