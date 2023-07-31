using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class MainMenu : MonoBehaviour {
    public GameObject attach, product, repair, quiz;
    public GameObject header;
    public GameObject body;
    public GameObject bodycontent;
    public GameObject exitWindow, morebtn, productPanel, detail;
    public GameObject[] prod;
    private SpecialProductItem[] item = new SpecialProductItem[4];
    public bool isAdmin;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void GoLogin()
    {
        SceneManager.LoadScene("Login");
    }
    public void InstallGuideBtnClick()
    {
        if (Global.isAdmin)
        {
            if (!Global.isLogin)
            {
                StartCoroutine(Utils.ShowMessage("회원가입을 하여야 이용할 수 있습니다.", MessageType.WARNING));
                Invoke("GoLogin", 3);
                return;
            }
        }
        SceneManager.LoadScene("InstallGuide");
    }
    public void FaqBtnClick()
    {
        if (Global.isAdmin)
        {
            if (!Global.isLogin)
            {
                StartCoroutine(Utils.ShowMessage("회원가입을 하여야 이용할 수 있습니다."));
                Invoke("GoLogin", 3);
            }
        }
        SceneManager.LoadScene("Faq");
    }
    public void RepairBtnClick()
    {
        if (!Global.isLogin)
        {
            StartCoroutine(Utils.ShowMessage("회원가입을 하여야 이용할 수 있습니다.", MessageType.WARNING));
            Invoke("GoLogin", 3);
            return;
        }
        SceneManager.LoadScene("Repair");
    }
    public void ProductBtnClick()
    {
        if (Global.isAdmin)
        {
            if (!Global.isLogin)
            {
                StartCoroutine(Utils.ShowMessage("회원가입을 하여야 이용할 수 있습니다.", MessageType.WARNING));
                Invoke("GoLogin", 3);
            }
        }
        SceneManager.LoadScene("Product");
    }
    public void SpecialProductBtnClick()
    {
        if (!Global.isLogin)
        {
            StartCoroutine(Utils.ShowMessage("회원가입을 하여야 이용할 수 있습니다.", MessageType.WARNING));
            Invoke("GoLogin", 3);
            return;
        }
        SceneManager.LoadScene("SpecialProduct");
    }
    public void QuizBtnClick()
    {
        if (!Global.isLogin)
        {
            StartCoroutine(Utils.ShowMessage("회원가입을 하여야 이용할 수 있습니다.", MessageType.WARNING));
            Invoke("GoLogin", 3);
            return;
        }
        SceneManager.LoadScene("Quiz");
    }
    public void UserListBtnClick()
    {
        if (!Global.isLogin)
        {
            StartCoroutine(Utils.ShowMessage("회원가입을 하여야 이용할 수 있습니다.", MessageType.WARNING));
            Invoke("GoLogin", 3);
            return;
        }
        SceneManager.LoadScene("UserList");
    }
    public void PushSettingsBtnClick()
    {
        if (!Global.isLogin)
        {
            StartCoroutine(Utils.ShowMessage("회원가입을 하여야 이용할 수 있습니다.", MessageType.WARNING));
            Invoke("GoLogin", 3);
            return;
        }
        SceneManager.LoadScene("PushSettings");
    }

    public void ExitBtnClick()
    {
        exitWindow.SetActive(true);
    }

    public void ExitNoBtnClick()
    {
        exitWindow.SetActive(false);
    }

    public void ExitYesBtnClick()
    {
        Application.Quit();
    }

    public void MoreBtnClick()
    {
        if (!Global.isLogin)
        {
            StartCoroutine(Utils.ShowMessage("회원가입을 하여야 리용할수 있습니다.", MessageType.WARNING));
            Invoke("GoLogin", 4);
            return;
        }
        SceneManager.LoadScene("SpecialProduct");
    }

    public void Product1Click()
    {
        if (!Global.isLogin)
        {
            StartCoroutine(Utils.ShowMessage("회원가입을 하여야 이용할 수 있습니다.", MessageType.WARNING));
            Invoke("GoLogin", 3);
            return;
        }
        detail.GetComponent<ProductDetail>().SetValues(true, item[0], "특가상품상세보기");
    }
    public void Product2Click()
    {
        if (!Global.isLogin)
        {
            StartCoroutine(Utils.ShowMessage("회원가입을 하여야 이용할 수 있습니다.", MessageType.WARNING));
            Invoke("GoLogin", 3);
            return;
        }
        detail.GetComponent<ProductDetail>().SetValues(true, item[1], "특가상품상세보기");
    }
    public void Product3Click()
    {
        if (!Global.isLogin)
        {
            StartCoroutine(Utils.ShowMessage("회원가입을 하여야 이용할 수 있습니다.", MessageType.WARNING));
            Invoke("GoLogin", 3);
            return;
        }
        detail.GetComponent<ProductDetail>().SetValues(true, item[2], "특가상품상세보기");
    }
    public void Product4Click()
    {
        if (!Global.isLogin)
        {
            StartCoroutine(Utils.ShowMessage("회원가입을 하여야 이용할 수 있습니다.", MessageType.WARNING));
            Invoke("GoLogin", 3);
            return;
        }
        detail.GetComponent<ProductDetail>().SetValues(true, item[3], "특가상품상세보기");
    }

    void OnEnable()
    {
        gameObject.SetActive(Global.isAdmin == isAdmin);
        if (Global.isAdmin != isAdmin) return;
        if (morebtn!=null) morebtn.SetActive(!isAdmin);
        if (productPanel!= null) productPanel.SetActive(!isAdmin);
        Utils.SetUIElement(header, body, bodycontent);
        attach.SetActive(false);
        product.SetActive(false);
        repair.SetActive(false);
        quiz.SetActive(false);
        if (!isAdmin)
        {
            foreach(GameObject a in prod)
            {
                a.GetComponentInChildren<Text>().fontSize = (int)(100f / 2880 * Screen.width);
                a.SetActive(false);
            }
            StartCoroutine(ShowProducts());
        }
        StartCoroutine(GetInfo());
    }
    IEnumerator GetInfo()
    {
        if (!Utils.IsEmail(Global.useremail)) yield break;
        Dictionary<string, string> postHeader = new Dictionary<string, string>();
        postHeader.Add("Content-type", "application/json");
        string attach_time = PlayerPrefs.GetString("attach_time", "2021-01-01 00:00:00");
        string product_time = PlayerPrefs.GetString("product_time", "2021-01-01 00:00:00");
        string repair_time = PlayerPrefs.GetString("repair_time", "2021-01-01 00:00:00");
        string question_time = PlayerPrefs.GetString("question_time", "2021-01-01 00:00:00");
        Dictionary<string, string> dic = new Dictionary<string, string>();
        string jsonStr = JsonUtility.ToJson(new LoginData(Global.useremail, Global.userpassword, attach_time, product_time, repair_time, question_time));
        print(jsonStr);
        byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);
        WWW www = new WWW(Global.isAdmin ? Global.URL_LOGINADMIN : Global.URL_LOGINUSER, data, postHeader);
        yield return www;
        if (!string.IsNullOrEmpty(www.error)) yield break;
        JSONNode rlt = JSON.Parse(www.text);
        print(www.text);
        if (rlt["status"].AsBool)
        {
            Global.isLogin = true;
            Global.userid = rlt["result"]["id"].AsInt;
            Global.attachcount = rlt["result"]["attachsCount"].AsInt;
            Global.productcount = rlt["result"]["products_count"].AsInt;
            Global.repaircount = rlt["result"]["repair_count"].AsInt;
            Global.quizcount = rlt["result"]["question_count"].AsInt;
            attach.SetActive(Global.attachcount > 0 && !Global.isAdmin && rlt["settings"][5]["value"].AsInt == 1);
            product.SetActive(Global.productcount > 0 && !Global.isAdmin && rlt["settings"][5]["value"].AsInt == 1);
            repair.SetActive(Global.repaircount > 0 && !Global.isAdmin);
            quiz.SetActive(Global.quizcount > 0 && !Global.isAdmin && rlt["settings"][2]["value"].AsInt == 1);
            attach.GetComponentInChildren<Text>().text = Global.attachcount + "";
            product.GetComponentInChildren<Text>().text = Global.productcount + "";
            repair.GetComponentInChildren<Text>().text = Global.repaircount + "";
            quiz.GetComponentInChildren<Text>().text = Global.quizcount + "";
        }
    }
    IEnumerator ShowProducts()
    {
        WWW www = new WWW(Global.URL_SPECIALPRODUCTLIST + "0?order_by=desc");
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            JSONNode a = JSON.Parse(www.text);
            if (a["status"].AsBool)
            {
                int cnt = a["count"].AsInt;
                for (int i = 0; i < Mathf.Min(4, cnt); i++)
                {
                    prod[i].SetActive(true);
                    item[i] = JsonUtility.FromJson<SpecialProductItem>(a["result"][i].ToString());
                    prod[i].GetComponent<ProductSimple>().SetValues(a["result"][i].ToString());
                }
            }
        }
    }
}
