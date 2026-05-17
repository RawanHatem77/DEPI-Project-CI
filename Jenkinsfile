@Library('depi')_

pipeline {
    // agent any
    tools {
        jdk 'jdk-11'
        maven 'mvn-3-5-4'
    }
    stages {
        stage('Build java') {
            steps {
                script {
                    def mvn = new come.depi.mvnClass()
                    mvn.packgeJar('-DskipTests')
                }
            }
        }
        stage('Test java') {
            steps {
                script {
                    def mvn = new come.depi.mvnClass()
                    mvn.testJar('-')
                }
            }
        }
        stage('Build docker ') {
            steps {
                script {
                    def docker = new come.depi.dockerClass()
                    docker.dockerBuild('myapp', 'latest')  // check parameters
                }
            }
        }
    }
}
