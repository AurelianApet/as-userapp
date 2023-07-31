using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class ProductSimple : MonoBehaviour {
    public GameObject proName, proImage;
    private int id;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetValues(string str)
    {
        print(str);
        JSONNode a = JSON.Parse(str);
        string path = a["folder"]+"/"+a["image"];
        string title = a["title"];
        proName.GetComponent<Text>().text = title;
        StartCoroutine(Utils.ShowImage(proImage, Global.URL_IMAGEPATH_SPECIAL + path));
    }
}
