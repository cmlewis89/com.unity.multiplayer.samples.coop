using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

namespace Unity.Multiplayer.Samples.BossRoom.RuntimeTests
{
    public class BossRoomPackageMonoBehaviourTest
    {
        [UnityTest]
        public IEnumerator MonoBehaviourTest_Works()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();
        }
    }

    public class MyMonoBehaviourTest : MonoBehaviour, IMonoBehaviourTest
    {
        private int frameCount;
        public bool IsTestFinished
        {
            get { return frameCount > 10; }
        }

        void Update()
        {
            frameCount++;
        }
    }
}
