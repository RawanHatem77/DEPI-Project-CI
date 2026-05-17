@Library('Depi')_

pipeline {
    agent any
    tools {
        jdk 'jdk-11'
        maven 'mvn-3-5-4'
    }
    stages {
        stage('Build java') {
            steps {
                script {
                    mvn = new com.depi.mvnclass(this)
                    mvn.packageJar('-DskipTests')
                }
            }
        }
        stage('Test java') {
            steps {
                script {
                    mvn = new com.depi.mvnclass(this)
                    mvn.testJar('-')
                }
            }
        }
        stage('Build docker ') {
            steps {
                script {
                    docker = new com.depi.dockerClass(this)
                    docker.dockerBuild('myapp', 'latest')  // check parameters
                }
            }
        }
    }
}
