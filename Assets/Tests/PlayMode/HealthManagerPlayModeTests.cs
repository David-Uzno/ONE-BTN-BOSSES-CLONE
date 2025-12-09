using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class HealthManagerPlayModeTests
{
    [UnityTest]
    public IEnumerator TakeDamage_DecreasesHealthCorrectly()
    {
        // Crear HealthManager
        GameObject gameObjectInstance = new();
        HealthManager healthManager = gameObjectInstance.AddComponent<HealthManager>();

        // Inicializar imágenes de vida
        healthManager.GetType()
            .GetField("_lifeImages", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(healthManager, new List<UnityEngine.UI.Image>());

        // Establecer salud inicial a 5
        healthManager.GetType()
            .GetField("_health", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(healthManager, 5);

        // Llamar TakeDamage con 2 de daño
        healthManager.TakeDamage(2);

        // Obtener el valor actual de _health
        int health = (int)healthManager.GetType()
            .GetField("_health", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(healthManager);

        // Verificar que la salud sea 3
        Assert.AreEqual(3, health);

        yield return null;
    }

    [UnityTest]
    public IEnumerator TakeDamage_DisablesLifeImagesCorrectly()
    {
        // Crear HealthManager y GameObject
        GameObject gameObjectInstance = new();
        HealthManager healthManager = gameObjectInstance.AddComponent<HealthManager>();

        // Crear imágenes simuladas
        List<Image> images = new();
        for (int i = 0; i < 3; i++)
        {
            GameObject healthManagerImage = new();
            Image healthImage = healthManagerImage.AddComponent<Image>();
            healthImage.enabled = true;
            images.Add(healthImage);
        }

        // Asignar imágenes y salud por reflexión
        typeof(HealthManager)
            .GetField("_lifeImages", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(healthManager, images);
        typeof(HealthManager)
            .GetField("_health", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(healthManager, 3);

        // Esperar un frame
        yield return null;

        // Daño y esperar
        healthManager.TakeDamage(1);
        yield return null;

        // Verificar imágenes activas/desactivadas
        Assert.IsTrue(images[0].enabled);
        Assert.IsTrue(images[1].enabled);
        Assert.IsFalse(images[2].enabled);

        // Más daño y esperar
        healthManager.TakeDamage(2);
        yield return null;

        // Verificar todas desactivadas
        Assert.IsFalse(images[0].enabled);
        Assert.IsFalse(images[1].enabled);
        Assert.IsFalse(images[2].enabled);

        // Limpiar
        Object.DestroyImmediate(gameObjectInstance);
        foreach (Image healthImage in images)
            Object.DestroyImmediate(healthImage.gameObject);
    }
}
