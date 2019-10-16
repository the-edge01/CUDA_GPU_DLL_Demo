//2019 Eric Johnson
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <math.h>
#include <cuda_runtime.h>

#include <helper_functions.h>
#include <helper_cuda.h>

#include "cuda_computation_common.h"


int* d_array_in;
int* d_array_out;

int array_size = 0;


extern "C" int __declspec(dllexport) __stdcall init(int array_size_local){
	array_size = array_size_local;

	//memory allocation for input data
	checkCudaErrors(cudaMalloc((void **)&d_array_in, array_size * sizeof(int)));

	//memory allocation for output data
	checkCudaErrors(cudaMalloc((void **)&d_array_out, array_size * sizeof(int)));

	return array_size; 
}

//frees the memory created in init
extern "C" void __declspec(dllexport) __stdcall cleanup(){

	checkCudaErrors(cudaFree(d_array_in));
	checkCudaErrors(cudaFree(d_array_out));

	return;
}

extern "C" void __declspec(dllexport) __stdcall loadArray(int* data){

	checkCudaErrors(cudaMemcpy(d_array_in, data, array_size * sizeof(int), cudaMemcpyHostToDevice));
	return;
}

extern "C" void __declspec(dllexport) __stdcall compute(int adder, int* buffer){
	GPU_DLL_DEMO_GPU(
		d_array_in,
		d_array_out,
		adder,
		array_size
		);

	checkCudaErrors(cudaMemcpy(buffer, d_array_out, array_size * sizeof(int), cudaMemcpyDeviceToHost));
	
	return;
}



////////////////////////////////////////////////////////////////////////////////
// Main program
////////////////////////////////////////////////////////////////////////////////
int main(int argc, char **argv)
{
	int channel_count = 4;
	init(channel_count);

	int* imgdata = (int*)malloc(channel_count);
	for (int i = 0; i < channel_count; i++){
		imgdata[i] = 42;
	}
	loadArray(imgdata);

	compute(5, imgdata);


	cleanup();
	return 0;

}
