using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> _leaderBoardEntries;
    private const string DefaultTimeText = "00:00:000";

    private DatabaseReference _databaseReference;

    private void Start()
    {
        InitializeFirebase();
        LoadLeaderboardData();
    }

    private void InitializeFirebase()
    {
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void LoadLeaderboardData()
    {
        string userId = "userId_placeholder";
        _databaseReference.Child("users").Child(userId).Child("saves").GetValueAsync().ContinueWithOnMainThread(OnLeaderboardDataLoaded);
    }

    private void OnLeaderboardDataLoaded(System.Threading.Tasks.Task<DataSnapshot> task)
    {
        if (task.IsFaulted || task.Result == null || !task.Result.HasChildren)
        {
            SetDefaultLeaderboard();
            return;
        }

        List<float> allPoints = CollectAllPoints(task.Result);
        DisplayLeaderboard(allPoints);
    }

    private List<float> CollectAllPoints(DataSnapshot snapshot)
    {
        List<float> allPoints = new();
        foreach (DataSnapshot saveSnapshot in snapshot.Children)
        {
            string json = saveSnapshot.GetRawJsonValue();
            DataGame dataGame = JsonUtility.FromJson<DataGame>(json);
            if (dataGame != null && dataGame._pointsPerLevel != null)
            {
                allPoints.AddRange(dataGame._pointsPerLevel);
            }
        }
        allPoints.RemoveAll(pointValue => pointValue == float.MaxValue);
        allPoints.Sort();
        return allPoints;
    }

    private void DisplayLeaderboard(List<float> allPoints)
    {
        for (int i = 0; i < _leaderBoardEntries.Count; i++)
        {
            if (i < allPoints.Count)
            {
                _leaderBoardEntries[i].text = FormatTime(allPoints[i]);
            }
            else
            {
                _leaderBoardEntries[i].text = DefaultTimeText;
            }
        }
    }

    private void SetDefaultLeaderboard()
    {
        for (int i = 0; i < _leaderBoardEntries.Count; i++)
        {
            _leaderBoardEntries[i].text = DefaultTimeText;
        }
    }

    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time - (int)time) * 1000);
        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}
