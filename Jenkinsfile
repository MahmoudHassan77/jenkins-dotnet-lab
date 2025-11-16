pipeline {
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:9.0'  // Use any version you need
            args '-u root:root -v /tmp:/tmp'
        }
    }

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
                sh 'dotnet restore ProductApi.sln'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build ProductApi.sln --configuration Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                sh '''
                    echo "Installing trx2junit converter..."
                    dotnet tool install --global trx2junit
                    export PATH="$PATH:/root/.dotnet/tools"

                    echo "Running tests..."
                    dotnet test ProductApi.sln --configuration Release --logger "trx;LogFileName=test-results.trx"

                    echo "Converting TRX → JUnit XML..."
                    trx2junit **/test-results.trx
                '''
            }
        }

        stage('Publish') {
            steps {
                sh 'dotnet publish ProductApi.Api/ProductApi.Api.csproj -c Release -o ./publish'
            }
        }

        stage('Archive Artifacts') {
            steps {
                archiveArtifacts artifacts: 'publish/**/*', fingerprint: true
            }
        }

        stage('Docker Build & Push') {
            agent any  // Docker within Docker: run outside the .NET container
            steps {
                script {
                    sh "docker build -t ${DOCKER_HUB_USERNAME}/${DOCKER_IMAGE_NAME}:${DOCKER_IMAGE_TAG} ."
                    sh "docker tag ${DOCKER_HUB_USERNAME}/${DOCKER_IMAGE_NAME}:${DOCKER_IMAGE_TAG} ${DOCKER_HUB_USERNAME}/${DOCKER_IMAGE_NAME}:latest"

                    withCredentials([usernamePassword(credentialsId: 'docker-hub-credentials', passwordVariable: 'DOCKER_PASSWORD', usernameVariable: 'DOCKER_USERNAME')]) {
                        sh 'echo $DOCKER_PASSWORD | docker login -u $DOCKER_USERNAME --password-stdin'
                        sh "docker push ${DOCKER_HUB_USERNAME}/${DOCKER_IMAGE_NAME}:${DOCKER_IMAGE_TAG}"
                        sh "docker push ${DOCKER_HUB_USERNAME}/${DOCKER_IMAGE_NAME}:latest"
                    }
                }
            }
        }
    }

    post {
        always {
            cleanWs()
        }
    }
}
