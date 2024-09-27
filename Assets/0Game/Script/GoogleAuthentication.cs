using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Google;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class GoogleAuthentication : MonoBehaviour
{

    public HomeManager homeManager;
    public string imageURL;
    public TextMeshProUGUI userNameTxt, userEmailTxt;
    public Image profilePic;
    public GameObject profilePanel;
    private GoogleSignInConfiguration configuration;
    public string webId ;



    void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webId,
            RequestIdToken = true,
            UseGameSignIn = false,
            RequestEmail = true
        };

        profilePic.enabled = false;
    }

    public void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
            OnAuthenticationFinished, TaskScheduler.Default);
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator =
                task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                        (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.LogError("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.LogError("Got unexpected exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.LogError("Cancelled");
        }
        else
        {
            StartCoroutine(UpdateUI(task.Result));
        }
    }

    IEnumerator UpdateUI(GoogleSignInUser user)
    {
        Debug.Log("Welcome: " + user.DisplayName + "!");

        userNameTxt.text = user.DisplayName;
        GoogleSheetsData.googleSheets.username = user.DisplayName;
        if (userEmailTxt != null) userEmailTxt.text = user.Email;
        imageURL = user.ImageUrl.ToString();
        profilePic.enabled = true;
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageURL);
        yield return request.SendWebRequest();
       GoogleSheetsData.googleSheets.profileTexture = DownloadHandlerTexture.GetContent(request);
        Rect rect = new Rect(0, 0, GoogleSheetsData.googleSheets.profileTexture.width, GoogleSheetsData.googleSheets.profileTexture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        profilePic.sprite = Sprite.Create(GoogleSheetsData.googleSheets.profileTexture, rect, pivot);

        homeManager.Login();
        profilePanel.SetActive(true);
    }

    public void OnSignOut()
    {
        userNameTxt.text = "";
        if (userEmailTxt != null) userEmailTxt.text = "";

        imageURL = "";
        profilePanel.SetActive(false);
        Debug.Log("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

}
