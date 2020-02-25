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

        //static Channel<int> channel;
        private static int Count = 0;
        public static Channel<int> channelMain = null;

        #region snippet1
        public ChannelReader<int> DelayCounter(int delay)
        {

            //if (channelMain == null) {

            var channel = Channel.CreateUnbounded<int>();
            channelMain = channel;
            _ = WriteItems(channelMain.Writer, 20, delay);

            //}

            return channelMain.Reader;
        }

        public override Task OnConnectedAsync()
        {
            Count++;
            base.OnConnectedAsync();


            Clients.All.SendAsync("updateCount", Count);
            return Task.CompletedTask;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Count--;
            base.OnDisconnectedAsync(exception);
            
            Clients.All.SendAsync("updateCount", Count);
            return Task.CompletedTask;
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
