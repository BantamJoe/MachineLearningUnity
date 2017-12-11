#include "PerceptronTest.h"

namespace PerceptronTest
{
    SimplePerceptron* sp;

    void SimplePerceptronTest()
    {
        sp = new SimplePerceptron(0.01f, 2, 1);
        
        std::cout << "Perceptron created." << std::endl;

        float input[]{ 1, 2, 3, 4, 5 };
        float output[]{ 1, 1, 1, -1, -1 };

        std::cout << "Input: " << "1, 2, 3, 4, 5" << std::endl;
        std::cout << "Output: " << "1, 1, 1, -1, -1" << std::endl;

        sp->FillData(input, output);
        sp->Init();

        std::cout << std::endl;
        std::cout << "Weights : " << std::endl << sp->getWeights() << std::endl;
        std::cout << "Error : " << sp->getError() << std::endl;

        std::cout << std::endl << "Running 1000 times" << std::endl;

        sp->Run(1000);

        std::cout << std::endl;
        std::cout << "Weights : " << std::endl << sp->getWeights() << std::endl;
        std::cout << "Error : " << sp->getError() << std::endl;

        delete sp;
        std::cout << std::endl << "Perceptron deleted." << std::endl;
    }

    void DllTest()
    {
        f_CreatePerceptron CreatePerceptron;
        f_DestroyPerceptron DestroyPerceptron;
        f_FillData FillData;
        f_Run Run;
        f_GetOutput GetOutput;
        if (!PrepareDll(CreatePerceptron, DestroyPerceptron, FillData, Run, GetOutput))
        {
            return;
        }

        CreatePerceptron(0.01f, 2, 1);
        std::cout << "Perceptron created." << std::endl;

        float input[]{ 1, 2, 3, 4, 5 };
        float output[]{ 1, 1, 1, -1, -1 };

        std::cout << "Input: " << "1, 2, 3, 4, 5" << std::endl;
        std::cout << "Output: " << "1, 1, 1, -1, -1" << std::endl;

        FillData(input, output);

        std::cout << std::endl << "Output for 1 is " << GetOutput(new float[1] {1}) << std::endl;
        std::cout << "Running 1000 times" << std::endl;
        Run(1000);
        std::cout << "Output for 1 is " << GetOutput(new float[1] {1}) << std::endl;

        DestroyPerceptron();
        std::cout << std::endl << "Perceptron deleted." << std::endl;

    }

    bool PrepareDll(f_CreatePerceptron& createPerceptron,
                    f_DestroyPerceptron& destroyPerceptron,
                    f_FillData& fillData,
                    f_Run& run,
                    f_GetOutput& getOutput)
    {
        HINSTANCE hGetProcIDDLL = LoadLibrary("..\\x64\\Release\\machineLearning.dll");

        if (!hGetProcIDDLL) {
            std::cout << "could not load the dynamic library" << std::endl;
            return false;
        }

        // resolve function address here
        /*createPerceptron = (f_CreatePerceptron)GetProcAddress(hGetProcIDDLL, "CreatePerceptron");
        if (!createPerceptron) {
            std::cout << "could not locate the function " << "CreatePerceptron" << std::endl;
            return false;
        }*/
        if (!GetFuncFromDll<f_CreatePerceptron>(hGetProcIDDLL, createPerceptron, "CreatePerceptron")
            || !GetFuncFromDll<f_DestroyPerceptron>(hGetProcIDDLL, destroyPerceptron, "DestroyPerceptron")
            || !GetFuncFromDll<f_FillData>(hGetProcIDDLL, fillData, "FillData")
            || !GetFuncFromDll<f_Run>(hGetProcIDDLL, run, "Run")
            || !GetFuncFromDll<f_GetOutput>(hGetProcIDDLL, getOutput, "GetOutput"))
        {
            return false;
        }

        return true;
    }
}