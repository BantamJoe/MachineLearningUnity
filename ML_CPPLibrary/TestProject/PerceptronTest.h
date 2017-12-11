#pragma once

#include <windows.h>
#include <iostream>
#include "Perceptron.h"

/* Define a function pointer for our imported function.
* This reads as "introduce the new type f_funci as the type:
*                pointer to a function returning an int and
*                taking no arguments.
*
* Make sure to use matching calling convention (__cdecl, __stdcall, ...)
* with the exported function. __stdcall is the convention used by the WinAPI
*/
typedef void(__stdcall *f_CreatePerceptron)(float a, int n, int din);
typedef void(__stdcall *f_DestroyPerceptron)();
typedef void(__stdcall *f_FillData)(float input[], float output[]);
typedef int(__stdcall *f_Run)(int iterations);
typedef float(__stdcall *f_GetOutput)(float input[]);

using namespace MachineLearning;
using namespace Perceptron;

namespace PerceptronTest 
{
    void SimplePerceptronTest();
    void DllTest();

    bool PrepareDll(f_CreatePerceptron& createPerceptron,
                    f_DestroyPerceptron& destroyPerceptron,
                    f_FillData& fillData,
                    f_Run& run,
                    f_GetOutput& getOutput);

    template<typename T>
    bool GetFuncFromDll(const HMODULE& dllmodule, T & receptor, const char * name)
    {
        receptor = (T)GetProcAddress(dllmodule, name);
        if (!receptor)
        {
            std::cout << "Could not locate the function " << name << "." << std::endl;
            return false;
        }
        return true;
    }
}