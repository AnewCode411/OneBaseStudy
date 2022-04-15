using System.Collections.Generic;
using UnityEngine;


namespace ET
{
    public class RuntimeConfig: MonoBehaviour
    {
        [Tooltip("包类型, 1000: Debug, 2000: Release, 3000:Online")]
        public int apkType = 1000;

        [Tooltip("代码是否使用Dll模式运行, false时表示用本地代码")]
        public bool isDllCode = false;
        [Tooltip("资源是否用AB模式运行, false时表示用本地代码")]
        public bool isABRes = false;
        [Tooltip("配置数据是否用AB模式运行, false时表示用本地代码")]
        public bool isABCfg = false;

        [Tooltip("配置是否直接在指定场景中直接运行游戏")]
        public bool runScene = false;

        [Tooltip("不显示Console")]
        public bool noConsole = false;

        [Tooltip("是否分大小包标志, true时表示整包")]
        public bool fullPackage = true;
        [Tooltip("语言模式")]
        public string langue = "zh";

        private void Awake()
		{
            
        }

        private void Start()
        {

        }

    }
}
