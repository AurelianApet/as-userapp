using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushAlarm : MonoBehaviour {

    public GameObject background;
    public GameObject text;
    private float time = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time>=5.0f)
        {
            gameObject.SetActive(false);
        }
	}

    public void SetText(string str, MessageType type)
    {
        text.GetComponent<Text>().text = str;
        background.GetComponent<Image>().color = GetBackColor(type);
        text.GetComponent<Text>().color = GetTextColor(type);
    }

    public Color GetBackColor(MessageType type)
    {
        if (type == MessageType.SUCCESS) return Color.green;
        if (type == MessageType.INFO) return new Color32(0x16, 0xa0, 0x85, 0xff);
        if (type == MessageType.WARNING) return new Color32(255, 99, 71, 255);
        if (type == MessageType.FAILD) return Color.red;
        return Color.white;
    }

    public Color GetTextColor(MessageType type)
    {
        if (type == MessageType.SUCCESS) return Color.white;
        if (type == MessageType.INFO) return new Color(0.1f, 0.5f, 0.1f, 0.8f);
        if (type == MessageType.WARNING) return Color.white;
        if (type == MessageType.FAILD) return Color.white;
        return Color.white;
    }

    void OnEnable()
    {
        time = 0;
    }
}
