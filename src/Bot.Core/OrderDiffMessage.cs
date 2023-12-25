namespace Bot.Core;

public sealed class OrderDiffMessage {
	public IEnumerable<Order> Orders = default!;
	public CustomOrderState State = default;
}