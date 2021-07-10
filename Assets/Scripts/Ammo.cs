using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{    
    float duration = 3;
    public bool shooted;
    public Vector3 shootOrigin;
    public delegate void HitEventHandler();
    public event HitEventHandler OnAmmoHit;
    public delegate void HitTargetEventHandler(int id);
    public event HitTargetEventHandler OnAmmoHitTarget;
    public GameObject explosionPrefab;
    AudioSource ammoFallingSound;
    AudioSource ammoHittingGround;

    void Start()
    {
        ammoFallingSound = GameObject.Find("SFX").transform.Find("FallingDown").GetComponent<AudioSource>();
        ammoHittingGround = GameObject.Find("SFX").transform.Find("HittingGround").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (shooted)
        {
            transform.Rotate(360 *Time.deltaTime , 0, 0);
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                ammoFallingSound.Play();
                AmmoHit();
                shooted = false;
            }
        }
    }

    void AmmoHit()
    {
        OnAmmoHit?.Invoke();
        if (explosionPrefab)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        OnAmmoHit = null;
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!shooted)
            return;
        if (other.GetComponent<Target>() != null)
        {
            OnAmmoHitTarget?.Invoke(other.GetComponent<Target>().ID);
            other.GetComponent<Target>().ReceiveDamage(100, shootOrigin);            
        }
        else
        {
            ammoHittingGround.Play();
        }
        AmmoHit();
    }

    // void OnCollisionEnter(Collision other)
    // {
        
    //     if (other.gameObject.GetComponent<Target>() != null && shooted)
    //     {
    //         OnAmmoHitTarget?.Invoke(other.gameObject.GetComponent<Target>().ID);
    //         other.gameObject.GetComponent<Target>().ReceiveDamage(100, shootOrigin);
    //         AmmoHit();
    //     }
    // }
}
