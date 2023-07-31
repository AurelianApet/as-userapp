using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairItem : MonoBehaviour {
    private string username, date, user_address;
    private string seller_address, sellername, description, image;
    private string status, sell_date;
    public GameObject Username, Address, chattingbtn;
    private int id, request_id;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetValues(RepairingItem item)
    {
        gameObject.SetActive(true);
        Username.GetComponent<Text>().fontSize = (int)(130f/2880*Screen.width);
        Address.GetComponent<Text>().fontSize = (int)(130f / 2880 * Screen.width);
        chattingbtn.GetComponent<Text>().fontSize = (int)(100f / 2880 * Screen.width);
        Username.GetComponent<Text>().text = username = item.username;
        date = item.date;
        Address.GetComponent<Text>().text = user_address = item.user_address;
        seller_address = item.seller_address;
        sellername = item.sellername;
        description = item.description;
        image = Global.URL_IMAGEPATH_REPAIR + item.image;
        id = item.id;
        request_id = item.request_id;
        sell_date = item.sell_date;
    }

    public void DetailBtnClick()
    {
        RepairAndAccept s = (RepairAndAccept)FindObjectOfType(typeof(RepairAndAccept));
        s.Detail(GetReparingItem());
    }

    public void DeleteBtnClick()
    {
        RepairAndAccept s = (RepairAndAccept)FindObjectOfType(typeof(RepairAndAccept));
        s.Delete(GetReparingItem());
    }

    public void ChattingBtnClick()
    {
        RepairAndAccept s = (RepairAndAccept)FindObjectOfType(typeof(RepairAndAccept));
        s.Chatting(GetReparingItem());
    }

    public RepairingItem GetReparingItem()
    {
        return new RepairingItem(id, request_id, username, date, user_address, seller_address, sellername, description, image, status, sell_date);
    }
}
