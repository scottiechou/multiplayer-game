using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkInfo : NetworkBehaviour
{

    // Use this for initialization
    void Start()
    {

    }
    private void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        characters.Clear();
    }
    public Text text;
    public static List<Character> characters = new List<Character>();
    public bool GameStart = false;
    public DateTime startTime = DateTime.MinValue;
    // Update is called once per frame

    void Update()
    {
        characters = characters.OrderByDescending(t => t.HitCount).ToList();
        string s_rank = "";
        for (int i = 0; i < characters.Count; i++)
            s_rank += (i + 1) + ". " + characters[i].c_name + " (" + characters[i].HitCount + ")\n";


        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i] == null)
            {
                characters.RemoveAt(i);
                continue;
            }

            characters[i].RpcGetPlayerCount(characters.Count);
            characters[i].RpcGetHitRank(s_rank);

            if (characters.Count > 1)
            {
                if (startTime == DateTime.MinValue)
                    startTime = DateTime.Now;
                TimeSpan ts = DateTime.Now - startTime;
                if (ts.Minutes < 1)
                    characters[i].RpcGetGameStart(true, "遊戲時間 : " + ts.Minutes.ToString("0#") + ":" + ts.Seconds.ToString("0#"), false);
                else
                    characters[i].RpcGetGameStart(false, "遊戲時間 : 遊戲結束", true);
            }
            else
            {
                startTime = DateTime.MinValue;
                characters[i].RpcGetGameStart(false, "遊戲時間 : 尚未開始", true);
                characters[i].HitCount = 0;
            }
        }

    }

    public static void Login(Character character)
    {
        characters.Add(character);
        character.RpcGetPlayerCount(characters.Count);
    }
}
