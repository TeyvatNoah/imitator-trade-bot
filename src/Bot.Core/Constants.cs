namespace Bot.Core;

public static class StartupConfiguration {
	public const string PlatformAPIKey = "PLATFORM_APIKEY";
	public const string PlatformSecret = "PLATFORM_SECRET";
	public const string PlatFormPassphrase = "PLATFORM_PASSPHRASE";
	public const string FollowerAPIKey = "FOLLOWER_APIKEY";
	public const string FollowerSecret = "FOLLOWER_SECRET";
	public const string FollowerPassphrase = "FOLLOWER_PASSPHRASE";
	public const string LogDirectory = "./logs";
	public const string Product = "product";
}


public static class OKEXOrderState {
	public const string Unfilled = "live";
	public const string PartialFilled = "partially_filled";
	public const string Canceled = "canceled";
	public const string Filled = "filled";
}