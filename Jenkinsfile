pipeline {
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:9.0'  // Use any version you need
            args '-u root:root -v /tmp:/tmp'
        }
    }

    stages {

        stage('Checkout') {
            steps {
                echo 'Checking out code...'
                checkout scm
            }
        }

        stage('Build') {
            steps {
                script {
                    // Restoring dependencies
                    //bat "cd ${DOTNET_CLI_HOME} && dotnet restore"
                    sh "dotnet restore"

                    // Building the application
                    sh "dotnet build --configuration Release"
                }
            }
        }

        stage('Test') {
            steps {
                script {
                    // Running tests
                    sh "dotnet test --no-restore --configuration Release"
                }
            }
        }
        stage('Publish') {
            steps {
                script {
                    // Publishing the application
                    sh "dotnet publish --no-restore --configuration Release --output .\\publish"
                }
            }
        }
        stage('Docker Build and Push') {
            agent {
                docker { 
                    reuseNode true
                }
            }

            steps {
              withDockerRegistry(credentialsId: 'f263c7a8-7cb5-4cee-80a1-7abfe08a005b', url: 'https://index.docker.io/v1/') {
                sh 'docker build -t product-api .'
                sh 'docker tag  product-api atwa77/product-api:latest'
                sh 'docker push atwa77/product-api:latest'
              }
            }
        }
    }

    post {
        always {
             echo 'Build, test, and publish successful!'
        }
    }
}
