using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
    // 객체별 풀을 만들어 찾기 쉽게 
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

    // GameObject 아니면 컴퍼넌트만 받기
    public T Get<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            // prefab의 이름을 key로 사용
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
                // 컴포넌트가 부착되어있는 게임오브젝트가 반환
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
    // 여러 버전의 Get을 만들어 주는게 좋다
    //public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    //{
    // prefab의 이름을 key로 사용
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

        //// prefab의 이름을 key로 사용
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
            // prefab의 이름을 key로 사용
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
        // 유니티용 ObjectPool 생성자의 매개변수에는 4가지의 함수가 필요함
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            // 만들어질때 작업
            createFunc: () =>
            {
                GameObject go = Instantiate(prefab);
                go.name = key;
                return go;
            },
            // 가져올때 작업
            actionOnGet: (GameObject go) =>
            {
                go.SetActive(true);
                // 게임신 기준으로 생성을 위한 
                go.transform.SetParent(null);
            },
            // 반납할때 작업
            actionOnRelease: (GameObject go) =>
            {
                go.SetActive(false);
                go.transform.SetParent(transform);
            },
            // 삭제할때 작업
            actionOnDestroy: (GameObject go) =>
            {
                Destroy(go);
            }
            );
        poolDic.Add(key, pool);
    }
}
