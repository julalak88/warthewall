using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    public Button playBtn;
    public Button howtoBtn;
    public Button singleBtn;
    public Button multiBtn;
    public Button easyBtn;
    public Button normalBtn;
    public Button hardBtn;
    public Button googleBtn;
    public Button guestBtn;
    public CanvasGroup modePanel;
    public CanvasGroup levelPanel;
    public float time;
    public float timeFade;
    public Ease ease;
    public GameObject howToPanel;
    void Start()
    {

    }

    public void StartGame()
    {
        modePanel.gameObject.SetActive(true);
        modePanel.DOFade(1, time);
        singleBtn.transform.DOScale(Vector3.one, time).SetEase(ease);
        multiBtn.transform.DOScale(Vector3.one, time).SetEase(ease);
        playBtn.transform.localScale = Vector3.zero;
        howtoBtn.transform.localScale = Vector3.zero;
    }

    public void HowToPlay()
    {

        if (howToPanel.activeSelf)
        {
            howToPanel.SetActive(false);
        }
        else
        {
            howToPanel.SetActive(true);
        }

    }
    public void SelectMode(string _mode)
    {
        PlayerPrefs.SetString("mode", _mode);
        if (_mode.Equals("single"))
        {
            levelPanel.gameObject.SetActive(true);
            levelPanel.DOFade(1, timeFade).OnComplete(() =>
           {

               easyBtn.transform.DOScale(Vector3.one, time).SetEase(ease);
               normalBtn.transform.DOScale(Vector3.one, time).SetEase(ease);
               hardBtn.transform.DOScale(Vector3.one, time).SetEase(ease);

           });
            modePanel.DOFade(0, timeFade).OnComplete(() =>
           {

               modePanel.gameObject.SetActive(false);

           });
        }
        else
        {
            LoadScene();
        }
    }

    public void Login()
    {
        guestBtn.interactable = false;
        googleBtn.interactable = false;
        googleBtn.transform.localScale = Vector3.zero;
        guestBtn.transform.localScale = Vector3.zero;
        playBtn.transform.DOScale(Vector3.one, time).SetEase(ease);
        howtoBtn.transform.DOScale(Vector3.one, time).SetEase(ease);

    }

    public void SelectLevel(int index)
    {
        PlayerPrefs.SetInt("level", index);
        LoadScene();
    }


    void LoadScene()
    {
        SceneManager.LoadScene("Game");
    }
}
