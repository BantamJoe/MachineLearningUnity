#include "Perceptron.h"
#include <random>
#include <iostream>

namespace MachineLearning 
{
    namespace Perceptron 
    {
        Perceptron::Perceptron(const OutputMode& mode,
			const float& a, const int& n,
			const int& n_val, const int& din,
			const int& dout) 
		{
			this->outputMode = mode;
			this->a = a;
			this->n = n;
			this->n_val = n_val;
			this->din = din;
			this->dout = dout;

			in = Eigen::MatrixXf(n, din);
			out = Eigen::MatrixXf(n, dout);
			t = Eigen::MatrixXf(n, dout);

			if (n_val > 0)
			{
				in_val = Eigen::MatrixXf(n_val, din);
				out_val = Eigen::MatrixXf(n_val, dout);
				t_val = Eigen::MatrixXf(n_val, dout);
			}

			w = Eigen::VectorXf((din + 1) * dout);

			mc = std::vector<int>();
			mc.reserve(n);

			e = e_val = -1;
		}

		bool Perceptron::FillData_train(float input[], float output[])
		{
			in = Eigen::Map<Eigen::MatrixXf>(input, n, din);
			t = Eigen::Map<Eigen::MatrixXf>(output, n, dout);
			return true;
		}
		bool Perceptron::FillData_val(float input[], float output[])
		{
			if (n_val > 0)
			{
				in_val = Eigen::Map<Eigen::MatrixXf>(input, n_val, din);
				t_val = Eigen::Map<Eigen::MatrixXf>(output, n_val, dout);
				return true;
			}
			return false;
		}

		bool Perceptron::Init()
		{
			float rdm;

			for (size_t i = 0; i < w.size(); ++i)
			{
				rdm = rand() / static_cast<float>(RAND_MAX);
				w(i) = rdm - 0.5f;
			}

			if (outputMode == Classification)
			{
				ComputeError();
			}
			else if (outputMode == Regression)
			{
				ComputeError(0.01f);
			}
			return true;
		}

		void Perceptron::Train(const int& iterations) 
		{
			for (size_t i = 0; i < iterations; ++i)
			{
				if (e == 0)
					return;

				int badOne = mc[rand() % (int)e];
				
				for (size_t o = 0; o < dout; o++)
				{
					float diff = t(badOne, o) - out(badOne, o);
					for (size_t d = 0; d < din; d++)
					{
						w(o * (din+1) + d) += a * in(badOne, d) * diff;
					}
				}

				if (outputMode == Classification)
				{
					ComputeError();
				}
				else if (outputMode == Regression)
				{
					ComputeError(0.01f);
				}
			}
		}

		void Perceptron::ComputeError(const float& threshold)
		{
			e = 0;
			mc.clear();

			for (size_t m = 0; m < n; ++m)
			{
				CalculateOutput(m);
				float diff = CompareOutputs(m);
				if (diff >= threshold)
				{
					mc.push_back(m);
					e += diff;
				}
			}
		}
		void Perceptron::ComputeError_val(const float& threshold)
		{
			if (n_val < 0) 
			{
				std::cout << "ComputeError_val() ERROR : No validation data." << std::endl;
				return;
			}

			e_val = 0;
			for (size_t m = 0; m < n_val; ++m)
			{
				CalculateOutput_val(m);
				float diff = CompareOutputs_val(m);
				if (diff >= threshold)
				{
					e_val += diff;
				}
			}


		}

		void Perceptron::CalculateOutput(const int& index)
		{
			for (size_t o = 0; o < dout; ++o)
			{
				float sum = 0;
				for (size_t d = 0; d < din; ++d)
				{
					sum += in(index, d) * w(o * (din + 1) + d);
				}
				sum += w(din); // biais

				if (outputMode == Classification)
				{
					out(index, o) = tanh(sum);
				}
				else if (outputMode == Regression) 
				{
					out(index, o) = sum;
				}
			}
		}
		void Perceptron::CalculateOutput_val(const int& index)
		{
			if (n_val < 0)
			{
				std::cout << "CalculateOutput_val() ERROR : No validation data." << std::endl;
				return;
			}

			for (size_t o = 0; o < dout; ++o)
			{
				float sum = 0;
				for (size_t d = 0; d < din; ++d)
				{
					sum += in_val(index, d) * w(o * (din + 1) + d);
				}
				sum += w(din); // biais

				if (outputMode == Classification)
				{
					out_val(index, o) = tanh(sum);
				}
				else if (outputMode == Regression)
				{
					out_val(index, o) = sum;
				}
			}
		}

		Eigen::VectorXf Perceptron::CalculateOutput(Eigen::VectorXf input) 
		{
			Eigen::VectorXf output = Eigen::VectorXf(dout);
			if (input.size() != din)
			{
				std::cout << "CalculateOutput() ERROR : Invalid input size." << std::endl;
				return output;
			}

			for (size_t o = 0; o < dout; ++o)
			{
				float sum = 0;
				for (size_t d = 0; d < din; ++d)
				{
					sum += input(d) * w(o * (din + 1) + d);
				}
				sum += w(din); // biais

				if (outputMode == Classification)
				{
					output(o) = tanh(sum);
				}
				else if (outputMode == Regression)
				{
					output(o) = sum;
				}
			}

			return output;
		}

		float Perceptron::CompareOutputs(const int& index)
		{
			if (outputMode == Classification)
			{
				for (size_t o = 0; o < dout; o++)
				{
					if (out(index, o) > 0 && t(index, o) < 0)
						return 1; 
					else if (out(index, o) < 0 && t(index, o) > 0)
						return 1;
				}
				return 0; // no error
			}
			else if (outputMode == Regression) 
			{
				float errorSum = 0;
				for (size_t o = 0; o < dout; o++)
				{
					float diff = abs(out(index, o) - t(index, o));
					errorSum += diff;
				}
				return errorSum;
			}
			return 0;
		}
		float Perceptron::CompareOutputs_val(const int& index)
		{
			if (outputMode == Classification)
			{
				for (size_t o = 0; o < dout; o++)
				{
					if (out_val(index, o) > 0 && t_val(index, o) < 0)
						return 1;
					else if (out_val(index, o) < 0 && t_val(index, o) > 0)
						return 1;
				}
				return 0; // no error
			}
			else if (outputMode == Regression)
			{
				float errorSum = 0;
				for (size_t o = 0; o < dout; o++)
				{
					float diff = abs(out_val(index, o) - t_val(index, o));
					errorSum += diff;
				}
				return errorSum;
			}
			return 0;
		}

		float Perceptron::GetError_train() 
		{
			if (e < 0)
				std::cout << "GetError_train() warning : perceptron has not been initialized yet." << std::endl;
			return e;
		}
		float Perceptron::GetError_val()
		{
			if (e_val < 0) 
			{
				std::cout << "GetError_val() warning : perceptron has not been initialized yet." << std::endl;
				return e_val;
			}

			if (outputMode == Classification)
			{
				ComputeError_val();
			}
			else if (outputMode == Regression) 
			{
				ComputeError_val(0.01f);
			}
			return e_val;
		}
		void Perceptron::Debug_Weights()
		{
			std::cout << "Current weights : " << w << std::endl;
		}



		/*
		FinalPerceptron::FinalPerceptron(int mode, float learningFactor,
			int dataCount, int inputs, int outputs, int validationCount)
		{
			switch (mode)
			{
			case 1: 
				outputMode = Regression;
				break;
			default:
				outputMode = Classification;
				break;
			}

			a = learningFactor;
			n = dataCount;
			din = inputs;
			dout = outputs;
			l = 1;

			// d = 

			in = Eigen::MatrixXf(n, din);
			t = Eigen::MatrixXf(n, dout);
			out = Eigen::MatrixXf(n, dout);

			w = Eigen::MatrixXf();

			w = Eigen::VectorXf(din + 1);
			wbest = Eigen::VectorXf(din + 1);

			mc = std::vector<int>();
			mc.reserve(n);
		}
		*/
	}
}