// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Y")]
        private static extern void Y(uint id, uint qubit);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCY")]
        private static extern void MCY(uint id, uint count, uint[] ctrls, uint qubit);

        public virtual void Y__Body(Qubit target)
        {
            this.CheckQubit(target);

            Y(this.Id, (uint)target.Id);
        }

        public virtual void Y__ControlledBody(IQArray<Qubit> controls, Qubit target)
        {
            this.CheckQubits(controls, target);

            SafeControlled(controls,
                () => Y__Body(target),
                (count, ids) => MCY(this.Id, count, ids, (uint)target.Id));
        }
    }
}
