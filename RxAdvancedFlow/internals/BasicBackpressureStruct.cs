﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RxAdvancedFlow.internals
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct BasicBackpressureStruct
    {
        bool cancelled;

        long p00, p01, p02, p03, p04, p05, p06, p07;
        long p08, p09, p0A, p0B, p0C, p0D, p0E;

        long requested;

        long p10, p11, p12, p13, p14, p15, p16, p17;
        long p18, p19, p1A, p1B, p1C, p1D, p1E;

        int wip;

        long p20, p21, p22, p23, p24, p25, p26, p27;
        long p28, p29, p2A, p2B, p2C, p2D, p2E;

        internal bool IsCancelled()
        {
            return Volatile.Read(ref cancelled);
        }

        internal void Cancel()
        {
            Volatile.Write(ref cancelled, true);
        }

        internal bool TryCancel()
        {
            if (IsCancelled())
            {
                return false;
            }

            Volatile.Write(ref cancelled, true);
            return true;
        }

        public long Requested()
        {
            return Volatile.Read(ref requested);
        }

        public long Request(long n)
        {
            return BackpressureHelper.Add(ref requested, n);
        }

        public long Produced(long n)
        {
            return Interlocked.Add(ref requested, -n);
        }

        public bool Enter()
        {
            return Interlocked.Increment(ref wip) == 1;
        }

        public bool TryEnter()
        {
            return Volatile.Read(ref wip) == 0 && Interlocked.CompareExchange(ref wip, 1, 0) == 0;
        }

        public bool Leave()
        {
            return Interlocked.Decrement(ref wip) == 0;
        }

        public int Leave(int actions)
        {
            return Interlocked.Add(ref wip, -actions);
        }

        public bool TryLeave(ref int missed)
        {
            int m = Interlocked.Add(ref wip, -missed);
            missed = m;
            return m == 0;
        }
    }
}
