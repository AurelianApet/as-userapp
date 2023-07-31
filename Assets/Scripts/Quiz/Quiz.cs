using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quiz : MonoBehaviour {
    public GameObject content, quizItem;
    public GameObject header, body, bodycontent, loading, chat, addbtn;
    public GameObject quizAdd;
    // Use this for initialization
    void Start () {
        Global.quizcount = 0;
        PlayerPrefs.SetString("question_time", Utils.GetCurTime());
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void BackBtnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AddBtnClick()
    {
        quizAdd.SetActive(true);
    }

    void OnEnable()
    {
        Global.quizcount = 0;
        PlayerPrefs.SetString("question_time", Utils.GetCurTime());
        addbtn.SetActive(!Global.isAdmin);
        Utils.SetUIElement(header, body, bodycontent);
        int cnt = content.transform.childCount;
        for (int i=0; i< cnt; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
        StartCoroutine(GetQuizList());
    }

    IEnumerator GetQuizList()
    {
        var www = UnityEngine.Networking.UnityWebRequest.Get(Global.URL_QUIZLIST + (Global.isAdmin?"0":"0?search_user="+Global.username));
        if (!string.IsNullOrEmpty(Global.token)) www.SetRequestHeader("Auth_Token", Global.token);
        loading.SetActive(true);
        yield return www.SendWebRequest();
        loading.SetActive(false);
        if (string.IsNullOrEmpty(www.error))
        {
            if (!SimpleJSON.JSON.Parse(www.downloadHandler.text)["status"].AsBool)
            {
                StartCoroutine(Utils.ShowMessage("현시 할 문의가 없습니다.", MessageType.FAILD));
            } else
            {
                ShowQuizList(JsonUtility.FromJson<QuizList>(www.downloadHandler.text));
            }
        } 
        else
        {
            StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
        }
    }

    public void ShowQuizList(QuizList list)
    {
        float H = 600f / 2880 * Screen.width;
        for (int i=0; i<list.count; i++)
        {
            QuizItem item = list.result[i];
            GameObject obj = Instantiate(quizItem);
            obj.transform.SetParent(content.transform);
            RectTransform a = obj.GetComponent<RectTransform>();
            a.offsetMin = Vector2.zero;
            a.offsetMax = new Vector2(0, H);
            a.anchoredPosition = new Vector2(0, -(H + 10) * i);
            a.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<QuizUIItem>().SetValues(item, i == 0, gameObject);
        }
    }
}
