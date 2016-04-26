﻿using ReactiveStreamsCS;
using RxAdvancedFlow.internals.subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxAdvancedFlow.internals.single
{
    sealed class SingleFromPublisher<T> : ISingle<T>
    {
        readonly IPublisher<T> source;

        internal SingleFromPublisher(IPublisher<T> source)
        {
            this.source = source;
        }

        public void Subscribe(ISingleSubscriber<T> s)
        {
            throw new NotImplementedException();
        }

        sealed class SinglePublisherSubscriber : ISubscriber<T>, IDisposable
        {
            readonly ISingleSubscriber<T> actual;

            int count;

            T value;

            ISubscription s;

            public void Dispose()
            {
                s.Cancel();
            }

            public void OnComplete()
            {
                if (count == 1)
                {
                    actual.OnSuccess(value);
                }
                else
                if (count == 0) 
                {
                    actual.OnError(new IndexOutOfRangeException("Source is empty"));
                }
            }

            public void OnError(Exception e)
            {
                if (count < 2)
                {
                    actual.OnError(e);
                }
                else
                {
                    RxAdvancedFlowPlugins.OnError(e);
                }
            }

            public void OnNext(T t)
            {
                value = t;
                if (++count == 2)
                {
                    Dispose();

                    actual.OnError(new IndexOutOfRangeException("Source contains more than one element"));
                }
            }

            public void OnSubscribe(ISubscription s)
            {
                if (OnSubscribeHelper.SetSubscription(ref this.s, s))
                {
                    actual.OnSubscribe(this);

                    s.Request(long.MaxValue);
                }
            }
        }
    }
}