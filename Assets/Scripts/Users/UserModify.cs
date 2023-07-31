using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class UserModify : MonoBehaviour {
    public GameObject header, body, bodycontent;
    public GameObject email, pwd, pwd_rep, username, address, loading;
    public GameObject[] titles;
    public GameObject Users;
    int id;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetValues(UserItem obj)
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

    public void ModifyButtonClick()
    {
        string username = this.username.GetComponent<InputField>().text;
        string pwd = this.pwd.GetComponent<InputField>().text;
        string pwd_rep = this.pwd_rep.GetComponent<InputField>().text;
        string address = this.address.GetComponent<InputField>().text;
        string email = this.email.GetComponent<InputField>().text;
        if (string.IsNullOrEmpty(username))
        {
            StartCoroutine(Utils.ShowMessage("사용자명을 입력하세요.", MessageType.WARNING));
            return;
        }
        //if (string.IsNullOrEmpty(pwd))
        //{
        //    return;
        //}
        //if (string.IsNullOrEmpty(pwd_rep))
        //{
        //    return;
        //}
        if (string.IsNullOrEmpty(email))
        {
            StartCoroutine(Utils.ShowMessage("이메일을 입력하세요.", MessageType.WARNING));
            return;
        }
        //if (pwd != pwd_rep)
        //{
        //    return;
        //}
        if (string.IsNullOrEmpty(address))
        {
            StartCoroutine(Utils.ShowMessage("연락처를 입력하세요.", MessageType.WARNING));
            return;
        }
        UserItem item = new UserItem(username, email, address, id, "");
        Dictionary<string, string> postHeader = new Dictionary<string, string>();
        postHeader.Add("Content-type", "application/json");
        if (!string.IsNullOrEmpty(Global.token)) postHeader.Add("Auth_Token", Global.token);
        string jsonStr = JsonUtility.ToJson(item);
        byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);
        WWW www = new WWW(Global.URL_USERMODIFY+id, data, postHeader);
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
            if (rlt["status"].AsBool)
            {
                gameObject.SetActive(false);
                Users.SetActive(false);
                Users.SetActive(true);
            } else
            {
                if (Global.Debug) Debug.Log(rlt["message"]);
            }
        } else
        {
            if (Global.Debug) Debug.Log(www.error);
        }
    }
}
