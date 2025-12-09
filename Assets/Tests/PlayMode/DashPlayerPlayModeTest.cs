using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class DashPlayerPlayModeTest
{
    private GameObject _playerGameObject;
    private DashPlayer _dashPlayer;
    private Collider2D _collider;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _playerGameObject = new GameObject("Player");
        _collider = _playerGameObject.AddComponent<BoxCollider2D>();
        RotationControllerStub movementController = _playerGameObject.AddComponent<RotationControllerStub>();
        _dashPlayer = _playerGameObject.AddComponent<DashPlayer>();

        // Asignar referencias necesarias
        typeof(DashPlayer).GetField("_collider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(_dashPlayer, _collider);
        typeof(DashPlayer).GetField("_movementController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(_dashPlayer, movementController);

        // Asignar un TextMeshProUGUI falso si es necesario
        GameObject canvasGO = new("Canvas");
        TextMeshProUGUI tmp = canvasGO.AddComponent<TextMeshProUGUI>();
        typeof(DashPlayer).GetField("_counterText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(_dashPlayer, tmp);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Dash_DisablesColliderOnStart()
    {
        // Inicializar el contador para permitir el dash
        typeof(DashPlayer).GetField("_counter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(_dashPlayer, (byte)100);

        // Simular el dash llamando a Movement()
        _dashPlayer.GetType().GetMethod("Movement", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(_dashPlayer, null);

        yield return null; // Esperar un frame

        Assert.IsFalse(_collider.enabled, "El collider debería estar desactivado al iniciar el dash.");
    }

    [UnityTest]
    public IEnumerator Dash_SpeedResets_WhenCounterReachesZero()
    {
        // Inicializar contador y velocidad
        typeof(DashPlayer).GetField("_counter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(_dashPlayer, (byte)2);

        // Simular dash
        _dashPlayer.GetType().GetMethod("Movement", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(_dashPlayer, null);

        // Esperar a que contador llegue a 0 y velocidad se resetee
        yield return new WaitForSeconds(0.1f);

        // Verificar velocidad
        RotationControllerStub movementController = (RotationControllerStub)typeof(DashPlayer).GetField("_movementController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(_dashPlayer);

        Assert.AreEqual(movementController._initialSpeed, movementController._currentSpeed, 0.01f, "La velocidad debe volver al valor inicial cuando el contador llega a 0.");
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(_playerGameObject);
        yield return null;
    }

    private class RotationControllerStub : RotationController
    {
        public RotationControllerStub()
        {
            _initialSpeed = 1.0f;
            _currentSpeed = 1.0f;
        }
    }
}
