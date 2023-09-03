using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveTeleportRay : MonoBehaviour
{
    public GameObject LeftTeleportRay;
    public GameObject RightTeleportRay;

    public InputActionProperty LeftActive;
    public InputActionProperty RightActive;

    private void Update()
    {
        LeftTeleportRay.SetActive(LeftActive.action.ReadValue<Vector2>() != Vector2.zero);
        RightTeleportRay.SetActive(RightActive.action.ReadValue<Vector2>() != Vector2.zero);
    }
}
