using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameMode : NetworkBehaviour {

    [SyncVar(hook = "PlayerWon")]
    private string playerWon = "Sanic";

    [SerializeField]
    private Text winDisplay;

    [ServerCallback]
    void OnTriggerEnter(Collider collider) {
        //Debug.Log("Hi! "+ collider.transform.parent.gameObject.name);
        PlayerController controller = collider.transform.parent.gameObject.GetComponent<PlayerController>();
        if (controller) {
            //Debug.Log("Player: " + controller.DisplayName);
            playerWon = controller.DisplayName;
        }
    }

	private void PlayerWon(string value) {
        playerWon = value;
        if (winDisplay) {
            winDisplay.enabled = true;
            winDisplay.text = "Player " + playerWon + " won!";
        }
    }
}
