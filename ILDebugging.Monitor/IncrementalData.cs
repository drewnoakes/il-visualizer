using System;
using System.Collections.Generic;
using System.Linq;
using ILDebugging.Visualizer;

namespace ILDebugging.Monitor
{
    [Serializable]
    public class IncrementalMethodBodyInfo
    {
        [Serializable]
        public class AgingInstruction
        {
            public int Age;
            public string ILString;
        }

        public int Identity;
        public string TypeName;
        public AgingInstruction[] Instructions;
        public string MethodToString;

        // hidden field
        public List<int> LengthHistory;

        public static IncrementalMethodBodyInfo Create(MethodBodyInfo mbi, List<int> history = null)
        {
            var imbi = new IncrementalMethodBodyInfo
            {
                Identity = mbi.Identity,
                TypeName = mbi.TypeName,
                MethodToString = mbi.MethodToString,
                Instructions = mbi.Instructions.Select(i => new AgingInstruction {ILString = i}).ToArray(),
                LengthHistory = new List<int>((IEnumerable<int>)history ?? new int[0]) {mbi.Instructions.Count}
            };

            for (var i = 0; i < imbi.LengthHistory.Count - 1; i++)
            {
                for (var a = imbi.LengthHistory[i]; a < imbi.LengthHistory[i + 1]; a++)
                {
                    imbi.Instructions[a].Age = i + 1;
                }
            }

            return imbi;
        }
    }
}
