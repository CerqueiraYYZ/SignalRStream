using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SignalRR.SignalRChat.Hubs
{
    public class StreamHub : Hub
    {
        #region snippet1
        public ChannelReader<int> DelayCounter(int delay)
        {
            var channel = Channel.CreateUnbounded<int>();
            _ = WriteItems(channel.Writer, 20, delay);
            return channel.Reader;
        }

        private async Task WriteItems(ChannelWriter<int> writer, int count, int delay)
        {
            for (var i = 0; i < count; i++)
            {
                //For every 5 items streamed, add twice the delay
                if (i % 5 == 0)
                    delay = delay * 2;
                await writer.WriteAsync(i);
                await Task.Delay(delay);
            }
            writer.TryComplete();
        }
        #endregion

        #region snippet2
        public async Task UploadStream(ChannelReader<string> stream)
        {
            while (await stream.WaitToReadAsync())
            {
                while (stream.TryRead(out var item))
                {
                    // do something with the stream item
                    Console.WriteLine(item);
                }
            }
        }
        #endregion
    }
}
