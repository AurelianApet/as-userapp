using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FaqModify : MonoBehaviour {
    public GameObject header, body, bodycontent, dropdownitem;
    public GameObject category, description, faq, loading;
    Dictionary<int, string> categories = new Dictionary<int, string>();
    List<int> keys = new List<int>();
    List<string> values = new List<string>();
    private int id;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
    }

    public void BackBtnClick()
    {
        gameObject.SetActive(false);
    }

    public void ModifyBtnClick()
    {
        StartCoroutine(GetResult());
    }
    public void SetValues(FaqItem item, Dictionary<int,string> categories)
    {
        Utils.SetUIElement(header, body, bodycontent);
        foreach (Text t in description.GetComponentsInChildren<Text>())
        {
            t.fontSize = (int)(130f / 2880 * Screen.width);
        }
        RectTransform a = dropdownitem.GetComponent<RectTransform>();
        a.anchorMin = new Vector2(0, 1);
        a.anchorMax = new Vector2(1, 1);
        a.offsetMax = Vector2.zero;
        a.offsetMin = new Vector2(0, -400f / 2880 * Screen.width);
        dropdownitem.GetComponentInChildren<Text>().fontSize = (int)(140f / 2880 * Screen.width);

        this.categories.Clear();
        this.keys.Clear();
        this.values.Clear();
        foreach (int s in categories.Keys)
        {
            this.categories.Add(s, categories[s]);
            keys.Add(s);
            values.Add(categories[s]);
        }
        values.Add("");
        category.GetComponent<Dropdown>().ClearOptions();
        category.GetComponent<Dropdown>().AddOptions(values);
        category.GetComponent<Dropdown>().captionText.text = categories[item.categorie];
        id = item.id;
        description.GetComponent<InputField>().text = item.answer;
        int k = -1;
        for (int i = 0; i < keys.Count; i++) if (keys[i] == item.categorie) k = i;
        category.GetComponent<Dropdown>().value = k;
    }
    IEnumerator GetResult()
    {
        WWWForm form = new WWWForm();
        Dictionary<string, string> postHeader = form.headers;
        postHeader.Add("Content-type", "application/json");
        form.AddField("question", "OK");
        form.AddField("answer", description.GetComponent<InputField>().text);
        form.AddField("categorie", keys[category.GetComponent<Dropdown>().value]);
        using (var w = UnityWebRequest.Post(Global.URL_FAQITEMMODIFY + id, form))
        {
            if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
            loading.SetActive(true);
            yield return w.SendWebRequest();
            loading.SetActive(false);
            if (!string.IsNullOrEmpty(w.error))
            {
                if (Global.Debug) print(w.error);
                StartCoroutine(Utils.ShowMessage("봉사기접속에서 오유가 발생하였습니다.", MessageType.FAILD));
            }
            else
            {
                //if (Global.Debug) print(w.);
            }
            gameObject.SetActive(false);
            faq.SetActive(false);
            faq.SetActive(true);
        }
    }
}
