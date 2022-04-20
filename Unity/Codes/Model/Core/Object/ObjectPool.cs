using System;
using System.Collections.Generic;

namespace ET
{
    public class ObjectPool: IDisposable
    {
        private readonly Dictionary<Type, Queue<Entity>> pool = new Dictionary<Type, Queue<Entity>>();
        
        public static ObjectPool Instance = new ObjectPool();
        
        private ObjectPool()
        {
        }

        public Entity Fetch(Type type)
        {
            Queue<Entity> queue = null;
            if (!pool.TryGetValue(type, out queue))
            {
                return Activator.CreateInstance(type) as Entity;
            }

            if (queue.Count == 0)
            {
                return Activator.CreateInstance(type) as Entity;
            }
            return queue.Dequeue();
        }

        public void Recycle(Entity obj)
        {
            Type type = obj.GetType();
            Queue<Entity> queue = null;
            if (!pool.TryGetValue(type, out queue))
            {
                queue = new Queue<Entity>();
                pool.Add(type, queue);
            }
            queue.Enqueue(obj);
        }

        public void Dispose()
        {
            this.pool.Clear();
        }

        /// <summary>
        /// 获取对象池数量。
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                foreach(var q in this.pool.Values) {
                    count += q.Count;
                }
                return count;
            }
        }

        /// <summary>
        /// 获取所有对象池。
        /// </summary>
        /// <param name="sort">是否根据对象池的优先级排序。</param>
        /// <returns>所有对象池。</returns>
        public Entity[] GetAllObjectPools(bool sort)
        {
            Entity[] arr = new Entity[this.Count];

            int count = 0;
            foreach(var q in this.pool.Values) 
            {
                foreach(var e in q) 
                {
                    arr[count] = e;
                    ++count;
                }
            }

            return arr;
        }
    }
}