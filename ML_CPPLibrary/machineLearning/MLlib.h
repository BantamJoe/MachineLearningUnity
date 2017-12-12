#pragma once

#define MLlib_API __declspec(dllexport)

extern "C"
{
    void MLlib_API __stdcall CreatePerceptron(float a, int n, int din);
    void MLlib_API __stdcall DestroyPerceptron();

    void MLlib_API __stdcall FillData(float input[], float output[]);
    int MLlib_API __stdcall Run(int iterations);
    float MLlib_API __stdcall GetOutput(float input[], bool useBest);
}