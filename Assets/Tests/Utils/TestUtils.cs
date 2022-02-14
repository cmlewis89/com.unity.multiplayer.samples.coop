using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Unity.Multiplayer.Samples.BossRoom.TestUtils
{
    public abstract class TestUtils
    {
        public static void ClickButtonByName(string name)
        {
            var buttonGameObject = GameObject.Find(name);

            Assert.IsNotNull(buttonGameObject,
                $"Button GameObject with name {name} not found in scene!");

            EventSystem.current.SetSelectedGameObject(buttonGameObject);

            var buttonComponent = buttonGameObject.GetComponent<Button>();

            Assert.IsNotNull(buttonComponent, $"Button component not found on {buttonGameObject.name}!");

            buttonComponent.onClick.Invoke();
        }

        public static IEnumerator AssertIsSceneLoaded(string sceneName)
        {
            var waitUntilSceneLoaded = new WaitForSceneLoad(sceneName);

            yield return waitUntilSceneLoaded;

            Assert.That(!waitUntilSceneLoaded.timedOut);
        }
    }

    public class WaitForSceneLoad : CustomYieldInstruction
    {
        const float k_MaxSceneLoadDuration = 10f;

        string m_SceneName;

        float m_LoadSceneStart;

        float m_MaxLoadDuration;

        public bool timedOut { get; private set; }

        public override bool keepWaiting
        {
            get
            {
                var scene = SceneManager.GetSceneByPath(m_SceneName);

                var isSceneLoaded = scene.IsValid() && scene.isLoaded;

                if (Time.time - m_LoadSceneStart >= m_MaxLoadDuration)
                {
                    timedOut = true;
                }

                return !isSceneLoaded && !timedOut;
            }
        }

        public WaitForSceneLoad(string sceneName, float maxLoadDuration = k_MaxSceneLoadDuration)
        {
            m_LoadSceneStart = Time.time;
            m_SceneName = sceneName;
            m_MaxLoadDuration = maxLoadDuration;
        }
    }
}
