using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Controllers;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game.Views
{
    public class CollectiblesView : MonoBehaviour
    {
        [SerializeField]
        private Transform container;

        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsProvider assetsProvider;

        [Inject]
        private CollectiblesController collectiblesController;

        private string[] assetNames;

        private readonly Dictionary<int, Collectible> elements = new();

        [Inject]
        private async UniTaskVoid Construct()
        {
            collectiblesController.OnAdd += Add;
            collectiblesController.OnRemove += Remove;
            collectiblesController.OnUpdatePositions += UpdatePositions;

            assetNames = assetsRepository.AssetNames(AssetTag.Collectible);

            await assetsProvider.CacheReferences<Collectible>(assetNames);

            collectiblesController.Initialize();
        }

        public int GetId(Collectible collectible)
        {
            foreach (var element in elements)
                if (element.Value == collectible)
                    return element.Key;

            return -1;
        }

        private void Add(CollectibleModel model)
        {
            var assetName = assetNames.GetRandom();
            var element = assetsProvider.Instantiate<Collectible>(assetName, model.Position, container);
            elements.Add(model.Id, element);
        }

        private void Remove(CollectibleModel model)
        {
            assetsProvider.Release(elements[model.Id]);
            elements.Remove(model.Id);
        }

        private void UpdatePositions(List<CollectibleModel> models)
        {
            foreach (var model in models)
                elements[model.Id].transform.position = model.Position;
        }

        private void OnDestroy()
        {
            collectiblesController.OnAdd -= Add;
            collectiblesController.OnRemove -= Remove;
            collectiblesController.OnUpdatePositions -= UpdatePositions;
        }
    }
}
