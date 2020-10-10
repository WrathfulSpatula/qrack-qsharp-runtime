// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{

    public partial class QrackSimulator
    {
        public class QrackSimExpFrac : Intrinsic.ExpFrac
        {
            public QrackSimExpFrac(QrackSimulator m) : base(m)
            {
            }

            public static double Angle(long numerator, long power) =>
                (System.Math.PI * numerator) / (1 << (int)power);

            public override Func<(IQArray<Pauli>, long, long, IQArray<Qubit>), QVoid> __Body__ => (args) =>
            {
                var (paulis, numerator, power, qubits) = args;
                var angle = Angle(numerator, power);
                return Exp__.Apply((paulis, angle, qubits));
            };

            public override Func<(IQArray<Pauli>, long, long, IQArray<Qubit>), QVoid> __AdjointBody__ => (args) =>
            {
                var (paulis, numerator, power, qubits) = args;
                var angle = Angle(numerator, power);
                return Exp__.Adjoint.Apply((paulis, angle, qubits));
            };

            public override Func<(IQArray<Qubit>, (IQArray<Pauli>, long, long, IQArray<Qubit>)), QVoid> __ControlledBody__ => (args) =>
            {
                var (ctrls, (paulis, numerator, power, qubits)) = args;
                var angle = Angle(numerator, power);
                return Exp__.Controlled.Apply((ctrls, (paulis, angle, qubits)));
            };

            public override Func<(IQArray<Qubit>, (IQArray<Pauli>, long, long, IQArray<Qubit>)), QVoid> __ControlledAdjointBody__ => (args) =>
            {
                var (ctrls, (paulis, numerator, power, qubits)) = args;
                var angle = Angle(numerator, power);
                return Exp__.Adjoint.Controlled.Apply((ctrls, (paulis, angle, qubits)));
            };
        }
    }
}
