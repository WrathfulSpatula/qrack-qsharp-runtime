// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        public virtual void ApplyUncontrolledRz__Body(double angle, Qubit target)
        {
            this.CheckQubit(target, nameof(target));
            CheckAngle(angle);
            R(this.Id, Pauli.PauliZ, angle, (uint)target.Id);
        }

        public virtual void ApplyUncontrolledRz__AdjointBody(double angle, Qubit target)
        {
            ApplyUncontrolledRz__Body(-angle, target);
        }
    }
}
