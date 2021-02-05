// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "S")]
        private static extern void S(uint id, uint qubit);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "AdjS")]
        private static extern void AdjS(uint id, uint qubit);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCS")]
        private static extern void MCS(uint id, uint count, uint[] ctrls, uint qubit);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCAdjS")]
        private static extern void MCAdjS(uint id, uint count, uint[] ctrls, uint qubit);

        public virtual void S__Body(Qubit target)
        {
            this.CheckQubit(target);

            S(this.Id, (uint)target.Id);
        }

        public virtual void S__ControlledBody(IQArray<Qubit> controls, Qubit target)
        {
            this.CheckQubits(controls, target);

            SafeControlled(controls,
                () => S__Body(target),
                (count, ids) => MCS(this.Id, count, ids, (uint)target.Id));
        }

        public virtual void S__AdjointBody(Qubit target)
        {
            this.CheckQubit(target);

            AdjS(this.Id, (uint)target.Id);
        }

        public virtual void S__ControlledAdjointBody(IQArray<Qubit> controls, Qubit target)
        {
            this.CheckQubits(controls, target);

            SafeControlled(controls,
                () => S__AdjointBody(target),
                (count, ids) => MCAdjS(this.Id, count, ids, (uint)target.Id));
        }
    }
}
