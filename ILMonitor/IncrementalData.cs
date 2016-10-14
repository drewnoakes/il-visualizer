using System;
using System.Collections.Generic;

namespace ClrTest.Reflection {
    [Serializable]
    public class IncrementalMethodBodyInfo {
        [Serializable]
        public class AgingInstruction {
            public int Age;
            public string ILString;
        }

        public int Identity;
        public string TypeName;
        public AgingInstruction[] Instructions;
        public string MethodToString;

        // hidden field
        public List<int> LengthHistory;

        public static IncrementalMethodBodyInfo Create(MethodBodyInfo mbi) {
            return Create(mbi, null);
        }

        public static IncrementalMethodBodyInfo Create(MethodBodyInfo mbi, List<int> history) {
            IncrementalMethodBodyInfo imbi = new IncrementalMethodBodyInfo();

            imbi.Identity = mbi.Identity;
            imbi.TypeName = mbi.TypeName;
            imbi.MethodToString = mbi.MethodToString;

            int count = mbi.Instructions.Count;
            imbi.Instructions = new AgingInstruction[count];

            for (int i = 0; i < count; i++) {
                imbi.Instructions[i] = new AgingInstruction();
                imbi.Instructions[i].ILString = mbi.Instructions[i];
            }

            imbi.LengthHistory = new List<int>();
            if (history != null) {
                imbi.LengthHistory.AddRange(history);
            }
            imbi.LengthHistory.Add(count);

            for (int i = 0; i < imbi.LengthHistory.Count - 1; i++) {
                for (int a = imbi.LengthHistory[i]; a < imbi.LengthHistory[i + 1]; a++) {
                    imbi.Instructions[a].Age = i + 1;
                }
            }
            return imbi;
        }
    }
}
