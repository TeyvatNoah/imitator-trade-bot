using System.Threading.Channels;

namespace Bot.OKEXApi;

public sealed class ChannelWrapper<T> {
	public required Channel<T> Channel;
}