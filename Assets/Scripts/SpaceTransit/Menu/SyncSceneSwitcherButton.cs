using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceTransit.Menu
{

    public sealed class SyncSceneSwitcherButton : AutoRegisterButton
    {

        [SerializeField]
        private string sceneName;

        protected override void Click() => SceneManager.LoadScene(sceneName);

    }

}
