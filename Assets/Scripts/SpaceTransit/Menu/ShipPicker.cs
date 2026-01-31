using SpaceTransit.Movement;
using SpaceTransit.Vaulter;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class ShipPicker : AutoRegisterButton
    {

        [SerializeField]
        private VaulterController[] options;

        private TextMeshProUGUI _text;

        private int _index = -1;

        private VaulterController _current;

        protected override void Awake()
        {
            base.Awake();
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start() => Click();

        protected override void Click()
        {
            if (MovementController.Current && MovementController.Current.IsMounted)
                return;
            if (_current)
                Destroy(_current.gameObject);
            _index = Wrap(_index + 1);
            _text.text = options[Wrap(_index + 1)].gameObject.name;
            _current = Instantiate(options[_index], World.Current, false);
        }

        private int Wrap(int index) => index >= options.Length ? 0 : index;

    }

}
