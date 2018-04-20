#pragma once

#define MLlib_API __declspec(dllexport)

extern "C"
{
	int MLlib_API __stdcall Return42();


	bool MLlib_API __stdcall CreatePerceptron(
		int mode, float a, int n, int n_val, int din, int dout);
	bool MLlib_API __stdcall DestroyPerceptron();
	bool MLlib_API __stdcall FillData_train(float input[], float output[]);
	bool MLlib_API __stdcall FillData_val(float input[], float output[]);
	bool MLlib_API __stdcall Init();
	void MLlib_API __stdcall Train(int iterations);
	float MLlib_API __stdcall GetError_train();
	float MLlib_API __stdcall GetError_val();
	float MLlib_API * __stdcall CalculateOutput(float input[], int inputSize);
	void MLlib_API __stdcall Debug_Weights();


    void MLlib_API __stdcall SP_CreatePerceptron(float a, int n, int din);
    void MLlib_API __stdcall SP_DestroyPerceptron();
    void MLlib_API __stdcall SP_FillData(float input[], float output[]);
    int MLlib_API __stdcall SP_Run(int iterations);
    float MLlib_API __stdcall SP_GetOutput(float input[], bool useBest);
}