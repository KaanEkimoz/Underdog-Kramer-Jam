using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShelvesManager : MonoBehaviour
{
    public List<ShelfDetector> shelves = new();

    private void Start()
    {
        if (shelves.Count == 0)
            shelves = Object.FindObjectsByType<ShelfDetector>(FindObjectsSortMode.None).ToList();

    }

}
