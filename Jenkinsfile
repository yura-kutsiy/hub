pipeline { 
    agent { label "default" } 
    options {
        ansiColor('xterm')
        timestamps ()
        buildDiscarder(logRotator(numToKeepStr: '10'))
    }
    stages {
        stage('Post html'){
            steps {
                sh '''
                    popeye -l error -o html --force-exit-zero --save --output-file popeye.html
                   '''
            }
        }
        stage('Publish juint') {
            steps {
                sh '''
                    popeye -l error -o junit --force-exit-zero --save --output-file popeye.xml
                    # junit skipMarkingBuildUnstable: true, testResults: '/tmp/popeye/popeye.xml'
                   '''
                sh 'echo "deploy with GitOps"'
                withChecks('Integration Tests') {
                    junit '/tmp/popeye/popeye.xml'
                }
            }
        }
    }
}