// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators.Qrack
{
    public partial class QrackSimulator
    {
        public class QrackSimrandom : Quantum.Intrinsic.Random
        {
            [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "random_choice")]
            private static extern Int64 random_choice(uint id, Int64 size, double[] p);

            private uint SimulatorId { get; }


            public QrackSimrandom(QrackSimulator m) : base(m)
            {
                this.SimulatorId = m.Id;
            }

            public override Func<IQArray<double>, Int64> __Body__ => (p) =>
            {
                return random_choice(this.SimulatorId, p.Length, p.ToArray());
            };            
        }
    }
}
