﻿using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities;
using Zenject;

namespace Entry.Views
{
    public class LoadingBlendView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup panel;

        [Inject]
        private SceneLoader sceneLoader;

        [Inject]
        private void Construct()
        {
            sceneLoader.OnSceneLoadEnd += FadeOut;
            sceneLoader.OnSceneUnloadStart += FadeIn;

            panel.gameObject.SetActive(false);
        }

        private async UniTask FadeIn()
        {
            panel.gameObject.SetActive(true);

            await panel.AnimateAlpha(0f, 1f, 0.2f);
        }

        private async UniTask FadeOut()
        {
            await panel.AnimateAlpha(1f, 0f, 0.2f);

            panel.gameObject.SetActive(false);
        }

        public void OnDestroy()
        {
            sceneLoader.OnSceneLoadEnd -= FadeOut;
            sceneLoader.OnSceneUnloadStart -= FadeIn;
        }
    }
}
