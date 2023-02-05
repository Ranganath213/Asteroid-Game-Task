using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource playerShootSource;
    [SerializeField]
    private AudioClip playerShootSound;

    [SerializeField]
    private AudioSource playerThrustSource;

    [SerializeField]
    private AudioSource smallExplosionSource;
    [SerializeField]
    private AudioClip smallExplosionSound;

    [SerializeField]
    private AudioSource mediumExplosionSource;
    [SerializeField]
    private AudioClip mediumExplosionSound;

    [SerializeField]
    private AudioSource largeExplosionSource;
    [SerializeField]
    private AudioClip largeExplosionSound;

    [SerializeField]
    private AudioSource extraShipSource;
    [SerializeField]
    private AudioClip extraShipSound;

    [SerializeField]
    private AudioSource beat1Source;
    [SerializeField]
    private AudioClip beat1Sound;

    [SerializeField]
    private AudioSource beat2Source;
    [SerializeField]
    private AudioClip beat2Sound;

    public float beatRate;

    private void Start()
    {
        StartCoroutine(BackgroundBeatSound());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (!playerThrustSource.isPlaying)
            {
                playerThrustSource.Play();
            }
        }
        else
        {
            if (playerThrustSource.isPlaying)
            {
                playerThrustSource.Stop();
            }
        }
    }

    private IEnumerator BackgroundBeatSound()
    {
        while (true)
        {
            beat1Source.PlayOneShot(beat1Sound, 1f);

            yield return new WaitForSeconds(beatRate);

            beat2Source.PlayOneShot(beat2Sound, 1f);

            yield return new WaitForSeconds(beatRate);
        }
    }

    public void PlayPlayerShootSound()
    {
        playerShootSource.PlayOneShot(playerShootSound, 0.6f);
    }

    public void PlaySmallExplosionSound()
    {
        smallExplosionSource.PlayOneShot(smallExplosionSound, 1f);
    }

    public void PlayMediumExplosionSound()
    {
        mediumExplosionSource.PlayOneShot(mediumExplosionSound, 1f);
    }

    public void PlayLargeExplosionSound()
    {
        largeExplosionSource.PlayOneShot(largeExplosionSound, 1f);
    }

    public void PlayExtraShipSound()
    {
        StartCoroutine(ExtraShipSound());
    }

    private IEnumerator ExtraShipSound()
    {
        for (int i = 0; i < 6; i++)
        {
            extraShipSource.PlayOneShot(extraShipSound, 1f);

            yield return new WaitForSeconds(0.136f);
        }
    }
}
