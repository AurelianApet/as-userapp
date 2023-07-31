using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfo : MonoBehaviour {
    public GameObject userNo, userName, userEmail, userAddress;
    public int id;
    public UserItem mItem;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public UserItem GetUserItem()
    {
        int userno = int.Parse(userNo.GetComponent<Text>().text);
        string username = userName.GetComponent<Text>().text;
        string useremail = userEmail.GetComponent<Text>().text;
        string useraddress = userAddress.GetComponent<Text>().text;
        return new UserItem(username, useremail, useraddress, userno);
    }

    public void SetValues(UserItem item)
    {
        userNo.GetComponent<Text>().text = item.id.ToString();
        userName.GetComponent<Text>().text = item.name;
        userEmail.GetComponent<Text>().text = item.email;
        userAddress.GetComponent<Text>().text = item.address;
        mItem = item;
    }

    public void ModifyBtnClick()
    {
        Users s = (Users)FindObjectOfType(typeof(Users));
        s.Modify(this);
    }

    public void DeleteBtnClick()
    {
        Users s = (Users)FindObjectOfType(typeof(Users));
        s.Delete(this);
    }

    public void MoreBtnClick()
    {
        Users s = (Users)FindObjectOfType(typeof(Users));
        s.Detail(this);
    }
}
