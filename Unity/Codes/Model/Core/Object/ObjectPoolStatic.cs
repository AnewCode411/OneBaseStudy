using System;
using System.Collections.Generic;
namespace ET
{
    public static class ObjectPoolStatic
    {

        public static int getCount() {      //  对象池数量

            return ObjectPool.Instance.Pool().Count;
        }

        public static Type getPoolTypeByIndex(object index) {


            int i = int.Parse(index.ToString());
            int count = 0;
            foreach(var key in ObjectPool.Instance.Pool().Keys) {

                if (count == i)
                {
                    return key;
                }
                ++count;
            }

            return null;
        }


        public static int getTypeCount(object type) {

            int len = 0;

            Queue<Entity> queue = null;
            if (ObjectPool.Instance.Pool().TryGetValue((Type)type, out queue)) {
                len = queue.Count;
            }

            return len;
        }

        public static Entity getEntityByIndex(object type, object index) {


            Queue<Entity> queue = null;
            if (ObjectPool.Instance.Pool().TryGetValue((Type)type, out queue)) {
                
                int v = (int)index;
                int i = 0;
                foreach(var e in queue) {

                    if (v == i)
                        return e;

                    ++i;
                }
            }

            return null;
        }
    }
}
