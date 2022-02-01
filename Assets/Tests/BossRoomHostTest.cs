using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Unity.Multiplayer.Samples.BossRoom.RuntimeTests
{
    public class BossRoomHostTest : IPrebuildSetup
    {
        public void Setup()
        {

        }

        [UnityTest]
        public IEnumerator NetworkManager_HostSmokeTest_True()
        {
            // load Startup scene
            SceneManager.LoadSceneAsync("Startup");

            while (SceneManager.GetActiveScene().name != "MainMenu")
            {
                yield return null;
            }

            // MainMenu scene
            var hostButtonGameObject = GameObject.Find("Host Btn");
            var hostButton = hostButtonGameObject.GetComponent<Button>();
            hostButton.OnSubmit(new PointerEventData(EventSystem.current));

            yield return null;

            var confirmButtonGameObject = GameObject.Find("Confirmation Button");
            var confirmButton = confirmButtonGameObject.GetComponent<Button>();
            confirmButton.OnSubmit(new PointerEventData(EventSystem.current));

            yield return null;

            // verify hosting is successful
            Assert.That(NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening);

            while (SceneManager.GetActiveScene().name != "CharSelect")
            {
                yield return null;
            }

            // select a character
            var characterButtonGameObject = GameObject.Find("PlayerSeat (0)");
            var characterButton = characterButtonGameObject.GetComponentInChildren<Button>();
            characterButton.OnSubmit(new PointerEventData(EventSystem.current));

            yield return null;

            // hit ready
            var readyButtonGameObject= GameObject.Find("Ready Btn");
            var readyButton = readyButtonGameObject.GetComponentInChildren<Button>();
            readyButton.OnSubmit(new PointerEventData(EventSystem.current));

            while (SceneManager.GetActiveScene().name != "BossRoom")
            {
                yield return null;
            }

            yield return new WaitForSeconds(10f);
        }
    }
}
