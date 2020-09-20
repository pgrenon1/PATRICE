using UnityEngine;

public static class Utility
{
    public static void SpawnVFX(GameObject prefab, Vector3 position, Quaternion rotation, float scale, float destroyTimer = 2f, Transform parent = null)
    {
        GameObject vfx = GameObject.Instantiate(prefab, position, rotation, parent);
        vfx.transform.localScale *= scale;

        GameObject.Destroy(vfx, destroyTimer);
    }
}

public static class Extensions
{
    public static bool IsPlayer(this Component component)
    {
        return component.GetComponentInParent<Player>();
    }

    public static bool TryDamage(this Collider collider, float damage)
    {
        Damageable damageable = collider.GetComponentInParent<Damageable>();

        if (damageable != null)
        {
            damageable.ApplyDamage(damage);
            return true;
        }

        return false;
    }
}