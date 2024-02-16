using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public List<ScoreRecord> ScoreRecords;

    private List<int> scoreList;

    private void OnEnable()
    {
        scoreList = GameManager.instance.GetScoreListData();
    }
    private void Start()
    {
        SetLeaderboardData();
    }
    public void SetLeaderboardData()
    {
        for (int i = 0; i < ScoreRecords.Count; i++)
        {
            if (i < scoreList.Count)
            {
                ScoreRecords[i].SetScoreText(scoreList[i]);
                ScoreRecords[i].gameObject.SetActive(true);
            }
            else
            {
                ScoreRecords[i].gameObject.SetActive(false);
            }
        }
    }

}
