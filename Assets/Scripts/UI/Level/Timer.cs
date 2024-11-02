using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;          // Muestra el tiempo en pantalla durante el juego
    [SerializeField] TextMeshProUGUI finalTimeText;     // Muestra el tiempo final al ganar
    [SerializeField] TextMeshProUGUI highScoreMessage;  // Muestra el mensaje de récord

    float elapsedTime;
    bool isGameOver = false;
    float bestTime;

    void Start()
    {
        // Recupera el mejor tiempo guardado o establece uno muy alto si no existe
        bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
    }

    void Update()
    {
        if (!isGameOver)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void GameOver()
    {
        isGameOver = true;

        // Muestra el tiempo total que tardó el jugador
        finalTimeText.text = "Tiempo total: " + FormatTime(elapsedTime);

        // Verifica si el tiempo actual es un nuevo récord
        if (elapsedTime < bestTime)
        {
            bestTime = elapsedTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            highScoreMessage.text = "¡Nuevo récord!";
        }
        else
        {
            highScoreMessage.text = "No superaste el récord.";
        }
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
