using System;
using System.Reflection.Emit;
using System.Reflection;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Runtime.CompilerServices;

class Foo
{

    [MethodImpl(MethodImplOptions.NoInlining)]
    public string MyMethod(string x)
    {
        Console.WriteLine(x);
        return x;
    }
}

class Program
{
    static void Main(string[] args)
    {
        M1();
        M2();
    }

    static void M1()
    {
        DynamicMethod dm = new DynamicMethod("HelloWorld", typeof(void), new Type[] { }, typeof(Program), false);

        ILGenerator il = dm.GetILGenerator();

        TestShowVisualizer(dm);
        il.Emit(OpCodes.Ldstr, "hello, world");
        TestShowVisualizer(dm);
        il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
        TestShowVisualizer(dm);
        il.Emit(OpCodes.Ret);
        TestShowVisualizer(dm);
        dm.Invoke(null, null);
    }

    static void M2()
    {
        // DynamicMethod wrapper method

        DynamicMethod dm = new DynamicMethod("MyMethodWrapper", typeof(object), new Type[] { typeof(object[]) }, typeof(Program), true);
        ILGenerator il = dm.GetILGenerator();
        Label l1 = il.DefineLabel();
        LocalBuilder returnLocal = il.DeclareLocal(typeof(object));


        // grab the method parameters of the method we wish to wrap
        ParameterInfo[] methodParameters = typeof(Foo).GetMethod("MyMethod").GetParameters();
        int parameterLength = methodParameters.Length;
        MethodInfo method = typeof(Foo).GetMethod("MyMethod");

        // check to see if the call to MyMethodWrapper has the required amount of arguments in the object[] array.
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldlen);
        il.Emit(OpCodes.Conv_I4);
        il.Emit(OpCodes.Ldc_I4, parameterLength + 1);
        il.Emit(OpCodes.Beq_S, l1);
        il.Emit(OpCodes.Ldstr, "insufficient arguments");
        il.Emit(OpCodes.Newobj, typeof(System.ArgumentException).GetConstructor(new Type[] { typeof(string) }));
        il.Emit(OpCodes.Throw);
        il.MarkLabel(l1);

        // pull out the Foo instance from the first element in the object[] args array
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldc_I4_0);
        il.Emit(OpCodes.Ldelem_Ref);
        // cast the instance to Foo
        il.Emit(OpCodes.Castclass, typeof(Foo));

        // pull out the parameters to the instance method call and push them on to the IL stack
        for (int i = 0; i < parameterLength; i++)
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
        TestShowVisualizer(dm);
    }

    public static void TestShowVisualizer(object objectToVisualize)
    {
        VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(ClrTest.Reflection.MethodBodyVisualizer), typeof(ClrTest.Reflection.MethodBodyObjectSource));
        visualizerHost.ShowVisualizer();
    }
}
