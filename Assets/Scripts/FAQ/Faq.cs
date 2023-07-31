using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class Faq : MonoBehaviour {
    public GameObject header, body, bodycontent;
    public ScrollingTab tab;
    public GameObject faqitem;
    public GameObject DeleteWindow, ModifyWindow, loading, addscene, addbtn;
    private int curSel = 0;
    private int deleteId = 0;
    Dictionary<int, string> categories = new Dictionary<int, string>();
    List<int> keys = new List<int>();
    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnEnable()
    {
        Utils.SetUIElement(header, tab);
        addbtn.SetActive(Global.isAdmin);
        Refresh();
    }
    void Refresh()
    {
        tab.Clear();
        tab.noItem.SetActive(true);
        StartCoroutine(ShowList());
    }
    IEnumerator ShowList()
    {
        WWW www1 = new WWW(Global.URL_PRODUCTCATEGORY + 0);
        loading.SetActive(true);
        yield return www1;
        if (string.IsNullOrEmpty(www1.error))
        {
            JSONNode root = JSON.Parse(www1.text);
            if (root["status"].AsBool)
            {
                tab.SelectedCategory(curSel);
                categories.Clear();
                keys.Clear();
                foreach (string str in root["result"].Keys) keys.Add(int.Parse(str));
                foreach(int str in keys)
                {
                    categories.Add(str, root["result"][str+""]["info"][0]["name"]);
                }
                tab.AddCategory(categories);
            }
            tab.noItem.SetActive(false);
        }
        else
        {
            tab.noItem.SetActive(true);
            StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다."));
        }
        WWW www = new WWW(Global.URL_FAQLIST + 0 + "?category=" + keys[curSel]);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            FaqList list = JsonUtility.FromJson<FaqList>(www.text);
            if (list.status)
            {
                tab.noItem.SetActive(list.count == 0);
                int H = (int)(490f/2880*Screen.width);
                for (int i = 0; i < list.count; i++)
                {
                    tab.AddContent(GetBodyItem(list.result[i]), H, i);
                }
            } else
            {
            }
        }
        loading.SetActive(false);
    }

    public GameObject GetBodyItem(FaqItem item)
    {
        GameObject obj = Instantiate(faqitem);
        obj.GetComponent<FaqUIItem>().SetValues(item, gameObject);
        return obj;
    }

    public void BackBtnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void AddBtnClick()
    {
        addscene.GetComponent<FaqAdd>().SetValues(categories);
        addscene.SetActive(true);
    }

    public void Delete(int id)
    {
        deleteId = id;
        DeleteWindow.SetActive(true);
    }

    public void Modify(FaqItem item)
    {
        ModifyWindow.GetComponent<FaqModify>().SetValues(item, categories);
        ModifyWindow.SetActive(true);
    }

    public void DeleteYesBtnClick()
    {
        DeleteWindow.SetActive(false);
        StartCoroutine(DeleteResult());
    }

    IEnumerator DeleteResult()
    {
        var w = UnityWebRequest.Delete(Global.URL_FAQITEMDELETE + deleteId);
        if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
        loading.SetActive(true);
        yield return w.SendWebRequest();
        loading.SetActive(false);
        DeleteWindow.SetActive(false);
        if (w.isNetworkError)
        {
            StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
        }
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void DeleteNoBtnClick()
    {
        DeleteWindow.SetActive(false);
    }

    public void SelectedItemChange(int id)
    {
        curSel = id;
        Refresh();
    }
}
