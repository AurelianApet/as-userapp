using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Login : MonoBehaviour {
    public GameObject loading;
    public GameObject Username, Password, register;
    public GameObject header, body, bodycontent, rigisterBtn;
    public GameObject exitWindow;
	// Use this for initialization
	void Start () {
		
	}

    void OnEnable()
    {
        Utils.SetUIElement(header, body, bodycontent);
        rigisterBtn.GetComponentInChildren<Text>().fontSize = (int)(130f / 2880 * Screen.width);
        if (Global.isAdmin) rigisterBtn.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void QuitBtnClick()
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

    public void LoginBtnClick()
    {
        string username = Username.GetComponent<InputField>().text;
        if (username=="")
        {
            StartCoroutine(Utils.ShowMessage("이메일을 입력하세요.", MessageType.WARNING));
            return;
        }
        if (!Utils.IsEmail(username))
        {
            StartCoroutine(Utils.ShowMessage("이메일형식이 정확하지 않습니다.", MessageType.WARNING));
            return;
        }
        string password = Password.GetComponent<InputField>().text;
        if (!Utils.IsPass(password) && password.Length<3)
        {
            StartCoroutine(Utils.ShowMessage("비밀번호는 6~16자리의 숫자 또는 문자이여야 합니다.", MessageType.WARNING));
            return;
        }
        
        Dictionary<string, string> postHeader = new Dictionary<string, string>();
        postHeader.Add("Content-type", "application/json");
        string attach_time = PlayerPrefs.GetString("attach_time", "2021-01-01 00:00:00");
        string product_time = PlayerPrefs.GetString("product_time", "2021-01-01 00:00:00");
        string repair_time = PlayerPrefs.GetString("repair_time", "2021-01-01 00:00:00");
        string question_time = PlayerPrefs.GetString("question_time", "2021-01-01 00:00:00");
        Dictionary<string, string> dic = new Dictionary<string, string>();
        string jsonStr = JsonUtility.ToJson(new LoginData(username, password, attach_time, product_time, repair_time, question_time));
        print(jsonStr);
        byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);
        WWW www = new WWW(Global.isAdmin ? Global.URL_LOGINADMIN : Global.URL_LOGINUSER, data, postHeader);
        StartCoroutine(WaitForResponse(www));
    }

    private IEnumerator WaitForResponse(WWW www)
    {
        loading.SetActive(true);
        yield return www;
        if (string.IsNullOrEmpty(www.error)) // there is no error
        {
            Global.useremail = Username.GetComponent<InputField>().text;
            Global.userpassword = Password.GetComponent<InputField>().text;
            JSONNode rlt = JSON.Parse(www.text);
            if (rlt["status"].AsBool)
            {
                Global.isLogin = true;
                Global.userid = rlt["result"]["id"].AsInt;
                Global.attachcount = rlt["result"]["attachsCount"].AsInt;
                Global.productcount = rlt["result"]["products_count"].AsInt;
                Global.repaircount = rlt["result"]["repair_count"].AsInt;
                Global.quizcount = rlt["result"]["question_count"].AsInt;
                Global.token = rlt["token"];
                if (!Global.isAdmin) StartCoroutine(GetUserValue());
                else
                {
                    loading.SetActive(false);
                    SceneManager.LoadScene("MainMenu");
                }
            }
            else
            {
                if (Global.Debug) Debug.Log("WWW Result: " + rlt["message"]);
                StartCoroutine(Utils.ShowMessage(rlt["message"], MessageType.FAILD));
                loading.SetActive(false);
            }
        } else // error
        {
            if (Global.Debug)
            {
                Debug.Log("WWW Error!: " + www.error);
                Debug.Log("Connection failed.");
            }
            StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
            loading.SetActive(false);
        }
    }

    public void RegisterBtnClick()
    {
        register.SetActive(true);
    }

    IEnumerator GetUserValue()
    {
        var w = UnityWebRequest.Get(Global.URL_USERINFO + "?id=" + Global.userid);
        if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
        yield return w.SendWebRequest();
        if (string.IsNullOrEmpty(w.error))
        {
            print(w.downloadHandler.text);
            JSONNode a = JSON.Parse(w.downloadHandler.text);
            if (a["status"].AsBool)
            {
                JSONNode b = a["result"];
                Global.username = b["name"];
                Global.useremail = b["email"];
                //Global.address = b["address"];
                //Global.created = b["created"];
                ServerModel.User user = new ServerModel.User() { name = Global.useremail };
                GeneralDataManager.it.currentUser = user;
                NetworkManager.it.Emit(ServerMethod.USER_CONNECT, user.ToJSON());
            }
        }
        else print(w.error);
        loading.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
