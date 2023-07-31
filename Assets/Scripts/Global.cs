using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Global {
    public static string token = "";
    public static bool isAdmin = false;
    public static bool isLogin = false;
    public static string username = "";
    public static string useremail = "";
    public static string userpassword = "";
    public static int userid = -1;
    public static int attachcount = 0;
    public static int productcount = 0;
    public static int repaircount = 0;
    public static int quizcount = 0;
    public static readonly bool Debug = true;
    public static readonly int PORTNUMBER = 3000; // This is used in socketIOComponent
    public static readonly string URL_SERVER = "46.149.195.23";
    //public static readonly string URL_SERVER = "localhost";
    public static readonly string URL_API_PATH = "http://" + URL_SERVER + ":8080/as";
    public static readonly string URL_PAYMENTCREATE = URL_API_PATH + "/api/payment/create_payment";
    public static readonly string URL_QCHATCREATE = URL_API_PATH + "/api/chat/create_qchat";
    public static readonly string URL_QCHATALL = URL_API_PATH + "/api/chat/question_chat/";
    public static readonly string URL_QCHATDELETE = URL_API_PATH + "/api/chat/delete_qchat/";
    public static readonly string URL_RCHATCREATE = URL_API_PATH + "/api/chat/create_rchat";
    public static readonly string URL_RCHATALL = URL_API_PATH + "/api/chat/repair_chat/";
    public static readonly string URL_RCHATDELETE = URL_API_PATH + "/api/chat/delete_rchat/";

    public static readonly string URL_IMAGEPATH_SPECIAL = URL_API_PATH + "/uploads/shop_images/";
    public static readonly string URL_IMAGEPATH_ORDERING = URL_API_PATH + "/uploads/attachs/";
    public static readonly string URL_IMAGEPATH_CHAT = URL_API_PATH + "/uploads/chat/";
    public static readonly string URL_IMAGEPATH_GUIDE = URL_API_PATH + "/uploads/guides/";
    public static readonly string URL_IMAGEPATH_REPAIR = URL_API_PATH + "/uploads/repairs/";

    public static readonly string URL_OPTIONREAD = URL_API_PATH + "/api/settings/all_settings";
    public static readonly string URL_OPTIONUPDATE = URL_API_PATH + "/api/settings/update_settings";
    public static readonly string URL_LOGINADMIN = URL_API_PATH + "/api/admin/login";
//    public static readonly string URL_LOGOUTADMIN = URL_API_PATH + "/api/products";
    public static readonly string URL_LOGINUSER = URL_API_PATH + "/api/users/login";
    public static readonly string URL_INSTALLGUIDELIST = URL_API_PATH + "/api/guides/all_guides/";
    public static readonly string URL_INSTALLGUIDEADD = URL_API_PATH + "/api/guides/create_guide";
    public static readonly string URL_INSTALLGUIDEDELETE = URL_API_PATH + "/api/guides/delete_guide/";
    public static readonly string URL_INSTALLGUIDEMODIFY = URL_API_PATH + "/api/guides/update_guide/";
    public static readonly string URL_INSTALLGUIDEONEGET = URL_API_PATH + "/api/guides/one_guide";
    public static readonly string URL_FAQLIST = URL_API_PATH + "/api/repeats/all_repeats/";
    public static readonly string URL_FAQITEMADD = URL_API_PATH + "/api/repeats/create_repeat";
    public static readonly string URL_FAQITEMDELETE = URL_API_PATH + "/api/repeats/delete_repeat/";
    public static readonly string URL_FAQITEMMODIFY = URL_API_PATH + "/api/repeats/update_repeat/";
    public static readonly string URL_REPAIRANDACCEPTLIST = URL_API_PATH + "/api/repairs/all_repairs/";
    //    public static readonly string URL_REPAIRANDACCEPTCOMPLETED = URL_API_PATH + "/api/products";
    public static readonly string URL_REPAIRANDACCEPONEGET = URL_API_PATH + "/api/repairs/one_repair";
    public static readonly string URL_REPAIRANDACCEPTREQUEST = URL_API_PATH + "/api/repairs/create_repair";
    public static readonly string URL_REPAIRANDACCEPTMODIFY = URL_API_PATH + "/api/repairs/update_repair/";
    public static readonly string URL_REPAIRANDACCEPTDELETE = URL_API_PATH + "/api/repairs/delete_repair/";
    public static readonly string URL_ORDERINGPRODUCTLIST = URL_API_PATH + "/api/attachs/all_attachs/";
    public static readonly string URL_ORDERINGPRODUCTAdd = URL_API_PATH + "/api/attachs/create_attach";
    public static readonly string URL_ORDERINGPRODUCTDELETE = URL_API_PATH + "/api/attachs/delete_attach/";
    public static readonly string URL_ORDERINGPRODUCTMODIFY = URL_API_PATH + "/api/attachs/update_attach/";
    public static readonly string URL_ORDERINGPRODUCTCATEGORYADD = URL_API_PATH + "/api/attachs/create_category";
    public static readonly string URL_ORDERINGPRODUCTCATEGORYDELETE = URL_API_PATH + "/api/attachs/delete_category/";
    public static readonly string URL_ORDERINGPRODUCTONE = URL_API_PATH + "/api/attachs/one_attach";
    public static readonly string URL_SPECIALPRODUCTLIST = URL_API_PATH + "/api/products/all_products/";
    public static readonly string URL_SPECIALPRODUCTADD = URL_API_PATH + "/api/products/create_product";
    public static readonly string URL_SPECIALPRODUCTDELETE = URL_API_PATH + "/api/products/delete_product/";
    public static readonly string URL_SPECIALPRODUCTMODIFY = URL_API_PATH + "/api/products/update_product/";
    public static readonly string URL_SPECIALPRODUCTCATEGORYLIST = URL_API_PATH + "/api/categories/all_categories/";
    public static readonly string URL_SPECIALPRODUCTCATEGORYADD = URL_API_PATH + "/api/categories/create_category";
    public static readonly string URL_SPECIALPRODUCTCATEGORYDELETE = URL_API_PATH + "/api/categories/delete_category/";
    public static readonly string URL_SPECIALPRODUCTONE = URL_API_PATH + "/api/products/one_product";
    public static readonly string URL_USERLIST = URL_API_PATH + "/api/users/all_users/0";
    public static readonly string URL_REGISTERUSER = URL_API_PATH + "/api/users/register";
    public static readonly string URL_USERDELETE = URL_API_PATH + "/api/users/userDel/";
    public static readonly string URL_USERMODIFY = URL_API_PATH + "/api/users/update_user/";
    public static readonly string URL_USERINFO = URL_API_PATH + "/api/users/one_user";
    public static readonly string URL_QUIZLIST = URL_API_PATH + "/api/questions/all_questions/";
    public static readonly string URL_QUIZDELTE = URL_API_PATH + "/api/questions/delete_question/";
    public static readonly string URL_QUIZADD = URL_API_PATH + "/api/questions/create_question/";
//    public static readonly string URL_PUSHOPTION = URL_API_PATH + "/api/products";
    public static readonly string URL_PRODUCTCATEGORY = URL_API_PATH + "/api/categories/all_categories/";
    public static readonly string URL_ORDERINGCATEGORY = URL_API_PATH + "/api/attachs/all_categories/";
    public static readonly string URL_PRODUCTORDER = URL_API_PATH + "/api/orders/create_order";
    public static readonly string URL_FILEINCHATTING = URL_API_PATH + "/api/uploadfile";
}


[Serializable]
public class LoginData
{
    public string email;
    public string password;
    public string attach_time;
    public string product_time;
    public string repair_time;
    public string question_time;

    public LoginData(string us, string pw, string a, string p, string r, string q)
    {
        email = us;
        password = pw;
        attach_time = a;
        product_time = p;
        repair_time = r;
        question_time = q;
    }
}

[Serializable]
public class LoginResult
{
    public bool status;
    public string message;
    public int id;
}

[Serializable]
public class InstallGuideItem
{
    public int id;
    public string folder;
    public string title;
    public string setup_guide;
//    public string categorie;
    public string date;
    //public InstallGuideItem(string url, string content)
    //{
    //    attachmentUrl = url;
    //    this.content = content;
    //}
}

[Serializable]
public class InstallGuideItems
{
    public bool status;
    public int count;
    public List<InstallGuideItem> result = null;
    public void Add(InstallGuideItem a)
    {
        if (result == null) result = new List<InstallGuideItem>();
        result.Add(a);
        count++;
    }
}

[Serializable]
public class FaqItem
{
    public int id;
    public string question;
    public int categorie;
    public string date;
    public string answer;
    public FaqItem(string question, int categorie, string date, string answer, int id)
    {
        this.id = id;
        this.question = question;
        this.categorie = categorie;
        this.date = date;
        this.answer = answer;
    }
}

[Serializable]
public class FaqList
{
    public bool status;
    public int count;
    public List<FaqItem> result;
}

[Serializable]
public class UserItem
{
    public int id;
    public string name;
    public string email;
    public string address;
    public string password;
    public string created;
    public UserItem(string username, string email, string address, int id = 0, string password = "", string created = "")
    {
        this.id = id;
        this.name = username;
        this.email = email;
        this.address = address;
        this.password = password;
        this.created = created;
    }
}

[Serializable]
public class UserList
{
    public bool status;
    public int count;
    public List<UserItem> result;
    public void AddUser(UserItem ui)
    {
        result.Add(ui);
    }
}

[Serializable]
public class QuizItem
{
    public int id;
    public string username;
    public string user_id;
    public string text;
    public string date;
    public QuizItem(int id, string username, string user_id, string date, string text)
    {
        this.id = id; this.username = username;
        this.date = date; this.text = text;
        this.user_id = user_id;
    }
}

[Serializable]
public class QuizList
{
    public bool status;
    public int count;
    public List<QuizItem> result;
}

[Serializable]
public class RepairingItem
{
    public int id;
    public int request_id;
    public string username;
    public string date;
    public string user_address;
    public string seller_address;
    public string sellername;
    public string description;
    public string image;
    public string status;
    public string sell_date;

    public RepairingItem(int id, int request_id, string username, string date,
        string user_address, string seller_address, string sellername, string description,
        string image, string status, string sell_date)
    {
        this.id = id; this.request_id = request_id;
        this.username = username; this.date = date;
        this.user_address = user_address;
        this.seller_address = seller_address;
        this.sellername = sellername;
        this.description = description;
        this.image = image;
        this.status = status;
        this.sell_date = sell_date;
    }
}

[Serializable]
public class RepairList
{
    public bool status;
    public int count;
    public List<RepairingItem> result;
}

[Serializable]
public class SpecialProductItem
{
    public string vendor_name;
    public string vendor_id;
    public int id;
    public string folder;
    public string image;
    public string time;
    public string time_update;
    public string visibility;
    public int categorie;
    public string quantity;
    public string procurement;
    public string url;
    public string position;
    public string title;
    public string description;
    public double price;
    public string serial_number;
    public string abbr;
    public string for_id;
    public SpecialProductItem(string image, string url, double price, string serial_number, string description, int id)
    {
        this.image = image;
        this.url = url;
        this.price = price;
        this.serial_number = serial_number;
        this.description = description;
        this.id = id;
    }
}

[Serializable]
public class SpecialProductItemList
{
    public bool status;
    public int count;
    public List<SpecialProductItem> result;
}
