#include "MLlib.h"

#include "Perceptron.h"

using namespace MachineLearning;
using namespace Perceptron;

SimplePerceptron* sp;

extern "C" 
{
    void MLlib_API __stdcall CreatePerceptron(float a, int n, int din)
    {
        sp = new SimplePerceptron(a, n, din);
    }

    void MLlib_API __stdcall DestroyPerceptron()
    {
        sp->~SimplePerceptron();
    }

    void MLlib_API __stdcall FillData(float input[], float output[])
    {
        sp->FillData(input, output);

        sp->Init();
    }

    int MLlib_API __stdcall Run(int iterations)
    {
        return sp->Run(iterations);
    }

    float MLlib_API __stdcall GetOutput(float input[], bool useBest)
    {
        return sp->CalculateOutput(input, useBest);
    }
}