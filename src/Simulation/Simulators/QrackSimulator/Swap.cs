using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SWAP")]
        private static extern uint SWAP(uint id, uint q1, uint q2);

        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CSWAP")]
        private static extern uint CSWAP(uint id, uint count, uint[] ctrls, uint q1, uint q2);

        public virtual void SWAP__Body(Qubit target1, Qubit target2)
        {
            var ctrls1 = new QArray<Qubit>(target1);
            var ctrls2 = new QArray<Qubit>(target2);
            this.CheckQubits(ctrls1, ctrls2);

            SWAP(this.Id, (uint)target1.Id, (uint)target2.Id);
        }

        public virtual void SWAP__ControlledBody(IQArray<Qubit> controls, Qubit target1, Qubit target2)
        {
            if ((controls == null) || (controls.Count == 0))
            {
                SWAP__Body(target1, target2);
            }
            else
            {
                var ctrls_1 = QArray<Qubit>.Add(controls, new QArray<Qubit>(target1));
                var ctrls_2 = QArray<Qubit>.Add(controls, new QArray<Qubit>(target2));
                this.CheckQubits(ctrls_1, ctrls_2);

                SafeControlled(controls,
                    () => SWAP__Body(target1, target2),
                    (count, ids) => CSWAP(this.Id, count, ids, (uint)target1.Id, (uint)target2.Id));
            }
        }

        public class QrackSimSwap : Intrinsic.SWAP
        {
            private QrackSimulator Simulator { get; }

            public QrackSimSwap(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override Func<(Qubit, Qubit), QVoid> __Body__ => (args) =>
            {
                var (q1, q2) = args;

                Simulator.SWAP__Body(q1, q2);

                return QVoid.Instance;
            };


            public override Func<(IQArray<Qubit>, (Qubit, Qubit)), QVoid> __ControlledBody__ => (args) =>
            {
                var (controls, (q1, q2)) = args;

                Simulator.SWAP__ControlledBody(controls, q1, q2);

                return QVoid.Instance;
            };
        }
    }
}
