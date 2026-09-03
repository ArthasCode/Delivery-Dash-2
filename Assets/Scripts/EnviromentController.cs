using UnityEngine;
using System.Collections.Generic;

public class EnviromentController : MonoBehaviour
{
    private struct ObjectState
    {
        public GameObject gameObject;
        public Vector3 initialPosition;
        public Quaternion initialRotation;
        public Rigidbody rigidbody;
    }

    private List<ObjectState> trackedObjects = new List<ObjectState>();

    private void Awake()
    {
        // Cache initial states of all child objects
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            // Skip the parent object itself
            if (child == transform) continue;

            ObjectState state = new ObjectState
            {
                gameObject = child.gameObject,
                initialPosition = child.localPosition,
                initialRotation = child.localRotation,
                rigidbody = child.GetComponent<Rigidbody>()
            };

            trackedObjects.Add(state);
        }
    }

    public void ResetEnvironment()
    {
        // Restore initial transform and reset physical momentum
        foreach (var state in trackedObjects)
        {
            state.gameObject.SetActive(true);
            state.gameObject.transform.localPosition = state.initialPosition;
            state.gameObject.transform.localRotation = state.initialRotation;

            if (state.rigidbody != null)
            {
                state.rigidbody.linearVelocity = Vector3.zero;
                state.rigidbody.angularVelocity = Vector3.zero;
            }
        }
    }
}
