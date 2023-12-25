namespace Bot.Core;

public sealed partial class Configuration {
	private string FollowerAppKey { get; set; }
	private string FollowerSecret { get; set; }
	private string FollowerPassphrase { get; set; }

	private string PlatformAppKey { get; set; }
	private string PlatformSecret { get; set; }
	private string PlatFormPassphrase { get; set; }
	public string AccountLevel { get; private set; }
	public string PositionMode { get; private set; }
	public string ProductID { get; private set; }
	public bool AutoLoan { get; private set; }
	public string IsolatedMode { get; private set; }
	
	public string IsolatedMgnMode { get; private set; }
	public string CrossMgnMode { get; private set; }
	public string IsolatedPositionSide { get; private set; }
	public string CrossPositionSide { get; private set; }
	public float IsolatedLeverage { get; private set; }
	public float CrossLeverage { get; private set; }
	public double PricePrecision { get; set; }
	public double SizePrecision { get; set; }
	public double MinSize { get; set; }
	public string CTType { get; set; } = default!;
}