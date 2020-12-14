using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DunGen;
using UnityEngine.AI;

public class NavMeshGenerator : MonoBehaviour
{
    [SerializeField] private RuntimeDungeon dungeon;

    void Update()
    {
        if (!dungeon.Generator.IsGenerating )
        {
            GetComponent<NavMeshSurface>().BuildNavMesh();
            Debug.Log("NavMesh genarated");

            //TestBatching();

            this.enabled = false;
        }
    }

    private void TestBatching()
    {
        GameObject g = new GameObject();

        foreach (Transform child in GameObject.Find("Dungeon").GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("Door"))
            {
                child.parent = g.transform;
            }
        }

        foreach (Transform child in GameObject.Find("Dungeon").GetComponentsInChildren<Transform>())
        {
            Tile tile;
            if (child.TryGetComponent<Tile>(out tile))
            {
                StaticBatchingUtility.Combine(child.gameObject);
            }
        }

        StaticBatchingUtility.Combine(GameObject.Find("Dungeon"));
        Debug.Log("Batching done");
    }
}
