// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        public void Reset__Body(Qubit target)
        {
            // The native simulator doesn't have a reset operation, so simulate
            // it via an M follow by a conditional X.
            this.CheckQubit(target);
            var res = M(this.Id, (uint)target.Id);
            if (res == 1)
            {
                X(this.Id, (uint)target.Id);
            }
        }

        public class QrackSimReset : Intrinsic.Reset
        {
            private QrackSimulator Simulator { get; }

            public QrackSimReset(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override Func<Qubit, QVoid> __Body__ => (q1) =>
            {
                Simulator.Reset__Body(q1);

                return QVoid.Instance;
            };

        }
    }
}
