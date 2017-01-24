using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private float speed = 250f;

    private Rigidbody playerBody;

	void Start () {
        if (player) {
            playerBody = player.GetComponent<Rigidbody>();
        }else {
            Debug.LogError("Missing player object for player controller!");
        }

        if (!playerBody) {
            Debug.LogError("Missing rigidbody object for player object!");
        }
	}
	
	void Update () {
        if (Input.GetAxis("Vertical") > 0f) {
            playerBody.AddForce(mainCamera.transform.forward * speed * Time.deltaTime, ForceMode.Force);
        }
	}
}
