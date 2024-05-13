using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Default = 0,
    Coin,

}

/*
 * Object want to use Pool need to implement GameUnit
 */
public static class SimplePool
{
    static Dictionary<PoolType, Pool> keyPools = new Dictionary<PoolType, Pool>();

    public static void Preload(GameUnit gameUnit, Transform parent, int amount)
    {
        if (!keyPools.ContainsKey(gameUnit.poolType))
        {
            Pool pool = new Pool();
            pool.Preload(gameUnit, parent, amount);
            keyPools.Add(gameUnit.poolType, pool);
        }
    }

    public static GameUnit Spawn(PoolType poolType, Vector3 pos, Quaternion rot)
    {
        GameUnit gameUnit = null;

        if (keyPools.ContainsKey(poolType))
        {
            gameUnit = keyPools[poolType].Spawn();
        }
        else
        {
            Debug.LogError("Pools doesn't contain poolType!");
        }

        gameUnit.tf.position = pos;
        gameUnit.tf.rotation = rot;

        return gameUnit;
    }

    public static void Despawn(GameUnit gameUnit)
    {
        keyPools[gameUnit.poolType].Despawn(gameUnit);
    }

    public static void CollectAll()
    {
        foreach (var item in keyPools)
        {
            item.Value.Collect();
        }
    }

    public class Pool
    {
        List<GameUnit> gameUnits = new List<GameUnit>();
        List<GameUnit> activeUnits = new List<GameUnit>();
        GameUnit unitPrefab;
        Transform parent;

        public void Preload(GameUnit gameUnit, Transform parent, int amount)
        {
            unitPrefab = gameUnit;
            this.parent = parent;

            for (int i = 0; i < amount; i++)
            {
                GameUnit unit = GameObject.Instantiate(gameUnit, parent);
                gameUnits.Add(unit);
                unit.gameObject.SetActive(false);
            }
        }

        public GameUnit Spawn()
        {
            GameUnit unit = null;
            if (gameUnits.Count > 0)
            {
                unit = gameUnits[0];
                gameUnits.RemoveAt(0);
            }
            else
            {
                unit = GameObject.Instantiate(unitPrefab, parent);
            }
            unit.gameObject.SetActive(true);
            activeUnits.Add(unit);

            return unit;
        }

        public void Despawn(GameUnit gameUnit)
        {
            gameUnit.gameObject.SetActive(false);
            gameUnits.Add(gameUnit);
            activeUnits.Remove(gameUnit);
        }

        public void Collect()
        {
            for (int i = 0; i < activeUnits.Count; i++)
            {
                Despawn(activeUnits[i]);
            }
        }
    }
}
