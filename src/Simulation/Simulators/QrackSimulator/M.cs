// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "M")]
        private static extern uint M(uint id, uint q);
        public virtual Result M__Body(Qubit target)
        {
            this.CheckQubit(target);
            //setting qubit as measured to allow for release
            target.IsMeasured = true;
            return M(this.Id, (uint)target.Id).ToResult();
        }

        public class QrackSimM : Intrinsic.M
        {
            private QrackSimulator Simulator { get; }

            public QrackSimM(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override Func<Qubit, Result> __Body__ => (q1) =>
            {
                return Simulator.M__Body(q1);
            };
        }
    }
}
