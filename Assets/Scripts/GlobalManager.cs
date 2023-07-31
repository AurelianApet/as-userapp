using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PanelIndex
{
    LODING, LOGIN, MAINMENU, INSTALLGUIDE, INSTALLGUIDEAdd, FAQ, FAQADD,
    ORDERINGPRODUCT, ADDACATEGORY, ADDAPRODUCT, USERS, QUIZ, PUSHOPTION,
    REPAIRANDACCEPT, SPECIALPRODUCT, SPECIALPRODUCTADD, CHATTING, REPAIRDETAIL,
    SPECIALPRODUCTCATEGORYADD, PRODUCTMODIFY, INSTALLGUIDESHOWIMAGEORVIDEO, INSTALLGUIDEMODIFY,
    FAQITEMMODIFY, SPECIALPRODUCTDETIAL, ORDERINGPRODUCTDETAIL, PUSHALARM, USERMODIFY
}

public class GlobalManager : MonoBehaviour {

    public GameObject[] panels;

    private static GlobalManager instance = null;

    public static GlobalManager GetInstance()
    {
        return instance;
    }

	// Use this for initialization
	void Start () {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        NetworkManager.it.AddEventCallback(ServerMethod.CONNECT,
            (data) =>
            {
                Debug.Log("Connected");
            });
        NetworkManager.it.AddEventCallback(ServerMethod.RECEIVE_MESSAGE,
            (data) =>
            {
                ServerModel.Message message = JsonUtility.FromJson<ServerModel.Message>(data);
                if (Chatting.GetInstance()!= null)
                {
                    Chatting.GetInstance().ReceiveMessage(message);
                }
            });

        NetworkManager.it.AddEventCallback(ServerMethod.OTHER_USER_CONNECT,
            (data) =>
            {
                var userDic = GeneralDataManager.it.userDictionary;
                var user = JsonUtility.FromJson<ServerModel.User>(data);
                if (!userDic.ContainsKey(user.name))
                {
                    userDic.Add(user.name, user);
                    GeneralDataManager.it.currentUser = user;
                }
            });

        NetworkManager.it.AddEventCallback(ServerMethod.OTHER_USER_DISCONNECT,
            (data) =>
            {
                var userDic = GeneralDataManager.it.userDictionary;
                var user = JsonUtility.FromJson<ServerModel.User>(data);
                if (userDic.ContainsKey(user.name))
                {
                    userDic.Remove(user.name);
                }
            });
        foreach (GameObject obj in panels)
        {
            if (obj != null) obj.SetActive(false);
        }
//        panels[1].SetActive(true);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnClickGuiBtn(int id)
    {
        if (id == 1) // login button in panel
        {
            panels[(int)PanelIndex.LOGIN].GetComponent<Login>().LoginBtnClick();
        }

        foreach(GameObject obj in panels)
        {
            if (obj != null) obj.SendMessage("OnGuiBtn", id, SendMessageOptions.DontRequireReceiver);
        }
    }
}
