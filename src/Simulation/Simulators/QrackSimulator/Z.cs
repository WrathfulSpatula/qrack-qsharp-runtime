// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Z")]
        private static extern void Z(uint id, uint qubit);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCZ")]
        private static extern void MCZ(uint id, uint count, uint[] ctrls, uint qubit);

        public virtual void Z__Body(Qubit target)
        {
            this.CheckQubit(target);

            Z(this.Id, (uint)target.Id);
        }

        public virtual void Z__ControlledBody(IQArray<Qubit> controls, Qubit target)
        {
            this.CheckQubits(controls, target);

            SafeControlled(controls,
                () => Z__Body(target),
                (count, ids) => MCZ(this.Id, count, ids, (uint)target.Id));
        }
    }
}
