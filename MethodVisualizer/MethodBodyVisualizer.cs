using System;
using System.Diagnostics;
using Microsoft.VisualStudio.DebuggerVisualizers;

[assembly: DebuggerVisualizer(
    typeof(DynamicMethodVisualizer.MethodBodyVisualizer),
    typeof(DynamicMethodVisualizer.MethodBodyObjectSource),
    Target = typeof(System.Reflection.Emit.DynamicMethod),
    Description = "DynamicMethod Visualizer")
]

namespace DynamicMethodVisualizer
{
    public class MethodBodyVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            using (MethodBodyViewer viewer = new MethodBodyViewer())
            {
                viewer.SetObjectProvider(objectProvider);
                viewer.ShowDialog();
            }
        }
    }
}
