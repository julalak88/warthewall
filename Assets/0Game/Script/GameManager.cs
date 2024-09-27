using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public TurnManager turnManager;
    public ResultControl resultControl;
    public Player player1;
    public Player player2;
    public Image imageProfile;
    public TextMeshProUGUI username;
    private void Awake()
    {

        instance = this;
    }

    void Start()
    {

        if (PlayerPrefs.GetString("mode").Equals("single"))
        {
            StartSinglePlayerMode();
        }
        else
        {
            StartTwoPlayerMode();
        }

        if (!string.IsNullOrEmpty(GoogleSheetsData.googleSheets.username))
        {

            SetupGmailProfile();
        }
    }

    void SetupGmailProfile()
    {
        imageProfile.enabled = true;
        username.text = GoogleSheetsData.googleSheets.username;
        Rect rect = new Rect(0, 0, GoogleSheetsData.googleSheets.profileTexture.width, GoogleSheetsData.googleSheets.profileTexture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        imageProfile.sprite = Sprite.Create(GoogleSheetsData.googleSheets.profileTexture, rect, pivot);

    }
    public void StartTwoPlayerMode()
    {

        player2.isBot = false;


        player1.SetupGameData();
        player2.SetupGameData();
        turnManager.enabled = true;

    }

    public void StartSinglePlayerMode()
    {

        player2.isBot = true;

        player1.SetupGameData();
        player2.SetupGameData();
        turnManager.enabled = true;
    }
}

