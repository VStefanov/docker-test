podTemplate(label: 'mypod', containers: [
    containerTemplate(name: 'git', image: 'alpine/git', ttyEnabled: true, command: 'cat'),
    containerTemplate(name: 'docker', image: 'docker', command: 'cat', ttyEnabled: true)
  ],
  volumes: [
    hostPathVolume(mountPath: '/var/run/docker.sock', hostPath: '/var/run/docker.sock'),
  ]
  ) {
    node('mypod') {
        stage('Clone repository') {
            container('git') {
                sh 'git clone -b master https://github.com/VStefanov/docker-test.git'
            }
        }

        stage('Build') {
            container('docker') {
                dir('docker-test/') {
                    sh 'docker build -t alphata/web -f MyTestApp/Dockerfile .'
                    sh 'docker build -t alphata/api -f MyTestApp.Api/Dockerfile .'
                    sh 'docker build -t alphata/worker -f MyTestApp.Worker/Dockerfile .'
                    sh 'docker build -t alphata/nginx -f nginx/Dockerfile .'
                }
            }
        }

        stage('Publish') {
            container('docker') {
                dir('docker-test/') {
                    sh "docker login -u '${DOCKER_ID}' -p '${DOCKER_PASSWORD}'"
                    sh 'docker push alphata/web'
                    sh 'docker push alphata/api'
                    sh 'docker push alphata/worker'
                    sh 'docker push alphata/nginx'
                }
            }
        }
    }
}