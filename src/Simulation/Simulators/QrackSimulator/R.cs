// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.InteropServices;
using Microsoft.Quantum.Intrinsic.Interfaces;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "R")]
        private static extern void R(uint id, Pauli basis, double angle, uint qubit);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCR")]
        private static extern void MCR(uint id, Pauli basis, double angle, uint count, uint[] ctrls, uint qubit);
        void IIntrinsicR.Body(Pauli pauli, double angle, Qubit target)
        {
            this.CheckQubit(target);
            CheckAngle(angle);

            R(this.Id, pauli, angle, (uint)target.Id);
        }

        void IIntrinsicR.AdjointBody(Pauli pauli, double angle, Qubit target)
        {
            ((IIntrinsicR)this).Body(pauli, -angle, target);
        }

        void IIntrinsicR.ControlledBody(IQArray<Qubit> controls, Pauli pauli, double angle, Qubit target)
        {
            this.CheckQubits(controls, target);
            CheckAngle(angle);

            SafeControlled(controls,
                () => ((IIntrinsicR)this).Body(pauli, angle, target),
                (count, ids) => MCR(this.Id, pauli, angle, count, ids, (uint)target.Id));
        }

        void IIntrinsicR.ControlledAdjointBody(IQArray<Qubit> controls, Pauli pauli, double angle, Qubit target)
        {
            ((IIntrinsicR)this).ControlledBody(controls, pauli, -angle, target);
        }
    }
}
