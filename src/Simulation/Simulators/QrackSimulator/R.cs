// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "R")]
        private static extern void R(uint id, Pauli basis, double angle, uint qubit);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCR")]
        private static extern void MCR(uint id, Pauli basis, double angle, uint count, uint[] ctrls, uint qubit);
        public virtual void R__Body(Pauli pauli, double angle, Qubit target)
        {
            this.CheckQubit(target);
            CheckAngle(angle);

            R(this.Id, pauli, angle, (uint)target.Id);
        }

        public virtual void R__AdjointBody(Pauli pauli, double angle, Qubit target)
        {
            R__Body(pauli, -angle, target);
        }

        public virtual void R__ControlledBody(IQArray<Qubit> controls, Pauli pauli, double angle, Qubit target)
        {
            this.CheckQubits(controls, target);
            CheckAngle(angle);

            SafeControlled(controls,
                () => R__Body(pauli, angle, target),
                (count, ids) => MCR(this.Id, pauli, angle, count, ids, (uint)target.Id));
        }


        public virtual void R__ControlledAdjointBody(IQArray<Qubit> controls, Pauli pauli, double angle, Qubit target)
        {
            R__ControlledBody(controls, pauli, -angle, target);
        }

        public class QrackSimR : Intrinsic.R
        {
            private QrackSimulator Simulator { get; }

            public QrackSimR(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override Func<(Pauli, double, Qubit), QVoid> __Body__ => (_args) =>
            {
                var (basis, angle, q1) = _args;

                Simulator.R__Body(basis, angle, q1);

                return QVoid.Instance;
            };

            public override Func<(Pauli, double, Qubit), QVoid> __AdjointBody__ => (_args) =>
            {
                var (basis, angle, q1) = _args;

                Simulator.R__AdjointBody(basis, angle, q1);

                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, (Pauli, double, Qubit)), QVoid> __ControlledBody__ => (_args) =>
            {
                var (ctrls, (basis, angle, q1)) = _args;

                Simulator.R__ControlledBody(ctrls, basis, angle, q1);

                return QVoid.Instance;
            };


            public override Func<(IQArray<Qubit>, (Pauli, double, Qubit)), QVoid> __ControlledAdjointBody__ => (_args) =>
            {
                var (ctrls, (basis, angle, q1)) = _args;

                Simulator.R__ControlledAdjointBody(ctrls, basis, angle, q1);

                return QVoid.Instance;
            };
        }
    }
}
