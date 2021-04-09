// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Quantum.Intrinsic.Interfaces;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        void IIntrinsicApplyUncontrolledRz.Body(double angle, Qubit target)
        {
            this.CheckQubit(target, nameof(target));
            CheckAngle(angle);
            R(this.Id, Pauli.PauliZ, angle, (uint)target.Id);
        }

        void IIntrinsicApplyUncontrolledRz.AdjointBody(double angle, Qubit target)
        {
            ((IIntrinsicApplyUncontrolledRz)this).Body(-angle, target);
        }
    }
}
