using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProyectile : MonoBehaviour

{
    private float _speed;
    private Vector3 _direction;

    public void Initialize(float speed, Vector3 direction)
    {
        _speed = speed;
        _direction = direction.normalized;
    }

    private void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }
}