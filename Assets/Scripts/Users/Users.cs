using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Users : MonoBehaviour {
    public GameObject header, body, bodycontent;
    private int startButtonId = 1400;
    public GameObject content;
    public GameObject userItem;
    public GameObject UserModify, UserDelete, UserDetail;
    public GameObject deleteWindow, loading;
    private UserItem curDel;
    private IEnumerator process = null;
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
        int cnt = content.transform.childCount;
        for (int i=0; i< cnt; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
        StartCoroutine(process = GetUserList());
    }
    void OnDisable()
    {
        if (process != null)
        {
            StopCoroutine(process);
        }
    }

    IEnumerator GetUserList()
    {
        var www = UnityWebRequest.Get(Global.URL_USERLIST);
        if (!string.IsNullOrEmpty(Global.token)) www.SetRequestHeader("Auth_Token", Global.token);
        loading.SetActive(true);
        yield return www.SendWebRequest();
        loading.SetActive(false);
        if (string.IsNullOrEmpty(www.error))
        {
            UserList ulist = JsonUtility.FromJson<UserList>(www.downloadHandler.text);
            if (ulist != null)
            {
                ShowUserList(ulist);
            }
            else
            {

            }
        } else
        {
            StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.WARNING));
        }
    }

    public void ShowUserList(UserList users)
    {
        float H = 490f / 2880 * Screen.width;
        for (int i=0; i<users.count; i++)
        {
            GameObject obj = Instantiate(userItem);
            obj.SendMessage("SetValues", users.result[i], SendMessageOptions.DontRequireReceiver);
            obj.transform.SetParent(content.transform);
            RectTransform a = obj.GetComponent<RectTransform>();
            a.offsetMin = Vector2.zero;
            a.offsetMax = new Vector2(0, H);
            a.anchoredPosition = new Vector2(0, -(H + 10) * i);
            a.localScale = new Vector3(1, 1, 1);
        }
    }

    public void Modify(UserInfo info)
    {
        UserModify.GetComponent<UserModify>().SetValues(info.GetUserItem());
        UserModify.SetActive(true);
    }

    public void Delete(UserInfo info)
    {
        curDel = info.GetUserItem();
        deleteWindow.SetActive(true);
    }

    public void Detail(UserInfo info)
    {
//        Debug.Log(info.userName);
    }

    public void DeleteYesBtnClick()
    {
        StartCoroutine(GetResult(Global.URL_USERDELETE + curDel.id));
    }

    IEnumerator GetResult(string url)
    {
        var delReq = UnityWebRequest.Delete(url);
        if (!string.IsNullOrEmpty(Global.token)) delReq.SetRequestHeader("Auth_Token", Global.token);
        loading.SetActive(true);
        yield return delReq.SendWebRequest();
        if (!delReq.isNetworkError)
        {
            deleteWindow.SetActive(false);
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
        else
        {
//            if (Global.Debug) Debug.Log(delReq.error);
        }
        loading.SetActive(false);
    }

    public void DeleteNoBtnClick()
    {
        deleteWindow.SetActive(false);
    }
}
