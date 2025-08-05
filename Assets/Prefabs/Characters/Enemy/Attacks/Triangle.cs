using UnityEngine;

public class Triangle : MonoBehaviour
{
    [SerializeField] private float _disableDelay = 3f;

    private void Awake()
    {
        transform.position = new Vector3(0, 0, 0);
        Invoke("DisableGameObject", _disableDelay);
    }

    private void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
