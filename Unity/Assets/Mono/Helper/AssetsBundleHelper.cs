using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ET
{
    public static class AssetsBundleHelper
    {
        public const string AB_EDITOR_DIR = "Res_AB";
        public const string PATCH = "patch";
        public const string EXTRA = "extra";
        public const string USERDATA = "userdata";
        public static string assetBundlePlatform;
        public static string protocal;
        public static string abDataPath;
        public static string patchDataPath;
        public static string extraDataPath;
        public static string userDataPath;


        public static Dictionary<string, UnityEngine.Object> LoadBundle(string assetBundleName)
        {
            assetBundleName = assetBundleName.ToLower();

            Dictionary<string, UnityEngine.Object> objects = new Dictionary<string, UnityEngine.Object>();
            if (!Define.IsAsync)
            {
                if (Define.IsEditor)
                {
                    string[] realPath = null;
                    realPath = Define.GetAssetPathsFromAssetBundle(assetBundleName);
                    foreach (string s in realPath)
                    {
                        //string assetName = Path.GetFileNameWithoutExtension(s);
                        UnityEngine.Object resource = Define.LoadAssetAtPath(s);
                        objects.Add(resource.name, resource);
                    }
                }
                return objects;
            }

            string p = Path.Combine(PathHelper.AppHotfixResPath, assetBundleName);
            UnityEngine.AssetBundle assetBundle = null;
            if (File.Exists(p))
            {
                assetBundle = UnityEngine.AssetBundle.LoadFromFile(p);
            }
            else
            {
                p = Path.Combine(PathHelper.AppResPath, assetBundleName);
                assetBundle = UnityEngine.AssetBundle.LoadFromFile(p);
            }

            if (assetBundle == null)
            {
                // 获取资源的时候会抛异常，这个地方不直接抛异常，因为有些地方需要Load之后判断是否Load成功
                Log.Warning($"assets bundle not found: {assetBundleName}");
                return objects;
            }

            UnityEngine.Object[] assets = assetBundle.LoadAllAssets();
            foreach (UnityEngine.Object asset in assets)
            {
                objects.Add(asset.name, asset);
            }
            return objects;
        }

        public static void Init()
        {
            var platform = Application.platform;
            var isAndroid = false;
#if UNITY_ANDROID
            isAndroid = true;
#endif
            if (platform == RuntimePlatform.Android)
            {
                assetBundlePlatform = "Android";
                protocal = "";
                abDataPath = string.Format("{0}!assets/", Application.dataPath);
                patchDataPath = string.Format("{0}/{1}/", Application.persistentDataPath, PATCH);
                extraDataPath = string.Format("{0}/{1}/", Application.persistentDataPath, EXTRA);
                userDataPath = string.Format("{0}/{1}/", Application.persistentDataPath, USERDATA);
            }
            else if (platform == RuntimePlatform.IPhonePlayer)
            {
                assetBundlePlatform = "iOS";
                protocal = "file://";
                abDataPath = string.Format("{0}/", Application.streamingAssetsPath);
                patchDataPath = string.Format("{0}/{1}/", Application.persistentDataPath, PATCH);
                extraDataPath = string.Format("{0}/{1}/", Application.persistentDataPath, EXTRA);
                userDataPath = string.Format("{0}/{1}/", Application.persistentDataPath, USERDATA);
            }
            else if (platform == RuntimePlatform.WindowsPlayer)
            {
                assetBundlePlatform = "StandaloneWindows64";
                protocal = "file:///";
                abDataPath = string.Format("{0}/", Application.streamingAssetsPath);
                patchDataPath = string.Format("{0}/{1}/", Application.dataPath, PATCH);
                extraDataPath = string.Format("{0}/{1}/", Application.dataPath, EXTRA);
                userDataPath = string.Format("{0}/{1}/", Application.dataPath, USERDATA);
            }
            else if (platform == RuntimePlatform.WindowsEditor)
            {
                assetBundlePlatform = isAndroid ? "Android" : "StandaloneWindows64";
                protocal = "file:///";
                abDataPath = string.Format("{0}/../{1}/{2}/", Application.dataPath, AB_EDITOR_DIR, assetBundlePlatform);
                patchDataPath = string.Format("{0}/../{1}/", Application.dataPath, PATCH);
                extraDataPath = string.Format("{0}/../{1}/", Application.dataPath, EXTRA);
                userDataPath = string.Format("{0}/../{1}/", Application.dataPath, USERDATA);
            }
            else if (platform == RuntimePlatform.OSXPlayer)
            {
                assetBundlePlatform = "StandaloneOSXUniversal";
                protocal = "file:///";
                abDataPath = string.Format("{0}/", Application.streamingAssetsPath);
                patchDataPath = string.Format("{0}/{1}/", Application.dataPath, PATCH);
                extraDataPath = string.Format("{0}/{1}/", Application.dataPath, EXTRA);
                userDataPath = string.Format("{0}/{1}/", Application.dataPath, USERDATA);
            }
            else if (platform == RuntimePlatform.OSXEditor)
            {
                assetBundlePlatform = isAndroid ? "Android" : "StandaloneOSX";
                protocal = "file://";
                abDataPath = string.Format("{0}/../{1}/{2}/", Application.dataPath, AB_EDITOR_DIR, assetBundlePlatform);
                patchDataPath = string.Format("{0}/../{1}/", Application.dataPath, PATCH);
                extraDataPath = string.Format("{0}/../{1}/", Application.dataPath, EXTRA);
                userDataPath = string.Format("{0}/../{1}/", Application.dataPath, USERDATA);
            }
            else
            {
                assetBundlePlatform = "";
                protocal = "";
                abDataPath = "";
                patchDataPath = "";
                extraDataPath = "";
                userDataPath = "";
            }
            
            FileHelper.CheckAndCreateDir(patchDataPath);
            FileHelper.CheckAndCreateDir(extraDataPath);
            FileHelper.CheckAndCreateDir(userDataPath);
            
        }
    }
}