using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioController : MonoBehaviour {

    private AudioSource source;

    [SerializeField]
    private AudioClip bounceClip;
    [SerializeField]
    private AudioClip boostClip;
    [SerializeField]
    private AudioClip boostClip2;

    private float mag = 0f;
    private float lastEmited = 0f;
    [SerializeField]
    [Tooltip("Delay between sounds emited")]
    private float effectDelay = 0.3f;

	void Start () {
        source = GetComponent<AudioSource>();
        source.loop = false;
        source.playOnAwake = false;
        //Debug.Log("Started");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            //This is just too ugly
            if (Random.value > 0.5f) {
                if (boostClip) {
                    AudioSource.PlayClipAtPoint(boostClip, transform.position, 2f);
                }
            }else {
                if (boostClip2) {
                    AudioSource.PlayClipAtPoint(boostClip2, transform.position, 2f);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collisionInfo) {
        //Debug.Log("Collided");
        //Debug.Log(collisionInfo.relativeVelocity.magnitude);
        //collisionInfo.relativeVelocity.magnitude usually max at 20f
        mag = collisionInfo.relativeVelocity.magnitude;
        if (bounceClip && mag > 1f && Time.realtimeSinceStartup-lastEmited > effectDelay) {
            lastEmited = Time.realtimeSinceStartup;
            source.pitch = Mathf.Clamp(Random.value/2f + (mag / 8f), 0f, 2f);
            source.volume = Mathf.Clamp01(0.01f + (mag / 30f));
            source.PlayOneShot(bounceClip);
        }
    }
}
