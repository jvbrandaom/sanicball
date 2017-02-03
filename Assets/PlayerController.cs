using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private float speed = 250f;
    private bool isOnGround;
    [SerializeField]
    [Tooltip("This is the ground inclination. Default is 0.6")]
    private float groundSlope = 0.6f;
    private Rigidbody playerBody;

	void Start () {
        playerBody = GetComponent<Rigidbody>();
	}
	
	void Update () {
        if (Input.GetAxis("Vertical") > 0f && isOnGround) {
            playerBody.AddForce(mainCamera.transform.forward * speed * Time.deltaTime, ForceMode.Force);
            isOnGround = false;
        }
	}

    void OnCollisionStay(Collision collisionInfo) {
        ContactPoint contactPoint = collisionInfo.contacts[0];
        //Debug.Log(contactPoint.normal);
        isOnGround = contactPoint.normal.y > groundSlope;
    }
}
