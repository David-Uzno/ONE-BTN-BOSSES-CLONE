using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RotationControllerPlayModeTest
{
    [UnityTest]
    public IEnumerator Start_InitializesCorrectly()
    {
        // Crear GameObject y agregar RotationController
        GameObject playerObject = new("Player");
        RotationController controller = playerObject.AddComponent<RotationController>();

        // Asignar _rotationPoint usando Reflection
        System.Reflection.FieldInfo field = typeof(RotationController).GetField("_rotationPoint", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(controller, Vector2.zero);

        // Dejar correr un frame para que Start() se ejecute
        yield return null;

        // Verificar IsInitialized == true
        Assert.IsTrue(controller.IsInitialized, "IsInitialized debe ser true después de Start()");

        // Verificar _currentSpeed == _initialSpeed
        Assert.AreEqual(controller._initialSpeed, controller._currentSpeed, "_currentSpeed debe ser igual a _initialSpeed después de Start()");

        // Verificar que transform.position cambió al valor devuelto por el layout real
        ILevelLayout layout = LevelLayoutResolver.Resolve(Vector2.zero);
        Vector2 expectedPosition = layout.GetPoint(layout.GetParameter(playerObject.transform.position));
        Assert.AreEqual(expectedPosition, (Vector2)playerObject.transform.position, "transform.position debe ser igual al punto del layout después de Start()");
    }
}
