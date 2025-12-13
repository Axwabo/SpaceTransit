using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Menu
{

    [RequireComponent(typeof(Button))]
    public abstract class AutoRegisterButton : MonoBehaviour
    {

        protected virtual void Awake() => GetComponent<Button>().onClick.AddListener(Click);

        protected abstract void Click();

    }

}
