// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "H")]
        private static extern void H(uint id, uint qubit);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCH")]
        private static extern void MCH(uint id, uint count, uint[] ctrls, uint qubit);

        public virtual void H__Body(Qubit target)
        {
            this.CheckQubit(target);

            H(this.Id, (uint)target.Id);
        }

        public virtual void H__ControlledBody(IQArray<Qubit> controls, Qubit target)
        {
            this.CheckQubits(controls, target);

            SafeControlled(controls,
                () => H__Body(target),
                (count, ids) => MCH(this.Id, count, ids, (uint)target.Id));
        }
    }
}
