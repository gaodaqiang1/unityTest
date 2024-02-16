using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<int> scoreList;
    private int score;
    private string dataPath;
    private void Awake()
    {
        dataPath = Application.persistentDataPath + "/leaderboard.json";
        scoreList = GetScoreListData();

        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }
    private void OnEnable()
    {
        EventHandler.GameOverEvent += OnGameOverEvent;
        EventHandler.GetPointEvent += OnGetPointEvent;
    }


    private void OnDisable()
    {
        EventHandler.GameOverEvent -= OnGameOverEvent;
        EventHandler.GetPointEvent -= OnGetPointEvent;
    }

    private void OnGetPointEvent(int point)
    {
        score = point;
    }

    private void OnGameOverEvent()
    {
        //TODO: 在List里添加新的分数，排序
        if (!scoreList.Contains(score))
        {
            scoreList.Add(score);
        }
        scoreList.Sort();
        scoreList.Reverse();

        File.WriteAllText(dataPath, JsonConvert.SerializeObject(scoreList));

    }
    /// <summary>
    /// 读取保存数据的记录
    /// </summary>
    /// <returns></returns>
    public List<int> GetScoreListData()
    {
        if (File.Exists(dataPath))
        {
            string jsonData = File.ReadAllText(dataPath);
            return JsonConvert.DeserializeObject<List<int>>(jsonData);

        }
        return new List<int>();
    }
}
