using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class RepairAndAccept : MonoBehaviour {
    public GameObject content, repairItem, deleteWindow, addbtn;
    public GameObject header, body, bodycontent, loading, detail, chatscene;
    public GameObject addWindow;
    // Use this for initialization
    void Start () {
        Global.repaircount = 0;
        PlayerPrefs.SetString("repair_time", Utils.GetCurTime());
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
        Global.repaircount = 0;
        PlayerPrefs.SetString("repair_time", Utils.GetCurTime());
        Utils.SetUIElement(header, body, bodycontent);
        int cnt = content.transform.childCount;
        for (int i=0; i< cnt; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
        addbtn.SetActive(!Global.isAdmin);
        StartCoroutine(GetRepairingItems());
    }

    IEnumerator GetRepairingItems()
    {
        loading.SetActive(true);
        var www = UnityWebRequest.Get(Global.URL_REPAIRANDACCEPTLIST + (Global.isAdmin?"0":"0?search_user="+Global.username));
        if (!string.IsNullOrEmpty(Global.token)) www.SetRequestHeader("Auth_Token", Global.token);
        yield return www.SendWebRequest();
        loading.SetActive(false);
        if (!string.IsNullOrEmpty(www.error))
        {
            StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다."));
        }
        else
        {
            RepairList a = JsonUtility.FromJson<RepairList>(www.downloadHandler.text);
            ShowItems(a);
        }
    }

    public void ShowItems(RepairList a)
    {
        float H = 400f / 2800 * Screen.width;
        for (int i=0; i<a.count; i++)
        {
            GameObject obj = Instantiate(repairItem);
            obj.transform.SetParent(content.transform);
            RectTransform b = obj.GetComponent<RectTransform>();
            b.offsetMin = Vector2.zero;
            b.offsetMax = new Vector2(0, H);
            b.anchoredPosition = new Vector2(0, -(H + 10) * i);
            b.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<RepairItem>().SetValues(a.result[i]);
        }
    }

    public void Detail(RepairingItem item)
    {
        detail.gameObject.SetActive(true);
        detail.GetComponent<InstallGuideIVShow>().SetValues(item, "수리접수상세보기");
    }

    private RepairingItem temp;

    public void Delete(RepairingItem item)
    {
        temp = item;
        deleteWindow.SetActive(true);
    }

    public void DeleteYesBtnClick()
    {
        StartCoroutine(DeleteResult());
    }

    IEnumerator DeleteResult()
    {
        using (var w = UnityWebRequest.Delete(Global.URL_REPAIRANDACCEPTDELETE + temp.id))
        {
            if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
            loading.SetActive(true);
            yield return w.SendWebRequest();
            if (w.isNetworkError)
            {
                StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다."));
            }
            else
            {
                StartCoroutine(Utils.ShowMessage("삭제되었습니다."));
            }
            var w1 = UnityWebRequest.Delete(Global.URL_RCHATDELETE + temp.id);
            if (!string.IsNullOrEmpty(Global.token)) w1.SetRequestHeader("Auth_Token", Global.token);
            yield return w1.SendWebRequest();

            loading.SetActive(false);
            deleteWindow.SetActive(false);
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }

    public void DeleteNoBtnClick()
    {
        deleteWindow.SetActive(false);
    }

    public void Chatting(RepairingItem item)
    {
        Chatting chat = chatscene.GetComponent<Chatting>();
        chat.gameObject.SetActive(true);
        chat.Chat(gameObject, item.id, item.username, "수리접수");
    }

    public void AddBtnClick()
    {
        addWindow.SetActive(true);
    }
}
