using UnityEngine;
using System.Collections;

/// <summary>
/// Renders the attatched collider bounds on the editor
/// </summary>
[RequireComponent(typeof(Collider))]
public class DrawZone : MonoBehaviour {
    public bool draw = true;

#if UNITY_EDITOR
    [SerializeField]
    private Color volumeColor = new Color(200, 120, 0, 0.3f);
    [SerializeField]
    private Color frameColor = new Color(200, 120, 0, 1f);

    //The collider to draw on the editor
    private Collider bcollider;

    void OnDrawGizmos() {
        if (bcollider == null)
            bcollider = GetComponent<Collider>();

        if (!draw) return;

        Vector3 bounds = bcollider.bounds.extents * 2;
        Gizmos.color = volumeColor;
        Gizmos.DrawCube(transform.position, bounds);
        Gizmos.color = frameColor;
        Gizmos.DrawWireCube(transform.position, bounds);
    }
#endif
}
