using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private GameObject[] playerHpObjs;

    private void Awake() => Instance = this;

    private void Start() => DontDestroyOnLoad(gameObject);

    public void SetHp()
    {
        for(int i = 0; i < 3 - DataManager.Instance.playerHp; i++)
            playerHpObjs[i].SetActive(false);

        if(DataManager.Instance.playerHp <= 0)
            DataManager.Instance.GameOver();
    }
}