namespace Bot.Core;

public sealed class OrderFinishedMessage {
	public IEnumerable<Order> Orders = default!;
	public PlatformOrderState State = default;
}