#include "MLlib.h"

#include "Perceptron.h"
#include "SimplePerceptron.h"

using namespace MachineLearning;

Perceptron::SimplePerceptron* sp;
Perceptron::Perceptron* p;

extern "C" 
{
	int MLlib_API __stdcall Return42()
	{
		return 42;
	}

	bool MLlib_API CreatePerceptron(int mode, float a, int n, int n_val, int din, int dout)
	{
		Perceptron::OutputMode outmode;
		if (mode == 0)
			outmode = Perceptron::Classification;
		else if (mode == 1)
			outmode = Perceptron::Regression;
		else
			return false;

		p = new Perceptron::Perceptron(outmode, a, n, n_val, din, dout);
		return p != nullptr;
	}

	bool MLlib_API DestroyPerceptron()
	{
		p->~Perceptron();
		return p == nullptr;
	}

	bool MLlib_API FillData_train(float input[], float output[])
	{
		return p->FillData_train(input, output);
	}

	bool MLlib_API FillData_val(float input[], float output[])
	{
		return p->FillData_val(input, output);
	}

	bool MLlib_API Init()
	{
		return p->Init();
	}

	void MLlib_API Train(int iterations)
	{
		p->Train(iterations);
	}

	float MLlib_API GetError_train()
	{
		return p->GetError_train();
	}

	float MLlib_API GetError_val()
	{
		return p->GetError_val();
	}

	float MLlib_API * CalculateOutput(float input[], int inputSize)
	{
		Eigen::VectorXf eigenInput = Eigen::VectorXf(inputSize);
		eigenInput = Eigen::Map<Eigen::VectorXf>(input, inputSize);
		Eigen::VectorXf eigenOutput = p->CalculateOutput(eigenInput);
		return eigenOutput.data();
	}

	void MLlib_API Debug_Weights()
	{
		p->Debug_Weights();
	}



#pragma region SimplePerceptron

	void MLlib_API __stdcall SP_CreatePerceptron(float a, int n, int din)
	{
		sp = new Perceptron::SimplePerceptron(a, n, din);
	}

	void MLlib_API __stdcall SP_DestroyPerceptron()
	{
		sp->~SimplePerceptron();
	}

	void MLlib_API __stdcall SP_FillData(float input[], float output[])
	{
		sp->FillData(input, output);

		sp->Init();
	}

	int MLlib_API __stdcall SP_Run(int iterations)
	{
		return sp->Run(iterations);
	}

	float MLlib_API __stdcall SP_GetOutput(float input[], bool useBest)
	{
		return sp->CalculateOutput(input, useBest);
	}

#pragma endregion

    
}