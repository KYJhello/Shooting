using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
    // ��ü�� Ǯ�� ����� ã�� ���� 
    Dictionary<string, ObjectPool<GameObject>> poolDic;

    private void Awake()
    {
        poolDic = new Dictionary<string, ObjectPool<GameObject>>();
    }

    public bool IsContain<T>(T original)where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
                return false;

        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    // GameObject �ƴϸ� ���۳�Ʈ�� �ޱ�
    public T Get<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            // prefab�� �̸��� key�� ���
            if (!poolDic.ContainsKey(prefab.name))
            {
                CreatePool(prefab.name, prefab);
            }

            ObjectPool<GameObject> pool = poolDic[prefab.name];
            GameObject go = pool.Get();
            go.transform.parent = parent;
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go as T;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
            {
                // ������Ʈ�� �����Ǿ��ִ� ���ӿ�����Ʈ�� ��ȯ
                CreatePool(key, component.gameObject);
            }
            GameObject go = poolDic[key].Get();
            go.transform.parent = parent;
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }
    // ���� ������ Get�� ����� �ִ°� ����
    //public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    //{
    // prefab�� �̸��� key�� ���
    //        if (!poolDic.ContainsKey(prefab.name))
    //        {
    //            CreatePool(prefab.name, prefab);
    //}

    //ObjectPool<GameObject> pool = poolDic[prefab.name];
    //GameObject go = pool.Get();
    //go.transform.position = position;
    //go.transform.rotation = rotation;
    //return go;
    //}
    public T Get<T>(T original, Vector3 position, Quaternion rotation) where T : Object
    {
        return Get<T>(original, position, rotation, null);
    }

    public T Get<T>(T original, Transform parent) where T : Object
    {
        return Get<T>(original, Vector3.zero, Quaternion.identity, parent);
    }

    public T Get<T>(T original) where T : Object
    {
        return Get(original, Vector3.zero, Quaternion.identity, null);

        //// prefab�� �̸��� key�� ���
        //if (!poolDic.ContainsKey(prefab.name))
        //{
        //    CreatePool(prefab.name, prefab);
        //}

        //ObjectPool<GameObject> pool = poolDic[prefab.name];
        //return pool.Get();
    }
    public bool Release<T>(T instance) where T : Object
    {
        if (instance is GameObject)
        {
            GameObject go = instance as GameObject;
            string key = go.name;
            // prefab�� �̸��� key�� ���
            if (!poolDic.ContainsKey(key))
            {
                return false;
            }
            poolDic[key].Release(go);
            return true;
        }
        else if (instance is Component)
        {
            Component component = instance as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
            {
                return false;
            }
            poolDic[key].Release(component.gameObject);
            return true;
        }
        else
        {
            return false;
        }
        //if(!poolDic.ContainsKey(go.name))
        //{
        //    return false;
        //}

        //ObjectPool<GameObject> pool = poolDic[go.name];
        //pool.Release(go);
        //return true;
    }
    private void CreatePool(string key, GameObject prefab)
    {
        // ����Ƽ�� ObjectPool �������� �Ű��������� 4������ �Լ��� �ʿ���
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            // ��������� �۾�
            createFunc: () =>
            {
                GameObject go = Instantiate(prefab);
                go.name = key;
                return go;
            },
            // �����ö� �۾�
            actionOnGet: (GameObject go) =>
            {
                go.SetActive(true);
                // ���ӽ� �������� ������ ���� 
                go.transform.SetParent(null);
            },
            // �ݳ��Ҷ� �۾�
            actionOnRelease: (GameObject go) =>
            {
                go.SetActive(false);
                go.transform.SetParent(transform);
            },
            // �����Ҷ� �۾�
            actionOnDestroy: (GameObject go) =>
            {
                Destroy(go);
            }
            );
        poolDic.Add(key, pool);
    }
}
