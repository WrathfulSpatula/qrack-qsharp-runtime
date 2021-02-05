// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        public virtual void ApplyUncontrolledRy__Body(double angle, Qubit target)
        {
            this.CheckQubit(target, nameof(target));
            CheckAngle(angle);
            R(this.Id, Pauli.PauliY, angle, (uint)target.Id);
        }

        public virtual void ApplyUncontrolledRy__AdjointBody(double angle, Qubit target)
        {
            ApplyUncontrolledRy__Body(-angle, target);
        }
    }
}
