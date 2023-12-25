using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Threading.Channels;

using Bot.Core;
using Bot.OKEXApi;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using ZLogger;


var builder = Host.CreateApplicationBuilder(args);

builder.Environment.ContentRootPath = Directory.GetCurrentDirectory();
builder.Configuration.AddJsonFile("settings.json", false, true);
builder.Configuration.AddEnvironmentVariables("BOT_");
builder.Configuration.AddCommandLine(args);

// builder.Services.AddSingleton<FollowerUser>(sp => {
// 	// var config = sp.GetRequiredService<IOptions<Configuration>>().Value;
// 	var config = sp.GetService<IConfiguration>();
// 	var AppKey = config!.GetValue<string>(StartupConfiguration.FollowerAppKey);
// 	var Secret = config!.GetValue<string>(StartupConfiguration.FollowerSecret);
// 	var Passphrase = config!.GetValue<string>(StartupConfiguration.FollowerPassphrase);
	
// 	if (AppKey is null || Secret is null || Passphrase is null) {
// 		throw new ArgumentNullException("请检查跟单账户 APIKEY, SECRET, PASSPHRASE 等信息是否传入!");
// 	}

// 	return new(AppKey, Secret, Passphrase);
// });

builder.Services.Configure<HostOptions>(config => {
		// 并行启动并行停止
		config.ServicesStartConcurrently = true;
		config.ServicesStopConcurrently = true;
		// worker关闭则应用关闭
		config.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.StopHost;
		// 启动关闭超时
		config.StartupTimeout = TimeSpan.FromMinutes(1);
		config.ShutdownTimeout = TimeSpan.FromSeconds(30);
});

builder.Services
	.AddSingleton(
		OKEXUserInit(
			StartupConfiguration.PlatformAPIKey,
			StartupConfiguration.PlatformSecret,
			StartupConfiguration.PlatFormPassphrase,
			"平台策略",
			(a, s, p) => new PlatformUser(a, s, p)
		)
	)
	.AddSingleton(
		OKEXUserInit(
			StartupConfiguration.FollowerAPIKey,
			StartupConfiguration.FollowerSecret,
			StartupConfiguration.FollowerPassphrase,
			"跟单",
			(a, s, p) => new FollowerUser(a, s, p)
		)
	)
	.AddSingleton(sp => new ChannelWrapper<OrderDiffMessage>() {
			Channel = Channel.CreateBounded<OrderDiffMessage>(new BoundedChannelOptions(2000) {
				FullMode = BoundedChannelFullMode.Wait
			})
	})
	.AddSingleton(sp => new ChannelWrapper<OrderFinishedMessage>() {
			Channel = Channel.CreateBounded<OrderFinishedMessage>(new BoundedChannelOptions(2000) {
				FullMode = BoundedChannelFullMode.Wait
			})
	})
	.AddSingleton<ConcurrentDictionary<string, Transaction>>()
	.AddSingleton<Configuration>();


builder.Services
	.AddHostedService<PlatformWatcherWorker>()
	.AddHostedService<FollowerWorker>()
	.AddHostedService<StatusWorker>()
	.AddHostedService<AnalysisWorker>()
	.AddHostedService<Configuration.ConfigWorker>();


#if WINDOWS
builder.Services.AddWindowsService(config => {
	config.ServiceName = "OKEXBOT Service";
});
#elif LINUX
builder.Services.AddSystemd();
#endif
// builder.Services.AddHostedService<ConfigWorker>();
// builder.Services.AddHostedService<ConfigWorker>();
// builder.Services.AddHostedService<ConfigWorker>();


builder.Logging
	.ClearProviders()
	.SetMinimumLevel(LogLevel.Information)
	#if RELEASE
	.AddFilter("Microsoft", LogLevel.None)
	.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None)
	#endif
	.AddZLoggerConsole()
	.AddZLoggerRollingFile(options => {
		options.RollingInterval = ZLogger.Providers.RollingInterval.Day;
		// options.RollingSizeKB = 1024 * 1024 * 10;
		options.UseJsonFormatter(formatter => {
			// TODO 我真是草了,此选项莫名无效
			// 初步判定此对象没有被使用
			formatter.JsonSerializerOptions = new() {
				WriteIndented = true,
				DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			};
			// formatter.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
		});
		options.FilePathSelector = (timestamp, sequenceNumber) => Path.Combine(StartupConfiguration.LogDirectory, $"{timestamp.ToLocalTime():yyyy-MM-dd}_{sequenceNumber:000}.log");
	});


using var host = builder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();

// TODO 还是let it crash?
AppDomain.CurrentDomain.UnhandledException += (_, e) => {
	logger.LogError((Exception)e.ExceptionObject, "未知异常");
};

AppDomain.CurrentDomain.ProcessExit += (sender, e) => {
	// 退出太快其实也来不及记录
	// TODO 是否需要做别的什么?
	logger.LogInformation("程序已完全终止.");
};

host.Run();

// 想象中这样就OK了,但没有泛型lambda
// var OKEXUserInit = <T>(string appKey, string secret, string passphrase) => (IStreamProvider sp) => {
// 		var config = sp.GetService<IConfiguration>();
// 		var _appKey = config!.GetValue<string>(appKey);
// 		var _secret = config!.GetValue<string>(secret);
// 		var _passphrase = config!.GetValue<string>(passphrase);
		
// 		if (_appKey is null || _secret is null || _passphrase is null) {
// 			throw new ArgumentNullException("请检查平台策略账户 APIKEY, SECRET, PASSPHRASE 等信息是否传入!");
// 		}
		
// 		return new T(_appKey, _secret, _passphrase);
// }


// 没有泛型lambda但有泛型本地函数也行
// 但new T只能无参或初始化设定项
// static Func<IServiceProvider, T> OKEXUserInit<T>(string appKey, string secret, string passphrase) where T: OKEXApi, new() {
// 	return sp => {
// 		var config = sp.GetService<IConfiguration>();
// 		var _appKey = config!.GetValue<string>(appKey);
// 		var _secret = config!.GetValue<string>(secret);
// 		var _passphrase = config!.GetValue<string>(passphrase);
		
// 		if (_appKey is null || _secret is null || _passphrase is null) {
// 			throw new ArgumentNullException("请检查平台策略账户 APIKEY, SECRET, PASSPHRASE 等信息是否传入!");
// 		}

// 		return new(_appKey, _secret, _passphrase);
// 		// WTF,可以初始化设定项但不可以new T(a, b, c)
// 		// return new() {
// 		// 	test = 1
// 		// };
// 	};
// }

// 于是只能再加一个Func在调用处指定类型实例化
// 但这一切真的值得吗? 你是不是有毛病? 为了两个重复的地方搞这么个东西真的有可读性?
// just for fun
static Func<IServiceProvider, T> OKEXUserInit<T>(string apiKey, string secret, string passphrase, string userType, Func<string, string, string, T> factory) where T: OKEXApi {
	return sp => {
		var config = sp.GetService<IConfiguration>();
		var _appKey = config!.GetValue<string>(apiKey);
		var _secret = config!.GetValue<string>(secret);
		var _passphrase = config!.GetValue<string>(passphrase);
		
		if (_appKey is null || _secret is null || _passphrase is null) {
			throw new ArgumentNullException($"请检查{userType}账户 APIKEY, SECRET, PASSPHRASE 等信息是否传入!");
		}

		return factory(_appKey, _secret, _passphrase);
	};
}
