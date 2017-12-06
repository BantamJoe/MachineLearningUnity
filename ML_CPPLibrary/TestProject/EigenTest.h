#pragma once

#include <Eigen/Dense>
#include <iostream>

namespace EigenTest 
{
    void Test01();
    void Test02();
    void Test03();
 
    void Debug(char* label, Eigen::MatrixXd mat);
    void Debug(char* label, Eigen::VectorXd vec);

    Eigen::MatrixXd PseudoInverse(const Eigen::MatrixXd& mat);
}