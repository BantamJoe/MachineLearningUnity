#pragma once

#define TESTDLLSORT_API __declspec(dllexport) 

extern "C" 
{
    TESTDLLSORT_API void TestSort(int a[], int length);
    TESTDLLSORT_API void PutInMatrix(const int i, const int j);
}