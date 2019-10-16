//2019 Eric Johnson

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GPU_DLL_Demo
{
    public class Compute
    {
        const string dll = "cuda_computation.dll";
        [DllImport(dll, CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int init(int array_size_local);

        [DllImport(dll, CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern void cleanup();

        [DllImport(dll, CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern void loadArray(int[] data);

        [DllImport(dll, CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern void compute(int adder, int[] buffer);

        int array_size = 0;

        public Compute(int array_size_l)
        {
            array_size = array_size_l;

            init(array_size); //set up the cuda
        }

        ~Compute()  // destructor
        {
            Debug.WriteLine("Cleaning up GPU");
            cleanup(); //clean up the GPU
        }

        public int[] GetComputed(int adder)
        {
            int[] output = new int[array_size];
            compute(adder, output);
            return output;
		}

        public void LoadArrayGPU(int[] array)
        {
            loadArray(array);
        }

    }
}
