// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "T")]
        private static extern void T(uint id, uint qubit);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "AdjT")]
        private static extern void AdjT(uint id, uint qubit);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCT")]
        private static extern void MCT(uint id, uint count, uint[] ctrls, uint qubit);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCAdjT")]
        private static extern void MCAdjT(uint id, uint count, uint[] ctrls, uint qubit);

        public virtual void T__Body(Qubit target)
        {
            this.CheckQubit(target);

            T(this.Id, (uint)target.Id);
        }

        public virtual void T__ControlledBody(IQArray<Qubit> controls, Qubit target)
        {
            this.CheckQubits(controls, target);

            SafeControlled(controls,
                () => T__Body(target),
                (count, ids) => MCT(this.Id, count, ids, (uint)target.Id));
        }

        public virtual void T__AdjointBody(Qubit target)
        {
            this.CheckQubit(target);

            AdjT(this.Id, (uint)target.Id);
        }

        public virtual void T__ControlledAdjointBody(IQArray<Qubit> controls, Qubit target)
        {
            this.CheckQubits(controls, target);

            SafeControlled(controls,
                () => T__AdjointBody(target),
                (count, ids) => MCAdjT(this.Id, count, ids, (uint)target.Id));
        }

        public class QrackSimT : Intrinsic.T
        {
            private QrackSimulator Simulator { get; }

            public QrackSimT(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override Func<Qubit, QVoid> __Body__ => (q1) =>
            {
                Simulator.T__Body(q1);

                return QVoid.Instance;
            };

            public override Func<Qubit, QVoid> __AdjointBody__ => (q1) =>
            {
                Simulator.T__AdjointBody(q1);

                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> __ControlledBody__ => (_args) =>
            {
                var (ctrls, q1) = _args;

                Simulator.T__ControlledBody(ctrls, q1);

                return QVoid.Instance;
            };


            public override Func<(IQArray<Qubit>, Qubit), QVoid> __ControlledAdjointBody__ => (_args) =>
            {
                var (ctrls, q1) = _args;

                Simulator.T__ControlledAdjointBody(ctrls, q1);

                return QVoid.Instance;
            };
        }
    }
}
