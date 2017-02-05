using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameMode : NetworkBehaviour {

    [SyncVar(hook = "PlayerWon")]
    private string playerWon = "Sanic";

    private Text winDisplay;

    [ServerCallback]
    void OnTriggerEnter(Collider collider) {
        if (collider.tag.Equals("Player")) {
            PlayerController controller = collider.gameObject.GetComponentInParent<PlayerController>();
            if (controller) {
                playerWon = controller.DisplayName;
            }
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
