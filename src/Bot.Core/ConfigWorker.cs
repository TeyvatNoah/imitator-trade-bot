namespace Bot.Core;

using System.Runtime.CompilerServices;

using Bot.OKEXApi;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using ZLogger;

public sealed partial class Configuration {

	// 配置仅允许ConfigWorker修改,没有友元类的实现方式
	public sealed class ConfigWorker: BackgroundService, IHostedLifecycleService {
			private readonly ILogger<ConfigWorker> _logger;
			private readonly IHostApplicationLifetime _appLifetime;
			private readonly IConfiguration _config;
			private readonly IHostEnvironment _env;
			private readonly FollowerUser _follower;
			private readonly PlatformUser _platformWatcher;
			private readonly Configuration _botConfiguration;
		
		public ConfigWorker(
			ILogger<ConfigWorker> logger,
			IHostApplicationLifetime appLifetime,
			IConfiguration config,
			IHostEnvironment env,
			FollowerUser follower,
			PlatformUser platformWatcher,
			Configuration botConfiguration
		) {
			_logger = logger;
			_appLifetime = appLifetime;
			_config = config;
			_env = env;
			_follower = follower;
			_platformWatcher = platformWatcher;
			_botConfiguration = botConfiguration;
			// 继承了IHostedLifecycleService可以不用下面的了
			// appLifetime.ApplicationStarted.Register(OnStarted);
			// appLifetime.ApplicationStopped.Register(OnStopped);
			// 就为了这一行,不能用主构造函数要多写好几行感觉也有点蠢
			config.GetReloadToken().RegisterChangeCallback(OnConfigurationChanged, null);
		}
		
		// public void OnStarted() {
		// 	logger.LogInformation("配置Worker已启动.");
		// }
		
		// public async Task StartingAsync(CancellationToken cancellationToken) {
		// 	await Task.Delay(TimeSpan.FromSeconds(10));
		// 	// return Task.CompletedTask;
		// }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task StartedAsync(CancellationToken cancellationToken) {
			_logger.LogInformation("配置Worker已启动.");
			return Task.CompletedTask;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task StoppingAsync(CancellationToken cancellationToken) {
			return Task.CompletedTask;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task StoppedAsync(CancellationToken cancellationToken) {
			_logger.LogInformation("配置Worker已停止.");
			return Task.CompletedTask;
		}

		
		// 此生命周期阻塞所有woker的StartAsync,可以执行一些必要初始化
		public async Task StartingAsync(CancellationToken cancellationToken) {
			_logger.LogInformation("正在设置交易账户...");

			_botConfiguration.FollowerAppKey = _config.GetValue<string>(StartupConfiguration.FollowerAppKey)!;
			_botConfiguration.FollowerSecret = _config.GetValue<string>(StartupConfiguration.FollowerSecret)!;
			_botConfiguration.FollowerPassphrase= _config.GetValue<string>(StartupConfiguration.FollowerPassphrase)!;
			_botConfiguration.PlatformAppKey = _config.GetValue<string>(StartupConfiguration.PlatformAppKey)!;
			_botConfiguration.PlatformSecret = _config.GetValue<string>(StartupConfiguration.PlatformSecret)!;
			_botConfiguration.PlatFormPassphrase = _config.GetValue<string>(StartupConfiguration.PlatFormPassphrase)!;
			_botConfiguration.ProductID = _config.GetValue<string>(StartupConfiguration.Product)!;
			
			_logger.ZLogInformation($"当前交易对:{_botConfiguration.ProductID}");
			

			try {
				var t0 = _platformWatcher.GetAccountConfiguration();
				var t1 = _platformWatcher.GetAccountLeverage(_botConfiguration.ProductID, MgnMode.Isolated);
				var t2 = _platformWatcher.GetAccountLeverage(_botConfiguration.ProductID, MgnMode.Cross);
				var t3 = _platformWatcher.GetInstruments(_botConfiguration.ProductID, OKEXTradeType.SWAP);

				// 异常及早退出
				await Task.WhenAll(t0, t1, t2, t3);

				var r0 = await t0;
				var r1 = await t1;
				var r2 = await t2;
				var r3 = await t3;

				_botConfiguration.AccountLevel = r0.AccountLevel;
				_botConfiguration.PositionMode = r0.PositionMode;
				_botConfiguration.AutoLoan = r0.AutoLoan;
				_botConfiguration.IsolatedMode = r0.CTIsoMode;
				_botConfiguration.IsolatedMgnMode = r1.TradeMode!;
				_botConfiguration.IsolatedLeverage = r1.Leverage;
				_botConfiguration.IsolatedPositionSide = r1.PosSide!;
				_botConfiguration.CrossMgnMode = r2.TradeMode!;
				_botConfiguration.CrossLeverage = r2.Leverage;
				_botConfiguration.CrossPositionSide = r2.PosSide!;
				_botConfiguration.PricePrecision = r3.PricePrecision;
				_botConfiguration.SizePrecision = r3.SizePrecision;
				_botConfiguration.MinSize = r3.MinSize;
				_botConfiguration.CTType = r3.CTType;
				
				_logger.ZLogInformation($"""
				目标账户配置:
				交易对: {_botConfiguration.ProductID}
				账户层级: {_botConfiguration.AccountLevel}
				持仓方式: {_botConfiguration.PositionMode}
				是否自动借币: {_botConfiguration.AutoLoan}
				保证金划转模式: {_botConfiguration.IsolatedMode}
				逐仓杠杆: {_botConfiguration.IsolatedLeverage}
				逐仓保证金模式: {_botConfiguration.IsolatedMgnMode}
				逐仓持仓方向: {_botConfiguration.IsolatedPositionSide}
				全仓杠杆: {_botConfiguration.CrossLeverage}
				全仓保证金模式: {_botConfiguration.CrossMgnMode}
				全仓持仓方向: {_botConfiguration.CrossPositionSide}
				目标交易对配置:
				价格精度: {_botConfiguration.PricePrecision}
				数量精度: {_botConfiguration.SizePrecision}
				最小数量: {_botConfiguration.MinSize}
				""");
				
				// 串行
				_logger.LogInformation("设置账户模式...");
				await _follower.SetAccountMode(_botConfiguration.AccountLevel);
				_logger.LogInformation("设置持仓模式...");
				await _follower.SetAccountPositionMode(_botConfiguration.PositionMode);
				_logger.LogInformation("设置逐仓保证金划转模式...");
				await _follower.SetIsolatedMode(_botConfiguration.IsolatedMode);
				_logger.LogInformation("设置逐仓杠杆...");
				await _follower.SetAccountLeverage(_botConfiguration.ProductID, MgnMode.Isolated, _botConfiguration.IsolatedPositionSide, _botConfiguration.IsolatedLeverage);
				_logger.LogInformation("设置全仓杠杆...");
				await _follower.SetAccountLeverage(_botConfiguration.ProductID, MgnMode.Cross, null, _botConfiguration.CrossLeverage);
				
			} catch(Exception e) {
				_logger.LogError(e, "启动失败");
				_appLifetime.StopApplication(1);
			}
		}

		protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
			while (!cancellationToken.IsCancellationRequested) {
				await Task.Delay(TimeSpan.FromMinutes(1));
				try {

				// 允许出错,记录一下即可
					var t0 = _platformWatcher.GetAccountConfiguration();
					var t1 = _platformWatcher.GetAccountLeverage(_botConfiguration.ProductID, MgnMode.Isolated);
					var t2 = _platformWatcher.GetAccountLeverage(_botConfiguration.ProductID, MgnMode.Cross);
					var t3 = _platformWatcher.GetInstruments(_botConfiguration.ProductID, OKEXTradeType.SWAP);

					// 异常及早退出
					await Task.WhenAll(t0, t1, t2, t3);

					var r0 = await t0;
					var r1 = await t1;
					var r2 = await t2;
					var r3 = await t3;
					
					// 检测配置变更
					if (
						_botConfiguration.AccountLevel != r0.AccountLevel
						|| _botConfiguration.PositionMode != r0.PositionMode
						|| _botConfiguration.AutoLoan != r0.AutoLoan
						|| _botConfiguration.IsolatedMode != r0.CTIsoMode
					) {
						_logger.LogInformation("检测到账户配置变更.");
						_botConfiguration.AccountLevel = r0.AccountLevel;
						_botConfiguration.PositionMode = r0.PositionMode;
						_botConfiguration.AutoLoan = r0.AutoLoan;
						_botConfiguration.IsolatedMode = r0.CTIsoMode;
					}

					if (
						_botConfiguration.IsolatedMgnMode != r1.TradeMode!
						|| _botConfiguration.IsolatedLeverage != r1.Leverage
						|| _botConfiguration.IsolatedPositionSide != r1.PosSide!
					) {
						_logger.LogInformation("检测到逐仓杠杆配置变更.");
						_botConfiguration.IsolatedMgnMode = r1.TradeMode!;
						_botConfiguration.IsolatedLeverage = r1.Leverage;
						_botConfiguration.IsolatedPositionSide = r1.PosSide!;
					}

					if (
						_botConfiguration.CrossMgnMode != r2.TradeMode!
						|| _botConfiguration.CrossLeverage != r2.Leverage
						|| _botConfiguration.CrossPositionSide != r2.PosSide!
					) {
						_logger.LogInformation("检测到全仓杠杆配置变更.");
						_botConfiguration.CrossMgnMode = r2.TradeMode!;
						_botConfiguration.CrossLeverage = r2.Leverage;
						_botConfiguration.CrossPositionSide = r2.PosSide!;
					}

					if (
						_botConfiguration.PricePrecision != r3.PricePrecision
						|| _botConfiguration.SizePrecision != r3.SizePrecision
						|| _botConfiguration.MinSize != r3.MinSize
						|| _botConfiguration.CTType != r3.CTType
					) {
						_logger.LogInformation("检测到精度配置变更.");
						_botConfiguration.PricePrecision = r3.PricePrecision;
						_botConfiguration.SizePrecision = r3.SizePrecision;
						_botConfiguration.MinSize = r3.MinSize;
						_botConfiguration.CTType = r3.CTType;
					}

					_logger.ZLogInformation($"""
					当前账户配置:
					交易对: {_botConfiguration.ProductID}
					账户层级: {_botConfiguration.AccountLevel}
					持仓方式: {_botConfiguration.PositionMode}
					是否自动借币: {_botConfiguration.AutoLoan}
					保证金划转模式: {_botConfiguration.IsolatedMode}
					逐仓杠杆: {_botConfiguration.IsolatedLeverage}
					逐仓保证金模式: {_botConfiguration.IsolatedMgnMode}
					逐仓持仓方向: {_botConfiguration.IsolatedPositionSide}
					全仓杠杆: {_botConfiguration.CrossLeverage}
					全仓保证金模式: {_botConfiguration.CrossMgnMode}
					全仓持仓方向: {_botConfiguration.CrossPositionSide}
					目标交易对配置:
					价格精度: {_botConfiguration.PricePrecision}
					数量精度: {_botConfiguration.SizePrecision}
					最小数量: {_botConfiguration.MinSize}
					""");

				_logger.LogInformation("设置账户模式...");
				await _follower.SetAccountMode(_botConfiguration.AccountLevel);
				_logger.LogInformation("设置持仓模式...");
				await _follower.SetAccountPositionMode(_botConfiguration.PositionMode);
				_logger.LogInformation("设置逐仓保证金划转模式...");
				await _follower.SetIsolatedMode(_botConfiguration.IsolatedMode);
				_logger.LogInformation("设置逐仓杠杆...");
				await _follower.SetAccountLeverage(_botConfiguration.ProductID, MgnMode.Isolated, _botConfiguration.IsolatedPositionSide, _botConfiguration.IsolatedLeverage);
				_logger.LogInformation("设置全仓杠杆...");
				await _follower.SetAccountLeverage(_botConfiguration.ProductID, MgnMode.Cross, null, _botConfiguration.CrossLeverage);
				} catch (Exception e) {
					_logger.LogError(e, "检测配置异常");
				}
			}
		}
		// public override async Task StopAsync(CancellationToken cancellationToken) {
		// 	await base.StopAsync(cancellationToken);
		// }
		
		public void OnConfigurationChanged(object? _) {
			_logger.LogInformation("配置文件已变更.");
			_botConfiguration.ProductID = _config.GetValue<string>(StartupConfiguration.Product)!;

			_logger.ZLogInformation($"""
			当前账户配置:
			交易对: {_botConfiguration.ProductID}
			账户层级: {_botConfiguration.AccountLevel}
			持仓方式: {_botConfiguration.PositionMode}
			是否自动借币: {_botConfiguration.AutoLoan}
			保证金划转模式: {_botConfiguration.IsolatedMode}
			逐仓杠杆: {_botConfiguration.IsolatedLeverage}
			逐仓保证金模式: {_botConfiguration.IsolatedMgnMode}
			逐仓持仓方向: {_botConfiguration.IsolatedPositionSide}
			全仓杠杆: {_botConfiguration.CrossLeverage}
			全仓保证金模式: {_botConfiguration.CrossMgnMode}
			全仓持仓方向: {_botConfiguration.CrossPositionSide}
			目标交易对配置:
			价格精度: {_botConfiguration.PricePrecision}
			数量精度: {_botConfiguration.SizePrecision}
			最小数量: {_botConfiguration.MinSize}
			""");
		}
		
		// public void OnStopped() {
		// 	logger.LogInformation("配置Worker已停止.");
		// }
	}
	
}