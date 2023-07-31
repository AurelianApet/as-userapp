using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextShow : MonoBehaviour
{
    public Text content, time;
    public RawImage image;
    public bool isMime;
    public GameObject playbtn;
    public GameObject show;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        content.fontSize = (int)(130f / 2880 * Screen.width);
        time.fontSize = (int)(70f / 2880 * Screen.width);
        RectTransform h = time.gameObject.GetComponent<RectTransform>();
        h.anchorMin = new Vector2(0, 1);
        h.anchorMax = new Vector2(1, 1);
        h.offsetMin = Vector2.zero;
        h.offsetMax = new Vector2(0, Utils.Conv(80));

        RectTransform g = playbtn.gameObject.GetComponent<RectTransform>();
        g.anchorMax = g.anchorMin = new Vector2(0.5f, 0.5f);
        g.offsetMin = new Vector2(-Utils.Conv(500), -Utils.Conv(500));
        g.offsetMax = new Vector2(Utils.Conv(500), Utils.Conv(500));
    }

    public void SetText(ServerModel.Message value, int H, GameObject o)
    {
        show = o;
        time.text = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();
        if (value.type == "0" || value.type == "2")
        {
            playbtn.SetActive(false);
            image.gameObject.SetActive(false);
            content.text = value.message;
            if (value.type == "2") content.text = "결제요청 : " + value.message;
            RectTransform a = content.gameObject.GetComponent<RectTransform>();
            int width = (int)Mathf.Min(Utils.Conv(2000), content.preferredWidth);
            int height = (int)(content.preferredHeight + Utils.Conv(80));
            a.anchoredPosition = Vector2.zero;
            a.offsetMax = new Vector2(-Utils.Conv(80), -Utils.Conv(80));
            a.offsetMin = new Vector2(Utils.Conv(80), -height);
            RectTransform b = gameObject.GetComponent<RectTransform>();
            b.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + Utils.Conv(80));
            if (isMime)
            {
                time.alignment = TextAnchor.MiddleRight;
                b.anchorMin = b.anchorMax = new Vector2(1, 1);
                b.pivot = new Vector2(1, 1);
                b.offsetMax = new Vector2(-Utils.Conv(80), -Utils.Conv(80) - H);
                b.offsetMin = new Vector2(-Utils.Conv(240) - width, -Utils.Conv(160) - height - H);
                gameObject.GetComponent<Image>().color = new Color32(0, 71, 151, 255);
                content.color = Color.white;
            }
            else
            {
                time.alignment = TextAnchor.MiddleLeft;
                b.anchorMin = b.anchorMax = new Vector2(0, 1);
                b.pivot = new Vector2(0, 1);
                b.offsetMax = new Vector2(Utils.Conv(240) + width, -Utils.Conv(80) - H);
                b.offsetMin = new Vector2(Utils.Conv(80), -Utils.Conv(160) - height - H);
                gameObject.GetComponent<Image>().color = Color.white;
                content.color = Color.black;
            }
        } else if (value.type == "1")
        {
            content.gameObject.SetActive(false);
            content.text = value.message;
            RectTransform a = image.gameObject.GetComponent<RectTransform>();
            int width = (int)Utils.Conv(1500);
            int height = (int)(width + Utils.Conv(80));
            a.anchoredPosition = Vector2.zero;
            a.offsetMax = new Vector2(-Utils.Conv(80), -Utils.Conv(80));
            a.offsetMin = new Vector2(Utils.Conv(80), -height);
            RectTransform b = gameObject.GetComponent<RectTransform>();
            b.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + Utils.Conv(80));
            if (isMime)
            {
                time.alignment = TextAnchor.MiddleRight;
                b.anchorMin = b.anchorMax = new Vector2(1, 1);
                b.pivot = new Vector2(1, 1);
                b.offsetMax = new Vector2(-Utils.Conv(80), -Utils.Conv(80) - H);
                b.offsetMin = new Vector2(-Utils.Conv(240) - width, -Utils.Conv(160) - height - H);
                gameObject.GetComponent<Image>().color = new Color32(0, 71, 151, 255);
//                content.color = Color.white;
            }
            else
            {
                time.alignment = TextAnchor.MiddleLeft;
                b.anchorMin = b.anchorMax = new Vector2(0, 1);
                b.pivot = new Vector2(0, 1);
                b.offsetMax = new Vector2(Utils.Conv(240) + width, -Utils.Conv(80) - H);
                b.offsetMin = new Vector2(Utils.Conv(80), -Utils.Conv(160) - height - H);
                gameObject.GetComponent<Image>().color = Color.white;
//                content.color = Color.black;
            }
            if (Utils.IsImage(content.text))
            {
                playbtn.SetActive(false);
                StartCoroutine(Utils.ShowImage(image.gameObject, content.text));
            }
            else if (Utils.IsVideo(content.text))
            {
                playbtn.SetActive(true);
            }
        }
    }

    public void OnPlayBtnClick()
    {
        show.SetActive(true);
        print(content.text);
        show.GetComponent<InstallGuideIVShow>().SetValues(content.text);
    }
}
