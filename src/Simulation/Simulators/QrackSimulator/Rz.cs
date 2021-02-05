// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        public virtual void Rz__Body(double angle, Qubit target)
        {
            this.CheckQubit(target, nameof(target));
            CheckAngle(angle);
            R(this.Id, Pauli.PauliZ, angle, (uint)target.Id);
        }

        public virtual void Rz__AdjointBody(double angle, Qubit target)
        {
            Rz__Body(-angle, target);
        }

        public virtual void Rz__ControlledBody(IQArray<Qubit> controls, double angle, Qubit target)
        {
            this.CheckQubits(controls, target);
            CheckAngle(angle);
            MCR(this.Id, Pauli.PauliZ, angle, (uint)controls.Length, controls.GetIds(), (uint)target.Id);
        }

        public virtual void Rz__ControlledAdjointBody(IQArray<Qubit> controls, double angle, Qubit target)
        {
            Rz__ControlledBody(controls, -angle, target);
        }
    }
}
