namespace Bot.Core;

public sealed class Order {
	// 交易类型, eg. 币币or合约
	public string? TradeType { get; set; }

	// 产品ID, eg. 合约ID BTC-USD-200329
	public string? ProductID { get; set; }

	// 平台内部订单ID
	public string PlatformOrderID { get; set; } = default!;

	// 保证金币种
	public string? Currency { get; set; }

	// 最终委托价格,单位币
	public double ConsignmentPrice { get; set; }

	// 未修正委托价格,单位币
	public double OriginalPrice { get; set; }

	// 委托数量
	public double Size { get; set; }

	// 未修正委托数量
	public double OriginalSize { get; set; }

	// 收益,仅用于成交订单
	public string? Pnl { get; set; }

	// 订单类型, eg,市价or限价
	public string? OrderType { get; set; }

	// 订单方向
	public string Side { get; set; } = default!;

	// 持仓方向
	public string? PosSide { get; set; }

	// 交易模式
	public string? TradeMode { get; set; }

	// 部分成交数量
	public string? PartialFillSize { get; set; }

	// 最新成交价, 注意此字段可能为""
	public string? LatestFillPrice { get; set; }

	// 最新成交ID
	public string? LatestFillID { get; set; }

	// 最新成交数量
	public string? LatestFillSize { get; set; }

	// 最新成交时间
	public string? LatestFillTime { get; set; }

	// 成交均价,可能为""
	public string? AverageFillPrice { get; set; }

	// 订单状态
	public string? OrderState { get; set; }

	// 杠杆倍数
	public float Leverage { get; set; }

	// 手续费
	public string? Fee { get; set; }

	// 订单种类, 交割or强平
	public string? OrderCategory { get; set; }

	// 订单取消原因代码
	public string? CancelReasonCode { get; set; }

	// 订单取消原因描述
	public string? CancelReasonDesc { get; set; }

	// 订单状态更新时间
	public string? OrderModifiedTime { get; set; }

	// 订单创建时间
	public string? OrderCreatedTime { get; set; }
	
	public PlatformOrderState PlatformState { get; set; } = PlatformOrderState.Unfilled;
	public CustomOrderState CustomState { get; set; } = CustomOrderState.New;
	
	public Order() {}
	
	public Order(Order order) {
		TradeType = order.TradeType;
		ProductID = order.ProductID;
		Currency = order.Currency;
		ConsignmentPrice = order.ConsignmentPrice;
		Size = order.Size;
		OrderType = order.OrderType;
		Side = order.Side;
		PosSide = order.PosSide;
	}
	
}

public enum PlatformOrderState {
	Unfilled,
	PartialFilled,
	Filled,
	Canceled
}

public enum CustomOrderState {
	New,
	Modified,
	Canceled
}