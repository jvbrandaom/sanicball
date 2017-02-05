using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class SanicNetworkManager : NetworkManager {

    /// <summary>
    /// The connecting player name
    /// </summary>
    private string playerName;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessage) {
        base.OnServerAddPlayer(conn, playerControllerId);

        //Later we will need to receive steam ID here to fetch game names for each user
        if (extraMessage != null) {
            conn.playerControllers[playerControllerId].gameObject.GetComponent<PlayerController>().DisplayName = extraMessage.ReadString();
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        //We redirect to our custom method since we must use our events
        OnServerAddPlayer(conn, playerControllerId, null);
    }

    // called when connected to a server
    public override void OnClientConnect(NetworkConnection conn) {
        //Player name message
        StringMessage message = new StringMessage();
        message.value = playerName;

        if (!ClientScene.ready)
            ClientScene.Ready(conn);
        ClientScene.AddPlayer(conn, 0, message);
    }

    public void SetClientPlayerName(string name) {
        playerName = name;
    }
}
