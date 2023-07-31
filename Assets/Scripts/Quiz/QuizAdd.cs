using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class QuizAdd : MonoBehaviour {
    public GameObject header, body, bodycontent, loading;
    public GameObject description, quiz;
//    public GameObject quiz;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        Utils.SetUIElement(header, body, bodycontent);
        foreach(Text a in description.GetComponentsInChildren<Text>())
        {
            a.fontSize = (int)(130f / 2880 * Screen.width);
        }
    }

    public void BackBtnClick()
    {
        gameObject.SetActive(false);
    }

    public void AddBtnClick()
    {
        string desc = description.GetComponent<InputField>().text;
        if (string.IsNullOrEmpty(desc))
        {
            StartCoroutine(Utils.ShowMessage("문의 할 내용을 입력해주세요.", MessageType.WARNING));
            return;
        }
        StartCoroutine(AddQuiz());
    }

    IEnumerator AddQuiz()
    {
        WWWForm form = new WWWForm();
        Dictionary<string, string> postHeader = form.headers;
        postHeader.Add("Content-type", "application/json");
        form.AddField("user_id", Global.userid);
        form.AddField("username", Global.username);
        form.AddField("quiz", description.GetComponent<InputField>().text);
        loading.SetActive(true);
        using (var w = UnityWebRequest.Post(Global.URL_QUIZADD, form))
        {
            if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
            yield return w.SendWebRequest();
            if (string.IsNullOrEmpty(w.error))
            {
                StartCoroutine(Utils.ShowMessage("문의가 추가되었습니다!", MessageType.SUCCESS));
                gameObject.SetActive(false);
                quiz.SetActive(false);
                quiz.SetActive(true);
            }
            else
            {
                StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
            }
        }
        loading.SetActive(false);
    }
}
