using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AddCategory : MonoBehaviour {
    public GameObject header, body, bodycontent;
    public GameObject category, backWindow, loading;
    public bool IsSpecial;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetValues(GameObject b, bool ok)
    {
        IsSpecial = ok;
        backWindow = b;
        Utils.SetUIElement(header, body, bodycontent);
        foreach(Text a in category.GetComponentsInChildren<Text>())
        {
            a.fontSize = (int)(120f / 2880 * Screen.width);
        }
        category.GetComponent<InputField>().text = "";
    }

    public void BackBtnClick()
    {
        gameObject.SetActive(false);
    }

    public void AddBtnClick()
    {
        string str = category.GetComponent<InputField>().text;
        if (string.IsNullOrEmpty(str))
        {
            StartCoroutine(Utils.ShowMessage("카테고리를 입력하세요.", MessageType.WARNING));
        } else
        {
            StartCoroutine(AddNewCategory(str));
        }
    }

    IEnumerator AddNewCategory(string str)
    {
        Dictionary<string, string> postHeader = new Dictionary<string, string>();
        postHeader.Add("Content-type", "application/json");
        if (!string.IsNullOrEmpty(Global.token)) postHeader.Add("Auth_Token", Global.token);
        byte[] data = System.Text.Encoding.UTF8.GetBytes("{\"categorie\":\"" + str + "\"}");
        using (var w = new WWW(IsSpecial?Global.URL_SPECIALPRODUCTCATEGORYADD:Global.URL_ORDERINGPRODUCTCATEGORYADD, data, postHeader))
        {
            loading.SetActive(true);
            yield return w;
            loading.SetActive(false);
            if (!string.IsNullOrEmpty(w.error))
            {
                if (Global.Debug) print(w.error);
                StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
            }
            else
            {
                //if (Global.Debug) print(w.);
            }
            gameObject.SetActive(false);
            backWindow.SetActive(false);
            backWindow.SetActive(true);
        }
    }
}
