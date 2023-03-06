pipeline { 
    agent { label "kaniko" } 
    options {
        ansiColor('xterm')
        timestamps ()
        buildDiscarder(logRotator(numToKeepStr: '10'))
    }
    stages {
        // stage('Test'){
        //     steps {
        //         sh 'echo "testing will be here"'
        //     }
        // }
        stage('Build') { 
            steps { 
            withCredentials([file(credentialsId: 'config.json', variable: 'FILE')]) {
                // sh 'use $FILE'
                container('kaniko') {
                    script {
                        sh '''
                            cat $FILE > /kaniko/.docker/config.json
                            /kaniko/executor --context `pwd` \
                                                --label include=init-image \
                                                --snapshotMode=full \
                                                --destination yurasdockers/kuberapi:0.1.1 \
                                                --cache=true \
                                                --cache-run-layers \
                                                --cache-copy-layers \
                                                --cache-repo yurasdockers/kuberapi-cache
                        '''
                    }
                }
            }           
            }
        }
        // stage('Deploy') {
        //     steps {
        //         sh 'echo "deploy with GitOps"'
        //     }
        // }
    }
}