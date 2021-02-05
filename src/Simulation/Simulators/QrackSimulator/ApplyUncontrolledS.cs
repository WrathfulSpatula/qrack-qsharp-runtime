﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        public virtual void ApplyUncontrolledS__Body(Qubit target)
        {
            this.CheckQubit(target);

            S(this.Id, (uint)target.Id);
        }

        public virtual void ApplyUncontrolledS__AdjointBody(Qubit target)
        {
            this.CheckQubit(target);

            AdjS(this.Id, (uint)target.Id);
        }
    }
}
