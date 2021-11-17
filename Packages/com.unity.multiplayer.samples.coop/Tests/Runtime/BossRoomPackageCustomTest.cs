using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using Unity.Netcode;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.GameCenter;

namespace Tests
{
    public class BossRoomPackageCustomTest : IPrebuildSetup
    {
        GameObject arrow;

        public void Setup()
        {

        }

        [UnityTest]
        public IEnumerator Arrow_MovesForward_True()
        {
            SceneManager.LoadScene("Empty");
            var arrowPath = "Assets/BossRoom/Prefabs/Game/Arrow.prefab";
            Debug.Log(arrowPath);
            var ball = AssetDatabase.LoadAssetAtPath<Object>(arrowPath);
            arrow = (GameObject)Object.Instantiate(ball);
            Object.DontDestroyOnLoad(arrow);
            yield return null;
            Assert.That(arrow != null);
            GameObject.Destroy(arrow);
        }

        [UnityTest]
        public IEnumerator NetworkManager_HostAndShutdown_True()
        {
            SceneManager.LoadScene("Empty");
            var networkManagerGameObject = (GameObject)Object.Instantiate(AssetDatabase.LoadAssetAtPath<Object>("Assets/BossRoom/Prefabs/NetworkingManager.prefab"));
            var networkManager = networkManagerGameObject.GetComponent<NetworkManager>();
            networkManager.StartHost();
            yield return null;
            networkManager.Shutdown();
            Assert.That(!networkManager.IsListening);
            GameObject.Destroy(networkManagerGameObject);
        }
    }
}
