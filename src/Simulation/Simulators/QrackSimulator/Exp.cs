// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Exp")]
        private static extern void Exp(uint id, uint n, Pauli[] paulis, double angle, uint[] ids);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCExp")]
        private static extern void MCExp(uint id, uint n, Pauli[] paulis, double angle, uint nc, uint[] ctrls, uint[] ids);

        public virtual void Exp__Body(IQArray<Pauli> paulis, double angle, IQArray<Qubit> targets)
        {
            this.CheckQubits(targets);
            CheckAngle(angle);

            if (paulis.Length != targets.Length)
            {
                throw new InvalidOperationException($"Both input arrays for Exp (paulis, targets), must be of same size.");
            }

            Exp(this.Id, (uint)paulis.Length, paulis.ToArray(), angle, targets.GetIds());
        }

        public virtual void Exp__AdjointBody(IQArray<Pauli> paulis, double angle, IQArray<Qubit> targets)
        {
            Exp__Body(paulis, -angle, targets);
        }

        public virtual void Exp__ControlledBody(IQArray<Qubit> controls, IQArray<Pauli> paulis, double angle, IQArray<Qubit> targets)
        {
            this.CheckQubits(controls, targets);
            CheckAngle(angle);

            if (paulis.Length != targets.Length)
            {
                throw new InvalidOperationException($"Both input arrays for Exp (paulis, qubits), must be of same size.");
            }

            SafeControlled(controls,
                () => Exp__Body(paulis, angle, targets),
                (count, ids) => MCExp(this.Id, (uint)paulis.Length, paulis.ToArray(), angle, count, ids, targets.GetIds()));
        }

        public virtual void Exp__ControlledAdjointBody(IQArray<Qubit> controls, IQArray<Pauli> paulis, double angle, IQArray<Qubit> targets)
        {
            Exp__ControlledBody(controls, paulis, -angle, targets);
        }

        public class QrackSimExp : Intrinsic.Exp
        {
            private QrackSimulator Simulator { get; }

            public QrackSimExp(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override Func<(IQArray<Pauli>, double, IQArray<Qubit>), QVoid> __Body__ => (_args) =>
            {
                var (paulis, angle, targets) = _args;

                Simulator.Exp__Body(paulis, angle, targets);

                return QVoid.Instance;
            };

            public override Func<(IQArray<Pauli>, double, IQArray<Qubit>), QVoid> __AdjointBody__ => (_args) =>
            {
                var (paulis, angle, targets) = _args;

                Simulator.Exp__AdjointBody(paulis, angle, targets);

                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, (IQArray<Pauli>, double, IQArray<Qubit>)), QVoid> __ControlledBody__ => (_args) =>
            {
                var (ctrls, (paulis, angle, targets)) = _args;

                Simulator.Exp__ControlledBody(ctrls, paulis, angle, targets);

                return QVoid.Instance;
            };


            public override Func<(IQArray<Qubit>, (IQArray<Pauli>, double, IQArray<Qubit>)), QVoid> __ControlledAdjointBody__ => (_args) =>
            {
                var (ctrls, (paulis, angle, targets)) = _args;

                Simulator.Exp__ControlledAdjointBody(ctrls, paulis, angle, targets);

                return QVoid.Instance;
            };
        }
    }
}
