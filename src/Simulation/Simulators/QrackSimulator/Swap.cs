using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        public class QrackSimSwap : Intrinsic.SWAP
        {
            [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SWAP")]
            private static extern uint SWAP(uint id, uint q1, uint q2);

            [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CSWAP")]
            private static extern uint CSWAP(uint id, uint count, uint[] ctrls, uint q1, uint q2);

            private QrackSimulator Simulator { get; }


            public QrackSimSwap(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override Func<(Qubit, Qubit), QVoid> __Body__ => (args) =>
            {
                var (q1, q2) = args;

                Simulator.CheckQubit(q1);

                SWAP(Simulator.Id, (uint)q1.Id, (uint)q2.Id);

                return QVoid.Instance;
            };


            public override Func<(IQArray<Qubit>, (Qubit, Qubit)), QVoid> __ControlledBody__ => (args) =>
            {
                var (ctrls, (q1, q2)) = args;

                Simulator.CheckQubits(ctrls, q1, q2);

                SafeControlled(ctrls,
                    () => this.Apply((q1, q2)),
                    (count, ids) => CSWAP(Simulator.Id, count, ids, (uint)q1.Id, (uint)q2.Id));

                return QVoid.Instance;
            };
        }
    }
}
