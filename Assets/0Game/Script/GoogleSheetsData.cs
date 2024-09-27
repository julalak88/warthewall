using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GoogleSheetsData : MonoBehaviour
{
    public static GoogleSheetsData googleSheets;
    [SerializeField] string gridId;
    [SerializeField] string id;

    [TextArea(5, 5)]
    public string result;
    [HideInInspector]
    public Texture2D profileTexture;
    public string username;
    public GameData gameDataList = new GameData();

 void Awake()
    {
        if(googleSheets==null){
            googleSheets = this;
        }else{
          
        }
    DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        StartCoroutine(GetGoogleSheetData());
    }

    IEnumerator GetGoogleSheetData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get($@"https://docs.google.com/spreadsheet/ccc?key={id}&usp=sharing&output=csv&id=KEY&gid={gridId}"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                
                string sheetData = www.downloadHandler.text;
                result = sheetData;

               
                ParseSheetData(sheetData);

            }
        }
    }

    void ParseSheetData(string csvData)
    {
      
        string[] lines = csvData.Split('\n');

        
        gameDataList.enemyLevel = new List<SheetData>();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            if (values.Length >= 6)
            {
                string name = values[0];

               
                switch (name)
                {
                    case "Player HP":
                        gameDataList.player = ParseSheetDataLine(values);
                        break;

                    case "Enemy HP easy":
                    case "Enemy HP normal":
                    case "Enemy HP hard":
                        gameDataList.enemyLevel.Add(ParseSheetDataLine(values));
                        break;

                    case "Normal Attack":
                        gameDataList.normalAttack = ParseSheetDataLine(values);
                        break;

                    case "Small Attack":
                        gameDataList.smallAttack = ParseSheetDataLine(values);
                        break;

                    case "Power Throw":
                        gameDataList.powerThorw = ParseSheetDataLine(values);
                        break;

                    case "Double Attack":
                        gameDataList.doubleAttack = ParseSheetDataLine(values);
                        break;

                    case "Heal":
                        gameDataList.heal = ParseSheetDataLine(values);
                        break;

                    case "Time to think":
                        gameDataList.timetoThink = ParseSheetDataLine(values);
                        break;

                    case "Time to warnning":
                        gameDataList.timetowarnning = ParseSheetDataLine(values);
                        break;

                    default:
                        Debug.Log("Unknown entry: " + name);
                        break;
                }
            }
        }
    }

   
    SheetData ParseSheetDataLine(string[] values)
    {
        SheetData data = new SheetData();
        data.name = values[0];
        data.amount = ParseInt(values[1]);
        data.damage = ParseInt(values[2]);
        data.hp = ParseInt(values[3]);
        data.missedChance = ParsePercentage(values[4]);
        data.sec = ParseFloat(values[5]);
        return data;
    }

   
    int ParseInt(string value)
    {
        int result;
        if (int.TryParse(value, out result))
        {
            return result;
        }
        return 0;
    }

    
    float ParseFloat(string value)
    {
        float result;
        if (float.TryParse(value, out result))
        {
            return result;
        }
        return 0;
    }

   
    float ParsePercentage(string value)
    {
        if (value.EndsWith("%"))
        {
            float percentage;
            if (float.TryParse(value.TrimEnd('%'), out percentage))
            {
                return percentage / 100f;
            }
        }
        return 0;
    }
}

[System.Serializable]
public class GameData
{
    public SheetData player;
    public List<SheetData> enemyLevel;
    public SheetData normalAttack;
    public SheetData smallAttack;
    public SheetData powerThorw;
    public SheetData doubleAttack;
    public SheetData heal;
    public SheetData timetoThink;
    public SheetData timetowarnning;
}

[System.Serializable]
public class SheetData
{
    public string name;
    public int amount;
    public int damage;
    public int hp;
    public float missedChance;
    public float sec;
}
