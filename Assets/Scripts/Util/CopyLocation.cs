using UnityEngine;
using System.Collections;

/// <summary>
/// Sets this object position to desired position every frame.
/// </summary>
public class CopyLocation : MonoBehaviour {
    /// <summary>
    /// The object to copy.
    /// </summary>
    [Tooltip("Object to copy position")]
    public Transform objectToCopy;

    [Header("Select the axis to copy.")]
    public bool x;
    public bool y;
    public bool z;

	// Use this for initialization
	void Start () {
        if(!objectToCopy)
            Debug.LogError("Select an object to copy!");	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 desiredPosition = transform.position;

        if(x)
            desiredPosition.x = objectToCopy.position.x;
        if(y)
            desiredPosition.y = objectToCopy.position.y;
        if(z)
            desiredPosition.z = objectToCopy.position.z;

        transform.position = desiredPosition;
    }
}
