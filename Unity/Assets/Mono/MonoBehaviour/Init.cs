using System.Collections;
using System.Threading;
using UnityEngine;
using Sirenix;

namespace ET
{
	// 1 mono模式 2 ILRuntime模式 3 mono热重载模式
	public enum CodeMode
	{
		Mono = 1,
		ILRuntime = 2,
		Reload = 3,
	}

	// "develop mode, 0正式 1开发 2压测"
	public enum DevelopMode
	{
		Release = 0,
		Dev = 1,
		PressureTest = 2,
	}

	//	log level
	public enum LogLevelMode
	{
		All = 0,
		TraceLevel = 1,
        DebugLevel = 2,       //  调试
        InfoLevel = 3,        //  一般信息
        ProtoLevel = 4,       //  协议
        WarningLevel = 5,     //  警告
        ErrorLevel = 6,       //  错误
        VitalLevel = 7,       //  重要流程
        FatalLevel = 8,       //  致命错误
	}
	
	public class Init : MonoBehaviour
	{
		public CodeMode CodeMode = CodeMode.Mono;
		public DevelopMode DevelopMode = DevelopMode.Dev;
		public LogLevelMode LogLevelMode = LogLevelMode.All;
		
		private void Awake()
		{
#if ENABLE_IL2CPP
			this.CodeMode = CodeMode.ILRuntime;
#endif
			
			System.AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				Log.Error(e.ExceptionObject.ToString());
			};
			
			SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);
			
			DontDestroyOnLoad(gameObject);
			
			LitJson.UnityTypeBindings.Register();

			ETTask.ExceptionHandler += Log.Error;

			Log.ILog = new UnityLogger();

			Options.Instance = new Options();

			CodeLoader.Instance.CodeMode = this.CodeMode;
			Options.Instance.Develop = (int)this.DevelopMode;	//设置 模式
			Options.Instance.LogLevel = (int)this.LogLevelMode;	//设置日志等级

			Log.Vital($"init CodeMode:  {this.CodeMode.ToString()}; \tDevelopMode:  {this.DevelopMode.ToString()}; \tLogLevelMode:  {this.LogLevelMode.ToString()}");
		}

		private void Start()
		{
			
			CodeLoader.Instance.Start();

			StartCoroutine(StartInit());
		}

		private IEnumerator StartInit()
		{
			RuntimeManager.Instance.StartInit();

			yield return null;
		}

		private void Update()
		{
			CodeLoader.Instance.Update();
		}

		private void LateUpdate()
		{
			CodeLoader.Instance.LateUpdate();
		}

		private void OnApplicationQuit()
		{
			CodeLoader.Instance.OnApplicationQuit();
			CodeLoader.Instance.Dispose();
		}
	}
}