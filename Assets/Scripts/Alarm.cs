using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alarm : MonoBehaviour
{
    public Text message;
    public GameObject background;
    public MessageType type;
    private float showTime = 3f;
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && false)
        {
            Destroy(gameObject.transform.parent.gameObject);
            return;
        }
        showTime -= Time.deltaTime;
        if (showTime<0)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
    void OnEnable()
    {
        message.fontSize = (int)(130f / 2880 * Screen.width);
        message.lineSpacing = 1.3f;
    }
    public void SetText(string txt, MessageType t)
    {
        RectTransform a = gameObject.GetComponent<RectTransform>();
        a.anchorMin = new Vector2(0.2f, 0.7f);
        a.anchorMax = new Vector2(0.8f, 0.7f);
        a.offsetMin = a.offsetMax = new Vector2(0, 0);
        RectTransform b = message.GetComponent<RectTransform>();
        b.anchorMin = new Vector2(0, 1);
        b.anchorMax = new Vector2(1, 1);
        b.anchoredPosition = new Vector2(0, Utils.Conv(50));
        message.text = txt;
        float width = message.preferredWidth;
        float height = message.preferredHeight;
        float x = 100;
        a.offsetMin = new Vector2(-Utils.Conv(x), -height - Utils.Conv(x*2));
        a.offsetMax = new Vector2(Utils.Conv(x), 0);
        b.offsetMin = new Vector2(Utils.Conv(x), -height - Utils.Conv(x));
        b.offsetMax = new Vector2(-Utils.Conv(x), -Utils.Conv(x));
    }
}
