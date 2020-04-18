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
                sh 'whoami'
                sh 'hostname -i'
                sh 'git clone -b master https://github.com/VStefanov/docker-test.git'
                sh 'ls -lta'
            }
        }

        stage('Build') {
            container('docker') {
                dir('docker-test/') {
                    sh 'docker build -t alphata/web -f MyTestApp/Dockerfile .'
                }
            }
        }

        stage('Publish') {
            container('docker') {
                dir('docker-test/') {
                    sh "echo '${DOCKER_PASSWORD}'' | docker login -u '${DOCKER_ID}' --password-stdin"
                    sh 'docker push alphata/web'
                }
            }
        }
    }
}