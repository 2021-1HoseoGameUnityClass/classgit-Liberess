using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public GameObject retryBtn;

    public bool isPlay;

    private string currentScene;

    public int playerHp;

    private void Awake() => Instance = this;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        isPlay = true;
        playerHp = 3;
        DontDestroyOnLoad(gameObject);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        isPlay = false;
        retryBtn.SetActive(true);
    }

    public void SaveData()
    {
        GameData gameData = new GameData();
        gameData.sceneName = currentScene;

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Save.dat");

        binaryFormatter.Serialize(file, gameData);
        file.Close();
    }

    public void LoadData()
    {
        if(File.Exists(Application.persistentDataPath + "/SaveSet.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/SaveSet.dat", FileMode.Open);

            if(file != null && file.Length > 0)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                GameData gameData = (GameData)binaryFormatter.Deserialize(file);

                file.Close();
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}