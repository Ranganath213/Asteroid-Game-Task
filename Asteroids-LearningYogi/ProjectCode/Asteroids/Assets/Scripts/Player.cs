using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Bullet bulletPrefab;

    public float thrustSpeed = 1f;
    public float turnSpeed = 1f;

    private bool thrusting;
    private float turnDirection;

    public int playerHealth;
    public int reduceHealthByDamage = 20;
    private Rigidbody2D rb;
    [SerializeField] private UbhSpreadNwayShot moonShapeShot;
    [SerializeField] private UbhLinearShot paintshapeShot;
    [SerializeField] private UbhCircleShot circleShot;
    [SerializeField] private UbhSpreadNwayShot normalShot;

    public bool isPowerUPActive;
    public bool isSheildOn;
    [SerializeField] private GameObject sheildObj;
    public PowerUPType _currentPower = PowerUPType.None;
    [SerializeField] private UbhShotCtrl baseShotCtrl;
    [SerializeField] private UbhShotCtrl spread8WayShotCtrl;
    [SerializeField] private UbhShotCtrl circlShotCtrl;
    [SerializeField] private UbhShotCtrl paintShotCtrl;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        if (GameManager.instance.isStarted) return;
        sheildObj.SetActive(isSheildOn);
        if (!isPowerUPActive)
            normalShot.m_centerAngle = this.transform.eulerAngles.z;
        else if (isPowerUPActive && _currentPower == PowerUPType.Spread8Way)
            moonShapeShot.m_centerAngle = this.transform.eulerAngles.z;
        else if (isPowerUPActive && _currentPower == PowerUPType.LinearPaint)
            paintshapeShot.m_angle = this.transform.eulerAngles.z;

        thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1.0f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1.0f;
        }
        else
        {
            turnDirection = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (thrusting)
        {
            rb.AddForce(transform.up * thrustSpeed);
        }

        if (turnDirection != 0)
        {
            rb.AddTorque(turnDirection * turnSpeed);
        }
    }

    private void Shoot()
    {
        if (GameManager.instance.isStarted == false && GameManager.instance.startText.activeSelf)
        { 
            GameManager.instance.spawner.StartGame();
            GameManager.instance.SwitchStartText(false);
        }
        //Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        //bullet.Project(transform.up);
        if (_currentPower == PowerUPType.Spread8Way)
            moonShapeShot.Shot();
        else if (_currentPower == PowerUPType.LinearPaint)
            paintshapeShot.Shot();
        else if (_currentPower == PowerUPType.Circle)
            circleShot.Shot();
        else normalShot.Shot();
        GameManager.instance.am.PlayPlayerShootSound();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PowerUp")
        {
            Powerup powerup = collision.gameObject.GetComponent<Powerup>();
            if (powerup != null)
            {

                //  GameManager.instance.StopPowerUp();
                GameManager.instance.StartPowerUp(powerup);
                _currentPower = powerup.type == PowerUPType.Sheild ? _currentPower : powerup.type;

                if (powerup.type == PowerUPType.LinearPaint || powerup.type == PowerUPType.Spread8Way || powerup.type == PowerUPType.Circle)
                    isPowerUPActive = true;

                if (powerup.type == PowerUPType.Sheild)
                    isSheildOn = true;

                if (isPowerUPActive)
                {
                    spread8WayShotCtrl.gameObject.SetActive(_currentPower == PowerUPType.Spread8Way);
                    paintShotCtrl.gameObject.SetActive(_currentPower == PowerUPType.LinearPaint);
                    circlShotCtrl.gameObject.SetActive(_currentPower == PowerUPType.Circle);
                    baseShotCtrl.gameObject.SetActive(false);
                }

                Destroy(collision.gameObject);
            }
        }

        if (isSheildOn)
            return;
        if (collision.gameObject.tag == "Asteroid")
        {
            playerHealth = playerHealth >= 0 ? playerHealth - reduceHealthByDamage : 0;
            GameManager.instance.healthBar.SetHealth(playerHealth);
            if (playerHealth <= 0f)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0f;

                gameObject.SetActive(false);

                GameManager.instance.PlayerDied();
            }

        }


    }

    public void ResetPowerUps()
    {
        spread8WayShotCtrl.gameObject.SetActive(false);
        paintShotCtrl.gameObject.SetActive(false);
        circlShotCtrl.gameObject.SetActive(false);
        baseShotCtrl.gameObject.SetActive(true);
        _currentPower = PowerUPType.None;
    }



}
