using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Intrinsic.Interfaces;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SWAP")]
        private static extern uint SWAP(uint id, uint q1, uint q2);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CSWAP")]
        private static extern uint CSWAP(uint id, uint count, uint[] ctrls, uint q1, uint q2);

        void IIntrinsicSWAP.Body(Qubit target1, Qubit target2)
        {
            this.CheckQubits(new QArray<Qubit>(new Qubit[] { target1, target2 }));

            SWAP(this.Id, (uint)target1.Id, (uint)target2.Id);
        }

        void IIntrinsicSWAP.ControlledBody(IQArray<Qubit> controls, Qubit target1, Qubit target2)
        {
            if ((controls == null) || (controls.Count == 0))
            {
                ((IIntrinsicSWAP)this).Body(target1, target2);
            }
            else
            {
                var ctrls_1 = QArray<Qubit>.Add(controls, new QArray<Qubit>(target1));
                var ctrls_2 = QArray<Qubit>.Add(controls, new QArray<Qubit>(target2));
                this.CheckQubits(ctrls_1, ctrls_2);

                SafeControlled(controls,
                    () => ((IIntrinsicSWAP)this).Body(target1, target2),
                    (count, ids) => CSWAP(this.Id, count, ids, (uint)target1.Id, (uint)target2.Id));
            }
        }
    }
}
