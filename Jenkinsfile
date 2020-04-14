pipeline {
    agent any
    stages{
        stage('Build'){
            steps {
                sh '''
                   cd $WORKSPACE
                   
                   chmod -R a+rwx $WORKSPACE

                   sudo docker build -t alphata/web -f MyTestApp/Dockerfile .
                   sudo docker build -t alphata/api -f MyTestApp.Api/Dockerfile .
                   sudo docker build -t alphata/worker -f MyTestApp.Worker/Dockerfile .

                   sudo docker images
                '''
            }
        }
    }
}