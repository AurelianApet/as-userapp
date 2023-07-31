using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FaqAdd : MonoBehaviour {
    public GameObject header, body, bodycontent;
    public GameObject category, description, faq, dropdownitem, loading;
    Dictionary<int, string> categories = new Dictionary<int, string>();
    List<int> keys = new List<int>();
    List<string> values = new List<string>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetValues(Dictionary<int, string> categories)
    {
        this.categories.Clear();
        this.keys.Clear();
        this.values.Clear();
        foreach (int s in categories.Keys)
        {
            this.keys.Add(s);
            this.values.Add(categories[s]);
            this.categories.Add(s, categories[s]);
        }
        this.values.Add("");
    }
    void OnEnable()
    {
        Utils.SetUIElement(header, body, bodycontent);
        foreach(Text t in description.GetComponentsInChildren<Text>())
        {
            t.fontSize = (int)(130f / 2880 * Screen.width);
        }
        RectTransform a = dropdownitem.GetComponent<RectTransform>();
        a.anchorMin = new Vector2(0, 1);
        a.anchorMax = new Vector2(1, 1);
        a.offsetMax = Vector2.zero;
        a.offsetMin = new Vector2(0, -400f / 2880 * Screen.width);
        dropdownitem.GetComponentInChildren<Text>().fontSize = (int)(140f / 2880 * Screen.width);
        category.GetComponent<Dropdown>().ClearOptions();
        category.GetComponent<Dropdown>().AddOptions(values);
        description.GetComponent<InputField>().text = "";
        if (categories.Count > 0) category.GetComponent<Dropdown>().value = 0;
    }

    public void BackBtnClick()
    {
        gameObject.SetActive(false);
    }

    public void AddBtnClick()
    {
        if (description.GetComponent<InputField>().text=="")
        {
            StartCoroutine(Utils.ShowMessage("질문내용을 입력해주세요.", MessageType.WARNING));
            return;
        }
        StartCoroutine(AddFaqItem());
    }

    IEnumerator AddFaqItem()
    {
        loading.SetActive(true);
        WWWForm form = new WWWForm();
        Dictionary<string, string> postHeader = form.headers;
        postHeader.Add("Content-type", "application/json");
        form.AddField("question", "OK");
        form.AddField("answer", description.GetComponent<InputField>().text);
        form.AddField("categorie", keys[category.GetComponent<Dropdown>().value]);
        //string jsonstr = JsonUtility.ToJson(new FaqItem(description.GetComponent<InputField>().text, category.GetComponent<Dropdown>().captionText.text, "aa", "aa", 0));
        //Debug.Log(jsonstr);
        //byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonstr);
        using (var w = UnityWebRequest.Post(Global.URL_FAQITEMADD, form))
        {
            if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
            yield return w.SendWebRequest();
            loading.SetActive(false);
            if (!string.IsNullOrEmpty(w.error))
            {
                if (Global.Debug) print(w.error);
                StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
            } else if (SimpleJSON.JSON.Parse(w.downloadHandler.text)["status"].AsBool)
            {
                StartCoroutine(Utils.ShowMessage("정확히 추가되었습니다.", MessageType.SUCCESS));
                gameObject.SetActive(false);
                faq.SetActive(false);
                faq.SetActive(true);
                //if (Global.Debug) print(w.);
            }
            else
            {
                StartCoroutine(Utils.ShowMessage("추가실패!", MessageType.WARNING));
            }
        }
    }
}
