using System.Collections.Generic;
using UnityEngine;

public class InfiniteLoopView : MonoBehaviour
{
    private int elementLength;
    private int loopLength;

    public int Speed { private get; set; }

    [SerializeField]
    private Transform container;

    private readonly List<GameObject> elements = new();

    public void Init(string assetName,
                     int elementLength,
                     int loopLength,
                     WallsAssetManager assetManager)
    {
        this.elementLength = elementLength;
        this.loopLength = loopLength;

        for (var i = 0; i < loopLength; i++)
        {
            var go = assetManager.Instantiate(assetName, i * Vector3.forward * elementLength, container);
            elements.Add(go);
        }
    }

    private void Update()
    {
        if (Speed == 0)
            return;

        var positionChange = -Vector3.forward * Speed * Time.deltaTime;

        foreach (var wall in elements)
        {
            wall.transform.position += positionChange;

            if (wall.transform.position.z < -elementLength)
                wall.transform.position += Vector3.forward * elementLength * loopLength;
        }
    }
}
