using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioController : MonoBehaviour {

    private AudioSource source;

    [SerializeField]
    private AudioClip bounceClip;

	void Start () {
        source = GetComponent<AudioSource>();
        source.loop = false;
        source.playOnAwake = false;
    }

    void OnCollsionEnter(Collision collisionInfo) {
        Debug.Log("Collided");
        Debug.Log(collisionInfo.relativeVelocity.magnitude);
        if (bounceClip) {
            //source.pitch = collisionInfo.relativeVelocity.magnitude;
            source.PlayOneShot(bounceClip);
        }
    }
}
