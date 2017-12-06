#include "EigenTest.h"

using Eigen::MatrixXd;
using Eigen::VectorXd;
using Eigen::ArrayXd;

namespace EigenTest
{
    // Assignation
    void Test01()
    {
        MatrixXd m1(2, 2);
        m1(0, 0) = 3;
        m1(1, 0) = 2.5;
        m1(0, 1) = -1;
        m1(1, 1) = m1(1, 0) + m1(0, 1);
        Debug("Matrix 1", m1);

        MatrixXd m2(5, 2);
        m2 << 1, 0.1,
            2, 0.2,
            3, 0.3,
            4, 0.4,
            5, 0.5;
        Debug("Matrix 2", m2);

        VectorXd v(3);
        v << 0.5, 0.5, 0.5;
        Debug("Vector", v);
    }

    // Transposed, inverse, calcul
    void Test02()
    {
        MatrixXd mat(5, 3);
        mat << -1, 1, 9,
            -1, 2, 8,
            1, -3, 7,
            1, 4, 6,
            -2, 5, -5;
        Debug("Matrix", mat);

        MatrixXd T = mat.transpose();
        Debug("Transposed", T);
        // Attention, ne jamais faire 
        // a = a.transpose(); 
        // mais plutot
        // a.transposeInPlace();

        // TMP
        MatrixXd mult = mat * T;
        Debug("Matrix * Transposed", mult);
        // END TMP

        MatrixXd inv = mult.inverse();
        Debug("Inverse", inv);

        MatrixXd PI = PseudoInverse(mat);
        Debug("Pseudo-Inverse", PI);
    }

    // Matrices and vectors
    void Test03()
    {
        MatrixXd mat(5, 2);
        mat << 1, 0.1,
            2, 0.2,
            3, 0.3,
            4, 0.4,
            5, 0.5;
        Debug("Matrix", mat);

        VectorXd One(5);
        One.setOnes();
        Debug("Vector of ones", One);

        MatrixXd X(mat);
        X.conservativeResize(Eigen::NoChange, X.cols() + 1);
        X.col(X.cols() - 1) = One;
        Debug("Addition", X);

        VectorXd W(3);
        W << 0.5, 0.5, 0.5;
        Debug("Another vector", W);

        VectorXd Result = X * W;
        Debug("Multiplication X * W", Result);
    }


    void Debug(char * label, Eigen::MatrixXd mat)
    {
        std::cout << label << " : " << std::endl << mat << std::endl << std::endl;
    }

    void Debug(char * label, Eigen::VectorXd vec)
    {
        std::cout << label << " : " << std::endl << vec << std::endl << std::endl;
    }

    MatrixXd PseudoInverse(const MatrixXd& mat)
    {
        return mat.transpose() * (mat * mat.transpose()).inverse();
    }
}