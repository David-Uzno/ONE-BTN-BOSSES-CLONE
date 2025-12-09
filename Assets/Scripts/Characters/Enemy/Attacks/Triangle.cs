using UnityEngine;

public class Triangle : MonoBehaviour
{
    [SerializeField] private float _disableDelay = 3f;

    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        CancelInvoke(nameof(DisableGameObject));
        Invoke(nameof(DisableGameObject), _disableDelay);
    }

    private void DisableGameObject()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
