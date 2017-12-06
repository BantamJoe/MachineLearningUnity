#include "test.h"

#include "Matrix.h"
#include "mat4.h"
#include "Perceptron.h"
#include <algorithm>

#include <iostream>
#include <Eigen\Dense>

using Eigen::MatrixXd;

int main()
{

}

extern "C" 
{
    void TestSort(int a[], int length) 
    {
        std::sort(a, a + length);
        for (size_t i = 0; i < length; i++)
        {
            a[i] = -a[i];
        }
        std::cout << "test" << std::endl;
    }

    void PutInMatrix(const int i, const int j)
    {
        //Matrix<int, 1, 1> mat = Matrix<int, 1, 1>();
        std::cout << "test" << std::endl;
    }
}

