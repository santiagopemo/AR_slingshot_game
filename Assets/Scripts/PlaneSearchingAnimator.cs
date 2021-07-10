using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSearchingAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource planeSearchingSound;
    void Start()
    {
        planeSearchingSound = GameObject.Find("SFX").transform.Find("Ping").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlaySound()
    {
        planeSearchingSound.Play();
    }
}
