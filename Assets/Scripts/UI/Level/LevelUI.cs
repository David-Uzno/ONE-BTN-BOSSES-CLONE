using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUI : MonoBehaviour
{
    [SerializeField] Timer timer; // Referencia al Timer para llamar a GameOver()

    // Llama este método al ganar
    public void OnVictory()
    {
        if (timer != null)
        {
            timer.GameOver();  // Llama a GameOver para mostrar el tiempo y el récord
        }
    }


    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Map()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
