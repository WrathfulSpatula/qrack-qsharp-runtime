// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Quantum.Intrinsic.Interfaces;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        void IIntrinsicReset.Body(Qubit target)
        {
            // The native simulator doesn't have a reset operation, so simulate
            // it via an M follow by a conditional X.
            this.CheckQubit(target);
            var res = M(this.Id, (uint)target.Id);
            if (res == 1)
            {
                X(this.Id, (uint)target.Id);
            }
            target.IsMeasured = true;
        }
    }
}
