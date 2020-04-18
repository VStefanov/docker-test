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

        stage('Maven Build') {
            container('docker') {
                dir('docker-test/') {
                    sh 'echo $WORKSPACE'
                    sh 'echo ls -lta'
                }
            }
        }
    }
}