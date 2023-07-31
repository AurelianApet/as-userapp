using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public enum MessageType
{
    SUCCESS, INFO, WARNING, FAILD
}

public class Utils : MonoBehaviour {
    public GameObject alarm;
    public GameObject menu;
    public GameObject curMenu = null;
    private static Utils instance = null;
    public static Utils it
    {
        get { return instance; }
    }
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    Vector2 prePos;
    void Update()
    {
        if (Global.isAdmin) return;
        if (Input.GetMouseButtonDown(0))
        {
            prePos = Input.mousePosition;
            if (prePos.x>Utils.Conv(1300))
            {
                if (curMenu != null) Destroy(curMenu);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 curPos = Input.mousePosition;
            if (prePos.x<Utils.Conv(150) && curPos.x>Utils.Conv(300))
            {
                curMenu = Instantiate(menu);
                Canvas curCanvas = null;
                foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    if (curCanvas == null)
                    {
                        curCanvas = obj.GetComponent<Canvas>();
                    }
                }
                curMenu.SetActive(true);
                curMenu.transform.SetParent(curCanvas.gameObject.transform);
                RectTransform a = curMenu.GetComponent<RectTransform>();
                a.anchorMin = a.anchorMax = new Vector2(0, 1);
                a.pivot = new Vector2(0, 1f);
                a.offsetMin = new Vector2(0, -Conv(5800));
                a.offsetMax = new Vector2(Conv(1300), 0);
            }
        }
    }
    public static Dictionary<string, string> AddTokenInHeader(Dictionary<string, string> dic)
    {
        dic.Add("Auth_Token", Global.token);
        return dic;
    }
    public static string GetCurTime()
    {
        System.DateTime cur = System.DateTime.Now;
        return cur.Year + "-" + cur.Month.ToString("00") + "-" + cur.Day.ToString("00") + " " + cur.Hour.ToString("00") + ":" + cur.Minute.ToString("00") + ":" + cur.Second.ToString("00");
    }
    public void GotoLogin()
    {
        StartCoroutine(Utils.ShowMessage("회원가입을 하여야 리용할 수 있습니다.", MessageType.WARNING));
        Invoke("Login", 3);
    }
    public void Login()
    {
        SceneManager.LoadScene("Login");
    }
    public static IEnumerator ShowImage(GameObject image, string path)
    {
        WWW www = new WWW(path);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            image.GetComponent<RawImage>().texture = www.texture;
        }
    }

    public static IEnumerator ShowMessage(string str, MessageType type = MessageType.SUCCESS)
    {
        if (GlobalManager.GetInstance() == null) yield break;
        GameObject pushalarm = Instantiate(it.alarm);
        Canvas curCanvas = null;
        foreach(GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (curCanvas == null)
            {
                curCanvas = obj.GetComponent<Canvas>();
            }
        }
        pushalarm.transform.SetParent(curCanvas.gameObject.transform);
        pushalarm.transform.localScale = new Vector3(1, 1, 1);
        RectTransform a = pushalarm.GetComponent<RectTransform>();
        a.anchorMin = Vector2.zero;
        a.anchorMax = new Vector2(1, 1);
        a.offsetMax = a.offsetMin = Vector2.zero;
        pushalarm.GetComponentInChildren<Alarm>().SetText(str, type);
        pushalarm.SetActive(true);
        yield return null;
    }
    
    public static List<string> ImageType = new List<string>()
    {
        "jpg","png","bmp",
        "JPG","PNG","BMP"
    };

    public static List<string> VideoType = new List<string>()
    {
        "avi", "mpg", "mpeg", "mp4",
        "AVI", "MPG", "MPEG", "MP4"
    };

    public static string FileType(string str)
    {
        string s = "";
        int t = str.LastIndexOf('.');
        for (int i = t + 1; i < str.Length; i++) s += str[i];
        return s;
    }

    public static string FileName(string str)
    {
        string s = "";
        int t = str.LastIndexOf(".");
        for (int i = t - 1; i >= 0; i--)
        {
            if (str[i] == '/' || str[i] == '\\') break;
            s.Insert(0, str[i].ToString());
        }
        print(s);
        return s;
    }
    public static bool IsDate(string str)
    {
        if (str.Length < 10) return false;
        for (int i=0; i<10; i++)
        {
            if (i == 4 || i == 7)
            {
                if (str[i] != '-') return false;
            }
            else if (str[i] < '0' || str[i] > '9') return false;
        }
        return true;
    }
    public static string GetFileNameFromPath(string str)
    {
        string s = "";
        int t = str.LastIndexOf("/");
        if (t>=0 && t<str.Length-1)
        {
            s = str.Substring(t + 1);
        }
        t = str.LastIndexOf("\\");
        if (t>=0 && t<str.Length - 1)
        {
            s = str.Substring(t + 1);
        }
        return s;
    }
    public static bool IsEmail(string str)
    {
        if (str == "") return false;
        int cnt = 0, pos = -1;
        for (int i = 0; i < str.Length; i++) if (str[i] == '@') {
                pos = i; cnt++;
            }
        if (cnt != 1) return false;
        if (pos == 0 || pos == str.Length - 1) return false;
        return true;
    }
    public static bool IsPass(string str)
    {
        int num = 0, al = 0;
        for (int i=0; i<str.Length; i++)
        {
            if (str[i] >= '0' && str[i] <= '9') num++;
            else if (str[i] >= 'a' && str[i] <= 'z') al++;
            else if (str[i] >= 'A' && str[i] <= 'Z') al++;
            else return false;
        }
        return num >0 && al >0 && str.Length > 5 && str.Length < 17;
    }
    public static float Conv(float x)
    {
        return x / 2880 * Screen.width;
    }
    public static void SetUIElement(GameObject header, GameObject body, GameObject bodycontent, float x = 5400f)
    {
        RectTransform h = header.GetComponent<RectTransform>();
        h.anchorMin = new Vector2(0, 1);
        h.anchorMax = new Vector2(1, 1);
        h.offsetMax = new Vector2(0, 0);
        h.offsetMin = new Vector2(0, -400.0f * Screen.width / 2880);

        RectTransform g = body.GetComponent<RectTransform>();
        g.anchorMin = new Vector2(0, 0);
        g.anchorMax = new Vector2(1, 1);
        g.offsetMax = new Vector2(0, -400.0f / 2880 * Screen.width);
        g.offsetMin = new Vector2(0, 0);

        RectTransform r = bodycontent.GetComponent<RectTransform>();
        r.anchorMin = new Vector2(0, 1);
        r.anchorMax = new Vector2(1, 1);
        r.offsetMax = new Vector2(0, 0);
        r.offsetMin = new Vector2(0, -Screen.width / 2880f * x);
    }

    public static void SetUIElement(GameObject header, ScrollingTab body)
    {
        RectTransform h = header.GetComponent<RectTransform>();
        h.anchorMin = new Vector2(0, 1);
        h.anchorMax = new Vector2(1, 1);
        h.offsetMax = new Vector2(0, 0);
        h.offsetMin = new Vector2(0, -400.0f * Screen.width / 2880);

        RectTransform g = body.gameObject.GetComponent<RectTransform>();
        g.anchorMin = new Vector2(0, 0);
        g.anchorMax = new Vector2(1, 1);
        g.offsetMax = new Vector2(0, -400.0f / 2880 * Screen.width);
        g.offsetMin = new Vector2(0, 0);
        body.change();
    }

    public static bool IsImage(string str)
    {
        if (string.IsNullOrEmpty(str) || !str.Contains(".")) return false;
        return ImageType.Contains(FileType(str));
    }

    public static bool IsVideo(string str)
    {
        if (string.IsNullOrEmpty(str) || !str.Contains(".")) return false;
        return VideoType.Contains(FileType(str));
    }
}
