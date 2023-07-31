using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class InstallGuide : MonoBehaviour {
    public GameObject loading;
    public GameObject item;
    public GameObject content;
    public GameObject deleteWindow;
    public GameObject header, body, bodycontent, addbtn;
    public GameObject addscene, modifyscene, show;

    private int deleteId = 0;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

    void OnEnable()
    {
        Utils.SetUIElement(header, body, bodycontent);
        int cnt = content.transform.childCount;
        for (int i=0; i < cnt; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
        StartCoroutine(ShowList());
        addbtn.SetActive(Global.isAdmin);
    }

    IEnumerator ShowList()
    {
        WWW www = new WWW(Global.URL_INSTALLGUIDELIST + 0);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
        } else
        {
            InstallGuideItems res = JsonUtility.FromJson<InstallGuideItems>(www.text);
            for(int i=0; i<res.count; i++)
            {
                InstallGuideItem a = res.result[i];
                GameObject obj = Instantiate(item);
                obj.GetComponent<InstallGuideUiItem>().SetValue(a, i, content, show, gameObject);
            }
        }
    }

    public void BackBtnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AddBtnClick()
    {
        addscene.SetActive(true);
    }

    public void DeleteConfirm(int id)
    {
        deleteId = id;
        deleteWindow.SetActive(true);
    }

    public void DeleteYesBtnClick()
    {
        StartCoroutine(DeleteItem());
    }
    IEnumerator DeleteItem()
    {
        using (var w = UnityWebRequest.Delete(Global.URL_INSTALLGUIDEDELETE + deleteId))
        {
            if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
            loading.SetActive(true);
            yield return w.SendWebRequest();
            loading.SetActive(false);
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            deleteWindow.SetActive(false);
        }
    }
    public void DeleteNoBtnClick()
    {
        deleteWindow.SetActive(false);
    }
}
