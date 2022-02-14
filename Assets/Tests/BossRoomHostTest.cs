using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using Unity.Multiplayer.Samples.BossRoom.TestUtils;
using Unity.Netcode;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Unity.Multiplayer.Samples.BossRoom.RuntimeTests
{
    public class BossRoomHostTest
    {
        const string k_BootstrapScenePath = "Assets/BossRoom/Scenes/Startup.unity";

        const string k_MainMenuScenePath = "Assets/BossRoom/Scenes/MainMenu.unity";

        const string k_CharSelectScenePath = "Assets/BossRoom/Scenes/CharSelect.unity";

        const string k_BossRoomScenePath = "Assets/BossRoom/Scenes/BossRoom.unity";

        [UnityTest]
        public IEnumerator NetworkManager_HostSmokeTest_True()
        {
            Assert.That(
                System.Array.Exists(EditorBuildSettings.scenes, scene => scene.path == k_BootstrapScenePath));

            // Bootstrap scene exists; load it
            SceneManager.LoadSceneAsync(k_BootstrapScenePath);

            var waitUntilStartupSceneLoaded = new WaitForSceneLoad(k_BootstrapScenePath);

            yield return waitUntilStartupSceneLoaded;

            Assert.That(!waitUntilStartupSceneLoaded.timedOut);

            // MainMenu is loaded as soon as Startup scene is launched, validate it is loaded
            var waitUntilMainMenuSceneLoaded = new WaitForSceneLoad(k_MainMenuScenePath);

            yield return waitUntilMainMenuSceneLoaded;

            Assert.That(!waitUntilMainMenuSceneLoaded.timedOut);

            // MainMenu scene

            TestUtils.TestUtils.ClickButtonByName("Host Btn");

            // if button is successfully clicked, a confirmation popup will appear; wait a frame for it to pop up
            yield return new WaitForFixedUpdate();

            TestUtils.TestUtils.ClickButtonByName("Confirmation Button");

            // confirming hosting will initialize the hosting process; next frame the results will be ready
            yield return new WaitForEndOfFrame();

            // verify hosting is successful
            Assert.That(NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening);

            // CharSelect is loaded as soon as hosting is successful, validate it is loaded
            var waitUntilCharSelectSceneLoaded = new WaitForSceneLoad(k_CharSelectScenePath);

            yield return waitUntilCharSelectSceneLoaded;

            Assert.That(!waitUntilCharSelectSceneLoaded.timedOut);

            // select first Character
            // TODO: add option to test with all Avatar classes (ie. seats 0-7)
            TestUtils.TestUtils.ClickButtonByName("ClickInteract0");

            // selecting a class will enable the "Ready" button, next frame it is selectable
            yield return new WaitForFixedUpdate();

            // hit ready
            TestUtils.TestUtils.ClickButtonByName("Ready Btn");

            // selecting ready as host with no other party members will load BossRoom scene

            var waitUntilBossRoomSceneLoaded = new WaitForSceneLoad(k_BossRoomScenePath);

            yield return waitUntilBossRoomSceneLoaded;

            Assert.That(!waitUntilBossRoomSceneLoaded.timedOut);

            // once loaded into BossRoom scene, wait 5 seconds and disconnect
            yield return new WaitForSeconds(5f);

            TestUtils.TestUtils.ClickButtonByName("Quit Button");

            // confirm button will be active next frame

            // NOTE: always wait until next FixedUpdate()
            // according to doc (link here): waiting for FixedUpdate will be the in the sequence of events the ideal
            // place to invoke UI elements (since according to the Unity events, where Update is polled comes right
            // after FixedUpdate)
            // can use this or null

            yield return null;

            TestUtils.TestUtils.ClickButtonByName("Confirm Button");

            // wait for NetworkUpdate?
            yield return new WaitForSeconds(1f);

            Assert.That(!NetworkManager.Singleton.IsListening, "NetworkManager not fully shut down!");

            // MainMenu is loaded as soon as a shutdown is encountered, validate it is loaded
            var waitUntilMainMenuSceneLoadedOnShutdown = new WaitForSceneLoad(k_MainMenuScenePath);

            yield return waitUntilMainMenuSceneLoadedOnShutdown;

            Assert.That(!waitUntilMainMenuSceneLoadedOnShutdown.timedOut);
        }
    }
}
