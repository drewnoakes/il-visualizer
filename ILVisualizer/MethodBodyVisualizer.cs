using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Diagnostics;

[assembly: DebuggerVisualizer(
               typeof(ClrTest.Reflection.MethodBodyVisualizer),
               typeof(ClrTest.Reflection.MethodBodyObjectSource),
               Target = typeof(System.Reflection.MethodBase),
               Description = "IL Visualizer")
]

namespace ClrTest.Reflection
{
    public class MethodBodyVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            using (var viewer = new MethodBodyViewer())
            {
                viewer.SetObjectProvider(objectProvider);
                viewer.ShowDialog();
            }
        }
    }
}