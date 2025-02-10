using UnityEngine;
using UnityEngine.Pool;

namespace JollyRoger.Utilities
{ 
    /// <summary>
    /// Simple helper to encapsulate basic object pool behaviour for use with Unity's object pools.
    /// Only has simple create, take, return and destroy methods.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPoolHelper<T> where T : MonoBehaviour
    {
        public T prefab;
        public IObjectPool<T> pool;

        public ObjectPoolHelper(T prefab)
        {
            this.prefab = prefab;
            pool = new ObjectPool<T>(Create, Take, Return, Destroy);
        }

        public T Get() => pool.Get();
        public void Release(T item) => pool.Release(item);
        public void Clear() => pool.Clear();

		private T Create() => GameObject.Instantiate(prefab);
		private void Take(T item) => item.gameObject.gameObject.SetActive(true);
		private void Return(T item) => item.gameObject.SetActive(false);
		private void Destroy(T item) => GameObject.Destroy(item.gameObject);
    }
}