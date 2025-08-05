using UnityEngine;

public class StartPlayer : MonoBehaviour
{
    [SerializeField] private MovementData _characterData;

    private void Start()
    {
        if (IsPlayerAlreadyPresent()) return;

        int playerIndex = PlayerPrefs.GetInt("PlayerIndex");
        GameObject newPlayer = InstantiatePlayer(playerIndex);

        SetPlayerTransform(newPlayer);
    }

    private bool IsPlayerAlreadyPresent()
    {
        GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");

        if (existingPlayer != null)
        {
            Debug.LogWarning("Ya existe un GameObject con tag 'Player'. No se instanciará otro.");
            transform.SetParent(existingPlayer.transform);
            return true;
        }

        return false;
    }

    private GameObject InstantiatePlayer(int playerIndex)
    {
        if (_characterData != null && playerIndex >= 0 && playerIndex < _characterData.Characters.Count)
        {
            return Instantiate(_characterData.Characters[playerIndex].Player);
        }

        Debug.LogWarning(GetInstantiationWarningMessage(playerIndex));
        return Instantiate(_characterData.Characters[0].Player);
    }

    private string GetInstantiationWarningMessage(int playerIndex)
    {
        if (_characterData == null)
        {
            return "Lista de CharacterData no está asignada. Usando prefab por defecto para el jugador.";
        }

        if (playerIndex < 0 || playerIndex >= _characterData.Characters.Count)
        {
            return "Índice fuera de rango. Usando prefab por defecto para el jugador.";
        }

        return "Error desconocido. Usando prefab por defecto para el jugador.";
    }

    private void SetPlayerTransform(GameObject player)
    {
        player.transform.position = transform.position;
        player.transform.SetParent(transform);
    }
}
