using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioController : MonoBehaviour {

    private AudioSource source;

    [SerializeField]
    private AudioClip bounceClip;

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

    void OnCollisionEnter(Collision collisionInfo) {
        //Debug.Log("Collided");
        //Debug.Log(collisionInfo.relativeVelocity.magnitude);
        //collisionInfo.relativeVelocity.magnitude usually max at 20f
        mag = collisionInfo.relativeVelocity.magnitude;
        if (bounceClip && mag > 1f && Time.realtimeSinceStartup-lastEmited > effectDelay) {
            lastEmited = Time.realtimeSinceStartup;
            source.pitch = Mathf.Clamp(Random.value/2f + (mag / 8f), 0f, 2f);
            source.volume = Mathf.Clamp01(0.1f + (mag / 22f));
            source.PlayOneShot(bounceClip);
        }
    }
}
