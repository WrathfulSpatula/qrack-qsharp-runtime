// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Quantum.Intrinsic.Interfaces;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        void IIntrinsicApplyUncontrolledSWAP.Body(Qubit qubit1, Qubit qubit2)
        {
            this.CheckQubits(new QArray<Qubit>(new Qubit[] { qubit1, qubit2 }));

            SWAP(this.Id, (uint)qubit1.Id, (uint)qubit2.Id);
        }
    }
}
