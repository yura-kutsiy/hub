pipeline { 
    agent { label "kaniko" } 
    options {
        ansiColor('xterm')
        timestamps ()
        buildDiscarder(logRotator(numToKeepStr: '10'))
    }
    stages {
                // the code here can access $pass and $user
            stage('Build') { 
                steps { 
                withCredentials([file(credentialsId: 'config.json', variable: 'FILE')]) {
                    // sh 'use $FILE'
                    container('kaniko') {
                        script {
                            sh '''
                                git clone https://github.com/adw0rd/instagrapi-rest.git
                                cd instagram-rest
                                cat $FILE > /kaniko/.docker/config.json
                                /kaniko/executor --context `pwd` \
                                                 --label include=init-image \
                                                 --snapshotMode=full \
                                                 --destination yurasdockers/insta-server
                            '''
                        }
                    }
                }           
                }
            }
        stage('Test'){
            steps {
                sh 'echo "testing will be here"'
            }
        }
        stage('Deploy') {
            steps {
                sh 'echo "deploy with GitOps"'
            }
        }
    }
}