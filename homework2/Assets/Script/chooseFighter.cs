using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class chooseFighter : MonoBehaviour
{
    public enum CharacterType { Assassin, Archer, Caster, Tank };

    public GameObject c1, c2, c3, c4, warning, sureBtn, cancelBtn;
    public GameObject assassin, archer, caster, tank;
    public CharacterType type = CharacterType.Assassin;
    private void Start()
    {
        c1.SetActive(true);
        c2.SetActive(false);
        c3.SetActive(false);
        c4.SetActive(false);
        warning.SetActive(false);
        cancelBtn.GetComponent<Button>().interactable = false;
    }

    public void ClickOnAssassin()
    {
        c1.SetActive(true);
        c2.SetActive(false);
        c3.SetActive(false);
        c4.SetActive(false);
        type = CharacterType.Assassin;
        Debug.Log("type changed");
    }
    public void ClickOnArcher()
    {
        c1.SetActive(false);
        c2.SetActive(true);
        c3.SetActive(false);
        c4.SetActive(false);
        type = CharacterType.Archer;
        Debug.Log("type changed");
    }
    public void ClickOnCaster()
    {
        c1.SetActive(false);
        c2.SetActive(false);
        c3.SetActive(true);
        c4.SetActive(false);
        type = CharacterType.Caster;
        Debug.Log("type changed");
    }
    public void ClickOnTank()
    {
        c1.SetActive(false);
        c2.SetActive(false);
        c3.SetActive(false);
        c4.SetActive(true);
        type = CharacterType.Tank;
        Debug.Log("type changed");
    }

    public void Warning()
    {
        warning.SetActive(true);
    }

    public void Sure()
    {
        sureBtn.GetComponent<Button>().interactable = false;
        cancelBtn.GetComponent<Button>().interactable = true;
        NetworkManager networkManager = GameObject.Find("NetworkManage").GetComponent<MyNetworkManager>();
        switch (type)
        {
            case CharacterType.Assassin:
                networkManager.playerPrefab = assassin; 
                break;
            case CharacterType.Archer:
                networkManager.playerPrefab = archer;
                break;
            case CharacterType.Caster:
                networkManager.playerPrefab = caster;
                break;
            case CharacterType.Tank:
                networkManager.playerPrefab = tank;
                break;
            default:
                break;
        }
        warning.SetActive(false);
    }

    public void Cancel()
    {
        sureBtn.GetComponent<Button>().interactable = true;
        cancelBtn.GetComponent<Button>().interactable = false;
        warning.SetActive(false);
    }

    public void WarningCancel()
    {
        warning.SetActive(false);
    }
}

