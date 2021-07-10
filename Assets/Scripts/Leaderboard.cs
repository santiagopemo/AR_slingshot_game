using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    public Text leadersList;
    public int leadersLength = 3;

    public void SetLeader(int scoreToSet)
    {
        for (int i = 1; i <= leadersLength; i++)
        {
            string key = "Leader" + i;
            if (PlayerPrefs.HasKey(key))
            {
                int score = PlayerPrefs.GetInt(key);
                if (scoreToSet > score)
                {
                    PlayerPrefs.SetInt(key, scoreToSet);
                    scoreToSet = score;
                }
            }
            else
            {
                PlayerPrefs.SetInt(key, scoreToSet);
                break;
            }
        }
    }

    public void PrintLeaderBoard()
    {
        string leaders = "";
        for (int i = 1; i <= leadersLength; i++)
        {
            string key = "Leader" + i;
            if (PlayerPrefs.HasKey(key))
            {
                leaders += $"{i} - {PlayerPrefs.GetInt(key)}\n";
            }
        }
        leadersList.text = leaders;
    }
}
