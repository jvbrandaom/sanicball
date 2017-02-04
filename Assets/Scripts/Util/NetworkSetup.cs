using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public struct GameObjectEntry{
    public GameObject obj;
    [Tooltip("Turn on/off if is running in the local player.")]
    public bool turnOn;
}

[System.Serializable]
public struct ObjectComponentsEntry {
    public GameObject owner;
    [Tooltip("Turn on/off if is running in the local player.")]
    public bool turnOn;
}

[System.Serializable]
public struct ComponentEntry {
    public Behaviour comp;
    [Tooltip("Turn on/off if is running in the local player.")]
    public bool turnOn;
}

/// <summary>
/// This script turns elements on or off depending if they are local player's or
/// dummies.
/// </summary>
public class NetworkSetup : NetworkBehaviour {

    [Tooltip("Gameobjects in this list will be turned on or off themselves.")]
    public GameObjectEntry[] gameObjects;

    [Tooltip("Gameobjects in this list will receive the on/off in each of their components.")]
    public ObjectComponentsEntry[] components;

    [Tooltip("Monobhaviours in this list will be turned on or off themselves.")]
    public ComponentEntry[] mbehaviours;

    // Player network setup
    void Start() {
        if (isLocalPlayer) {
            //Turn on or off any game objects
            if (gameObjects != null) {
                foreach(GameObjectEntry entry in gameObjects) {
                    entry.obj.SetActive(entry.turnOn);
                }
            }

            //Turn on or of any components
            if (components != null) {
                foreach (ObjectComponentsEntry entry in components) {
                    Behaviour[] behaviours = entry.owner.GetComponents<Behaviour>();
                    foreach (Behaviour behaviour in behaviours) behaviour.enabled = entry.turnOn;
                }
            }

            //Turn on or of individual components
            if (mbehaviours != null) {
                foreach(ComponentEntry entry in mbehaviours) {
                    entry.comp.enabled = entry.turnOn;
                }
            }
        }
    }
}
