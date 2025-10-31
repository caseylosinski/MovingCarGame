using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;            // drag your car from the scene
    public GameObject roadObject;       // drag the actual road object in the scene
    public GameObject coinPrefab;
    public GameObject obstaclePrefab;

    [Header("How many to spawn")]
    public int numberOfCoins = 30;
    public int numberOfObstacles = 15;

    [Header("Spawn region in front of player")]
    public float minForwardDistance = 5f;    // start spawning this far in front of car
    public float maxForwardDistance = 100f;  // furthest distance ahead of car we will spawn
    public float heightOffset = 0.5f;        // how high above the road to place them

    [Header("Spacing")]
    public float minDistanceBetweenItems = 2f; // avoid overlap

    private Bounds roadBoundsWorld;
    private List<Vector3> usedPositions = new List<Vector3>();

    void Start()
    {
        // figure out how wide the road is in world space
        roadBoundsWorld = GetRoadBounds(roadObject);

        // spawn coins first
        SpawnItems(coinPrefab, numberOfCoins);

        // then spawn obstacles
        SpawnItems(obstaclePrefab, numberOfObstacles);
    }

    Bounds GetRoadBounds(GameObject go)
    {
        // We try renderer first because most roads have MeshRenderers
        Renderer rend = go.GetComponentInChildren<Renderer>();
        if (rend != null) return rend.bounds;

        // Fallback to collider if no renderer
        Collider col = go.GetComponentInChildren<Collider>();
        if (col != null) return col.bounds;

        // Absolute fallback so it doesn't crash
        Debug.LogWarning("No Renderer/Collider found on roadObject. Using default bounds.");
        return new Bounds(go.transform.position, new Vector3(10f, 0f, 200f));
    }

    void SpawnItems(GameObject prefab, int count)
    {
        int safety = 0; // in case we can't find enough valid spots

        for (int i = 0; i < count && safety < count * 20; i++)
        {
            safety++;

            // 1. Pick a random Z *in front* of the player
            float zPos = Random.Range(
                player.position.z + minForwardDistance,
                player.position.z + maxForwardDistance
            );

            // 2. Clamp that Z to the road bounds so we don't spawn past the mesh
            zPos = Mathf.Clamp(zPos, roadBoundsWorld.min.z, roadBoundsWorld.max.z);

            // 3. Pick a random X across the width of the road
            float xPos = Random.Range(roadBoundsWorld.min.x, roadBoundsWorld.max.x);

            // 4. Build final position
            Vector3 candidatePos = new Vector3(
                xPos,
                roadBoundsWorld.center.y + heightOffset,
                zPos
            );

            // 5. Make sure it's not too close to another item
            if (!IsPositionValid(candidatePos, minDistanceBetweenItems))
            {
                i--; // try again for this index
                continue;
            }

            usedPositions.Add(candidatePos);
            Instantiate(prefab, candidatePos, Quaternion.Euler(90f, 0f, 0f));


        }
    }

    bool IsPositionValid(Vector3 pos, float minDist)
    {
        foreach (Vector3 used in usedPositions)
        {
            if (Vector3.Distance(used, pos) < minDist)
                return false;
        }
        return true;
    }
}
