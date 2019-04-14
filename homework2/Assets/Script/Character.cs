using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public enum CharacterType { Assassin, Archer, Caster, Tank };

public class Character : NetworkBehaviour
{
    [SyncVar]
    public int HP;
    [SyncVar]
    public int HitCount;
    public int maxHp;

    public int attack;

    public string c_name;

    public CharacterType type = CharacterType.Assassin;

    public GameObject MoveGrid;
    public GameObject AttackGrid;

    [SyncVar]
    public int location_x;
    [SyncVar]
    public int location_y;
    [SyncVar]
    public bool GameStart = false;
    Animator animator;

    private int move_y;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        location_x = (int)this.transform.position.x + 1;
        location_y = (int)(-this.transform.position.y + 4.5f);

        c_name = Network.player.ipAddress;

        if (isServer)
            NetworkInfo.Login(this);
        if (!isLocalPlayer)
            return;

        this.transform.position = new Vector3(location_x - 1, -location_y + 4.5f, 0);
    }

    [ClientRpc]
    public void RpcGetPlayerCount(int count)
    {
        Text text = GameObject.Find("txtPlayerCount").GetComponent<Text>();
        text.text = "目前人數 : " + count;
    }

    [ClientRpc]
    public void RpcGetHitRank(string s_rank)
    {
        Text text = GameObject.Find("txtRank").GetComponent<Text>();
        text.text = s_rank;
    }

    [ClientRpc]
    public void RpcGetGameStart(bool start, string txt, bool endGame)
    {
        GameStart = start;
        Text text = GameObject.Find("txtGameStart").GetComponent<Text>();
        Text txtTime = GameObject.Find("txtGameTime").GetComponent<Text>();
        text.text = "房間狀態 : 尚未開始";
        if (endGame)
            text.text = "房間狀態 : 結束";

        txtTime.text = txt;
        if (start)
        {
            text.text = "房間狀態 : 開始";
        }
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 newLocation = new Vector3(location_x - 1, -location_y + 4.5f, 0);
        if (!isLocalPlayer)
            return;

        if (Mathf.Abs(this.transform.position.x - newLocation.x) < 0.1f && Mathf.Abs(this.transform.position.y - newLocation.y) < 0.1f)
            this.transform.position = newLocation;
        else
            this.transform.Translate(new Vector3(newLocation.x - this.transform.position.x, newLocation.y - this.transform.position.y, 0) * Time.deltaTime * 2);
        if (animator != null)
        {
            if (this.transform.position != newLocation)
                animator.SetFloat("Speed", 0.11f);
            else
                animator.SetFloat("Speed", 0);
        }
    }

    bool b_click = true;
    void OnMouseDown()
    {
        Vector3 newLocation = new Vector3(location_x - 1, -location_y + 4.5f, 0);
        if (this.transform.position == newLocation && isLocalPlayer && GameStart)
        {
            GameObject g_object;
            Quaternion quaternion = new Quaternion();
            Vector3 g_position = this.transform.position;
            GameObject[] d_objects = GameObject.FindGameObjectsWithTag("Grid");
            for (int i = 0; i < d_objects.Length; i++)
                DestroyObject(d_objects[i]);
            if (b_click)
            {
                switch (type)
                {
                    case CharacterType.Assassin:
                        move_y = 2;
                        break;
                    case CharacterType.Archer:
                        move_y = 1;
                        break;
                    case CharacterType.Caster:
                        move_y = 2;
                        break;
                    case CharacterType.Tank:
                        move_y = 2;
                        break;
                    default:
                        break;
                }


                for (int i = -move_y; i <= move_y; i++)
                {
                    if (location_y - i < 0 || location_y - i > 9)
                        continue;
                    g_position = this.transform.position;
                    g_position += Vector3.up * i;


                    Move move;
                    if (location_x > 0)
                    {
                        g_object = Instantiate(MoveGrid, g_position + Vector3.left, quaternion, this.gameObject.transform);
                        move = g_object.GetComponent<Move>();
                        move.character = this.gameObject;
                        move.location_x = location_x - 1;
                        move.location_y = location_y - i;
                    }
                    if (location_x < 7)
                    {
                        g_object = Instantiate(MoveGrid, g_position + Vector3.right, quaternion, this.gameObject.transform);
                        move = g_object.GetComponent<Move>();
                        move.character = this.gameObject;
                        move.location_x = location_x + 1;
                        move.location_y = location_y - i;
                    }
                    if (i != 0)
                    {
                        g_object = Instantiate(MoveGrid, g_position, quaternion, this.gameObject.transform);
                        move = g_object.GetComponent<Move>();
                        move.character = this.gameObject;
                        move.location_x = location_x;
                        move.location_y = location_y - i;
                    }
                }
            }
            else
            {
                Attack attack;
                g_position = this.transform.position;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0)
                            continue;
                        g_object = Instantiate(AttackGrid, g_position + new Vector3(i, j, 0), quaternion, this.gameObject.transform);
                        attack = g_object.GetComponent<Attack>();
                        attack.character = this.gameObject;
                        attack.location_y = location_y - j;
                        attack.location_x = location_x + i;
                        if (type == CharacterType.Assassin)
                        {
                            if (i == 0 || j == 0)
                            {
                                attack.bouns = true;
                                SpriteRenderer renderer = g_object.GetComponent<SpriteRenderer>();
                                renderer.color = new Color(1, 0, 0, 0.5882353f);
                            }
                        }
                    }
                }
                if (type == CharacterType.Archer)
                {
                    for (int i = 2; i < 8; i++)
                    {
                        GenerateObject(g_position + new Vector3(i, 0, 0), i, 0);
                        GenerateObject(g_position + new Vector3(-i, 0, 0), -i, 0);
                        GenerateObject(g_position + new Vector3(0, i, 0), 0, -i);
                        GenerateObject(g_position + new Vector3(0, -i, 0), 0, i);
                    }
                }
                else if (type == CharacterType.Caster)
                {
                    GenerateObject(g_position + new Vector3(2, 0, 0), 2, 0);
                    GenerateObject(g_position + new Vector3(-2, 0, 0), -2, 0);
                    GenerateObject(g_position + new Vector3(0, 2, 0), 0, -2);
                    GenerateObject(g_position + new Vector3(0, 3, 0), 0, -3);
                    GenerateObject(g_position + new Vector3(0, -2, 0), 0, 2);
                }
                else if (type == CharacterType.Tank)
                {
                    GenerateObject(g_position + new Vector3(1, 1, 0), 1, -1);
                    GenerateObject(g_position + new Vector3(0, 1, 0), 0, -1);
                    GenerateObject(g_position + new Vector3(-1, 1, 0), -1, -1);
                }
            }
            b_click = !b_click;
        }
    }


    public void PlusHit()
    {
        if (!isLocalPlayer)
            return;
        if (!isServer)
            CmdHit();
        HitCount++;
        Text text = GameObject.Find("txtGameHit").GetComponent<Text>();
        text.text = "擊中數 : " + HitCount;
    }

    [Command]
    public void CmdHit()
    {
        HitCount++;
    }

    [Command]
    public void CmdReSpawn()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Spawn");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            GameObject game = gameObjects[i];
            int count = GameObject.FindGameObjectsWithTag("Character").Where(t => t.transform.position == game.transform.position).Count();
            if (count == 0)
            {
                location_x = (gameObjects[i].transform.position.x < 0 ? 0 : 7);
                location_y = (gameObjects[i].transform.position.y > 0 ? 0 : 9);
                gameObject.transform.position = gameObjects[i].transform.position;
                break;
            }
        }
        HP = maxHp;
        this.gameObject.SetActive(true);
    }


    public void GetNewLocation(int x, int y)
    {
        location_x = x;
        location_y = y;
        CmdGetNewLocation(x, y);
    }

    [Command]
    public void CmdGetNewLocation(int x, int y)
    {
        location_x = x;
        location_y = y;
    }

    public void GenerateObject(Vector3 vector, int x, int y)
    {
        Quaternion quaternion = new Quaternion();
        GameObject g_object;
        Attack attack;
        g_object = Instantiate(AttackGrid, vector, quaternion, this.gameObject.transform);

        attack = g_object.GetComponent<Attack>();
        attack.character = this.gameObject;
        attack.location_y = location_y + y;
        attack.location_x = location_x + x;

        attack.bouns = true;
        SpriteRenderer renderer = g_object.GetComponent<SpriteRenderer>();
        renderer.color = new Color(1, 0, 0, 0.5882353f);
        //NetworkServer.Spawn(g_object);
    }


}
