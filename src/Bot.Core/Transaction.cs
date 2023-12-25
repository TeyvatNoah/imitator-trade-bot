namespace Bot.Core;

public sealed class Transaction {
	public TransactionState State = TransactionState.Unfinished;

	public required Order PlatformOrder;
	public required Order FollowerOrder;
}


public enum TransactionState {
	Unfinished,
	Finished
}