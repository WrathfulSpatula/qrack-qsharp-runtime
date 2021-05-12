// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Quantum.Intrinsic.Interfaces;
using Microsoft.Quantum.Simulation.Core;
using System.Runtime.InteropServices;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetPermutation")]
        private static extern void SetPermutation(uint sid, uint p);

        public void ResetAll__Body(IQArray<Qubit> targets)
        {
            if (targets.Count == 0)
            {
                return;
            }

            for (int i = 0; i < targets.Length; i++)
            {
                this.CheckQubit(targets[i]);
            }

            if (targets.Count == QubitManager.AllocatedQubitsCount)
            {
                SetPermutation(this.Id, 0);
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].IsMeasured = true;
                }
                return;
            }

            for (int i = 0; i < targets.Length; i++)
            {
                var res = M(this.Id, (uint)targets[i].Id);
                if (res == 1)
                {
                    X(this.Id, (uint)targets[i].Id);
                    targets[i].IsMeasured = true;
                }
            }
        }

        public class QrackSimReset : Intrinsic.ResetAll
        {
            private QrackSimulator Simulator { get; }

            public QrackSimReset(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override System.Func<IQArray<Qubit>, QVoid> __Body__ => (q) =>
            {
                Simulator.ResetAll__Body(q);

                return QVoid.Instance;
            };

        }
    }
}
