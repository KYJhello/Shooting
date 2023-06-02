using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolPractice
{
    //public class ObjectPool : MonoBehaviour
    //{
    //    [SerializeField] Poolable poolablePrefab;

    //    [SerializeField] int poolSize;
    //    [SerializeField] int maxSize;

    //    private Stack<Poolable> objectPool = new Stack<Poolable>();

    //    private void Awake()
    //    {
    //        CreatePool();
    //    }
    //    private void CreatePool()
    //    {
    //        for (int i = 0; i < poolSize; i++)
    //        {
    //            Poolable poolable = Instantiate(poolablePrefab);
    //            poolable.gameObject.SetActive(false);
    //            poolable.transform.SetParent(transform);
    //            poolable.Pool = this;
    //            objectPool.Push(poolable);
    //        }
    //    }

    //    public Poolable Get()
    //    {
    //        // 대여
    //        if (objectPool.Count > 0)
    //        {
    //            Poolable poolable = objectPool.Pop();
    //            poolable.gameObject.SetActive(true);
    //            poolable.transform.parent = null;
    //            return poolable;

    //        }
    //        // 대여할곳에 남아있는게 없는 경우
    //        else
    //        {
    //            Poolable poolable = Instantiate(poolablePrefab);
    //            poolable.Pool = this;
    //            return poolable;
    //        }
    //    }
    //    public void Release(Poolable poolable)
    //    {
    //        // 반납 최대량 안넘으면 반납
    //        if (objectPool.Count < maxSize)
    //        {
    //            poolable.gameObject.SetActive(false);
    //            poolable.transform.SetParent(transform);
    //            objectPool.Push(poolable);
    //        }
    //        // 넘으면 삭제
    //        else
    //        {
    //            Destroy(poolable.gameObject);
    //        }

    //    }
    //}
}