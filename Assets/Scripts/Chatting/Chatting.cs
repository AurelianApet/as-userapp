using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerModel;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class Chatting : MonoBehaviour
{
    public GameObject payPanel;
    public GameObject payValue;
    public GameObject backObject;
    public GameObject messageItem, content;
    public GameObject typeText;
    public GameObject header, body, bodycontent, show, loading;
    public GameObject titlebar, sendBar, paymentbtn, paymentsystem, payment, paymentsuccess;
    public GameObject paymentRequest;
    public Text[] payRequestTxts;
    public Text[] txts;

    private string roomId, repair;
    List<Message> messages = new List<Message>();
    private string CurOponentUsername;
    public static Chatting instance = null;
    public static Chatting GetInstance()
    {
        return instance;
    }
    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Chat(GameObject obj, int Roomid, string userno, string title)
    {
        instance = this;
        backObject = obj;
        CurOponentUsername = userno;
        roomId = Roomid+"";
        repair = title == "수리접수"?"0":"1";
        RectTransform g = sendBar.GetComponent<RectTransform>();
        if (repair=="0") g.anchoredPosition = new Vector2(0, 0);
        titlebar.GetComponent<Text>().text = title;
        paymentbtn.SetActive(Global.isAdmin && title=="수리접수");
        string url;
        if (title=="수리접수")
        {
            url = Global.URL_RCHATALL + Roomid;
        } else
        {
            url = Global.URL_QCHATALL + Roomid;
        }
        StartCoroutine(GetAllMessage(url));
    }

    IEnumerator GetAllMessage(string url)
    {
        var w = UnityWebRequest.Get(url);
        if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
        yield return w.SendWebRequest();
        if (string.IsNullOrEmpty(w.error))
        {
            JSONNode a = JSON.Parse(w.downloadHandler.text);
            print(w.downloadHandler.text);
            if (a["status"].AsBool)
            {
                int cnt = a["count"].AsInt;
                JSONNode c = a["result"];
                for (int i=0; i< cnt; i++)
                {
                    Message b = new Message();
                    b.from = c[i]["from_name"];
                    b.to = c[i]["to_name"];
                    if (c[i].ContainsKey("type")) b.type = c[i]["type"];
                    else b.type = "0";
                    b.message = c[i]["message"];
                    AddMessage(b);
                }
            }
        } else
        {
            StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.WARNING));
        }
    }

    void OnEnable()
    {
        messages.Clear();
        int cnt = content.transform.childCount;
        for (int i = 0; i < cnt; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
        foreach (Text a in typeText.GetComponentsInChildren<Text>()) a.fontSize = (int)(130f / 2880 * Screen.width);
        foreach (Text a in payPanel.GetComponentsInChildren<Text>()) a.fontSize = (int)(130f / 2880 * Screen.width);
        foreach (Text a in txts) a.fontSize = (int)Utils.Conv(130);
        foreach (Text a in payRequestTxts) a.fontSize = (int)Utils.Conv(140);

        RectTransform h = header.GetComponent<RectTransform>();
        h.anchorMin = new Vector2(0, 1);
        h.anchorMax = new Vector2(1, 1);
        h.offsetMax = new Vector2(0, 0);
        h.offsetMin = new Vector2(0, -400.0f * Screen.width / 2880);

        RectTransform g = sendBar.GetComponent<RectTransform>();
        g.anchorMin = new Vector2(0, 0);
        g.anchorMax = new Vector2(1, 0);
        g.offsetMax = new Vector2(0, 800.0f * Screen.width / 2880);
        g.offsetMin = new Vector2(0, 0);
        g.anchoredPosition = new Vector2(0, -Utils.Conv(300));

        RectTransform c = body.GetComponent<RectTransform>();
        c.anchorMin = new Vector2(0, 0);
        c.anchorMax = new Vector2(1, 1);
        c.offsetMax = new Vector2(0, -400.0f * Screen.width / 2880);
        c.offsetMin = new Vector2(0, 800.0f * Screen.width / 2880);

        RectTransform d = bodycontent.GetComponent<RectTransform>();
        d.anchorMin = new Vector2(0, 1);
        d.anchorMax = new Vector2(1, 1);
        d.offsetMax = new Vector2(0, 0);
        d.offsetMin = new Vector2(0, -Utils.Conv(80));
    }

    public void BackBtnClick()
    {
        gameObject.SetActive(false);
    }

    public void AddMessages(List<Message> str)
    {
        foreach (Message m in str) AddMessage(m);
    }

    public void AddMessage(Message message)
    {
        messages.Add(message);
        AddingMessage(message);
    }

    void RemovingMessage(int id)
    {

    }

    void AddingMessage(Message message)
    {
        RectTransform a = content.GetComponent<RectTransform>();
        float height = a.rect.height;
        GameObject b = Instantiate(messageItem);
        b.transform.SetParent(content.transform);
        b.transform.localScale = new Vector3(1, 1, 1);
        if (Global.isAdmin)
        {
            b.GetComponent<TextShow>().isMime = message.from == "admin";
        } else
        {
            b.GetComponent<TextShow>().isMime = message.from == Global.username;
        }
        b.GetComponent<TextShow>().SetText(message, (int)height, show);
        a.offsetMin = Vector2.zero;
        a.offsetMax = new Vector2(0, height + Utils.Conv(160) + b.GetComponent<RectTransform>().rect.height);
    }

    IEnumerator upload = null;
    private string filepath, serverfilepath;
    public void SendMessage()
    {
        if (typeText == null) return;
        string sendmessage = typeText.GetComponent<InputField>().text;
        if (!string.IsNullOrEmpty(filepath) && Utils.GetFileNameFromPath(filepath) == sendmessage)
        {
            if (upload != null) return;
            typeText.GetComponent<InputField>().text = string.Empty;
            typeText.GetComponent<InputField>().ActivateInputField();
            upload = SendFile(filepath);
            StartCoroutine(upload);
            return;
        }
        typeText.GetComponent<InputField>().text = string.Empty;
        typeText.GetComponent<InputField>().ActivateInputField();
        if (string.Empty == sendmessage) return;
        if (Global.isAdmin)
        {
            Message message = new Message() { from = "admin", to = CurOponentUsername, type = "0", message = sendmessage, is_repair = repair, room_id = roomId};
            StartCoroutine(SaveMessage(message));
            NetworkManager.it.Emit(ServerMethod.SEND_MESSAGE, message.ToJSON());
        } else
        {
            Message message = new Message()
            {
                from = Global.username,
                to = "admin",
                type = "0",
                message = sendmessage,
                is_repair = repair,
                room_id = roomId
            };
            StartCoroutine(SaveMessage(message));
            NetworkManager.it.Emit(ServerMethod.SEND_MESSAGE, message.ToJSON());
        }
    }

    public IEnumerator SaveMessage(Message message)
    {
        print(message.ToJSON().ToString());
        WWWForm wf = new WWWForm();
        wf.AddField("id", message.room_id);
        wf.AddField("from", message.from);
        wf.AddField("to", message.to);
        wf.AddField("message", message.message);
        wf.AddField("type", message.type);
        wf.AddField("is_repair", message.is_repair);
        bool ok = titlebar.GetComponent<Text>().text=="수리접수";
        var w = UnityWebRequest.Post((ok ? Global.URL_RCHATCREATE : Global.URL_QCHATCREATE), wf);
        if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
        yield return w.SendWebRequest();
        if (string.IsNullOrEmpty(w.error))
        {
        } else
        {
            StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.WARNING));
        }
    }
    public void ShowPaymentAdmin()
    {
        payPanel.SetActive(true);
        payValue.GetComponent<InputField>().text = "0";
    }
    public void ShowPayment()
    {
        paymentRequest.SetActive(false);
        paymentsystem.SetActive(true);
    }
    public void SendPayMessage()
    {
        Message message = new Message() { from = Global.username, to = "admin", type = "0", message = "결제성공!", is_repair = repair, room_id=roomId};
        StartCoroutine(SaveMessage(message));
        NetworkManager.it.Emit(ServerMethod.SEND_MESSAGE, message.ToJSON());
    }
    public void PayRequestReject()
    {
        paymentRequest.SetActive(false);
        Message message = new Message() { from = "admin", to = CurOponentUsername, type = "2", message = "결제가 거절되였습니다.", is_repair = repair, room_id=roomId};
        StartCoroutine(SaveMessage(message));
        NetworkManager.it.Emit(ServerMethod.SEND_MESSAGE, message.ToJSON());
        AddMessage(message);
    }
    public void ShowPaymentConfirm(string str)
    {
        payRequestTxts[0].text = str;
        paymentRequest.SetActive(true);
    }

    public void SendPayment()
    {
        string sendmessage = payValue.GetComponent<InputField>().text;
        if (sendmessage == "0") return;
        payPanel.SetActive(false);
        Message message = new Message() { from = "admin", to = CurOponentUsername, type = "2", message = sendmessage , is_repair = repair, room_id=roomId};
        StartCoroutine(SaveMessage(message));
        NetworkManager.it.Emit(ServerMethod.SEND_MESSAGE, message.ToJSON());
    }
    public void ReceiveMessage(Message message)
    {
        print(message.ToJSON().ToString());
        print(roomId + " " + repair);
        if (message.is_repair != repair) return;
        if (message.room_id != roomId) return;
        if (Global.isAdmin)
        {
            if (message.from == "admin")
            {
                AddMessage(message);
            }
            else if (message.to == "admin")
            {
                AddMessage(message);
            }
        } else
        {
            if (message.from == Global.username) AddMessage(message);
            else if (message.to == Global.username)
            {
                if (message.is_web == "1")
                {
                    StartCoroutine(SaveMessage(message));
                }
                if (message.type == "2")
                {
                    ProductDetail.sItem = new SpecialProductItem("", "", double.Parse(message.message), "", "", -1);
                    ShowPaymentConfirm(message.message);
                }
                AddMessage(message);
            }
        }
    }

    public void AddFileBtnClick()
    {
        filepath = NativeGallery.PickImageOrVideo();
        if (!string.IsNullOrEmpty(filepath)) typeText.GetComponent<InputField>().text = Utils.GetFileNameFromPath(filepath);
        else typeText.GetComponent<InputField>().text = "";
    }

    public void AddTakePhotoBtnClick()
    {
        if (NativeCamera.DeviceHasCamera())
        {
            NativeCamera.TakePicture((path) =>
            {
                filepath = path;
                if (filepath != "") typeText.GetComponent<InputField>().text = Utils.GetFileNameFromPath(filepath);
                else typeText.GetComponent<InputField>().text = "";
            });
        }
    }

    public void AddRecordVideoBtnClick()
    {
        if (NativeCamera.DeviceHasCamera())
        {
            NativeCamera.RecordVideo((path) =>
            {
                filepath = path;
                if (filepath != "") typeText.GetComponent<InputField>().text = Utils.GetFileNameFromPath(filepath);
                else typeText.GetComponent<InputField>().text = "";
            });
        }
    }

    IEnumerator SendFile(string path)
    {
        if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path)) yield break;
        loading.SetActive(true);
        WWW www = new WWW(path);
        yield return www;
        WWWForm wf = new WWWForm();
        Dictionary<string, string> postHeader = wf.headers;
        postHeader.Add("Content-type", "application/json");
        if (Utils.IsImage(path))
        {
            Texture2D tex = www.texture;
            wf.AddBinaryData("userfile", tex.EncodeToPNG(), "1.png", "image/png");
        }
        else if (Utils.IsVideo(path)) wf.AddBinaryData("userfile", www.bytes, path, "video/" + Utils.FileType(path));
        using (var w = UnityWebRequest.Post(Global.URL_FILEINCHATTING, wf))
        {
            if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
            yield return w.SendWebRequest();
            loading.SetActive(false);
            if (w.isNetworkError || w.downloadHandler == null)
            {
                StartCoroutine(Utils.ShowMessage("파일전송이 실패했습니다.", MessageType.FAILD));
            } else
            {
                if (Global.isAdmin)
                {
                    Message message = new Message() { from = "admin", to = CurOponentUsername, type = "1", message = SimpleJSON.JSON.Parse(w.downloadHandler.text)["result"] , is_repair=repair, room_id=roomId};
                    StartCoroutine(SaveMessage(message));
                    NetworkManager.it.Emit(ServerMethod.SEND_MESSAGE, message.ToJSON());
                }
                else
                {
                    Message message = new Message() { from = Global.username, to = "admin", type = "1", message = SimpleJSON.JSON.Parse(w.downloadHandler.text)["result"], is_repair = repair, room_id = roomId};
                    StartCoroutine(SaveMessage(message));
                    NetworkManager.it.Emit(ServerMethod.SEND_MESSAGE, message.ToJSON());
                }
            }
        }
        upload = null;
        path = null;
    }
}
