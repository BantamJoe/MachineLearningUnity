#pragma once

#include <Eigen/Dense>
#include <vector>

namespace MachineLearning
{
    namespace NeuronalNetwork
    {
        class ActivationFunction
        {
        public:
            virtual float Compute(float value) = 0;
        };
        class AF_Tanh : ActivationFunction
        {
            // Inherited via ActivationFunction
            virtual float Compute(float value) override
            {
                return tanhf(value);
            }
        };

        class Neuron
        {
        private:
            ActivationFunction& g;

        public:
            float GetOutput();
        };
    }
}