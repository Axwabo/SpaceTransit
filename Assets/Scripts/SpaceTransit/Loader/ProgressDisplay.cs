using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Loader
{

    public sealed class ProgressDisplay : MonoBehaviour
    {

        public static List<LoadingProgress> Reports { get; } = new();

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private RectTransform rect;

        [SerializeField]
        private GameObject menu;

        [SerializeField]
        private GameObject cam;

        [SerializeField]
        private GameObject player;

        private void Awake()
        {
            menu.SetActive(false);
            player.SetActive(false);
        }

        private void Update()
        {
            if (Reports.Count == 0)
                return;
            var total = 0;
            var loaded = 0;
            foreach (var loader in Reports)
            {
                total += loader.Total;
                loaded += loader.Current;
            }

            if (total == 0)
                return;

            var progress = (float) loaded / total;
            text.text = progress.ToString("P0");
            rect.localScale = new Vector3(progress, 1);

            if (progress < 1)
                return;
            Begin();
            Reports.Clear();
            Destroy(gameObject);
            menu.SetActive(true);
        }

        private void Begin()
        {
            if (Reports.Count == 0)
                return;
            cam.SetActive(false);
            player.transform.parent = World.Current;
            player.SetActive(true);
        }

    }

}
