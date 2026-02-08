using SpaceTransit.Loader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceTransit.Menu
{

    public sealed class SyncSceneSwitcherButton : AutoRegisterButton
    {

        [SerializeField]
        private string sceneName;

        protected override void Click()
        {
            WorldChanger.Cancel();
            SceneManager.LoadScene(sceneName);
        }

    }

}
