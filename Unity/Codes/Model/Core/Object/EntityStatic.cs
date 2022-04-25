
namespace ET
{
    public static class EntityStatic
    {
        public static long getInstanceId(object obj)
        { 

            return ((Entity)obj).InstanceId;
        }
    }
}
