using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using TMPro;

public class TimerTests
{
	[Test]
	public void FormatTime_DevuelveElFormatoCorrecto()
	{
		// Crear GameObject y agregar Timer
		GameObject timerTestObject = new("TimerTest");
		Timer timer = timerTestObject.AddComponent<Timer>();

		// Obtener método privado FormatTime
		MethodInfo formatTimeMethod = typeof(Timer).GetMethod("FormatTime", BindingFlags.NonPublic | BindingFlags.Instance);
		Assert.IsNotNull(formatTimeMethod, "No se pudo encontrar el método FormatTime.");

		// Invocar FormatTime con tiempo de ejemplo
		string timerOutput = (string)formatTimeMethod.Invoke(timer, new object[] { 75.123f });

		// Verificar formato esperado
		Assert.AreEqual("1:15:123", timerOutput);

		// Limpiar GameObject
		Object.DestroyImmediate(timerTestObject);
	}

	[Test]
	public void UpdateTimeUI_MuestraTextoCorrecto()
	{
		// Crear GameObject y agregar Timer
		GameObject timerTestObject = new("TimerTest");
		Timer timer = timerTestObject.AddComponent<Timer>();

		// Crear TextMeshProUGUI simulado
		GameObject timerTextGameObject = new("CurrentTimeText");
        TextMeshProUGUI textMeshProInstance = timerTextGameObject.AddComponent<TextMeshProUGUI>();
		timer.GetType().GetField("_currentTimeText", BindingFlags.NonPublic | BindingFlags.Instance)
			.SetValue(timer, textMeshProInstance);

		// Asignar tiempo de ejemplo
		timer.GetType().GetField("_elapsedTime", BindingFlags.NonPublic | BindingFlags.Instance)
			.SetValue(timer, 75.123f);

		// Invocar UpdateTimeUI
		MethodInfo updateTimeUIMethod = typeof(Timer).GetMethod("UpdateTimeUI", BindingFlags.NonPublic | BindingFlags.Instance);
		Assert.IsNotNull(updateTimeUIMethod, "No se pudo encontrar el método UpdateTimeUI.");
		updateTimeUIMethod.Invoke(timer, null);

		// Verificar texto esperado
		Assert.AreEqual("1:15:123", textMeshProInstance.text);

		// Limpiar GameObjects
		Object.DestroyImmediate(timerTestObject);
		Object.DestroyImmediate(timerTextGameObject);
	}

	[Test]
	public void StopGameTimer_AsignaIsGameOverTrue()
	{
		// Crear GameObject y agregar Timer
		GameObject timerTestObject = new("TimerTest");
		Timer timer = timerTestObject.AddComponent<Timer>();

		// Crear y asignar TextMeshProUGUI simulado para _finalTimeDisplayText
		GameObject finalTimeTextObject = new("FinalTimeText");
		TextMeshProUGUI finalTimeTextInstance = finalTimeTextObject.AddComponent<TextMeshProUGUI>();
		timer.GetType().GetField("_finalTimeDisplayText", BindingFlags.NonPublic | BindingFlags.Instance)
			.SetValue(timer, finalTimeTextInstance);

		// Invocar StopGameTimer
		timer.StopGameTimer();

		// Obtener valor de _isGameOver
		bool isGameOver = (bool)timer.GetType().GetField("_isGameOver", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(timer);

		// Verificar que _isGameOver es true
		Assert.IsTrue(isGameOver);

		// Limpiar GameObjects
		Object.DestroyImmediate(timerTestObject);
		Object.DestroyImmediate(finalTimeTextObject);
	}
}
