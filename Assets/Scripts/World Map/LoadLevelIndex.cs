using UnityEngine;

public class LoadLevelIndex : MonoBehaviour
{
    public static LoadLevelIndex Instance {get; private set;}

    [Header("Levels")]
    public ushort _currentLevelIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
