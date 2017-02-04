using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    //Local player (client)
    public static PlayerController localPlayer;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private float speed = 250f;
    [SerializeField]
    private ForceMode force = ForceMode.Force;
    private bool isOnGround;
    [SerializeField]
    [Tooltip("This is the ground inclination. Default is 0.6")]
    private float groundSlope = 0.6f;
    [SerializeField]
    private Rigidbody playerBody;
    private Vector3 forward;
    private float stamina = 1;
    public float Stamina { get { return stamina; } }
    private bool running = false;
    [SerializeField]
    private float runningSpeed = 2;

    //Get sanic collision events
    private CollisionReporter reporter;

	void Start () {
        //Get the local player
        if(hasAuthority && isClient) localPlayer = this;

        //Find sanic collision reporter
        reporter = GetComponentInChildren<CollisionReporter>();
        //Register local collision responses
        if (reporter) {
            reporter.ECollisionStay += CollisionStay;
        }else {
            Debug.LogError("No collision reporter!");
        }
    }
	
	void Update () {
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0) {
            running = true;
            stamina -= 0.01f;
        }
        if (!Input.GetKey(KeyCode.LeftShift) && stamina <= 1) {
            running = false;
            stamina += 0.05f;
        }

        if (Input.GetAxis("Vertical") > 0f && isOnGround) {
            forward = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up);
            if (running) {
                playerBody.AddForce(forward * runningSpeed * speed * Time.deltaTime, force);
            }
            else {
                playerBody.AddForce(forward * speed * Time.deltaTime, force);
            }
            isOnGround = false;
        }
	}

    void CollisionStay(Collision collisionInfo) {
        ContactPoint contactPoint = collisionInfo.contacts[0];
        //Debug.Log(contactPoint.normal);
        isOnGround = contactPoint.normal.y > groundSlope;
    }
}
