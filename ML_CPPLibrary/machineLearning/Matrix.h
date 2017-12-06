#pragma once

template <typename T, int I, int J>
class Matrix 
{
private:
    int i;
    int j;

public:
    Matrix<T, I, J>() 
    {
        i = I;
        j = J;
    }

    int GetI() { return i; }
    int GetJ() { return j; }
};