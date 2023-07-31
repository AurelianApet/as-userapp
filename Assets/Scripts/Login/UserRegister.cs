using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class UserRegister : MonoBehaviour {
    public GameObject header, body, bodycontent, loading;
    public GameObject email, pwd, pwd_rep, username, address;
    public GameObject[] titles;
    int id;
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
        Utils.SetUIElement(header, body, bodycontent);
        foreach (GameObject t in titles)
        {
            t.GetComponent<Text>().fontSize = (int)(130f / 2880 * Screen.width);
        }
        GameObject[] tmp = new GameObject[] { email, pwd, pwd_rep, username, address };
        foreach (GameObject t in tmp)
        {
            foreach (Text x in t.GetComponentsInChildren<Text>()) x.fontSize = (int)(130f / 2880 * Screen.width);
        }
        email.GetComponent<InputField>().text = "";
        username.GetComponent<InputField>().text = "";
        address.GetComponent<InputField>().text = "";
        pwd.GetComponent<InputField>().text = "";
        pwd_rep.GetComponent<InputField>().text = "";
    }

    public void SetValues(UserItem obj)
    {
        UserItem a = (UserItem)obj;
        email.GetComponent<InputField>().text = a.email;
        username.GetComponent<InputField>().text = a.name;
        address.GetComponent<InputField>().text = a.address;
        pwd.GetComponent<InputField>().text = "";
        pwd_rep.GetComponent<InputField>().text = "";
        id = a.id;
    }

    public void BackBtnClick()
    {
        gameObject.SetActive(false);
    }

    public void RegisterButtonClick()
    {
        string username = this.username.GetComponent<InputField>().text;
        string pwd = this.pwd.GetComponent<InputField>().text;
        string pwd_rep = this.pwd_rep.GetComponent<InputField>().text;
        string address = this.address.GetComponent<InputField>().text;
        string email = this.email.GetComponent<InputField>().text;
        Global.useremail = email;
        Global.username = username;
        Global.userpassword = pwd;
        if (string.IsNullOrEmpty(email))
        {
            StartCoroutine(Utils.ShowMessage("이메일을 입력해주세요.", MessageType.WARNING));
            return;
        }
        if (!Utils.IsEmail(email))
        {
            StartCoroutine(Utils.ShowMessage("이메일형식이 정확하지 않습니다.", MessageType.WARNING));
            return;
        }
        if (string.IsNullOrEmpty(pwd))
        {
            StartCoroutine(Utils.ShowMessage("비밀번호를 입력하세요.", MessageType.WARNING));
            return;
        }
        if (!Utils.IsPass(pwd))
        {
            StartCoroutine(Utils.ShowMessage("비밀번호는 6~16자리의 숫자 또는 문자이여야 합니다.", MessageType.WARNING));
            return;
        }
        if (pwd != pwd_rep)
        {
            StartCoroutine(Utils.ShowMessage("비밀번호를 재확인해주세요.", MessageType.WARNING));
            return;
        }
        if (string.IsNullOrEmpty(username))
        {
            StartCoroutine(Utils.ShowMessage("이름을 입력하세요.", MessageType.WARNING));
            return;
        }
        if (string.IsNullOrEmpty(address))
        {
            StartCoroutine(Utils.ShowMessage("연락처를 입력하세요.", MessageType.WARNING));
            return;
        }

        string attach_time = PlayerPrefs.GetString("attach_time", "2021-01-01 00:00:00");
        string product_time = PlayerPrefs.GetString("product_time", "2021-01-01 00:00:00");
        string repair_time = PlayerPrefs.GetString("repair_time", "2021-01-01 00:00:00");
        string question_time = PlayerPrefs.GetString("question_time", "2021-01-01 00:00:00");
        WWWForm wf = new WWWForm();
        Dictionary<string, string> postHeader = wf.headers;
        postHeader.Add("Content-type", "application/json");
        wf.AddField("name", username);
        wf.AddField("address", address);
        wf.AddField("email", email);
        wf.AddField("password", pwd);
        wf.AddField("attach_time", attach_time);
        wf.AddField("product_time", product_time);
        wf.AddField("repair_time", repair_time);
        wf.AddField("question_time", question_time);
        WWW www = new WWW(Global.URL_REGISTERUSER, wf);
        StartCoroutine(GetResult(www));
    }

    IEnumerator GetResult(WWW www)
    {
        loading.SetActive(true);
        yield return www;
        loading.SetActive(false);
        if (string.IsNullOrEmpty(www.error))
        {
            JSONNode rlt = JSON.Parse(www.text);
            print(www.text);
            if (rlt["status"].AsBool)
            {
                Global.token = rlt["token"];
                Global.isLogin = true;
                Global.userid = rlt["id"].AsInt;
                yield return Utils.ShowMessage("회원가입을 축하합니다!");
                yield return new WaitForSeconds(3);
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                if (Global.Debug) Debug.Log(rlt["message"]);
                StartCoroutine(Utils.ShowMessage(rlt["message"], MessageType.FAILD));
            }
        }
        else
        {
            if (Global.Debug) Debug.Log(www.error);
            StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
        }
    }
}
