pipeline {
    agent any
    stages{
        stage('Build'){
            steps {
                sh '''
                   cd $WORKSPACE
                   
                   chmod -R a+rwx $WORKSPACE
                   chmod a+rwx /var/run/docker.sock

                   docker build -t alphata/web -f MyTestApp/Dockerfile .
                   docker build -t alphata/api -f MyTestApp.Api/Dockerfile .
                   docker build -t alphata/worker -f MyTestApp.Worker/Dockerfile .

                   docker images
                '''
            }
        }
    }
}