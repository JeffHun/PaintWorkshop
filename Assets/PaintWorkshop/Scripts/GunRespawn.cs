using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRespawn : MonoBehaviour
{
    public Transform Holster;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("SprayGun"))
            other.gameObject.transform.position = Holster.transform.position;
    }
}
