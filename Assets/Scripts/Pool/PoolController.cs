using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Need a object hold PoolController script
 */
public class PoolController : MonoBehaviour
{
    [SerializeField] PoolAmount[] prePools;
    [SerializeField] ParticleAmount[] preParticles;

    void Start()
    {
        for (int i = 0; i < prePools.Length; i++)
        {
            SimplePool.Preload(prePools[i].gameUnit, prePools[i].parent, prePools[i].amount);
        }
        
        for (int i = 0; i < preParticles.Length; i++)
        {
            ParticlePool.Preload(preParticles[i].prefab, preParticles[i].amount, preParticles[i].root);
            ParticlePool.Shortcut(preParticles[i].particleType, preParticles[i].prefab);
        }
    }
}

[System.Serializable]
public class PoolAmount
{
    public GameUnit gameUnit;
    public Transform parent;
    public int amount;
}

[System.Serializable]
public class ParticleAmount
{
    public Transform root;
    public ParticleType particleType;
    public ParticleSystem prefab;
    public int amount;
}
