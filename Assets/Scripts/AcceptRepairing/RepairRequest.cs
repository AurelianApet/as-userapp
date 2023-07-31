using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RepairRequest : MonoBehaviour {
    public GameObject header, body, bodycontent;
    public GameObject date, username, sellername, image;
    public GameObject address, selleraddress, description, loading, repairWindow;
    public Text[] titles;
    private List<string> uploadfiles = new List<string>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnEnable()
    {
        Utils.SetUIElement(header, body, bodycontent, 6900);
        GameObject[] a = new GameObject[] { date, username, sellername, image, address, selleraddress, description };
        foreach(GameObject c in a)
        {
            if (c!=null) foreach (Text d in c.GetComponentsInChildren<Text>()) d.fontSize = (int)(130f / 2880 * Screen.width);
        }
        foreach(Text b in titles)
        {
            b.fontSize = (int)(130f / 2880 * Screen.width);
        }
        uploadfiles.Clear();
        this.date.GetComponent<InputField>().text="";
        this.address.GetComponent<InputField>().text="";
        this.selleraddress.GetComponent<InputField>().text="";
        this.sellername.GetComponent<InputField>().text="";
        this.description.GetComponent<InputField>().text="";
        this.image.GetComponent<InputField>().text = "";
    }

    public void RegisterBtnClick()
    {
        string date = this.date.GetComponent<InputField>().text;
        if (!Utils.IsDate(date))
        {
            StartCoroutine(Utils.ShowMessage("날짜형식은 2021-01-01입니다.", MessageType.WARNING));
            return;
        }
        string address = this.address.GetComponent<InputField>().text;
        if (string.IsNullOrEmpty(address))
        {
            StartCoroutine(Utils.ShowMessage("연락처를 입력하세요.", MessageType.WARNING));
            return;
        }
        string seller_address = this.selleraddress.GetComponent<InputField>().text;
        if (string.IsNullOrEmpty(seller_address))
        {
            StartCoroutine(Utils.ShowMessage("구입처를 입력하세요.", MessageType.WARNING));
            return;
        }
        string seller_name = this.sellername.GetComponent<InputField>().text;
        if (string.IsNullOrEmpty(seller_name))
        {
            StartCoroutine(Utils.ShowMessage("수취인이름을 입력하세요.", MessageType.WARNING));
            return;
        }
        string description = this.description.GetComponent<InputField>().text;
        if (string.IsNullOrEmpty(description))
        {
            StartCoroutine(Utils.ShowMessage("요청사항을 입력하세요.", MessageType.WARNING));
            return;
        }
        string imagepath = uploadfiles.Count>0?uploadfiles[0]:"";
        WWWForm wf = new WWWForm();
        Dictionary<string, string> postHeader = wf.headers;
        postHeader.Add("Content-type", "application/json");
        wf.AddField("sell_date", date);
        wf.AddField("username", Global.username);
        wf.AddField("user_address", address);
        wf.AddField("request_id", Global.userid);
        wf.AddField("sellername", seller_name);
        wf.AddField("seller_address", seller_address);
        wf.AddField("description", description);
        StartCoroutine(RegisterValues(wf, imagepath));
    }

    IEnumerator RegisterValues(WWWForm wf, string path)
    {
        if (!string.IsNullOrEmpty(path))
        {
            var w = new WWW(path);
            yield return w;
            if (Utils.IsImage(path))
            {
                var t = w.texture;
                wf.AddBinaryData("userfile", t.EncodeToPNG(), "1.png", "image/png");
            }
            else if (Utils.IsVideo(path))
            {
                wf.AddBinaryData("userfile", w.bytes, path, "video/" + Utils.FileType(path));
            }
        }
        using (var w = UnityWebRequest.Post(Global.URL_REPAIRANDACCEPTREQUEST, wf))
        {
            if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
            loading.SetActive(true);
            yield return w.SendWebRequest();
            loading.SetActive(false);

            if (string.IsNullOrEmpty(w.error))
            {
                if (SimpleJSON.JSON.Parse(w.downloadHandler.text)["status"].AsBool)
                {
                    StartCoroutine(Utils.ShowMessage("수리접수요청이 추가되었습니다.", MessageType.SUCCESS));
                    gameObject.SetActive(false);
                    repairWindow.SetActive(false);
                    repairWindow.SetActive(true);
                } else
                {
                    StartCoroutine(Utils.ShowMessage("입력형식이 정확하지 않습니다.", MessageType.SUCCESS));
                }
            }
            else
            {
                StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
            }
        }
    }

    public void BackBtnClick()
    {
        gameObject.SetActive(false);
    }

    public void AddFileBtnClick()
    {
        uploadfiles.Clear();
        string path = NativeGallery.PickImageOrVideo();
        if (string.IsNullOrEmpty(path)) return;
        this.image.GetComponent<InputField>().text = Utils.GetFileNameFromPath(path);
        uploadfiles.Add(path);
    }
}
