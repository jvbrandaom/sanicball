using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionReporter : MonoBehaviour {

    public delegate void DCollsionEnter(Collision collisionInfo);
    public event DCollsionEnter ECollisionEnter;
    public delegate void DCollsionStay(Collision collisionInfo);
    public event DCollsionStay ECollisionStay;
    public delegate void DCollsionExit(Collision collisionInfo);
    public event DCollsionExit ECollisionExit;

    public delegate void DTriggerEnter(Collider other);
    public event DTriggerEnter ETriggerEnter;
    public delegate void DTriggerExit(Collider other);
    public event DTriggerExit ETriggerExit;

    void OnTriggerExit(Collider other) {
        if (ETriggerExit != null) {
            ETriggerExit(other);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (ETriggerEnter != null) {
            ETriggerEnter(other);
        }
    }

    void OnCollsionEnter(Collision collisionInfo) {
        if (ECollisionEnter != null) {
            ECollisionEnter(collisionInfo);
        }
    }

    void OnCollisionStay(Collision collisionInfo) {
        if (ECollisionStay != null) {
            ECollisionStay(collisionInfo);
        }
    }

    void OnCollsionExit(Collision collisionInfo) {
        if (ECollisionExit != null) {
            ECollisionExit(collisionInfo);
        }
    }
}
