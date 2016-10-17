using System.Diagnostics;
using System.Reflection;
using ILDebugging.Visualizer;
using Microsoft.VisualStudio.DebuggerVisualizers;

[assembly: DebuggerVisualizer(
               typeof(MethodBodyVisualizer),
               typeof(MethodBodyObjectSource),
               Target = typeof(MethodBase),
               Description = "IL Visualizer")
]

namespace ILDebugging.Visualizer
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