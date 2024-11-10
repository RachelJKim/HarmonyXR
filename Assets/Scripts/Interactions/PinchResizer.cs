using Oculus.Interaction;
using UnityEngine;

public class PinchResizer : MonoBehaviour
{
    public OVRHand leftHand;
    public OVRHand rightHand;
    public Transform leftTip;
    public Transform rightTip;
    public float scaleSpeed = 0.1f;

    private Vector3 initialScale;
    private bool isResizing = false;
    private GameObject pinchedObject;
    private float initialDistance;

    private GameObject GetPinchedObject()
    {
        // Check for overlapping colliders around the left fingertip within a radius
        Collider[] colliders = Physics.OverlapSphere(leftTip.transform.position, 0.02f);
        GameObject leftCollider = null;

        foreach (var collider in colliders)
        {
            if (collider.gameObject.GetComponent<NoteBubble>())
            {
                leftCollider = collider.gameObject;
                Debug.Log("Left Collider Found: " + leftCollider.name);
                break;
            }
        }

        if (leftCollider == null)
            return null;

        // Check if the same object is overlapping the right fingertip
        colliders = Physics.OverlapSphere(rightTip.transform.position, 0.02f);
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
        initialScale = new Vector3(0.1f, 0.1f, 0.1f); // Default initial scale
    }

    void Update()
    {
        bool leftPinch = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        bool rightPinch = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        if (leftPinch && rightPinch)
        {
            if (!isResizing)
            {
                pinchedObject = GetPinchedObject();
                if (pinchedObject != null)
                {
                    isResizing = true;
                    initialScale = pinchedObject.transform.localScale;

                    // Store the initial distance between the fingertips at the start of the pinch
                    initialDistance = Vector3.Distance(leftTip.transform.position, rightTip.transform.position);
                    Debug.Log("Pinching Started");
                }
            }

            if (pinchedObject != null)
            {
                // distance between the fingertips
                float fingerDistance = Vector3.Distance(leftTip.transform.position, rightTip.transform.position);

                // Resize the object based on the distance between the fingertips   
                pinchedObject.transform.localScale = new Vector3(fingerDistance / 2, fingerDistance / 2, fingerDistance / 2); ;
            }
        }
        else
        {
            if (isResizing)
            {
                isResizing = false;
                Debug.Log("Pinching Ended");
                pinchedObject = null;
            }
        }
    }
}
