using System;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using ILDebugging.Visualizer;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace ILDebugging.Sample
{
    internal class Foo
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public string MyMethod(string x)
        {
            Console.WriteLine(x);
            return x;
        }
    }

    internal class Program
    {
        private static void Main()
        {
            var dm = new DynamicMethod("MyMethodWrapper", typeof(object), new[] {typeof(object[])}, typeof(Program), skipVisibility: true);
            var il = dm.GetILGenerator();
            var l1 = il.DefineLabel();
            var returnLocal = il.DeclareLocal(typeof(object));

            // grab the method parameters of the method we wish to wrap
            var method = typeof(Foo).GetMethod(nameof(Foo.MyMethod));
            var methodParameters = method.GetParameters();
            var parameterLength = methodParameters.Length;

            // check to see if the call to MyMethodWrapper has the required amount of arguments in the object[] array.
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldlen);
            il.Emit(OpCodes.Conv_I4);
            il.Emit(OpCodes.Ldc_I4, parameterLength + 1);
            il.Emit(OpCodes.Beq_S, l1);
            il.Emit(OpCodes.Ldstr, "insufficient arguments");
            il.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new[] {typeof(string)}));
            il.Emit(OpCodes.Throw);
            il.MarkLabel(l1);

            // pull out the Foo instance from the first element in the object[] args array
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ldelem_Ref);
            // cast the instance to Foo
            il.Emit(OpCodes.Castclass, typeof(Foo));

            // pull out the parameters to the instance method call and push them on to the IL stack
            for (var i = 0; i < parameterLength; i++)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldc_I4, i + 1);
                il.Emit(OpCodes.Ldelem_Ref);

                // we've special cased it, for this code example
                if (methodParameters[i].ParameterType == typeof(string))
                {
                    il.Emit(OpCodes.Castclass, typeof(string));
                }

                // test or switch on parameter types, you'll need to cast to the respective type
                // ...
            }

            // call the wrapped method
            il.Emit(OpCodes.Call, method);
            // return what the method invocation returned
            il.Emit(OpCodes.Stloc, returnLocal);
            il.Emit(OpCodes.Ldloc, returnLocal);
            il.Emit(OpCodes.Ret);

            // Show the visualizer (via code)
            var developmentHost = new VisualizerDevelopmentHost(
                dm,
                typeof(MethodBodyVisualizer),
                typeof(MethodBodyObjectSource)
            );

            developmentHost.ShowVisualizer();
        }
    }
}