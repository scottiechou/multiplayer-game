using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Move : NetworkBehaviour
{
    public GameObject character;

    public int location_x;
    public int location_y;

    // Use this for initialization
    void Start()
    {
        List<GameObject> d_objects = GameObject.FindGameObjectsWithTag("Character").
            Where(t => t.transform.position == this.transform.position).ToList();
        if (d_objects.Count > 0)
        {
            DestroyObject(gameObject);
            return;
        }
        transform.position += new Vector3(0, 0, -1);
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        GameObject[] d_objects = GameObject.FindGameObjectsWithTag("Grid");
        for (int i = 0; i < d_objects.Length; i++)
            DestroyObject(d_objects[i]);
        Character c_character = character.GetComponent<Character>();
        c_character.GetNewLocation(location_x, location_y);
    }
}
