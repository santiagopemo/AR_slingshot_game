using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    public GameObject ammoPrefab;
    public GameObject currentAmmo = null;
    int forceMultiplier = 600; // 800
    int forceFordwardScalar = 4; // 5
    public int AmmoLeft {get; set;}

    public bool isDrag;
    float mouseZCoord;
    Vector3 mouseOffset;
    public AudioSource shootSound;

    public delegate void OnReloadEventHandler(int ammoLeft);
    public event OnReloadEventHandler OnReload;

    // Start is called before the first frame update
    void Start()
    {
        // shootSound = GetComponent<AudioSource>();
        AmmoLeft = 100;
        // Reload();
    }
    public void Reload()
    {
        if (currentAmmo == null && transform.childCount == 0 && AmmoLeft > 0)
        {
            currentAmmo = Instantiate(ammoPrefab, transform.position, transform.rotation, transform);
            currentAmmo.GetComponent<Rigidbody>().isKinematic = true;
            currentAmmo.GetComponent<Ammo>().OnAmmoHit += OnCurrentAmmoHit;
            AmmoLeft--;            
        }
        OnReload?.Invoke(AmmoLeft);
    }
    void Shoot()
    {
        if (currentAmmo != null && transform.childCount == 1)
        {
            currentAmmo.transform.parent = null;
            currentAmmo.GetComponent<Rigidbody>().isKinematic = false;
            currentAmmo.GetComponent<Rigidbody>().AddForce(GetShootForce());
            currentAmmo.GetComponent<Ammo>().shooted = true;
            currentAmmo.GetComponent<Ammo>().shootOrigin = currentAmmo.transform.position;       
            currentAmmo = null;
            shootSound.Play();
            // StartCoroutine(DelayedReload(1));
        }
    }
    public void Clear()
    {
        if (currentAmmo)
        {
            Destroy(currentAmmo);
            currentAmmo = null;            
        }
        AmmoLeft = 0;
    }

    IEnumerator DelayedReload(float Seconds)
    {
        yield return new WaitForSeconds(Seconds);
        Reload();
    }

    void OnCurrentAmmoHit()
    {
        StartCoroutine(DelayedReload(2));
    }

    public Vector3 GetShootForce()
    {
        Vector3 force = (transform.position - currentAmmo.transform.position) * forceMultiplier;
        force = ((transform.forward * force.magnitude * forceFordwardScalar) + force);
        return force;
    }

    public Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mouseZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void Update()
    {
        if (currentAmmo == null)
            return;
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isDrag)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == currentAmmo)
                {
                    GrabAmmo();
                    isDrag = true;
                }
            }
        }
        if (isDrag)
        {
            DragAmmo();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && isDrag)
        {
            ReleaseAmmo();
            isDrag = false;
        }
    }

    void GrabAmmo()
    {
        mouseZCoord = Camera.main.WorldToScreenPoint(currentAmmo.transform.position).z;
        mouseOffset = currentAmmo.transform.position - GetMouseWorldPos();
    }
    void DragAmmo()
    {
        currentAmmo.transform.position = GetMouseWorldPos() + mouseOffset;
        currentAmmo.transform.forward = GetShootForce().normalized;
    }
    void ReleaseAmmo()
    {
        Shoot();
    }
}
