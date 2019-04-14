using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {

	// Use this for initialization
	void Start () {
		
	}

    

    // Update is called once per frame
    void Update () {

	}

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Spawn");
        GameObject player = Instantiate(playerPrefab);
        Character character = player.GetComponent<Character>();
        for (int i = 0; i < gameObjects.Length; i++)
        {
            GameObject game = gameObjects[i];
            int count = GameObject.FindGameObjectsWithTag("Character").Where(t => t.transform.position == game.transform.position).Count();
            if (count == 0)
            {
                character.location_x = (gameObjects[i].transform.position.x < 0 ? 0 : 7);
                character.location_y = (gameObjects[i].transform.position.y > 0 ? 0 : 9);
                player.transform.position = gameObjects[i].transform.position;
                break;
            }
        }
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
