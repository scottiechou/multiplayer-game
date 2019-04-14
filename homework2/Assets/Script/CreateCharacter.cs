using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class CreateCharacter : NetworkBehaviour
{
    public struct status
    {
        public CharacterType type;
        public int x, y;
    };

    public List<GameObject> objects = new List<GameObject>();

    
    public override void OnStartServer()
    {
        
    }
    
   
    // Use this for initialization
    void Start()
    {
        //Debug.Log(isServer);
        for (int i = 0; i < objects.Count; i++)
        {
            Character character = objects[i].GetComponent<Character>();
            character.location_x = 0;
            character.location_y = i;
            GameObject test = Instantiate(objects[i]);
            NetworkServer.Spawn(test);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
