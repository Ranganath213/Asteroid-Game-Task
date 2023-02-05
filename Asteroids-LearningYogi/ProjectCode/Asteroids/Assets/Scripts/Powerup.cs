using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [Header("Power Up properties")]
    public PowerUPType type;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField]
    private float speed = 50f;
    [SerializeField]
    private float lifetime = 30f;
    [SerializeField] private Sprite sheildSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private TextMesh textMesh;
    public Vector2 size = Vector2.one;

    private void Start()
    {
        List<PowerUPType> totalPowerUps = GameManager.instance.totalPowerups;
        type = totalPowerUps[UnityEngine.Random.Range(0, totalPowerUps.Count)];
        SetupUI();
        transform.eulerAngles = new Vector3(0f, 0f, UnityEngine.Random.value * 360);
        transform.localScale = Vector3.one;
        textMesh.text = "" + (int)type;
        rb.mass = 1;
    }
    void SetupUI()
    {
        sr.sprite = type == PowerUPType.Sheild ? sheildSprite : normalSprite;


    }
    public void SetTrajectory(Vector2 direction)
    {
        rb.AddForce(direction);
        transform.localScale = size;
        Destroy(gameObject, lifetime);
    }
}
