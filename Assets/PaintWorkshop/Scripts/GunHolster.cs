using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHolster : MonoBehaviour
{
    public Transform Head;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, Head.transform.eulerAngles.y,0);
        transform.position = Head.position;
    }
}
