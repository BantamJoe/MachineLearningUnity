#pragma once

#include <Eigen/Dense>
#include <vector>

namespace MachineLearning
{
    namespace Perceptron 
    {
		enum OutputMode {
			Classification = 0,
			Regression = 1
		};

		// Perceptron mono-couche, multi-classes avec data de validation optionnelle.
		// Classification et régression.
		class Perceptron 
		{
		private:
			OutputMode outputMode;

			float a; // facteur d'apprentissage

			int n; // Nombre de data
			int n_val;
			int din; // Nombre d'inputs par data
			int dout; // Nombre d'outputs par data

			Eigen::MatrixXf in; // Inputs
			Eigen::MatrixXf out; // Output calculé
			Eigen::MatrixXf t; // Outputs cibles

			Eigen::MatrixXf in_val; // 
			Eigen::MatrixXf out_val; // Output de validation calculé
			Eigen::MatrixXf t_val; // Output de validation cible

			Eigen::VectorXf w; // Poids des paramètres
			
			float e; // Erreur
			float e_val; 

			std::vector<int> mc; // Indices des datas mal classees

			Perceptron();
			void ComputeError(const float& threshold = 1);
			void ComputeError_val(const float& threshold = 1);
			void CalculateOutput(const int& index);
			void CalculateOutput_val(const int& index);
			float CompareOutputs(const int& index);
			float CompareOutputs_val(const int& index);

		public:

			Perceptron(const OutputMode& mode,
				const float& a, const int& n,
				const int& n_val, const int& din,
				const int& dout);

			bool FillData_train(float input_train[], float output_train[]);
			bool FillData_val(float input_val[], float output_val[]);
			bool Init();
			void Train(const int& iterations);

			float GetError_train();
			float GetError_val();
			Eigen::VectorXf CalculateOutput(Eigen::VectorXf input);
			void Debug_Weights();
		};



		/*
        class FinalPerceptron
        {
        private:
			
			OutputMode outputMode; // classification or regression
			float a; // learning factor

			int n; // data amount
			int din; // inputs par data
			Eigen::MatrixXf in; // inputs
			
			int dout; // outputs per data
			Eigen::MatrixXf t; // target output
			Eigen::MatrixXf out; // found output
			
			int l; // number of layers (default is 1)
            Eigen::VectorXi d; // neurons per layer
            //Eigen::VectorXi dadd; // d (additif) 

            Eigen::MatrixXf w; // weights per layer
			Eigen::MatrixXf wbest;

			int e; // error count
			int ebest; // minimal error count
			std::vector<int> mc; // misclassified data indices

			int n_val;
			Eigen::MatrixXf in_val;
			Eigen::MatrixXf out_val;
			int e_val;
			int ebest_val;

			FinalPerceptron() {}

        public:

			// Single layer perceptron
			FinalPerceptron(int mode, float learningFactor,
				int dataCount, int inputs, 
				int outputs = 1, int validationCount = 0);

			// Multi layer perceptron
			FinalPerceptron(int mode, float learningFactor,
				int dataCount, int inputs, 
				int layers, int* neuronsPerLayer, 
				int outputs = 1, int validationCount = 0);

			~FinalPerceptron();
        };
		*/
    }
}