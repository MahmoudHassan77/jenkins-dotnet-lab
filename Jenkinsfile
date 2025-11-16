pipeline {
    agent none  // No default agent

    stages {
        stage('Checkout') {
            agent any
            steps {
                echo 'Checking out code...'
                checkout scm
            }
        }

        stage('Build & Test') {
            agent {
                docker {
                    image 'mcr.microsoft.com/dotnet/sdk:9.0'
                    args '-u root:root -v /tmp:/tmp'
                }
            }
            stages {
                stage('Restore') {
                    steps {
                        sh "dotnet restore"
                    }
                }
                stage('Build') {
                    steps {
                        sh "dotnet build --configuration Release"
                    }
                }
                stage('Test') {
                    steps {
                        sh "dotnet test --no-restore --configuration Release"
                    }
                }
                stage('Publish') {
                    steps {
                        sh "dotnet publish --no-restore --configuration Release --output ./publish"
                    }
                }
            }
        }

        stage('Docker Build and Push') {
            agent any  // Use Jenkins host with Docker installed
            steps {
                withDockerRegistry(credentialsId: 'f263c7a8-7cb5-4cee-80a1-7abfe08a005b', url: 'https://index.docker.io/v1/') {
                    sh 'docker build -t product-api .'
                    sh 'docker tag product-api atwa77/product-api:latest'
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