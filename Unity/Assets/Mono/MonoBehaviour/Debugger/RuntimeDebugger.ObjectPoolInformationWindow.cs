//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.Reflection;
using UnityEngine;

namespace ET
{
    public sealed partial class RuntimeDebugger : MonoSingleton<RuntimeDebugger>
    {

        private sealed class ObjectPoolInformationWindow : ScrollableDebuggerWindowBase
        {
            
            private Assembly m_Assembly = null;

            private string m_ObjectPoolClassNamePath = "ET.ObjectPoolStatic";
            private string m_EntityClassNamePath = "ET.EntityStatic";


            ////        ObjectPool
            private IStaticMethod m_getCount = null;
            private IStaticMethod m_getPoolTypeByIndex = null;
            private IStaticMethod m_getTypeCount = null;
            private IStaticMethod m_getEntityByIndex = null;


            ////        Entity
            private IStaticMethod m_getInstanceId = null;

            public override void Initialize(params object[] args)
            {
                
                m_Assembly = CodeLoader.Instance.assembly;

                ////        ObjectPool
                m_getCount = new MonoStaticMethod(m_Assembly, m_ObjectPoolClassNamePath, "getCount");
                m_getPoolTypeByIndex = new MonoStaticMethod(m_Assembly, m_ObjectPoolClassNamePath, "getPoolTypeByIndex");
                m_getTypeCount = new MonoStaticMethod(m_Assembly, m_ObjectPoolClassNamePath, "getTypeCount");
                m_getEntityByIndex = new MonoStaticMethod(m_Assembly, m_ObjectPoolClassNamePath, "getEntityByIndex");

                ////        Entity
                m_getInstanceId = new MonoStaticMethod(m_Assembly, m_EntityClassNamePath, "getInstanceId");
            }

            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Object Pool Information</b>");
                int allCount = (int)m_getCount.Get();
                GUILayout.BeginVertical("box");
                {
                    
                    DrawItem("Object Pool Count", allCount.ToString());
                }
                GUILayout.EndVertical();

                for (int i = 0; i < allCount; i++)
                {
                    DrawObjectPool(i);
                }
            }

            private void DrawObjectPool(int index)
            {
                Type ObjectPoolType = m_getPoolTypeByIndex.Get(index).GetType();
                GUILayout.Label(Utility.Text.Format("<b>Object Pool: {0}</b>", ObjectPoolType.FullName));
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Name", ObjectPoolType.Name);
                    //DrawItem("Type", EntityType.ObjectType.FullName);
                    //DrawItem("Auto Release Interval", EntityType.AutoReleaseInterval.ToString());
                    //DrawItem("Capacity", EntityType.Capacity.ToString());
                    //DrawItem("Used Count", EntityType.Count.ToString());
                    //DrawItem("Can Release Count", EntityType.CanReleaseCount.ToString());
                    //DrawItem("Expire Time", EntityType.ExpireTime.ToString());
                    //DrawItem("Priority", EntityType.Priority.ToString());
                    //ObjectInfo[] objectInfos = objectPool.GetAllObjectInfos();
                    // GUILayout.BeginHorizontal();
                    // {
                    //     GUILayout.Label("<b>Name</b>");
                    //     GUILayout.Label("<b>Locked</b>", GUILayout.Width(60f));
                    //     GUILayout.Label(EntityType.AllowMultiSpawn ? "<b>Count</b>" : "<b>In Use</b>", GUILayout.Width(60f));
                    //     GUILayout.Label("<b>Flag</b>", GUILayout.Width(60f));
                    //     GUILayout.Label("<b>Priority</b>", GUILayout.Width(60f));
                    //     GUILayout.Label("<b>Last Use Time</b>", GUILayout.Width(120f));
                    // }
                    // GUILayout.EndHorizontal();

                    int childCount = (int)m_getTypeCount.Get(ObjectPoolType);
                    DrawItem("Count", childCount.ToString());

                    if (childCount > 0)
                    {
                        for (int i = 0; i < childCount; i++)
                        {
                            GUILayout.BeginHorizontal();
                            {
                                object entity = m_getEntityByIndex.Get(ObjectPoolType, i);
                                GUILayout.Label($"InstanceId: {(long)m_getInstanceId.Get(entity)}");
                                // GUILayout.Label(string.IsNullOrEmpty(objectInfos[i].Name) ? "<None>" : objectInfos[i].Name);
                                // GUILayout.Label(objectInfos[i].Locked.ToString(), GUILayout.Width(60f));
                                // GUILayout.Label(objectPool.AllowMultiSpawn ? objectInfos[i].SpawnCount.ToString() : objectInfos[i].IsInUse.ToString(), GUILayout.Width(60f));
                                // GUILayout.Label(objectInfos[i].CustomCanReleaseFlag.ToString(), GUILayout.Width(60f));
                                // GUILayout.Label(objectInfos[i].Priority.ToString(), GUILayout.Width(60f));
                                // GUILayout.Label(objectInfos[i].LastUseTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"), GUILayout.Width(120f));
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    else
                    {
                        GUILayout.Label("<i>Object Pool is Empty ...</i>");
                    }
                }
                GUILayout.EndVertical();
            }
        }
    }
}
