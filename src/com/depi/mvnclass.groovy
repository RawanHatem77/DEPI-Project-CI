package com.depi

class mvnClass implements Serializable {
    def steps

    mvnClass(steps) {
        this.steps = steps

    def packageJar(packagejavaOpt) {
        sh " mvn clean package install ${packagejavaOpt} "
    }
    def testJar(testjavaOpt) {
        sh " mvn clean package install ${testjavaOpt} "
    }
    }

