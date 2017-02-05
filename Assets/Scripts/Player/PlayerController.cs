using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    //Local player (client)
    public static PlayerController localPlayer;

    [SerializeField]
    private Camera mainCamera;

    #region PlayerInfo
    [SyncVar]
    private string displayName = "Sanic";
    public string DisplayName { get { return displayName; } set { displayName = value; } }
    [SerializeField]
    private Text display;
    #endregion

    #region EFFECTS
    [SerializeField]
    private ParticleSystem boostParticles;
    private ParticleSystem.EmissionModule emission;

    [SerializeField]
    private Light playerGlowLight;

    GameObject model;
    Renderer sanicRenderer;
    Material sanicMaterial;
    float emissionIntensity = 0f;
    #endregion

    #region GENERAL MOVEMENT
    /// <summary>
    /// This is the general movement multiplier.
    /// Other multipliers multiply this and this multiply the speed.
    /// </summary>
    private float movementMultiplier = 1f;
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
    #endregion

    #region STAMINA
    private float stamina = 1;
    public float Stamina { get { return stamina; } }
    [SyncVar(hook = "ReceiveRunning")]
    private bool running = false;
    [SerializeField]
    [Tooltip("Multiplier for when sanic is boosting.")]
    private float runningSpeed = 2;
    #endregion

    [SerializeField]
    [Tooltip("Multiplier for when sanic is airborne.")]
    private float airControlMult = 0.1f;

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

        //Get the player model
        if (playerBody) {
            model = playerBody.gameObject;
            sanicRenderer = model.GetComponent<Renderer>();
            sanicMaterial = sanicRenderer.material;
            sanicMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        }

        //Get the effect emission module
        if (boostParticles) {
            emission = boostParticles.emission;
            if (emission.rateOverDistance.constant > 0) {
                emission.rateOverDistance = 0;
            }
        }

        //Get the player name
        if (display) {
            if(DisplayName == "") {
                DisplayName = NameGen.GenerateName(Random.Range(4,7));
            }
            display.text = DisplayName;
        }
        name = "p"+netId.Value+"_"+displayName;
    }

    void SetEmission(float intensity) {
        sanicMaterial.SetColor("_EmissionColor", Color.white * intensity);
        DynamicGI.UpdateMaterials(sanicRenderer);
        //DynamicGI.UpdateEnvironment();
    }
	
    [Command]
    void CmdSetRunning(bool state) {
        running = state;
    }

    void ReceiveRunning(bool state) {
        if (!hasAuthority) running = state;
    }

    void Update () {
        //Get the player name
        if (display && !display.text.Equals(displayName)) {
            display.text = DisplayName;
        }

        //Run state
        if (hasAuthority) {
            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0) {
                running = true;
                stamina -= 0.01f;
            } else if (stamina <= 0) {
                running = false;
            }
            if (!Input.GetKey(KeyCode.LeftShift) && stamina <= 1) {
                running = false;
                stamina += 0.05f;
            }

            //Broadcast running state
            CmdSetRunning(running);
        }

        //Effects
        if (running) {
            //Particles
            if (boostParticles && emission.rateOverDistance.constant == 0) {
                //Debug.Log(playerBody.velocity.magnitude);
                emission.rateOverDistance = Mathf.Clamp((2 * playerBody.velocity.magnitude), 8, 27);
            }

            //Lights
            if (playerGlowLight) {
                playerGlowLight.intensity = 0.1f + 2.5f * Mathf.Clamp01(playerBody.velocity.magnitude / 16f);
            }

            //Emission
            if (sanicRenderer) {
                emissionIntensity = Mathf.Clamp01(playerBody.velocity.magnitude / 16f);
                SetEmission(emissionIntensity);
            }
        } else {
            //Particles
            if (boostParticles && emission.rateOverDistance.constant > 0) {
                emission.rateOverDistance = 0;
            }

            //Lights
            if (playerGlowLight && playerGlowLight.intensity > 0) {
                playerGlowLight.intensity -= 0.02f;
            }

            //Emission
            if (sanicRenderer && emissionIntensity > 0f) {
                emissionIntensity -= 0.01f;
                SetEmission(emissionIntensity);
            }
        }

        if (Input.GetAxis("Vertical") != 0f && hasAuthority) {
            //Set the input value
            movementMultiplier = MapAxis(Input.GetAxis("Vertical"));

            //Debug.Log("Vert: "+ movementMultiplier);

            //Add the speed
            movementMultiplier *= (running ? runningSpeed : speed);

            //Debug.Log("Run: " + movementMultiplier);

            //Ground control
            movementMultiplier *= (isOnGround ? 1f : airControlMult);

            //Debug.Log("Air: " + movementMultiplier);

            forward = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up);
            playerBody.AddForce(forward * runningSpeed * movementMultiplier * Time.deltaTime, force);

            isOnGround = false;
        }
	}

    void CollisionStay(Collision collisionInfo) {
        ContactPoint contactPoint = collisionInfo.contacts[0];
        //Debug.Log(contactPoint.normal);
        isOnGround = contactPoint.normal.y > groundSlope;
    }

    private float MapAxis(float val) {
        return (val > 0f ? 1f : (val < 0f ? -1f : 0f));
    }

    public void Teleport(Vector3 to) {
        playerBody.isKinematic = true;
        playerBody.transform.position = to;
        playerBody.isKinematic = false;
    }
}
