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
                    popeye -l error -o html --force-exit-zero --save --output-file popeye.html
                   '''
            }
        }
        stage('Deploy') {
            steps {
                sh 'echo "deploy with GitOps"'
            }
        }
    }
}