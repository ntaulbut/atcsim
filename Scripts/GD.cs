using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

public static partial class GD
{

    //
    // Summary:
    //     Decodes a byte array back to a Variant value. If allowObjects is true decoding
    //     objects is allowed. WARNING: Deserialized object can contain code which gets
    //     executed. Do not set allowObjects to true if the serialized object comes from
    //     untrusted sources to avoid potential security threats (remote code execution).
    //
    // Parameters:
    //   bytes:
    //     Byte array that will be decoded to a Variant.
    //
    //   allowObjects:
    //     If objects should be decoded.
    //
    // Returns:
    //     The decoded Variant.
    public static Variant BytesToVar(Span<byte> bytes, bool allowObjects = false) => Godot.GD.BytesToVar(bytes, allowObjects);

    //
    // Summary:
    //     Converts from a Variant type to another in the best way possible. The type parameter
    //     uses the Godot.Variant.Type values.
    //
    // Returns:
    //     The Variant converted to the given type.
    public static Variant Convert(Variant what, Variant.Type type) => Godot.GD.Convert(what, type);

    //
    // Summary:
    //     Converts from decibels to linear energy (audio).
    //
    // Parameters:
    //   db:
    //     Decibels to convert.
    //
    // Returns:
    //     Audio volume as linear energy.
    public static float DbToLinear(float db) => Godot.GD.DbToLinear(db);
    private static string[] GetPrintParams(object[] parameters)
    {
        if (parameters == null)
        {
            return new string[1] { "null" };
        }

        return System.Array.ConvertAll(parameters, (object x) => x?.ToString() ?? "null");
    }

    //
    // Summary:
    //     Returns the integer hash of the variable passed.
    //
    // Parameters:
    //   var:
    //     Variable that will be hashed.
    //
    // Returns:
    //     Hash of the variable passed.
    public static int Hash(Variant var) => Godot.GD.Hash(var);

    //
    // Summary:
    //     Returns the Godot.Object that corresponds to instanceId. All Objects have a unique
    //     instance ID.
    //
    // Parameters:
    //   instanceId:
    //     Instance ID of the Object to retrieve.
    //
    // Returns:
    //     The Godot.Object instance.
    public static Godot.Object InstanceFromId(ulong instanceId) => Godot.GD.InstanceFromId(instanceId);

    //
    // Summary:
    //     Converts from linear energy to decibels (audio). This can be used to implement
    //     volume sliders that behave as expected (since volume isn't linear).
    //
    // Parameters:
    //   linear:
    //     The linear energy to convert.
    //
    // Returns:
    //     Audio as decibels.
    public static float LinearToDb(float linear) => Godot.GD.LinearToDb(linear);

    //
    // Summary:
    //     Loads a resource from the filesystem located at path. The resource is loaded
    //     on the method call (unless it's referenced already elsewhere, e.g. in another
    //     script or in the scene), which might cause slight delay, especially when loading
    //     scenes. To avoid unnecessary delays when loading something multiple times, either
    //     store the resource in a variable. Note: Resource paths can be obtained by right-clicking
    //     on a resource in the FileSystem dock and choosing "Copy Path" or by dragging
    //     the file from the FileSystem dock into the script. Important: The path must be
    //     absolute, a local path will just return null. This method is a simplified version
    //     of Godot.ResourceLoader.Load(System.String,System.String,Godot.ResourceLoader.CacheMode),
    //     which can be used for more advanced scenarios.
    //
    // Parameters:
    //   path:
    //     Path of the Godot.Resource to load.
    //
    // Returns:
    //     The loaded Godot.Resource.
    public static Resource Load(string path) => Godot.GD.Load(path);

    //
    // Summary:
    //     Loads a resource from the filesystem located at path. The resource is loaded
    //     on the method call (unless it's referenced already elsewhere, e.g. in another
    //     script or in the scene), which might cause slight delay, especially when loading
    //     scenes. To avoid unnecessary delays when loading something multiple times, either
    //     store the resource in a variable. Note: Resource paths can be obtained by right-clicking
    //     on a resource in the FileSystem dock and choosing "Copy Path" or by dragging
    //     the file from the FileSystem dock into the script. Important: The path must be
    //     absolute, a local path will just return null. This method is a simplified version
    //     of Godot.ResourceLoader.Load(System.String,System.String,Godot.ResourceLoader.CacheMode),
    //     which can be used for more advanced scenarios.
    //
    // Parameters:
    //   path:
    //     Path of the Godot.Resource to load.
    //
    // Type parameters:
    //   T:
    //     The type to cast to. Should be a descendant of Godot.Resource.
    public static T Load<T>(string path) where T : class => Godot.GD.Load<T>(path);

    //
    // Summary:
    //     Pushes an error message to Godot's built-in debugger and to the OS terminal.
    //     Note: Errors printed this way will not pause project execution.
    //
    // Parameters:
    //   message:
    //     Error message.
    public static void PushError(string message)
    {
        Debugger.Log(0, "err", "Error: " + String.Concat(message));
        Godot.GD.PushError(message);
    }


    //
    // Summary:
    //     Pushes a warning message to Godot's built-in debugger and to the OS terminal.
    //
    // Parameters:
    //   message:
    //     Warning message.
    public static void PushWarning(string message)
    {
        Debugger.Log(1, "wrn", "Warning: " + String.Concat(message) + "\r\n");
        Godot.GD.PushWarning(message);
    }

    //
    // Summary:
    //     Converts one or more arguments of any type to string in the best way possible
    //     and prints them to the console. Note: Consider using Godot.GD.PushError(System.String)
    //     and Godot.GD.PushWarning(System.String) to print error and warning messages instead
    //     of Godot.GD.Print(System.Object[]). This distinguishes them from print messages
    //     used for debugging purposes, while also displaying a stack trace when an error
    //     or warning is printed.
    //
    // Parameters:
    //   what:
    //     Arguments that will be printed.
    public static void Print(params object[] what)
    {
        String[] strings = GetPrintParams(what);
        Debugger.Log(2, "inf", String.Concat(strings) + "\r\n");
        Godot.GD.Print(what);
    }

    //
    // Summary:
    //     Converts one or more arguments of any type to string in the best way possible
    //     and prints them to the console. The following BBCode tags are supported: b, i,
    //     u, s, indent, code, url, center, right, color, bgcolor, fgcolor. Color tags only
    //     support named colors such as red, not hexadecimal color codes. Unsupported tags
    //     will be left as-is in standard output. When printing to standard output, the
    //     supported subset of BBCode is converted to ANSI escape codes for the terminal
    //     emulator to display. Displaying ANSI escape codes is currently only supported
    //     on Linux and macOS. Support for ANSI escape codes may vary across terminal emulators,
    //     especially for italic and strikethrough. Note: Consider using Godot.GD.PushError(System.String)
    //     and Godot.GD.PushWarning(System.String) to print error and warning messages instead
    //     of Godot.GD.Print(System.Object[]) or Godot.GD.PrintRich(System.Object[]). This
    //     distinguishes them from print messages used for debugging purposes, while also
    //     displaying a stack trace when an error or warning is printed.
    //
    // Parameters:
    //   what:
    //     Arguments that will be printed.
    public static void PrintRich(params object[] what)
    {
        String[] strings = GetPrintParams(what);
        Debugger.Log(2, "inf", String.Concat(strings) + "\r\n");
        Godot.GD.PrintRich(what);
    }

    //
    // Summary:
    //     Prints the current stack trace information to the console.
    public static void PrintStack() => Print(System.Environment.StackTrace);

    //
    // Summary:
    //     Prints one or more arguments to strings in the best way possible to standard
    //     error line.
    //
    // Parameters:
    //   what:
    //     Arguments that will be printed.
    public static void PrintErr(params object[] what)
    {
        String[] strings = GetPrintParams(what);
        Debugger.Log(0, "err", "Error: " + String.Concat(strings) + "\r\n");
        Godot.GD.PrintErr(what);
    }

    //
    // Summary:
    //     Prints one or more arguments to strings in the best way possible to console.
    //     No newline is added at the end. Note: Due to limitations with Godot's built-in
    //     console, this only prints to the terminal. If you need to print in the editor,
    //     use another method, such as Godot.GD.Print(System.Object[]).
    //
    // Parameters:
    //   what:
    //     Arguments that will be printed.
    public static void PrintRaw(params object[] what)
    {
        String[] strings = GetPrintParams(what);
        Debugger.Log(2, "inf", String.Concat(strings));
        Godot.GD.PrintRich(what);
    }

    //
    // Summary:
    //     Prints one or more arguments to the console with a space between each argument.
    //
    // Parameters:
    //   what:
    //     Arguments that will be printed.
    public static void PrintS(params object[] what)
    {
        String[] strings = GetPrintParams(what);
        Debugger.Log(2, "err", "Error: " + String.Join(' ', strings) + "\r\n");
        Godot.GD.PrintErr(what);
    }

    //
    // Summary:
    //     Prints one or more arguments to the console with a space between each argument.
    //
    // Parameters:
    //   what:
    //     Arguments that will be printed.
    public static void PrintT(params object[] what)
    {
        String[] strings = GetPrintParams(what);
        Debugger.Log(2, "err", "Error: " + String.Join('\t', strings) + "\r\n");
        Godot.GD.PrintErr(what);
    }

    //
    // Summary:
    //     Returns a random floating point value between 0.0 and 1.0 (inclusive).
    //
    // Returns:
    //     A random float number.
    public static float Randf() => Godot.GD.Randf();

    //
    // Summary:
    //     Returns a normally-distributed pseudo-random number, using Box-Muller transform
    //     with the specified mean and a standard deviation. This is also called Gaussian
    //     distribution.
    //
    // Returns:
    //     A random normally-distributed float number.
    public static double Randfn(double mean, double deviation) => Godot.GD.Randfn(mean, deviation);

    //
    // Summary:
    //     Returns a random unsigned 32-bit integer. Use remainder to obtain a random value
    //     in the interval [0, N - 1] (where N is smaller than 2^32).
    //
    // Returns:
    //     A random uint number.
    public static uint Randi() => Godot.GD.Randi();

    //
    // Summary:
    //     Randomizes the seed (or the internal state) of the random number generator. Current
    //     implementation reseeds using a number based on time. Note: This method is called
    //     automatically when the project is run. If you need to fix the seed to have reproducible
    //     results, use Godot.GD.Seed(System.UInt64) to initialize the random number generator.
    public static void Randomize() => Godot.GD.Randomize();

    //
    // Summary:
    //     Returns a random floating point value on the interval between from and to (inclusive).
    //
    // Returns:
    //     A random double number inside the given range.
    public static double RandRange(double from, double to) => Godot.GD.RandRange(from, to);

    //
    // Summary:
    //     Returns a random signed 32-bit integer between from and to (inclusive). If to
    //     is lesser than from, they are swapped.
    //
    // Returns:
    //     A random int number inside the given range.
    public static int RandRange(int from, int to) => Godot.GD.RandRange(from, to);

    //
    // Summary:
    //     Returns a random unsigned 32-bit integer, using the given seed.
    //
    // Parameters:
    //   seed:
    //     Seed to use to generate the random number. If a different seed is used, its value
    //     will be modified.
    //
    // Returns:
    //     A random uint number.
    public static uint RandFromSeed(ref ulong seed) => Godot.GD.RandFromSeed(ref seed);

    //
    // Summary:
    //     Returns a System.Collections.Generic.IEnumerable`1 that iterates from 0 to end
    //     in steps of 1.
    //
    // Parameters:
    //   end:
    //     The last index.
    public static IEnumerable<int> Range(int end) => Godot.GD.Range(end);

    //
    // Summary:
    //     Returns a System.Collections.Generic.IEnumerable`1 that iterates from start to
    //     end in steps of 1.
    //
    // Parameters:
    //   start:
    //     The first index.
    //
    //   end:
    //     The last index.
    public static IEnumerable<int> Range(int start, int end) => Godot.GD.Range(start, end);

    //
    // Summary:
    //     Returns a System.Collections.Generic.IEnumerable`1 that iterates from start to
    //     end in steps of step.
    //
    // Parameters:
    //   start:
    //     The first index.
    //
    //   end:
    //     The last index.
    //
    //   step:
    //     The amount by which to increment the index on each iteration.
    public static IEnumerable<int> Range(int start, int end, int step) => Godot.GD.Range(start, end, step);

    //
    // Summary:
    //     Sets seed for the random number generator.
    //
    // Parameters:
    //   seed:
    //     Seed that will be used.
    public static void Seed(ulong seed) => Godot.GD.Seed(seed);

    //
    // Summary:
    //     Converts one or more arguments of any type to string in the best way possible.
    //
    // Parameters:
    //   what:
    //     Arguments that will converted to string.
    //
    // Returns:
    //     The string formed by the given arguments.
    public static string Str(params Variant[] what) => Godot.GD.Str(what);

    //
    // Summary:
    //     Converts a formatted string that was returned by Godot.GD.VarToStr(Godot.Variant)
    //     to the original value.
    //
    // Parameters:
    //   str:
    //     String that will be converted to Variant.
    //
    // Returns:
    //     The decoded Variant.
    public static Variant StrToVar(string str) => Godot.GD.StrToVar(str);

    //
    // Summary:
    //     Encodes a Variant value to a byte array. If fullObjects is true encoding objects
    //     is allowed (and can potentially include code). Deserialization can be done with
    //     Godot.GD.BytesToVar(System.Span{System.Byte},System.Boolean).
    //
    // Parameters:
    //   var:
    //     Variant that will be encoded.
    //
    //   fullObjects:
    //     If objects should be serialized.
    //
    // Returns:
    //     The Variant encoded as an array of bytes.
    public static byte[] VarToBytes(Variant var, bool fullObjects = false) => Godot.GD.VarToBytes(var, fullObjects);

    //
    // Summary:
    //     Converts a Variant var to a formatted string that can later be parsed using Godot.GD.StrToVar(System.String).
    //
    // Parameters:
    //   var:
    //     Variant that will be converted to string.
    //
    // Returns:
    //     The Variant encoded as a string.
    public static string VarToStr(Variant var) => Godot.GD.VarToStr(var);

    //
    // Summary:
    //     Get the Godot.Variant.Type that corresponds for the given System.Type.
    //
    // Returns:
    //     The Godot.Variant.Type for the given type.
    public static Variant.Type TypeToVariantType(Type type) => Godot.GD.TypeToVariantType(type);

    //
    // Summary:
    //     Returns true if this byte array is empty or doesn't exist.
    //
    // Parameters:
    //   instance:
    //     The byte array check.
    //
    // Returns:
    //     Whether or not the array is empty.
    public static bool IsEmpty(this byte[] instance) => Godot.GD.IsEmpty(instance);

    //
    // Summary:
    //     Converts this byte array to a string delimited by the given string.
    //
    // Parameters:
    //   instance:
    //     The byte array to convert.
    //
    //   delimiter:
    //     The delimiter to use between items.
    //
    // Returns:
    //     A single string with all items.
    public static string Join(this byte[] instance, string delimiter = ", ") => Godot.GD.Join(instance, delimiter);

    //
    // Summary:
    //     Converts this byte array to a string with brackets.
    //
    // Parameters:
    //   instance:
    //     The byte array to convert.
    //
    // Returns:
    //     A single string with all items.
    public static string Stringify(this byte[] instance) => Godot.GD.Stringify(instance);

    //
    // Summary:
    //     Returns true if this int array is empty or doesn't exist.
    //
    // Parameters:
    //   instance:
    //     The int array check.
    //
    // Returns:
    //     Whether or not the array is empty.
    public static bool IsEmpty(this int[] instance) => Godot.GD.IsEmpty(instance);

    //
    // Summary:
    //     Converts this int array to a string delimited by the given string.
    //
    // Parameters:
    //   instance:
    //     The int array to convert.
    //
    //   delimiter:
    //     The delimiter to use between items.
    //
    // Returns:
    //     A single string with all items.
    public static string Join(this int[] instance, string delimiter = ", ") => Godot.GD.Join(instance, delimiter);

    //
    // Summary:
    //     Converts this int array to a string with brackets.
    //
    // Parameters:
    //   instance:
    //     The int array to convert.
    //
    // Returns:
    //     A single string with all items.
    public static string Stringify(this int[] instance) => Godot.GD.Stringify(instance);


    //
    // Summary:
    //     Returns true if this long array is empty or doesn't exist.
    //
    // Parameters:
    //   instance:
    //     The long array check.
    //
    // Returns:
    //     Whether or not the array is empty.
    public static bool IsEmpty(this long[] instance) => Godot.GD.IsEmpty(instance);

    //
    // Summary:
    //     Converts this long array to a string delimited by the given string.
    //
    // Parameters:
    //   instance:
    //     The long array to convert.
    //
    //   delimiter:
    //     The delimiter to use between items.
    //
    // Returns:
    //     A single string with all items.
    public static string Join(this long[] instance, string delimiter = ", ") => Godot.GD.Join(instance, delimiter);

    //
    // Summary:
    //     Converts this long array to a string with brackets.
    //
    // Parameters:
    //   instance:
    //     The long array to convert.
    //
    // Returns:
    //     A single string with all items.
    public static string Stringify(this long[] instance) => Godot.GD.Stringify(instance);

    //
    // Summary:
    //     Returns true if this float array is empty or doesn't exist.
    //
    // Parameters:
    //   instance:
    //     The float array check.
    //
    // Returns:
    //     Whether or not the array is empty.
    public static bool IsEmpty(this float[] instance) => Godot.GD.IsEmpty(instance);

    //
    // Summary:
    //     Converts this float array to a string delimited by the given string.
    //
    // Parameters:
    //   instance:
    //     The float array to convert.
    //
    //   delimiter:
    //     The delimiter to use between items.
    //
    // Returns:
    //     A single string with all items.
    public static string Join(this float[] instance, string delimiter = ", ") => Godot.GD.Join(instance, delimiter);

    //
    // Summary:
    //     Converts this float array to a string with brackets.
    //
    // Parameters:
    //   instance:
    //     The float array to convert.
    //
    // Returns:
    //     A single string with all items.
    public static string Stringify(this float[] instance) => Godot.GD.Stringify(instance);

    //
    // Summary:
    //     Returns true if this double array is empty or doesn't exist.
    //
    // Parameters:
    //   instance:
    //     The double array check.
    //
    // Returns:
    //     Whether or not the array is empty.
    public static bool IsEmpty(this double[] instance) => Godot.GD.IsEmpty(instance);

    //
    // Summary:
    //     Converts this double array to a string delimited by the given string.
    //
    // Parameters:
    //   instance:
    //     The double array to convert.
    //
    //   delimiter:
    //     The delimiter to use between items.
    //
    // Returns:
    //     A single string with all items.
    public static string Join(this double[] instance, string delimiter = ", ") => Godot.GD.Join(instance, delimiter);

    //
    // Summary:
    //     Converts this double array to a string with brackets.
    //
    // Parameters:
    //   instance:
    //     The double array to convert.
    //
    // Returns:
    //     A single string with all items.
    public static string Stringify(this double[] instance) => Godot.GD.Stringify(instance);

    //
    // Summary:
    //     Returns true if this string array is empty or doesn't exist.
    //
    // Parameters:
    //   instance:
    //     The string array check.
    //
    // Returns:
    //     Whether or not the array is empty.
    public static bool IsEmpty(this string[] instance) => Godot.GD.IsEmpty(instance);

    //
    // Summary:
    //     Converts this string array to a string delimited by the given string.
    //
    // Parameters:
    //   instance:
    //     The string array to convert.
    //
    //   delimiter:
    //     The delimiter to use between items.
    //
    // Returns:
    //     A single string with all items.
    public static string Join(this string[] instance, string delimiter = ", ") => Godot.GD.Join(instance, delimiter);

    //
    // Summary:
    //     Converts this string array to a string with brackets.
    //
    // Parameters:
    //   instance:
    //     The string array to convert.
    //
    // Returns:
    //     A single string with all items.
    public static string Stringify(this string[] instance) => Godot.GD.Stringify(instance);

    //
    // Summary:
    //     Returns true if this Color array is empty or doesn't exist.
    //
    // Parameters:
    //   instance:
    //     The Color array check.
    //
    // Returns:
    //     Whether or not the array is empty.
    public static bool IsEmpty(this Color[] instance) => Godot.GD.IsEmpty(instance);

    //
    // Summary:
    //     Converts this Color array to a string delimited by the given string.
    //
    // Parameters:
    //   instance:
    //     The Color array to convert.
    //
    //   delimiter:
    //     The delimiter to use between items.
    //
    // Returns:
    //     A single string with all items.
    public static string Join(this Color[] instance, string delimiter = ", ") => Godot.GD.Join(instance, delimiter);

    //
    // Summary:
    //     Converts this Color array to a string with brackets.
    //
    // Parameters:
    //   instance:
    //     The Color array to convert.
    //
    // Returns:
    //     A single string with all items.
    public static string Stringify(this Color[] instance) => Godot.GD.Stringify(instance);

    //
    // Summary:
    //     Returns true if this Vector2 array is empty or doesn't exist.
    //
    // Parameters:
    //   instance:
    //     The Vector2 array check.
    //
    // Returns:
    //     Whether or not the array is empty.
    public static bool IsEmpty(this Vector2[] instance) => Godot.GD.IsEmpty(instance);

    //
    // Summary:
    //     Converts this Vector2 array to a string delimited by the given string.
    //
    // Parameters:
    //   instance:
    //     The Vector2 array to convert.
    //
    //   delimiter:
    //     The delimiter to use between items.
    //
    // Returns:
    //     A single string with all items.
    public static string Join(this Vector2[] instance, string delimiter = ", ") => Godot.GD.Join(instance, delimiter);

    //
    // Summary:
    //     Converts this Vector2 array to a string with brackets.
    //
    // Parameters:
    //   instance:
    //     The Vector2 array to convert.
    //
    // Returns:
    //     A single string with all items.
    public static string Stringify(this Vector2[] instance) => Godot.GD.Stringify(instance);

    //
    // Summary:
    //     Returns true if this Vector2i array is empty or doesn't exist.
    //
    // Parameters:
    //   instance:
    //     The Vector2i array check.
    //
    // Returns:
    //     Whether or not the array is empty.
    public static bool IsEmpty(this Vector2i[] instance) => Godot.GD.IsEmpty(instance);

    //
    // Summary:
    //     Converts this Vector2i array to a string delimited by the given string.
    //
    // Parameters:
    //   instance:
    //     The Vector2i array to convert.
    //
    //   delimiter:
    //     The delimiter to use between items.
    //
    // Returns:
    //     A single string with all items.
    public static string Join(this Vector2i[] instance, string delimiter = ", ") => Godot.GD.Join(instance, delimiter);

    //
    // Summary:
    //     Converts this Vector2i array to a string with brackets.
    //
    // Parameters:
    //   instance:
    //     The Vector2i array to convert.
    //
    // Returns:
    //     A single string with all items.
    public static string Stringify(this Vector2i[] instance) => Godot.GD.Stringify(instance);

    //
    // Summary:
    //     Returns true if this Vector3 array is empty or doesn't exist.
    //
    // Parameters:
    //   instance:
    //     The Vector3 array check.
    //
    // Returns:
    //     Whether or not the array is empty.
    public static bool IsEmpty(this Vector3[] instance) => Godot.GD.IsEmpty(instance);

    //
    // Summary:
    //     Converts this Vector3 array to a string delimited by the given string.
    //
    // Parameters:
    //   instance:
    //     The Vector3 array to convert.
    //
    //   delimiter:
    //     The delimiter to use between items.
    //
    // Returns:
    //     A single string with all items.
    public static string Join(this Vector3[] instance, string delimiter = ", ") => Godot.GD.Join(instance, delimiter);

    //
    // Summary:
    //     Converts this Vector3 array to a string with brackets.
    //
    // Parameters:
    //   instance:
    //     The Vector3 array to convert.
    //
    // Returns:
    //     A single string with all items.
    public static string Stringify(this Vector3[] instance) => Godot.GD.Stringify(instance);

    //
    // Summary:
    //     Returns true if this Vector3i array is empty or doesn't exist.
    //
    // Parameters:
    //   instance:
    //     The Vector3i array check.
    //
    // Returns:
    //     Whether or not the array is empty.
    public static bool IsEmpty(this Vector3i[] instance) => Godot.GD.IsEmpty(instance);

    //
    // Summary:
    //     Converts this Vector3i array to a string delimited by the given string.
    //
    // Parameters:
    //   instance:
    //     The Vector3i array to convert.
    //
    //   delimiter:
    //     The delimiter to use between items.
    //
    // Returns:
    //     A single string with all items.
    public static string Join(this Vector3i[] instance, string delimiter = ", ") => Godot.GD.Join(instance, delimiter);

    //
    // Summary:
    //     Converts this Vector3i array to a string with brackets.
    //
    // Parameters:
    //   instance:
    //     The Vector3i array to convert.
    //
    // Returns:
    //     A single string with all items.
    public static string Stringify(this Vector3i[] instance) => Godot.GD.Stringify(instance);

    //
    // Summary:
    //     Returns true if this Vector4 array is empty or doesn't exist.
    //
    // Parameters:
    //   instance:
    //     The Vector4 array check.
    //
    // Returns:
    //     Whether or not the array is empty.
    public static bool IsEmpty(this Vector4[] instance) => Godot.GD.IsEmpty(instance);

    //
    // Summary:
    //     Converts this Vector4 array to a string delimited by the given string.
    //
    // Parameters:
    //   instance:
    //     The Vector4 array to convert.
    //
    //   delimiter:
    //     The delimiter to use between items.
    //
    // Returns:
    //     A single string with all items.
    public static string Join(this Vector4[] instance, string delimiter = ", ") => Godot.GD.Join(instance, delimiter);

    //
    // Summary:
    //     Converts this Vector4 array to a string with brackets.
    //
    // Parameters:
    //   instance:
    //     The Vector4 array to convert.
    //
    // Returns:
    //     A single string with all items.
    public static string Stringify(this Vector4[] instance) => Godot.GD.Stringify(instance);

    //
    // Summary:
    //     Returns true if this Vector4i array is empty or doesn't exist.
    //
    // Parameters:
    //   instance:
    //     The Vector4i array check.
    //
    // Returns:
    //     Whether or not the array is empty.
    public static bool IsEmpty(this Vector4i[] instance) => Godot.GD.IsEmpty(instance);

    //
    // Summary:
    //     Converts this Vector4i array to a string delimited by the given string.
    //
    // Parameters:
    //   instance:
    //     The Vector4i array to convert.
    //
    //   delimiter:
    //     The delimiter to use between items.
    //
    // Returns:
    //     A single string with all items.
    public static string Join(this Vector4i[] instance, string delimiter = ", ") => Godot.GD.Join(instance, delimiter);

    //
    // Summary:
    //     Converts this Vector4i array to a string with brackets.
    //
    // Parameters:
    //   instance:
    //     The Vector4i array to convert.
    //
    // Returns:
    //     A single string with all items.
    public static string Stringify(this Vector4i[] instance) => Godot.GD.Stringify(instance);
}