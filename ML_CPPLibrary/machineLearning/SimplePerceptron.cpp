#include "SimplePerceptron.h"
#include <random>
#include <iostream>

namespace MachineLearning
{
	namespace Perceptron
	{
		float SimplePerceptron::CalculateOutput(const int & index, bool useBest)
		{
			float sum = 0;
			for (size_t d = 0; d < din; d++)
			{
				if (useBest)
					sum += in(index, d) * wbest(d);
				else
					sum += in(index, d) * w(d);
			}
			if (useBest)
				sum += wbest(din);
			else
				sum += w(din); // biais

			return tanh(sum);
		}

		int SimplePerceptron::Run(int iterations)
		{
			if (ebest <= 0)
				return ebest;

			for (size_t i = 0; i < iterations; i++)
			{
				ImproveWeights();
				CalculateGlobalError();

				if (ebest <= 0)
					break;
			}

			return ebest;
		}

		float SimplePerceptron::CalculateOutput(float input[], bool useBest)
		{
			float sum = 0;
			for (size_t d = 0; d < din; d++)
			{
				if (useBest)
					sum += input[d] * wbest(d);
				else
					sum += input[d] * w(d);
			}
			if (useBest)
				sum += wbest(din);
			else
				sum += w(din); // biais

			return tanh(sum);
		}

		void SimplePerceptron::CalculateGlobalError()
		{
			e = 0;
			mc.clear();

			for (size_t m = 0; m < n; m++)
			{
				y(m) = CalculateOutput(m);
				if (y(m) >= 0 && t(m) <= 0 || y(m) <= 0 && t(m) >= 0)
				{
					mc.push_back(m);
					e++;
				}
			}

			if (e <= ebest)
			{
				wbest = Eigen::VectorXf(w);
				ebest = e;
			}
		}

		void SimplePerceptron::ImproveWeights()
		{
			if (e <= 0)
				return;

			int badOne = mc[rand() % e];
			float diff = t(badOne) - y(badOne);
			for (size_t d = 0; d < din; d++)
			{
				w(d) += a * in(badOne, d) * diff;
			}
			w(din) += a * diff;
		}

		SimplePerceptron::SimplePerceptron(const float& a, const int& n, const int& din)
		{
			this->a = a;
			this->n = n;
			this->din = din;

			in = Eigen::MatrixXf(n, din);
			t = Eigen::VectorXf(n);
			y = Eigen::VectorXf(n);

			w = Eigen::VectorXf(din + 1);
			wbest = Eigen::VectorXf(din + 1);

			mc = std::vector<int>();
			mc.reserve(n);
		}

		void SimplePerceptron::FillData(float input[], float output[])
		{
			in = Eigen::Map<Eigen::MatrixXf>(input, n, din);
			t = Eigen::Map<Eigen::VectorXf>(output, n);

			//std::cout << "Input matrix : " << in << std::endl;
		}

		void SimplePerceptron::FillData(Eigen::MatrixXf input, Eigen::MatrixXf output)
		{
			in = Eigen::MatrixXf(input);
			t = Eigen::MatrixXf(output);
		}

		void SimplePerceptron::Init()
		{
			ebest = INT_MAX;

			float rdm;

			for (size_t i = 0; i < din + 1; i++)
			{
				rdm = rand() / static_cast<float>(RAND_MAX);
				w(i) = rdm - 0.5f;
				wbest(i) = w(i);
			}

			CalculateGlobalError();
		}
	}
}