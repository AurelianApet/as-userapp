using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollHeadItem : MonoBehaviour {
    public GameObject text;
    public GameObject line;
    private string key, value;
    private int pos;
	// Use this for initialization
	void Start () {
		
	}
	
    void OnEnable()
    {
        text.GetComponent<Text>().fontSize = (int)(100f / 2880 * Screen.width);
    }

	// Update is called once per frame
	void Update () {
		
	}
    public void Selected(bool b = false)
    {
        line.SetActive(b);
    }
    public void SetValue(int key, string value, int pos)
    {
        text.GetComponent<Text>().text = value;
        this.pos = pos;
    }

    public void Click()
    {
        ScrollingTab a = FindObjectOfType<ScrollingTab>();
        a.SelectedCategory(pos);
    }
}
