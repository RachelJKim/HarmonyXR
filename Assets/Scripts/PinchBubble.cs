using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchBubble : MonoBehaviour
{
    public OVRHand leftHand;
    public OVRHand rightHand;
    public GameObject leftTip;
    public GameObject rightTip;
    public float scaleSpeed = 0.1f;
    public GameObject noteBubble;

    private Vector3 initialScale;
    private bool isPinching = false;
    private GameObject pinchedObject;

    private GameObject GetPinchedObject()
    {
        Collider[] colliders = Physics.OverlapSphere(leftTip.transform.position, 0.02f);
        //Debug.Log("HERE" + colliders.Length);
        GameObject leftCollider = null;
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Bubble"))
            {
                leftCollider = collider.gameObject;
                Debug.LogError("Left Collider Found" + leftCollider.name);
                break;
            }
        }
        Debug.Log("HERE2 " + leftCollider.name);

        colliders = Physics.OverlapSphere(rightTip.transform.position, 0.02f);
        Debug.LogError(colliders.Length);
        foreach (var collider in colliders)
        {
            if (collider.gameObject == leftCollider)
            {
                return leftCollider;
            }
        }

        return null;
    }

    void Start()
    {
        initialScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    void Update()
    {
        bool leftPinch = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        bool rightPinch = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        if (leftPinch && rightPinch)
        {
            if (!isPinching)
            {
                pinchedObject = GetPinchedObject();
                if (pinchedObject != null)
                {
                    isPinching = true;
                    Debug.LogError("Pinching Started");
                    initialScale = pinchedObject.transform.localScale;
                }
                
            }

            if (pinchedObject != null)
            {
                float distance = Vector3.Distance(leftTip.transform.position, rightTip.transform.position);
                //float scaleFactor = distance * scaleSpeed;
                //pinchedObject.transform.localScale = initialScale + new Vector3(scaleFactor, scaleFactor, scaleFactor);
                Debug.Log("HERE" + pinchedObject.name);
                pinchedObject.transform.localScale = new Vector3(distance/2, distance / 2, distance / 2);
            }
        }
        else
        {
            if (isPinching)
            {
                isPinching = false;
                //initialScale = transform.localScale;
                Debug.LogError("Pinching Ended");
                pinchedObject = null;
            }
        }


    }
}

