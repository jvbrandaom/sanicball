using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshDeformer))]
public class MeshDeformerRigidbody : MonoBehaviour {

    private MeshDeformer deformer;

	void Start () {
        deformer = GetComponent<MeshDeformer>();
    }
	
	void OnCollisionEnter(Collision col) {
        if (deformer) {
            ContactPoint hit = col.contacts[0];

            deformer.AddDeformingForce(hit.point, col.relativeVelocity.magnitude*6f);
        }
    }

}
