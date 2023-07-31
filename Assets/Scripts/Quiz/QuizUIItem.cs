using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizUIItem : MonoBehaviour {
    private int id;
    public GameObject time, firstline, manager, question;
    private QuizItem item;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetValues(QuizItem item, bool ok, GameObject ch)
    {
        manager = ch;
        id = item.id;
        time.GetComponent<Text>().fontSize = (int)(100f / 2880 * Screen.width);
        question.GetComponent<Text>().fontSize = (int)(130f / 2880 * Screen.width);
        time.GetComponent<Text>().text = item.date.Substring(0, 10);
        question.GetComponent<Text>().text = item.text;
        firstline.SetActive(ok);
        this.item = item;
    }

    public void Clickthis()
    {
        Chatting chat = manager.GetComponent<Quiz>().chat.GetComponent<Chatting>();
        chat.gameObject.SetActive(true);
        chat.Chat(manager, item.id, item.username, "문의");
    }
}
