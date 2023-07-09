using UnityEngine;

namespace Game.Views
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer[] meshRenderers;

#if UNITY_EDITOR
        private void OnValidate()
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
        }
#endif

        public void SetAlpha(float value)
        {
            foreach (var meshRenderer in meshRenderers)
                meshRenderer.material.color = new Color(1f, 1f, 1f, value);
        }
    }
}
