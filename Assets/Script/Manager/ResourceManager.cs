using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    Dictionary<string , Object> reources = new Dictionary<string , Object>();

    public T Load<T>(string path) where T : Object
    {
        // Ÿ�԰� ��η� Ű�� ����
        string key = $"{typeof(T)}.{path}";

        // as�� ���̳��� ĳ��Ʈ, ����ƽ ĳ��Ʈ ����Ϸ��� (T) ����ȯ ���
        if (reources.ContainsKey(key)) { return reources[key] as T;}

        T resource = Resources.Load<T>(path);
        reources.Add(key, resource);
        return resource;
    }
    public T Instantiate<T>(T original, Vector3 position, Quaternion rotation,Transform parent, bool pooling = false) where T : Object
    {
        // Ǯ���Ǿ�������s
        if (pooling)
        {
            return GameManager.Pool.Get(original, position, rotation, parent);
        }
        else
        {
            return Object.Instantiate(original, position, rotation, parent);
        }
    }
    public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, bool pooling = false) where T : Object
    {
        return Instantiate<T>(original, position, rotation, null, pooling);
    }

    public new T Instantiate<T>(T original, Transform parent, bool pooling = false) where T : Object
    {
        return Instantiate<T>(original, Vector3.zero, Quaternion.identity, parent, pooling);
    }

    public T Instantiate<T>(T original, bool pooling = false) where T : Object
    {
        return Instantiate<T>(original, Vector3.zero, Quaternion.identity, null, pooling);
    }
    public T Instantiate<T>(string path, Vector3 position, Quaternion rotation, Transform parent, bool pooling) where T : Object
    {
        T original = Load<T>(path);
        return Instantiate<T>(original, position, rotation, parent, pooling);
    }
    public T Instantiate<T>(string path, Vector3 position, Quaternion rotation, bool pooling = false) where T : Object
    {
        return Instantiate<T>(path, position, rotation, null, pooling);
    }

    public T Instantiate<T>(string path, Transform parent, bool pooling = false) where T : Object
    {
        return Instantiate<T>(path, Vector3.zero, Quaternion.identity, parent, pooling);
    }

    public T Instantiate<T>(string path, bool pooling = false) where T : Object
    {
        return Instantiate<T>(path, Vector3.zero, Quaternion.identity, null, pooling);
    }


    public void Destroy(GameObject go)
    {
        if (GameManager.Pool.Release(go)) { return; }
        GameObject.Destroy(go);
    }
    public void Destroy(GameObject go, float delay)
    {
        if (GameManager.Pool.IsContain(go))
            StartCoroutine(DelayReleaseRoutine(go, delay));
        else
            GameObject.Destroy(go, delay);
    }

    IEnumerator DelayReleaseRoutine(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Pool.Release(go);
    }

    public void Destroy(Component component, float delay = 0f)
    {
        Component.Destroy(component, delay);
    }
}
