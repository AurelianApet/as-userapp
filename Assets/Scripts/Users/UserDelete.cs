using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Networking;

public class UserDelete : MonoBehaviour {
    public GameObject header, body, bodycontent;
    public int id;
    public GameObject UserEmail, Username, UserAddress, Users;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetValues(UserItem item)
    {
        id = item.id;
        UserEmail.GetComponent<Text>().text = "Email : " + item.email;
        Username.GetComponent<Text>().text = "Username : " + item.name;
        UserAddress.GetComponent<Text>().text = "User Address: " + item.address;
    }

    public void DeleteBtnClick()
    {
        //string username = this.Username.GetComponent<InputField>().text;
        //string address = this.UserAddress.GetComponent<InputField>().text;
        //string email = this.UserEmail.GetComponent<InputField>().text;
        //UserItem item = new UserItem(username, email, address, id, "");
        StartCoroutine(GetResult(Global.URL_USERDELETE + id));
    }

    IEnumerator GetResult(string url)
    {
        UnityWebRequest delReq = UnityWebRequest.Delete(url);
        yield return delReq.SendWebRequest();
        if (!delReq.isNetworkError)
        {
            gameObject.SetActive(false);
            Users.SetActive(false);
            Users.SetActive(true);
        }
        else
        {
            if (Global.Debug) Debug.Log(delReq.error);
        }
    }

    public void BackBtnClick()
    {
        gameObject.SetActive(false);
    }
}
