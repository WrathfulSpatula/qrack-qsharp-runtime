// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Quantum.Intrinsic.Interfaces;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        void IIntrinsicApplyUncontrolledZ.Body(Qubit target)
        {
            this.CheckQubit(target);

            Z(this.Id, (uint)target.Id);
        }
    }
}
