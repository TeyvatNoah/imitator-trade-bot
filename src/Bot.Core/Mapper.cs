using Bot.OKEXApi;

using Riok.Mapperly.Abstractions;

namespace Bot.Core;

[Mapper]
public partial class OrderMapper {
	public partial Order OKEXOrderDtoToOrder(OEKXOrderDto o);
	public partial OKEXOrderArgDto OrderToOKEXOrderArg(Order o);
	public partial ModifiedOrderArgDto OrderToModifiedOKEXOrderArg(Order o);
}