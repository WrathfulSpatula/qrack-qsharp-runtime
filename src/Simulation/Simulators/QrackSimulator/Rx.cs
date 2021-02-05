// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        public virtual void Rx__Body(double angle, Qubit target)
        {
            this.CheckQubit(target, nameof(target));
            CheckAngle(angle);
            R(this.Id, Pauli.PauliX, angle, (uint)target.Id);
        }

        public virtual void Rx__AdjointBody(double angle, Qubit target)
        {
            Rx__Body(-angle, target);
        }

        public virtual void Rx__ControlledBody(IQArray<Qubit> controls, double angle, Qubit target)
        {
            this.CheckQubits(controls, target);
            CheckAngle(angle);
            MCR(this.Id, Pauli.PauliX, angle, (uint)controls.Length, controls.GetIds(), (uint)target.Id);
        }

        public virtual void Rx__ControlledAdjointBody(IQArray<Qubit> controls, double angle, Qubit target)
        {
            Rx__ControlledBody(controls, -angle, target);
        }

        public class QrackSimRx : Intrinsic.Rx
        {
            private QrackSimulator Simulator { get; }

            public QrackSimRx(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override Func<(double, Qubit), QVoid> __Body__ => (_args) =>
            {
                var (angle, q1) = _args;

                Simulator.Rx__Body(angle, q1);

                return QVoid.Instance;
            };

            public override Func<(double, Qubit), QVoid> __AdjointBody__ => (_args) =>
            {
                var (angle, q1) = _args;

                Simulator.Rx__AdjointBody(angle, q1);

                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, (double, Qubit)), QVoid> __ControlledBody__ => (_args) =>
            {
                var (ctrls, (angle, q1)) = _args;

                Simulator.Rx__ControlledBody(ctrls, angle, q1);

                return QVoid.Instance;
            };


            public override Func<(IQArray<Qubit>, (double, Qubit)), QVoid> __ControlledAdjointBody__ => (_args) =>
            {
                var (ctrls, (angle, q1)) = _args;

                Simulator.Rx__ControlledAdjointBody(ctrls, angle, q1);

                return QVoid.Instance;
            };
        }
    }
}
