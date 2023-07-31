using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class PushOption : MonoBehaviour {
    public GameObject payingfinished, answerfinished, payrequest, updating, addingproduct;
    public GameObject header, body, bodycontent, loading;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void BackBtnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void OnEnable()
    {
        Utils.SetUIElement(header, body, bodycontent);
        StartCoroutine(UpdateOption());
    }

    IEnumerator UpdateOption()
    {
        loading.SetActive(true);
        var w = UnityWebRequest.Get(Global.URL_OPTIONREAD);
        if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
        yield return w.SendWebRequest();
        if (string.IsNullOrEmpty(w.error))
        {
            JSONNode a = JSON.Parse(w.downloadHandler.text);
            if (a["status"].AsBool)
            {
                JSONNode b = a["result"];
//                payingfinished.GetComponent<Toggle>().isOn = b[1]["value"].AsInt == 1;
                answerfinished.GetComponent<Toggle>().isOn = b[2]["value"].AsInt == 1;
                payrequest.GetComponent<Toggle>().isOn = b[3]["value"].AsInt == 1;
                updating.GetComponent<Toggle>().isOn = b[5]["value"].AsInt == 1;
//                addingproduct.GetComponent<Toggle>().isOn = b[5]["value"].AsInt == 1;
            }
        }
        loading.SetActive(false);
    }

    public void SetBtnClick()
    {
        StartCoroutine(SetResult());
    }

    IEnumerator SetResult()
    {
        Dictionary<string, string> postHeader = new Dictionary<string, string>();
        postHeader.Add("Content-type", "application/json");
        if (!string.IsNullOrEmpty(Global.token)) postHeader.Add("Auth_Token", Global.token);
        Dictionary<string, string> values = new Dictionary<string, string>();
//        values.Add("paymentDone", payingfinished.GetComponent<Toggle>().isOn ? "1" : "0");
        values.Add("answerDone", answerfinished.GetComponent<Toggle>().isOn ? "1" : "0");
        values.Add("paymentRequest", payrequest.GetComponent<Toggle>().isOn ? "1" : "0");
        values.Add("productAdd", updating.GetComponent<Toggle>().isOn ? "1" : "0");
//        values.Add("productAdd", addingproduct.GetComponent<Toggle>().isOn ? "1" : "0");
        print(new JSONObject(values).ToString());
        byte[] data = System.Text.Encoding.UTF8.GetBytes(new JSONObject(values).ToString());
        using (var w = new WWW(Global.URL_OPTIONUPDATE, data, postHeader))
        {
            loading.SetActive(true);
            yield return w;
            loading.SetActive(false);
            print(w.text);
            if (!string.IsNullOrEmpty(w.error))
            {
                if (Global.Debug) print(w.error);
                StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
            }
            else
            {
                StartCoroutine(Utils.ShowMessage("설정이 보관되었습니다."));
            }
        }
    }
}
