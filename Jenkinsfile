pipeline {
    agent any
    
    environment {
        DOTNET_CLI_HOME = '/tmp/dotnet'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        DOTNET_NOLOGO = 'true'
        DOCKER_HUB_USERNAME = 'atwa77'
        DOCKER_IMAGE_NAME = 'productapi'
        DOCKER_IMAGE_TAG = "${BUILD_NUMBER}"
    }
    
    stages {
        stage('Checkout') {
            steps {
                echo 'Checking out code...'
                checkout scm
            }
        }
        
        stage('Restore Dependencies') {
            steps {
                echo 'Restoring NuGet packages...'
                sh 'dotnet restore ProductApi.sln'
            }
        }
        
        stage('Build') {
            steps {
                echo 'Building the solution...'
                sh 'dotnet build ProductApi.sln --configuration Release --no-restore'
            }
        }
        
        stage('Test') {
            steps {
                echo 'Running tests...'
                sh 'dotnet test ProductApi.sln --configuration Release --no-build --verbosity normal --logger "trx;LogFileName=test-results.trx"'
            }
            post {
                always {
                    // Publish test results
                    step([$class: 'MSTestPublisher', testResultsFile: '**/test-results.trx', failOnError: true, keepLongStdio: true])
                }
            }
        }
        
        stage('Publish') {
            steps {
                echo 'Publishing the application...'
                sh 'dotnet publish ProductApi.Api/ProductApi.Api.csproj --configuration Release --no-build --output ./publish'
            }
        }
        
        stage('Archive Artifacts') {
            steps {
                echo 'Archiving artifacts...'
                archiveArtifacts artifacts: 'publish/**/*', fingerprint: true
            }
        }
        
        stage('Docker Build & Push') {
            steps {
                script {
                    echo 'Building Docker image...'
                    sh "docker build -t ${DOCKER_HUB_USERNAME}/${DOCKER_IMAGE_NAME}:${DOCKER_IMAGE_TAG} ."
                    sh "docker tag ${DOCKER_HUB_USERNAME}/${DOCKER_IMAGE_NAME}:${DOCKER_IMAGE_TAG} ${DOCKER_HUB_USERNAME}/${DOCKER_IMAGE_NAME}:latest"
                    
                    echo 'Pushing Docker image to Docker Hub...'
                    withCredentials([usernamePassword(credentialsId: 'docker-hub-credentials', passwordVariable: 'DOCKER_PASSWORD', usernameVariable: 'DOCKER_USERNAME')]) {
                        sh 'echo $DOCKER_PASSWORD | docker login -u $DOCKER_USERNAME --password-stdin'
                        sh "docker push ${DOCKER_HUB_USERNAME}/${DOCKER_IMAGE_NAME}:${DOCKER_IMAGE_TAG}"
                        sh "docker push ${DOCKER_HUB_USERNAME}/${DOCKER_IMAGE_NAME}:latest"
                    }
                    
                    echo "Docker image pushed: ${DOCKER_HUB_USERNAME}/${DOCKER_IMAGE_NAME}:${DOCKER_IMAGE_TAG}"
                    echo "Docker image pushed: ${DOCKER_HUB_USERNAME}/${DOCKER_IMAGE_NAME}:latest"
                }
            }
        }
    }
    
    post {
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed!'
        }
        always {
            echo 'Cleaning up workspace...'
            cleanWs()
        }
    }
}
