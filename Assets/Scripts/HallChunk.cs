using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallChunk : MonoBehaviour
{
    [Range (0, 1)]
    public float objectSpawnChance;
    public MeshFilter leftDoor, rightDoor;
    public Mesh wallMesh, doorMesh;
    public Transform coworkerSpawnLeft, coworkerSpawnRight;
    public GameObject coworker;
    public RandomObject leftDoorObjects, rightDoorObjects, leftWallObjects, rightWallObjects;

    private bool doorIsLeft;

    public HallChunkConfig Generate ()
    {
        HallChunkConfig config = new HallChunkConfig (this);
        doorIsLeft = config.doorIsLeft;

        if (doorIsLeft)
        {
            leftDoor.mesh = doorMesh;
            rightDoor.mesh = wallMesh;

            config.doorObject = Random.value < objectSpawnChance ? leftDoorObjects.Generate () : -1;
            config.wallObject = Random.value < objectSpawnChance ? rightWallObjects.Generate () : -1;
        }
        else
        {
            leftDoor.mesh = wallMesh;
            rightDoor.mesh = doorMesh;

            config.doorObject = Random.value < objectSpawnChance ? rightDoorObjects.Generate () : -1;
            config.wallObject = Random.value < objectSpawnChance ? leftWallObjects.Generate () : -1;
        }

        return config;
    }

    public void Generate (HallChunkConfig config)
    {
        config.associatedChunk = this;
        doorIsLeft = config.doorIsLeft;

        if (doorIsLeft)
        {
            leftDoor.mesh = doorMesh;
            rightDoor.mesh = wallMesh;

            if (config.doorObject >= 0)
                leftDoorObjects.Generate (config.doorObject);
            if (config.wallObject >= 0)
                rightWallObjects.Generate (config.wallObject);
        }
        else
        {
            leftDoor.mesh = wallMesh;
            rightDoor.mesh = doorMesh;

            if (config.doorObject >= 0)
                rightDoorObjects.Generate (config.doorObject);
            if (config.wallObject >= 0)
                leftWallObjects.Generate (config.wallObject);
        }
    }

    public void SpawnCoworker ()
    {
        Instantiate (coworker, doorIsLeft ? coworkerSpawnLeft : coworkerSpawnRight);
    }
}

public class HallChunkConfig
{
    public HallChunk associatedChunk;
    public bool doorIsLeft;
    public int doorObject, wallObject;

    public HallChunkConfig (HallChunk associatedChunk)
    {
        this.associatedChunk = associatedChunk;
        doorIsLeft = Random.value < 0.5f;
    }
}
