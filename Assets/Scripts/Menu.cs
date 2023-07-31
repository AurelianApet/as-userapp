using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text[] txt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        foreach (Text t in txt) t.fontSize = (int)(130f / 2880 * Screen.width);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogoBtnClick()
    {
        if (!Global.isLogin) SceneManager.LoadScene("MainMenu");
    }

    public void InstallGuideBtnClick()
    {
        SceneManager.LoadScene("InstallGuide");
    }

    public void FaqBtnClick()
    {
        SceneManager.LoadScene("Faq");
    }

    public void RepairBtnClick()
    {
        if (!Global.isLogin)
        {
            Utils.it.GotoLogin();
            return;
        }
        SceneManager.LoadScene("Repair");
    }

    public void ProductBtnClick()
    {
        SceneManager.LoadScene("Product");
    }

    public void SpecialProductBtnClick()
    {
        if (!Global.isLogin)
        {
            Utils.it.GotoLogin();
            return;
        }
        SceneManager.LoadScene("SpecialProduct");
    }

    public void QuizBtnClick()
    {
        if (!Global.isLogin)
        {
            Utils.it.GotoLogin();
            return;
        }
        SceneManager.LoadScene("Quiz");
    }
}
