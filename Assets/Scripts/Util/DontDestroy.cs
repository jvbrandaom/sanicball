using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps objects
/// </summary>
public class DontDestroy : MonoBehaviour {
    private static bool created = false;

    void Awake() {
        if (!created) {
            // this is the first instance - make it persist
            DontDestroyOnLoad(gameObject);
            created = true;
        } else {
            // this must be a duplicate from a scene reload - DESTROY!
            Destroy(gameObject);
        }
    }

}
