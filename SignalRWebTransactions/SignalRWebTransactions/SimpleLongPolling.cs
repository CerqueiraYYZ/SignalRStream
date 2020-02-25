using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebTransactions
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SimpleLongPolling
    {
        private static List<SimpleLongPolling> _sSubscribers = new List<SimpleLongPolling>();

        public static void Publish(string channel, string message)
        {
            lock (_sSubscribers)
            {
                var all = _sSubscribers.ToList();
                foreach (var poll in all)
                {
                    if (poll._Channel == channel) poll.Notify(message);
                }
            }
        }

        private TaskCompletionSource<bool> _TaskCompleteion = new TaskCompletionSource<bool>();

        private string _Channel { get; set; }
        private string _Message { get; set; }
        public SimpleLongPolling(string channel)
        {
            this._Channel = channel;
            lock (_sSubscribers)
            {
                _sSubscribers.Add(this);
            }
        }

        private void Notify(string message)
        {
            this._Message = message;
            this._TaskCompleteion.SetResult(true);
        }

        public async Task<(string,bool)> WaitAsync()
        {
            await Task.WhenAny(_TaskCompleteion.Task, Task.Delay(60000)); // blocking wait until event occurs or timeout
            lock (_sSubscribers)
            {
                _sSubscribers.Remove(this);
            }
            return (this._Message,true);
        }
    }
}
