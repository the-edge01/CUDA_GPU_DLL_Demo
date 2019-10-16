//2019 Eric Johnson


#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "helper_cuda.h"

#include "cuda_computation_common.h"

////////////////////////////////////////////////////////////////////////////////
// GPU-specific defines
////////////////////////////////////////////////////////////////////////////////
//Round a / b to nearest higher integer value
inline int iDivUp(int a, int b)
{
	return (a % b != 0) ? (a / b + 1) : (a / b);
}


__global__ void GPU_DLL_DEMO_kernelCompute(
	int* d_input,
	int* d_output,
	const int adder,
	const int length
)
{
	const int n = (threadIdx.x + blockIdx.x * blockDim.x);

	if (n > length){//handle out of rangeP
		return;
	}

	d_output[n] = d_input[n] + adder;

}

extern "C" void GPU_DLL_DEMO_GPU(
	int* d_input,
	int* d_output,
	const int adder,
	const int length
)
{
	cudaFuncSetCacheConfig(GPU_DLL_DEMO_kernelCompute, cudaFuncCachePreferL1);
	
	dim3 threads(blocksize);
	dim3 blocks(iDivUp(length, threads.x));

	GPU_DLL_DEMO_kernelCompute << <blocks, threads>> >(
		d_input,
		d_output,
		adder,
		length
		);
	getLastCudaError("Kernel() execution failed\n");
}
