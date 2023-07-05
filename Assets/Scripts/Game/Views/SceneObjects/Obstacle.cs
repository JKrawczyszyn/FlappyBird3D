using UnityEngine;

namespace Fp.Game.Views
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer[] meshRenderers;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (meshRenderers == null || meshRenderers.Length <= 0)
                meshRenderers = GetComponentsInChildren<MeshRenderer>();
        }
#endif

        public void SetAlpha(float alpha)
        {
            foreach (var meshRenderer in meshRenderers)
                meshRenderer.material.color = new Color(1f, 1f, 1f, alpha);
        }
    }
}
