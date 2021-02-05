// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        public virtual void ApplyControlledZ__Body(Qubit control, Qubit target)
        {
            this.CheckQubits(new QArray<Qubit>(new Qubit[]{ control, target }));

            MCZ(this.Id, 1, new uint[]{(uint)control.Id}, (uint)target.Id);
        }
    }
}
