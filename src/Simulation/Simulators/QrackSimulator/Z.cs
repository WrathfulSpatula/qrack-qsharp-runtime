// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Z")]
        private static extern void Z(uint id, uint qubit);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCZ")]
        private static extern void MCZ(uint id, uint count, uint[] ctrls, uint qubit);

        public virtual void Z__Body(Qubit target)
        {
            this.CheckQubit(target);

            Z(this.Id, (uint)target.Id);
        }

        public virtual void Z__ControlledBody(IQArray<Qubit> controls, Qubit target)
        {
            this.CheckQubits(controls, target);

            SafeControlled(controls,
                () => Z__Body(target),
                (count, ids) => MCZ(this.Id, count, ids, (uint)target.Id));
        }

        public class QrackSimZ : Intrinsic.Z
        {
            private QrackSimulator Simulator { get; }

            public QrackSimZ(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override Func<Qubit, QVoid> __Body__ => (q1) =>
            {
                Simulator.Z__Body(q1);

                return QVoid.Instance;
            };


            public override Func<(IQArray<Qubit>, Qubit), QVoid> __ControlledBody__ => (args) =>
            {
                var (controls, target) = args;

                Simulator.Z__ControlledBody(controls, target);

                return QVoid.Instance;
            };
        }
    }
}
