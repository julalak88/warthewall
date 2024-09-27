using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
public class ResultControl : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public GameObject panel;
    public GameObject popup;
    string filePath;
    void Start()
    {

    }

    public void ShowResult(string result)
    {
        panel.SetActive(true);
        resultText.text = result;
        popup.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            StartCoroutine(TakeScreenshotAndShare());
        });
    }

    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());


        Destroy(ss);


    }

    public void Share()
    {
        new NativeShare().AddFile(filePath)
                   .SetSubject("WarTheWall Game")
                   .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
                   .Share();
    }


     public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

     public void BackHome()
    {
        SceneManager.LoadScene("Home");
    }


}
