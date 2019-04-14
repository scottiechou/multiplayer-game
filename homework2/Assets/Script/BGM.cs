using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {

    // Use this for initialization
    AssetBundle assetBundle;
	void Start () {
        WWW www = new WWW(@"file:///C:\Unity\assetbundle");
        assetBundle = www.assetBundle;

        AudioClip audioClip = new AudioClip();
        audioClip = assetBundle.LoadAsset<AudioClip>("Bgm");

        AudioSource audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
        assetBundle.Unload(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
