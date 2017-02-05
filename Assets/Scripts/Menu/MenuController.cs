using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    /// <summary>
    /// The field for the client player name.
    /// </summary>
    [SerializeField]
    private InputField clientNameField;

    /// <summary>
    /// The field for the server address.
    /// </summary>
    [SerializeField]
    private InputField addressField;

    [SerializeField]
    private GameObject controlsPanel;

    /// <summary>
    /// Call this when the client wants to join a server
    /// </summary>
    public void JoinGameBtn() {
        SanicNetworkManager.instance.SetClientPlayerName(clientNameField.text);
        SanicNetworkManager.instance.networkAddress = addressField.text;
        SanicNetworkManager.instance.StartClient();
    }

    /// <summary>
    /// Call this when the client wants to create a server
    /// </summary>
    public void HostMatchBtn() {
        SanicNetworkManager.instance.SetClientPlayerName(clientNameField.text);
        SanicNetworkManager.instance.StartHost();
    }

    public void ToogleControlPanel() {
	controlsPanel.SetActive(!controlsPanel.activeSelf);
    }

    public void Quit() {
	#if UNITY_EDITOR
	UnityEditor.EditorApplication.isPlaying = false;
	#else
	Application.Quit();
	#endif
    }
}
