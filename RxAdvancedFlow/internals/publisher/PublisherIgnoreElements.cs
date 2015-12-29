﻿using ReactiveStreamsCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxAdvancedFlow.internals.publisher
{
    sealed class PublisherIgnoreElements<T> : ISubscriber<T>
    {
        readonly ISubscriber<T> actual;

        public PublisherIgnoreElements(ISubscriber<T> actual)
        {
            this.actual = actual;
        }

        public void OnComplete()
        {
            actual.OnComplete();
        }

        public void OnError(Exception e)
        {
            actual.OnError(e);
        }

        public void OnNext(T t)
        {
            // deliberately ignored
        }

        public void OnSubscribe(ISubscription s)
        {
            actual.OnSubscribe(s);

            s.Request(long.MaxValue);
        }
    }
}
