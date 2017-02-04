using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

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
    private Rigidbody playerBody;
    private Vector3 forward;
    private float stamina = 1;
    public float Stamina { get { return stamina; } }
    private bool running = false;
    [SerializeField]
    private float runningSpeed = 2;
    public static PlayerController localPlayer;

	void Start () {
        playerBody = GetComponent<Rigidbody>();
        localPlayer = this;
	}
	
	void Update () {
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            Debug.Log(stamina);
            running = true;
            stamina -= 0.01f;
        }
        if (!Input.GetKey(KeyCode.LeftShift) && stamina <= 1)
        {
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

    void OnCollisionStay(Collision collisionInfo) {
        ContactPoint contactPoint = collisionInfo.contacts[0];
        //Debug.Log(contactPoint.normal);
        isOnGround = contactPoint.normal.y > groundSlope;
    }
}
