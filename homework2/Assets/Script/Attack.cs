using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Attack : NetworkBehaviour
{

    public GameObject character;

    public int location_x;
    public int location_y;

    public bool bouns = false;

    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer)
            this.enabled = false;
        if (location_x < 0 || location_x >= 8
            || location_y < 0 || location_y >= 10)
        {
            DestroyObject(gameObject);
            return;
        }
        transform.position += new Vector3(0, 0, -1);
        if(bouns)
            transform.position += new Vector3(0, 0, -1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    Character c_att;
    Character c_character;
    void OnMouseDown()
    {
        Vector3 newLocation = new Vector3(location_x - 1, -location_y + 4.5f, 0);
        List<GameObject> A_objects = GameObject.FindGameObjectsWithTag("Character").
            Where(t => t.transform.position == newLocation).ToList();
        c_character = character.GetComponent<Character>();
        Animator animator = character.GetComponent<Animator>();

        if (animator != null)
            animator.SetTrigger("Attack");

        if (A_objects.Count > 0)
        {
            c_att = A_objects[0].GetComponent<Character>();
            if (c_att != null)
            {
                if(bouns)
                    c_character.PlusHit();
                c_character.PlusHit();
            }
        }

        GameObject[] d_objects = GameObject.FindGameObjectsWithTag("Grid");
        for (int i = 0; i < d_objects.Length; i++)
            DestroyObject(d_objects[i]);
    }

}
