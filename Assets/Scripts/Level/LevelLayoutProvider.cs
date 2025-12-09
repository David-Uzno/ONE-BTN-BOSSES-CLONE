using UnityEngine;

public class LevelLayoutProvider : MonoBehaviour
{
    public static LevelLayoutProvider Instance { get; private set; }

    [SerializeField] private LevelLayoutType _layoutType = LevelLayoutType.Circular;
    [SerializeField] private Vector2 _center = Vector2.zero;
    [SerializeField] private float _radius = 7.5f;

    private ILevelLayout _layout;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _layout = CreateLayout();
    }

    public ILevelLayout GetLayout()
    {
        if (_layout == null)
        {
            _layout = CreateLayout();
        }
        return _layout;
    }

    private ILevelLayout CreateLayout()
    {
        return _layoutType
        switch
        {
            _ => new CircularPath(_center, _radius),
        };
    }
}
