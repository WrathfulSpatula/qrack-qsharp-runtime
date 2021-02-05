// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "X")]
        private static extern void X(uint id, uint qubit);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCX")]
        private static extern void MCX(uint id, uint count, uint[] ctrls, uint qubit);

        public virtual void X__Body(Qubit target)
        {
            this.CheckQubit(target);

            X(this.Id, (uint)target.Id);
        }

        public virtual void X__ControlledBody(IQArray<Qubit> controls, Qubit target)
        {
            this.CheckQubits(controls, target);

            SafeControlled(controls,
                () => X__Body(target),
                (count, ids) => MCX(this.Id, count, ids, (uint)target.Id));
        }
    }
}
