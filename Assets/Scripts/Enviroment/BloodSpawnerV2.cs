using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BloodSpawnerV2 : MonoBehaviour
{
    [SerializeField] private GameObject blood;
    [SerializeField] private int count;
    [SerializeField] private float range = 3;

    private static int bufferSize = 100;
    private static List<GameObject> projectors = new List<GameObject>();

    void Start()
    {
        if (PlayerInput.GraphicsQuality == 1)
            return;

        bufferSize = 25 + (int)Mathf.Pow(PlayerInput.GraphicsQuality, 1.5f) * 5;
        SpawnBlood();
    }

    private void SpawnBlood()
    {
        RaycastHit hit;

        for (int i = 0; i < count; i++)
        {
            Vector3 random = Vector3.Normalize(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
            Vector3 targetPos = transform.position + random * range;

            if (Physics.Linecast(transform.position, targetPos, out hit))
            {
                Quaternion rotation = Quaternion.LookRotation(transform.position - targetPos);

                if (projectors.Count < bufferSize)
                {
                    GameObject b = Instantiate(blood, transform.position, rotation);
                    projectors.Add(b);
                }
                else
                {
                    GameObject firstP = projectors[0];

                    projectors[bufferSize - 1] = firstP;
                    projectors.RemoveAt(0);
                    projectors.Add(firstP);
                    projectors[bufferSize - 1].transform.position = hit.point + Vector3.up;
                    projectors[bufferSize - 1].transform.rotation = rotation;
                }
            }

            Debug.DrawLine(transform.position, targetPos, Color.magenta, 10f);
        }
    }
}
