using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleType
{
    Hit,
}

public static class ParticlePool
{
    const int DEFAULT_POOL_SIZE = 3;

    private static Transform root;

    public static Transform Root
    {
        get
        {
            if (root == null)
            {
                PoolController controller = GameObject.FindObjectOfType<PoolController>();
                root = controller != null ? controller.transform : new GameObject("ParticlePool").transform;
            }

            return root;
        }
    }

    class Pool
    {
        Transform root = null;
        List<ParticleSystem> inactive;
        ParticleSystem prefab;
        int index;

        public Pool(ParticleSystem prefab, int quantity, Transform parent)
        {
            root = parent;
            this.prefab = prefab;
            inactive = new List<ParticleSystem>(quantity);

            for (int i = 0; i < quantity; i++)
            {
                ParticleSystem particle = (ParticleSystem)GameObject.Instantiate(prefab, root);
                particle.Stop();
                inactive.Add(particle);
            }
        }

        public int Count
        {
            get { return inactive.Count; }
        }

        public void Play(Vector3 pos, Quaternion rot)
        {
            index = index + 1 < inactive.Count ? index + 1 : 0;

            ParticleSystem obj = inactive[index];

            if (obj.isPlaying)
            {
                obj = (ParticleSystem)GameObject.Instantiate(prefab, root);
                obj.Stop();
                inactive.Insert(index, obj);
            }

            obj.transform.SetPositionAndRotation(pos, rot);
            obj.Play();
        }

        public void Release()
        {
            while (inactive.Count > 0)
            {
                GameObject.DestroyImmediate(inactive[0]);
                inactive.RemoveAt(0);
            }
            inactive.Clear();
        }
    }

    static Dictionary<ParticleType, ParticleSystem> shortcuts = new Dictionary<ParticleType, ParticleSystem>();
    static Dictionary<int, Pool> pools = new Dictionary<int, Pool>();

    static void Init(ParticleSystem prefab = null, int quantity = DEFAULT_POOL_SIZE, Transform parent = null)
    {
        if (prefab != null && !pools.ContainsKey(prefab.GetInstanceID()))
        {
            pools[prefab.GetInstanceID()] = new Pool(prefab, quantity, parent);
        }
    } 

    public static void Preload(ParticleSystem prefab, int quantity = 1, Transform parent = null)
    {
        Init(prefab, quantity, parent);
    }

    public static void Play(ParticleSystem prefab, Vector3 pos, Quaternion rot)
    {
        if (!pools.ContainsKey(prefab.GetInstanceID()))
        {
            Transform newRoot = new GameObject("VFX_" + prefab.name).transform;
            newRoot.SetParent(Root);
            pools[prefab.GetInstanceID()] = new Pool(prefab, 10, newRoot);
        }

        pools[prefab.GetInstanceID()].Play(pos, rot);
    }

    public static void Play(ParticleType particleType, Vector3 pos, Quaternion rot)
    {
        Play(shortcuts[particleType], pos, rot);
    }

    public static void Release(ParticleSystem prefab)
    {
        if (pools.ContainsKey(prefab.GetInstanceID()))
        {
            pools[prefab.GetInstanceID()].Release();
            pools.Remove(prefab.GetInstanceID());
        }
        else
        {
            GameObject.DestroyImmediate(prefab);
        }
    }

    public static void Release(ParticleType particleType)
    {
        Release(shortcuts[particleType]);
    }

    public static void Shortcut(ParticleType particleType, ParticleSystem particleSystem)
    {
        shortcuts.Add(particleType, particleSystem);
    }
}
