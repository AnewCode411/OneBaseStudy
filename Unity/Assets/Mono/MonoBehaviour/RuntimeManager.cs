using UnityEngine;

namespace ET
{

    public class RuntimeManager : MonoSingleton<RuntimeManager>
    {
        [Tooltip("游戏启动相关配置")]
        public RuntimeConfig runtimeConfig;

        [Tooltip("日志文件，会保存错误和异常日志到本地")]
        public RuntimeLog runtimeLog;

        public void StartInit()
        {
            Log.Vital($"RuntimeManager init");
            
            runtimeConfig = RuntimeConfig.Instance;
            runtimeLog = RuntimeLog.Instance;

            if (runtimeConfig.hasDebugger) {
                RuntimeDebuger xx = RuntimeDebuger.Instance;
            }

            Log.Vital($"RuntimeManager init: {runtimeConfig.apkType}");
        }
    }
}

