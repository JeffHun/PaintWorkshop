using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnim : MonoBehaviour
{
    public InputActionProperty PinchAnimAction;
    public InputActionProperty GripAnimAction;
    public Animator HandAnimator;

    void Update()
    {
        float triggerValue = PinchAnimAction.action.ReadValue<float>();
        HandAnimator.SetFloat("Trigger", triggerValue);

        float gripValue = GripAnimAction.action.ReadValue<float>();
        HandAnimator.SetFloat("Grip", gripValue);
    }
}
