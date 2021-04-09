// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Quantum.Intrinsic.Interfaces;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        void IIntrinsicApplyUncontrolledRx.Body(double angle, Qubit target)
        {
            this.CheckQubit(target, nameof(target));
            CheckAngle(angle);
            R(this.Id, Pauli.PauliX, angle, (uint)target.Id);
        }

        void IIntrinsicApplyUncontrolledRx.AdjointBody(double angle, Qubit target)
        {
            ((IIntrinsicApplyUncontrolledRx)this).Body(-angle, target);
        }
    }
}
