pipeline { 
    agent { label "default" } 
    options {
        ansiColor('xterm')
        timestamps ()
        buildDiscarder(logRotator(numToKeepStr: '10'))
    }
    stages {
        stage('Test'){
            steps {
                sh '''
                    popeye -l error -o html --save --output-file popeye.html
                    ls -al /tmp/popeye/
                    cat /tmp/popeye/popeye.html
                   '''
            }
        }
        stage('Deploy') {
            steps {
                sh 'cat /tmp/popeye/popeye.html'
                sh 'echo "deploy with GitOps"'
            }
        }
    }
}