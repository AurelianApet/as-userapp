using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class InstallGuidAdd : MonoBehaviour {
    private int startButtonId = 300;
    public GameObject title;
    public GameObject attachment;
    public GameObject content;
    public GameObject InstallGuide;
    public GameObject header, body, bodycontent;
    public GameObject loading;
    public List<string> uploadfiles;
    private string path = @"C:\Users\Admin\Videos\a.png";
    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnEnable()
    {
        Utils.SetUIElement(header, body, bodycontent);
        attachment.GetComponent<InputField>().text = "";
        content.GetComponent<InputField>().text = "";
        title.GetComponent<InputField>().text = "";
        uploadfiles.Clear();
        foreach(Text a in content.GetComponentsInChildren<Text>())
        {
            a.fontSize = (int)(140f / 2880 * Screen.width);
        }
    }

    public void BackBtnClick()
    {
        gameObject.SetActive(false);
    }

    public void AddBtnClick()
    {
        string bstr = content.GetComponent<InputField>().text;
        print(uploadfiles);
        if (string.IsNullOrEmpty(title.GetComponent<InputField>().text))
        {
            StartCoroutine(Utils.ShowMessage("제목을 입력해주세요", MessageType.WARNING));
            title.GetComponent<InputField>().Select();
            return;
        }
        if (bstr == "")
        {
            StartCoroutine(Utils.ShowMessage("설치방법을 입력해주세요.", MessageType.WARNING));
            content.GetComponent<InputField>().Select();
            return;
        }
        StartCoroutine(AddInstallGuide());
    }

    public void AddFileBtnClick()
    {
        string tpath = NativeGallery.PickImageOrVideo();
        if (string.IsNullOrEmpty(tpath)) return;
        path = tpath;
        uploadfiles.Add(path);
        if (uploadfiles.Count > 1)
            attachment.GetComponent<InputField>().text += ",";
        attachment.GetComponent<InputField>().text += Utils.GetFileNameFromPath(path);
    }
    IEnumerator AddInstallGuide()
    {
        loading.SetActive(true);
        WWWForm form = new WWWForm();
        Dictionary<string, string> postHeader = form.headers;
        postHeader.Add("Content-type", "application/json");
        if (uploadfiles.Count>0)
        {
            form.AddField("folder", System.DateTime.Now.ToFileTime().ToString());
            for (int i=0; i<uploadfiles.Count; i++)
            {
                path = uploadfiles[i];
                var w = new WWW(path);
                yield return w;
                if (Utils.IsImage(path))
                {
                    var t = w.texture;
                    form.AddBinaryData("others[]", t.EncodeToPNG(), "1.png", "image/png");
                }
                else if (Utils.IsVideo(path))
                {
                    form.AddBinaryData("others[]", w.bytes, path, "video/" + Utils.FileType(path));
                }
            }
        }
        form.AddField("setup_guide", content.GetComponent<InputField>().text);
        form.AddField("title", title.GetComponent<InputField>().text);
        using (var w = UnityWebRequest.Post(Global.URL_INSTALLGUIDEADD, form))
        {
            if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
            yield return w.SendWebRequest();
            loading.SetActive(false);
            if (w.isNetworkError)
            {
                StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
            }
            else if (JSON.Parse(w.downloadHandler.text)["status"].AsBool)
            {
                StartCoroutine(Utils.ShowMessage("정확히 추가되었습니다.", MessageType.SUCCESS));
                gameObject.SetActive(false);
                InstallGuide.SetActive(false);
                InstallGuide.SetActive(true);
            }
            else
            {
                StartCoroutine(Utils.ShowMessage("추가가 실패하였습니다.", MessageType.FAILD));
            }
        }
    }
}
