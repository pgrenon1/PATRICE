using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : SingletonMonoBehaviour<BoidManager> 
{
    const int threadGroupSize = 1024;

    public ComputeShader compute;

    public Dictionary<EnemyType, List<Boid>> Boids { get; set; } = new Dictionary<EnemyType, List<Boid>>();

    public void RegisterBoid(Boid boid, EnemyType enemyType, Transform target = null)
    {
        BoidSettings boidSettings = EnemyManager.Instance.enemySettings[enemyType].boidSettings;

        boid.Initialize(boidSettings, target);
    }

    //private void Start () {
    //    boids = FindObjectsOfType<Boid>();
    //    foreach (Boid b in boids)
    //    {
    //        b.Initialize(settings, FindObjectOfType<Player>().transform);
    //    }
    //}

    private void Update () 
    {
        if (Boids != null)
        {
            foreach (var enemyType in Boids.Keys)
            {
                UpdateBoids(enemyType);
            }
        }
    }

    private void UpdateBoids(EnemyType enemyType)
    {
        var settings = EnemyManager.Instance.enemySettings[enemyType].boidSettings;
        var boids = Boids[enemyType];

        int numBoids = boids.Count;
        var boidData = new BoidData[numBoids];

        for (int i = 0; i < boids.Count; i++)
        {
            boidData[i].position = boids[i].position;
            boidData[i].direction = boids[i].forward;
        }

        var boidBuffer = new ComputeBuffer(numBoids, BoidData.Size);
        boidBuffer.SetData(boidData);

        compute.SetBuffer(0, "boids", boidBuffer);
        compute.SetInt("numBoids", boids.Count);
        compute.SetFloat("viewRadius", settings.perceptionRadius);
        compute.SetFloat("avoidRadius", settings.avoidanceRadius);

        int threadGroups = Mathf.CeilToInt(numBoids / (float)threadGroupSize);
        compute.Dispatch(0, threadGroups, 1, 1);

        boidBuffer.GetData(boidData);

        for (int i = 0; i < boids.Count; i++)
        {
            boids[i].avgFlockHeading = boidData[i].flockHeading;
            boids[i].centreOfFlockmates = boidData[i].flockCentre;
            boids[i].avgAvoidanceHeading = boidData[i].avoidanceHeading;
            boids[i].numPerceivedFlockmates = boidData[i].numFlockmates;

            boids[i].UpdateBoid();
        }

        boidBuffer.Release();
        
    }

    public struct BoidData {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 flockHeading;
        public Vector3 flockCentre;
        public Vector3 avoidanceHeading;
        public int numFlockmates;

        public static int Size {
            get {
                return sizeof (float) * 3 * 5 + sizeof (int);
            }
        }
    }
}