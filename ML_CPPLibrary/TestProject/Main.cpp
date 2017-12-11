#include <iostream>

#include "EigenTest.h"
#include "PerceptronTest.h"

int main() 
{
    std::cout << "Beginning program." << std::endl;
    std::cout << "==================" << std::endl << std::endl;

    //EigenTest::Test01();
    //EigenTest::Test02();
    //EigenTest::Test03();
    
    //PerceptronTest::SimplePerceptronTest();
    PerceptronTest::DllTest();

    std::cout << std::endl << "==================" << std::endl;
    std::cout << "End of program. Enter a key to quit." << std::endl;

    int q;
    std::cin >> q;
}