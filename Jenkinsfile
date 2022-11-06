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
                // sh '''
                //     popeye -l error -o junit --force-exit-zero --save --output-file popeye.xml
                //     echo "Test result here ->" ; ls -al /tmp/popeye/popeye.xml
                //     # junit skipMarkingBuildUnstable: true, testResults: '/tmp/popeye/popeye.xml'
                //    '''
                sh 'echo "publish your test"'
                // withChecks('Integration Tests') {
                //     junit skipPublishingChecks: true, testResults: '/tmp/popeye/popeye.xml'
                // }
            }
        }
    }
}