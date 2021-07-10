using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Target : MonoBehaviour
{
    public AudioSource damgeSound;
    public int ID;
    public int points = 10;
    int health = 100;

    int pointsDivider = 1;
    public delegate void TargetDestroyedEventHandler (int id, int points);
    public event TargetDestroyedEventHandler OnTargetDestroy;
    void Start()
    {
        damgeSound = GameObject.Find("SFX").transform.Find("Pikachu").GetComponent<AudioSource>();
        damgeSound.playOnAwake = false;
    }
    public void ReceiveDamage(int damage, Vector3 shootOrigin)
    {
        health -= damage;
        if (health <= 0)
        {
            OnTargetDestroy?.Invoke(ID, GetPointsAccordingDistance(shootOrigin));
            OnTargetDestroy = null;
            damgeSound.Play();
            Destroy(this.gameObject);
        }
    }

    int GetPointsAccordingDistance(Vector3 shootOrigin)
    {
        return points + (int)Vector3.Distance(transform.position, shootOrigin) / pointsDivider;
    }

}
