using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameMode : NetworkBehaviour {

    enum GAMESTATE { RUNNING, ENDED }

    [SyncVar(hook = "PlayerWon")]
    private string playerWon = "Sanic";

    [SerializeField]
    private Text winDisplay;

    //Match control
    [SerializeField]
    private float postMatchTime = 6f;
    private float matchEndedTime = 0f;
    private GAMESTATE state = GAMESTATE.RUNNING;
    [SerializeField]
    private Text timerDisplay;

    void Start() {
        if(timerDisplay) timerDisplay.enabled = false;
    }

    [ServerCallback]
    void OnTriggerEnter(Collider collider) {
        //Debug.Log("Hi! "+ collider.transform.parent.gameObject.name);
        PlayerController controller = collider.transform.parent.gameObject.GetComponent<PlayerController>();
        if (controller && state.Equals(GAMESTATE.RUNNING)) {
            //Debug.Log("Player: " + controller.DisplayName);
            playerWon = controller.DisplayName;

            //End match
            matchEndedTime = Time.realtimeSinceStartup;
            state = GAMESTATE.ENDED;
        }
    }

    private void RespawnAllPlayers() {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach(PlayerController player in players) {
            Transform start = SanicNetworkManager.instance.GetStartPosition();
            player.Teleport(start.position);
        }
    }

    private void Update() {
        if (state.Equals(GAMESTATE.ENDED)) {
            float timeToRestart = Time.realtimeSinceStartup - matchEndedTime;

            //Update game display
            if (timerDisplay) {
                timerDisplay.enabled = true;
                timerDisplay.text = "Restart in " + Mathf.Floor(postMatchTime-timeToRestart) + "s";
            }

            //Reset match
            if (timeToRestart > postMatchTime) {
                state = GAMESTATE.RUNNING;
                timerDisplay.enabled = false;
                RespawnAllPlayers();
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
